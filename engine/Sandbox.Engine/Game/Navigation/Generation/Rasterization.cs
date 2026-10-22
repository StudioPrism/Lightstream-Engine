namespace Sandbox.Navigation.Generation;

[SkipHotload]
internal static class Rasterization
{
	internal enum Axis
	{
		X = 0,
		Y = 1,
		Z = 2
	}

	private static bool OverlapBounds( Vector3 aMin, Vector3 aMax, Vector3 bMin, Vector3 bMax )
	{
		return
			aMin.x <= bMax.x && aMax.x >= bMin.x &&
			aMin.y <= bMax.y && aMax.y >= bMin.y &&
			aMin.z <= bMax.z && aMax.z >= bMin.z;
	}

	/// <summary>
	/// Divides a convex polygon of max 12 vertices into two convex polygons across a separating axis
	/// </summary>
	private static void DividePoly( Span<Vector3> inVerts, int inVertsCount,
								  Span<Vector3> outVerts1, out int outVerts1Count,
								  Span<Vector3> outVerts2, out int outVerts2Count,
								  float axisOffset, Axis axis )
	{
		// Calculate how far positive or negative away from the separating axis is each vertex
		Span<float> inVertAxisDelta = stackalloc float[12]; // Max input vertices is 12
		for ( int inVert = 0; inVert < inVertsCount; ++inVert )
		{
			inVertAxisDelta[inVert] = axisOffset - inVerts[inVert][(int)axis];
		}

		int poly1Vert = 0;
		int poly2Vert = 0;

		for ( int inVertA = 0, inVertB = inVertsCount - 1; inVertA < inVertsCount; inVertB = inVertA, ++inVertA )
		{
			// If the two vertices are on the same side of the separating axis
			bool sameSide = (inVertAxisDelta[inVertA] >= 0) == (inVertAxisDelta[inVertB] >= 0);

			if ( !sameSide )
			{
				// Calculate the point of intersection with the separating axis
				float s = inVertAxisDelta[inVertB] / (inVertAxisDelta[inVertB] - inVertAxisDelta[inVertA]);

				outVerts1[poly1Vert] = inVerts[inVertB] + (inVerts[inVertA] - inVerts[inVertB]) * s;
				outVerts2[poly2Vert] = outVerts1[poly1Vert];
				poly1Vert++;
				poly2Vert++;

				// Add the inVertA point to the right polygon. Do NOT add points that are on the dividing line
				// since these were already added above
				if ( inVertAxisDelta[inVertA] > 0 )
				{
					outVerts1[poly1Vert] = inVerts[inVertA];
					poly1Vert++;
				}
				else if ( inVertAxisDelta[inVertA] < 0 )
				{
					outVerts2[poly2Vert] = inVerts[inVertA];
					poly2Vert++;
				}
			}
			else
			{
				// Add the inVertA point to the right polygon. Addition is done even for points on the dividing line
				if ( inVertAxisDelta[inVertA] >= 0 )
				{
					outVerts1[poly1Vert] = inVerts[inVertA];
					poly1Vert++;

					if ( inVertAxisDelta[inVertA] != 0 )
					{
						continue;
					}
				}

				outVerts2[poly2Vert] = inVerts[inVertA];
				poly2Vert++;
			}
		}

		outVerts1Count = poly1Vert;
		outVerts2Count = poly2Vert;
	}

