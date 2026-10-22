using System.Runtime.CompilerServices;

namespace Sandbox.Navigation.Generation;

internal partial class CompactHeightfield : IDisposable
{
	/// <summary>
	/// Clamps bounds to the heightfield grid, converting from world to grid coordinates
	/// </summary>
	/// <returns>False if the bounds are completely outside the heightfield</returns>
	public bool ClampBoundsToHeightField( in BBox worldBounds, out BBox gridBounds )
	{
		float invCellSize = 1.0f / CellSize;
		float invCellHeight = 1.0f / CellHeight;

		// Convert world bounds to grid coordinates
		gridBounds = new BBox(
			new Vector3(
				MathF.Floor( (worldBounds.Mins.x - BMin.x) * invCellSize ),
				MathF.Floor( (worldBounds.Mins.y - BMin.y) * invCellHeight ),
				MathF.Floor( (worldBounds.Mins.z - BMin.z) * invCellSize )
			),
			new Vector3(
				MathF.Floor( (worldBounds.Maxs.x - BMin.x) * invCellSize ),
				MathF.Floor( (worldBounds.Maxs.y - BMin.y) * invCellHeight ),
				MathF.Floor( (worldBounds.Maxs.z - BMin.z) * invCellSize )
			)
		);

		// Early-out if box is outside the bounds
		if ( gridBounds.Maxs.x < 0 || gridBounds.Mins.x >= Width ||
			gridBounds.Maxs.z < 0 || gridBounds.Mins.z >= Height )
		{
			return false;
		}

		// Clamp bounds to grid
		gridBounds = new BBox(
			new Vector3(
				Math.Max( 0, gridBounds.Mins.x ),
				gridBounds.Mins.y,
				Math.Max( 0, gridBounds.Mins.z )
			),
			new Vector3(
				Math.Min( Width - 1, gridBounds.Maxs.x ),
				gridBounds.Maxs.y,
				Math.Min( Height - 1, gridBounds.Maxs.z )
			)
		);

		return true;
	}

	/// <summary>
	/// Checks if a point is contained within a polygon
	/// </summary>
	private static bool PointInPoly( in Vector3 point, ReadOnlySpan<Vector3> vertices )
	{
		bool inside = false;
		int numVerts = vertices.Length;

		for ( int i = 0, j = numVerts - 1; i < numVerts; j = i++ )
		{
			Vector3 vi = vertices[i];
			Vector3 vj = vertices[j];

			if ( ((vi.z > point.z) != (vj.z > point.z)) &&
				(point.x < (vj.x - vi.x) * (point.z - vi.z) / (vj.z - vi.z) + vi.x) )
			{
				inside = !inside;
			}
		}

		return inside;
	}

	/// <summary>
	/// Marks areas in the heightfield that are within a convex polygon
	/// </summary>
	public void MarkConvexPolyArea( ReadOnlySpan<Vector3> vertices, BBox bounds, int areaId )
	{
		if ( !ClampBoundsToHeightField( bounds, out BBox clampedBounds ) )
			return;

		int zStride = Width;

		for ( int z = (int)clampedBounds.Mins.z; z <= (int)clampedBounds.Maxs.z; ++z )
		{
			for ( int x = (int)clampedBounds.Mins.x; x <= (int)clampedBounds.Maxs.x; ++x )
			{
				CompactCell cell = Cells[x + z * zStride];
				int maxSpanIndex = (int)(cell.Index + cell.Count);

				for ( int spanIndex = (int)cell.Index; spanIndex < maxSpanIndex; ++spanIndex )
				{
					// Skip if the span has been removed
					if ( Areas[spanIndex] == Constants.NULL_AREA )
						continue;

					CompactSpan span = Spans[spanIndex];

					// Skip if y extents don't overlap
					if ( span.StartY < clampedBounds.Mins.y || span.StartY > clampedBounds.Maxs.y )
						continue;

					Vector3 point = new Vector3(
						BMin.x + (x + 0.5f) * CellSize,
						0,
						BMin.z + (z + 0.5f) * CellSize
					);

					if ( PointInPoly( point, vertices ) )
					{
						if ( areaId == Constants.NULL_AREA || areaId > Areas[spanIndex] ) Areas[spanIndex] = areaId;
					}
				}
			}
		}
	}

