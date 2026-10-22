using System.Runtime.CompilerServices;

namespace Sandbox.Navigation.Generation;

[SkipHotload]
internal static class SpanFilter
{
	/// <summary>
	/// Applies (in-order per span):
	///  1. Low-hanging obstacle promotion
	///  2. Low-ceiling (clearance) rejection
	///  3. Ledge / steep slope rejection
	/// Single pass over the heightfield to improve cache locality.
	/// </summary>
	public static void Filter( int walkableHeight, int walkableClimb, Heightfield heightfield )
	{
		int xSize = heightfield.Width;
		int zSize = heightfield.Height;

		for ( int z = 0; z < zSize; ++z )
		{
			for ( int x = 0; x < xSize; ++x )
			{
				int columnIndex = x + z * xSize;
				var column = heightfield.GetColumn( columnIndex );
				if ( column.Length == 0 )
					continue;

				bool prevWasWalkable = false;
				int prevArea = Constants.NULL_AREA;
				int prevMax = 0;

				Span<SpanData> nEast = (x + 1 < xSize) ? heightfield.GetColumn( (x + 1) + z * xSize ) : default;
				Span<SpanData> nSouth = (z + 1 < zSize) ? heightfield.GetColumn( x + (z + 1) * xSize ) : default;
				Span<SpanData> nWest = (x - 1 >= 0) ? heightfield.GetColumn( (x - 1) + z * xSize ) : default;
				Span<SpanData> nNorth = (z - 1 >= 0) ? heightfield.GetColumn( x + (z - 1) * xSize ) : default;

				for ( int spanIndex = 0; spanIndex < column.Length; spanIndex++ )
				{
					SpanData span = column[spanIndex];

					// 1. Low-hanging promotion (uses ORIGINAL walkable state)
					bool wasWalkable = span.Area != Constants.NULL_AREA;
					if ( !wasWalkable && prevWasWalkable && span.MaxY - prevMax <= walkableClimb )
					{
						column[spanIndex].Area = prevArea;
					}

					// Update previous using the *original* walkable state, not mutated result.
					prevWasWalkable = wasWalkable;
					prevMax = span.MaxY;
					prevArea = span.Area;

					if ( span.Area == Constants.NULL_AREA )
						continue;

					int floor = span.MaxY;
					int ceilingIndex = spanIndex + 1;
					int ceiling = (ceilingIndex < column.Length) ? column[ceilingIndex].MinY : Constants.SPAN_MAX_HEIGHT;

					// 2. Clearance
					if ( !HasClearance( floor, ceiling, walkableHeight ) )
					{
						column[spanIndex].Area = Constants.NULL_AREA;
						continue;
					}

					// 3. Ledge / steep slope
					if ( IsLedge( floor, ceiling, walkableHeight, walkableClimb, nEast, nSouth, nWest, nNorth ) )
					{
						column[spanIndex].Area = Constants.NULL_AREA;
					}
				}
			}
		}
	}

	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	private static bool HasClearance( int floor, int ceiling, int walkableHeight ) => ceiling - floor >= walkableHeight;

	// Branch-minimized ledge test (ref-based helper instead of nested local function).
	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	private static bool IsLedge(
		int floor,
		int ceiling,
		int walkableHeight,
		int walkableClimb,
		Span<SpanData> nEast,
		Span<SpanData> nSouth,
		Span<SpanData> nWest,
		Span<SpanData> nNorth )
	{
		int lowestNeighborFloorDiff = Constants.SPAN_MAX_HEIGHT;
		int lowestTraversable = floor;
		int highestTraversable = floor;
		int isLedge = 0;

		ProcessNeighbor( nEast, floor, ceiling, walkableHeight, walkableClimb, ref isLedge, ref lowestNeighborFloorDiff, ref lowestTraversable, ref highestTraversable );
		ProcessNeighbor( nSouth, floor, ceiling, walkableHeight, walkableClimb, ref isLedge, ref lowestNeighborFloorDiff, ref lowestTraversable, ref highestTraversable );
		ProcessNeighbor( nWest, floor, ceiling, walkableHeight, walkableClimb, ref isLedge, ref lowestNeighborFloorDiff, ref lowestTraversable, ref highestTraversable );
		ProcessNeighbor( nNorth, floor, ceiling, walkableHeight, walkableClimb, ref isLedge, ref lowestNeighborFloorDiff, ref lowestTraversable, ref highestTraversable );

		int spreadFlag = (highestTraversable - lowestTraversable > walkableClimb) ? 1 : 0;
		int dropFlag = (lowestNeighborFloorDiff < -walkableClimb) ? 1 : 0;

		return (isLedge | spreadFlag | dropFlag) != 0;
	}

	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	private static void ProcessNeighbor(
		Span<SpanData> neighbor,
		int floor,
		int ceiling,
		int walkableHeight,
		int walkableClimb,
		ref int isLedge,
		ref int lowestNeighborFloorDiff,
		ref int lowestTraversable,
		ref int highestTraversable )
	{
		if ( isLedge != 0 )
			return;

		if ( neighbor.IsEmpty )
		{
			isLedge = 1;
			return;
		}

		int baseUnder = neighbor[0].MinY;
		if ( (Math.Min( ceiling, baseUnder ) - floor) >= walkableHeight )
		{
			isLedge = 1;
			return;
		}

		int length = neighbor.Length;
		for ( int i = 0; i < length; i++ )
		{
			SpanData ns = neighbor[i];
			int nFloor = ns.MaxY;
			int nCeiling = (i + 1 < length) ? neighbor[i + 1].MinY : Constants.SPAN_MAX_HEIGHT;

			int overlapHeight = Math.Min( ceiling, nCeiling ) - Math.Max( floor, nFloor );
			if ( overlapHeight < walkableHeight )
				continue;

			int diff = nFloor - floor;
			if ( diff < lowestNeighborFloorDiff )
				lowestNeighborFloorDiff = diff;

			if ( diff < -walkableClimb )
			{
				isLedge = 1;
				return;
			}

			int absDiff = Math.Abs( diff );
			if ( absDiff <= walkableClimb )
			{
				if ( nFloor < lowestTraversable ) lowestTraversable = nFloor;
				if ( nFloor > highestTraversable ) highestTraversable = nFloor;
			}
		}
	}
}