	/// <summary>
	/// Rasterize a single triangle to the heightfield
	/// </summary>
	private static void RasterizeTri( Vector3 v0, Vector3 v1, Vector3 v2,
									 int areaId, Heightfield heightfield,
									 Vector3 heightfieldBBMin, Vector3 heightfieldBBMax,
									 float cellSize, float inverseCellSize, float inverseCellHeight,
									 int flagMergeThreshold )
	{
		// Calculate the bounding box of the triangle
		Vector3 triBBMin = Vector3.Min( Vector3.Min( v0, v1 ), v2 );
		Vector3 triBBMax = Vector3.Max( Vector3.Max( v0, v1 ), v2 );

		// If the triangle does not touch the bounding box of the heightfield, skip it
		if ( !OverlapBounds( triBBMin, triBBMax, heightfieldBBMin, heightfieldBBMax ) )
		{
			return;
		}

		int w = heightfield.Width;
		int h = heightfield.Height;
		float by = heightfieldBBMax.y - heightfieldBBMin.y;

		// Calculate the footprint of the triangle on the grid's z-axis
		int z0 = (int)MathF.Floor( (triBBMin.z - heightfieldBBMin.z) * inverseCellSize );
		int z1 = (int)MathF.Floor( (triBBMax.z - heightfieldBBMin.z) * inverseCellSize );

		// Use -1 rather than 0 to cut the polygon properly at the start of the tile
		z0 = Math.Clamp( z0, -1, h - 1 );
		z1 = Math.Clamp( z1, 0, h - 1 );

		// Clip the triangle into all grid cells it touches
		Span<Vector3> inVerts = stackalloc Vector3[7];
		Span<Vector3> inRow = stackalloc Vector3[7];
		Span<Vector3> p1 = stackalloc Vector3[7];
		Span<Vector3> p2 = stackalloc Vector3[7];

		inVerts[0] = v0;
		inVerts[1] = v1;
		inVerts[2] = v2;
		int nvRow;
		int nvIn = 3;

		for ( int z = z0; z <= z1; ++z )
		{
			// Clip polygon to row. Store the remaining polygon as well
			float cellZ = heightfieldBBMin.z + z * cellSize;
			DividePoly( inVerts, nvIn, inRow, out nvRow, p1, out nvIn, cellZ + cellSize, Axis.Z );

			// Swap p1 into inVerts for next iteration
			Span<Vector3> temp = inVerts;
			inVerts = p1;
			p1 = temp;

			if ( nvRow < 3 )
				continue;
			if ( z < 0 )
				continue;

			// Find X-axis bounds of the row
			float minX = inRow[0].x;
			float maxX = inRow[0].x;
			for ( int vert = 1; vert < nvRow; ++vert )
			{
				minX = Math.Min( minX, inRow[vert].x );
				maxX = Math.Max( maxX, inRow[vert].x );
			}

			int x0 = (int)MathF.Floor( (minX - heightfieldBBMin.x) * inverseCellSize );
			int x1 = (int)MathF.Floor( (maxX - heightfieldBBMin.x) * inverseCellSize );
			if ( x1 < 0 || x0 >= w )
				continue;

			x0 = Math.Clamp( x0, -1, w - 1 );
			x1 = Math.Clamp( x1, 0, w - 1 );

			int nv;
			int nv2 = nvRow;

			for ( int x = x0; x <= x1; ++x )
			{
				// Clip polygon to column. Store the remaining polygon as well
				float cx = heightfieldBBMin.x + x * cellSize;
				DividePoly( inRow, nv2, p1, out nv, p2, out nv2, cx + cellSize, Axis.X );

				// Swap p2 into inRow for next iteration
				Span<Vector3> swap = inRow;
				inRow = p2;
				p2 = swap;

				if ( nv < 3 )
					continue;
				if ( x < 0 )
					continue;

				// Calculate min and max of the span
				float spanMin = p1[0].y;
				float spanMax = p1[0].y;
				for ( int vert = 1; vert < nv; ++vert )
				{
					spanMin = Math.Min( spanMin, p1[vert].y );
					spanMax = Math.Max( spanMax, p1[vert].y );
				}

				spanMin -= heightfieldBBMin.y;
				spanMax -= heightfieldBBMin.y;

				// Skip the span if it's completely outside the heightfield bounding box
				if ( spanMax < 0.0f )
					continue;
				if ( spanMin > by )
					continue;

				// Clamp the span to the heightfield bounding box
				if ( spanMin < 0.0f )
					spanMin = 0;
				if ( spanMax > by )
					spanMax = by;

				// Snap the span to the heightfield height grid
				ushort spanMinCellIndex = (ushort)Math.Clamp( (int)MathF.Floor( spanMin * inverseCellHeight ), 0, Constants.SPAN_MAX_HEIGHT );
				ushort spanMaxCellIndex = (ushort)Math.Clamp( (int)MathF.Ceiling( spanMax * inverseCellHeight ), spanMinCellIndex + 1, Constants.SPAN_MAX_HEIGHT );

				heightfield.AddOrMergeSpan( x, z, spanMinCellIndex, spanMaxCellIndex, areaId, flagMergeThreshold );
			}
		}
	}

	/// <summary>
	/// Rasterizes triangles to the heightfield using vertex and index arrays
	/// </summary>
	public static void RasterizeTriangles( Span<Vector3> verts, Span<int> tris, Span<int> triAreaIds,
										 Heightfield heightfield, int flagMergeThreshold )
	{
		var numTris = tris.Length / 3;

		// Rasterize the triangles
		float inverseCellSize = 1.0f / heightfield.CellSize;
		float inverseCellHeight = 1.0f / heightfield.CellHeight;

		for ( int triIndex = 0; triIndex < numTris; ++triIndex )
		{
			Vector3 v0 = verts[tris[triIndex * 3 + 0]];
			Vector3 v1 = verts[tris[triIndex * 3 + 1]];
			Vector3 v2 = verts[tris[triIndex * 3 + 2]];

			RasterizeTri( v0, v1, v2, triAreaIds[triIndex], heightfield,
							heightfield.BMin, heightfield.BMax, heightfield.CellSize,
							inverseCellSize, inverseCellHeight, flagMergeThreshold );
		}
	}
}