	/// <summary>
	/// Marks areas in the heightfield that are within an oriented box
	/// </summary>
	public void MarkBoxArea( BBox localBox, Transform transform, BBox worldBounds, int areaId )
	{
		if ( !ClampBoundsToHeightField( worldBounds, out BBox clampedBounds ) )
			return;

		int zStride = Width;

		// Calculate box center in its local space
		Vector3 boxCenter = localBox.Center;
		Vector3 extents = localBox.Extents;

		for ( int z = (int)clampedBounds.Mins.z; z <= (int)clampedBounds.Maxs.z; ++z )
		{
			for ( int x = (int)clampedBounds.Mins.x; x <= (int)clampedBounds.Maxs.x; ++x )
			{
				CompactCell cell = Cells[x + z * zStride];
				int maxSpanIndex = cell.Index + cell.Count;

				for ( int spanIndex = cell.Index; spanIndex < maxSpanIndex; ++spanIndex )
				{
					CompactSpan span = Spans[spanIndex];

					// Skip if the span has been removed
					if ( Areas[spanIndex] == Constants.NULL_AREA )
						continue;

					// Skip if y extents don't overlap
					if ( span.StartY < clampedBounds.Mins.y || span.StartY > clampedBounds.Maxs.y )
						continue;

					float cellX = BMin.x + (x + 0.5f) * CellSize;
					float cellY = BMin.y + (span.StartY + 0.5f) * CellHeight;
					float cellZ = BMin.z + (z + 0.5f) * CellSize;

					Vector3 coords = new Vector3( cellX, cellY, cellZ );

					// Transform point into box local space
					Vector3 coordsInBoxSpace = transform.PointToLocal( coords );

					// Calculate distance from box center
					Vector3 distanceFromCenter = coordsInBoxSpace - boxCenter;

					// Skip if outside box extents
					if ( MathF.Abs( distanceFromCenter.x ) > extents.x ||
						MathF.Abs( distanceFromCenter.y ) > extents.y ||
						MathF.Abs( distanceFromCenter.z ) > extents.z )
						continue;

					// Mark the span
					if ( areaId == Constants.NULL_AREA || areaId > Areas[spanIndex] ) Areas[spanIndex] = areaId;
				}
			}
		}
	}

	// Signed distance functions for primitives
	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	private static float SdCapsule( in Vector3 p, ref Capsule c )
	{
		Vector3 ba = c.CenterB - c.CenterA;
		Vector3 pa = p - c.CenterA;
		float baba = Vector3.Dot( ba, ba );
		float paba = Vector3.Dot( pa, ba );
		float h = Math.Clamp( paba / baba, 0f, 1f );
		return ((pa - ba * h).Length - c.Radius);
	}

	/// <summary>
	/// Marks areas in the heightfield that are within a capsule
	/// </summary>
	public void MarkCapsuleArea( Capsule localCapsule, Transform transform, BBox worldBounds, int areaId )
	{
		if ( !ClampBoundsToHeightField( worldBounds, out BBox clampedBounds ) )
			return;

		int zStride = Width;

		for ( int z = (int)clampedBounds.Mins.z; z <= (int)clampedBounds.Maxs.z; ++z )
		{
			for ( int x = (int)clampedBounds.Mins.x; x <= (int)clampedBounds.Maxs.x; ++x )
			{
				CompactCell cell = Cells[x + z * zStride];
				int maxSpanIndex = (int)(cell.Index + cell.Count);

				for ( int spanIndex = (int)cell.Index; spanIndex < maxSpanIndex; ++spanIndex )
				{
					CompactSpan span = Spans[spanIndex];

					// Skip if the span has been removed
					if ( Areas[spanIndex] == Constants.NULL_AREA )
						continue;

					float cellX = BMin.x + (x + 0.5f) * CellSize;
					float cellY = BMin.y + (span.StartY + 0.5f) * CellHeight;
					float cellZ = BMin.z + (z + 0.5f) * CellSize;

					Vector3 coords = new Vector3( cellX, cellY, cellZ );

					// Transform point into capsule local space
					Vector3 localCoords = transform.PointToLocal( coords );

					// Skip if outside capsule
					float dist = SdCapsule( localCoords, ref localCapsule );
					if ( dist > 0.0f )
						continue;

					// Mark the span
					if ( areaId == Constants.NULL_AREA || areaId > Areas[spanIndex] ) Areas[spanIndex] = areaId;
				}
			}
		}
	}

	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	private static float SdSphere( in Vector3 p, ref Sphere sphere )
	{
		return (sphere.Center - p).Length - sphere.Radius;
	}

	/// <summary>
	/// Marks areas in the heightfield that are within a sphere
	/// </summary>
	public void MarkSphereArea( Sphere localSphere, Transform transform, BBox worldBounds, int areaId )
	{
		if ( !ClampBoundsToHeightField( worldBounds, out BBox clampedBounds ) )
			return;

		int zStride = Width;

		for ( int z = (int)clampedBounds.Mins.z; z <= (int)clampedBounds.Maxs.z; ++z )
		{
			for ( int x = (int)clampedBounds.Mins.x; x <= (int)clampedBounds.Maxs.x; ++x )
			{
				CompactCell cell = Cells[x + z * zStride];
				int maxSpanIndex = (int)(cell.Index + cell.Count);

				for ( int spanIndex = (int)cell.Index; spanIndex < maxSpanIndex; ++spanIndex )
				{
					CompactSpan span = Spans[spanIndex];

					// Skip if the span has been removed
					if ( Areas[spanIndex] == Constants.NULL_AREA )
						continue;

					float cellX = BMin.x + (x + 0.5f) * CellSize;
					float cellY = BMin.y + (span.StartY + 0.5f) * CellHeight;
					float cellZ = BMin.z + (z + 0.5f) * CellSize;

					Vector3 coords = new Vector3( cellX, cellY, cellZ );

					// Transform point into sphere local space
					Vector3 localCoords = transform.PointToLocal( coords );

					// Skip if outside sphere
					float dist = SdSphere( localCoords, ref localSphere );
					if ( dist > 0.0f )
						continue;

					// Mark the span
					if ( areaId == Constants.NULL_AREA || areaId > Areas[spanIndex] ) Areas[spanIndex] = areaId;
				}
			}
		}
	}
}
