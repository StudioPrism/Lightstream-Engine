using System.Buffers;

namespace Sandbox.Navigation.Generation;

[SkipHotload]
internal static class AreaFilter
{
	/// <summary>
	/// Erodes the walkable area within the heightfield by the specified radius.
	/// </summary>
	/// <param name="erosionRadius">The radius of erosion in voxels</param>
	/// <param name="compactHeightfield">The compact heightfield to erode</param>
	/// <returns>True if the operation completed successfully</returns>
	public static void ErodeWalkableArea( int erosionRadius, CompactHeightfield compactHeightfield )
	{
		int xSize = compactHeightfield.Width;
		int zSize = compactHeightfield.Height;
		int zStride = xSize; // For readability

		using var pooledDistanceToBoundary = new PooledSpan<byte>( compactHeightfield.SpanCount * 2 );
		Span<byte> distanceToBoundary = pooledDistanceToBoundary.Span;
		distanceToBoundary.Fill( 0xFF );

		// Mark boundary cells
		for ( int z = 0; z < zSize; ++z )
		{
			for ( int x = 0; x < xSize; ++x )
			{
				CompactCell cell = compactHeightfield.Cells[x + z * zStride];
				for ( int spanIndex = cell.Index, maxSpanIndex = (cell.Index + cell.Count);
					spanIndex < maxSpanIndex; ++spanIndex )
				{
					if ( compactHeightfield.Areas[spanIndex] == Constants.NULL_AREA )
					{
						distanceToBoundary[spanIndex] = 0;
						continue;
					}

					CompactSpan span = compactHeightfield.Spans[spanIndex];

					// Check that there is a non-null adjacent span in each of the 4 cardinal directions
					int neighborCount = 0;
					for ( int direction = 0; direction < 4; ++direction )
					{
						int neighborConnection = Utils.GetCon( span, direction );
						if ( neighborConnection == Constants.NOT_CONNECTED )
						{
							break;
						}

						int neighborX = x + Utils.GetDirOffsetX( direction );
						int neighborZ = z + Utils.GetDirOffsetZ( direction );
						int neighborSpanIndex = compactHeightfield.Cells[neighborX + neighborZ * zStride].Index + neighborConnection;

						if ( compactHeightfield.Areas[neighborSpanIndex] == Constants.NULL_AREA )
						{
							break;
						}

						neighborCount++;
					}

					// At least one missing neighbor, so this is a boundary cell
					if ( neighborCount != 4 )
					{
						distanceToBoundary[spanIndex] = 0;
					}
				}
			}
		}

		// Pass 1
		for ( int z = 0; z < zSize; ++z )
		{
			for ( int x = 0; x < xSize; ++x )
			{
				CompactCell cell = compactHeightfield.Cells[x + z * zStride];
				int maxSpanIndex = (cell.Index + cell.Count);
				for ( int spanIndex = cell.Index; spanIndex < maxSpanIndex; ++spanIndex )
				{
					CompactSpan span = compactHeightfield.Spans[spanIndex];

					if ( Utils.GetCon( span, 0 ) != Constants.NOT_CONNECTED )
					{
						// (-1,0)
						int aX = x + Utils.GetDirOffsetX( 0 );
						int aZ = z + Utils.GetDirOffsetZ( 0 );
						int aIndex = (int)compactHeightfield.Cells[aX + aZ * xSize].Index + Utils.GetCon( span, 0 );
						CompactSpan aSpan = compactHeightfield.Spans[aIndex];
						byte newDistance = (byte)Math.Min( (int)distanceToBoundary[aIndex] + 2, 255 );
						if ( newDistance < distanceToBoundary[spanIndex] )
						{
							distanceToBoundary[spanIndex] = newDistance;
						}

						// (-1,-1)
						if ( Utils.GetCon( aSpan, 3 ) != Constants.NOT_CONNECTED )
						{
							int bX = aX + Utils.GetDirOffsetX( 3 );
							int bZ = aZ + Utils.GetDirOffsetZ( 3 );
							int bIndex = (int)compactHeightfield.Cells[bX + bZ * xSize].Index + Utils.GetCon( aSpan, 3 );
							newDistance = (byte)Math.Min( (int)distanceToBoundary[bIndex] + 3, 255 );
							if ( newDistance < distanceToBoundary[spanIndex] )
							{
								distanceToBoundary[spanIndex] = newDistance;
							}
						}
					}

					if ( Utils.GetCon( span, 3 ) != Constants.NOT_CONNECTED )
					{
						// (0,-1)
						int aX = x + Utils.GetDirOffsetX( 3 );
						int aZ = z + Utils.GetDirOffsetZ( 3 );
						int aIndex = (int)compactHeightfield.Cells[aX + aZ * xSize].Index + Utils.GetCon( span, 3 );
						CompactSpan aSpan = compactHeightfield.Spans[aIndex];
						byte newDistance = (byte)Math.Min( (int)distanceToBoundary[aIndex] + 2, 255 );
						if ( newDistance < distanceToBoundary[spanIndex] )
						{
							distanceToBoundary[spanIndex] = newDistance;
						}

						// (1,-1)
						if ( Utils.GetCon( aSpan, 2 ) != Constants.NOT_CONNECTED )
						{
							int bX = aX + Utils.GetDirOffsetX( 2 );
							int bZ = aZ + Utils.GetDirOffsetZ( 2 );
							int bIndex = (int)compactHeightfield.Cells[bX + bZ * xSize].Index + Utils.GetCon( aSpan, 2 );
							newDistance = (byte)Math.Min( (int)distanceToBoundary[bIndex] + 3, 255 );
							if ( newDistance < distanceToBoundary[spanIndex] )
							{
								distanceToBoundary[spanIndex] = newDistance;
							}
						}
					}
				}
			}
		}

		// Pass 2
		for ( int z = zSize - 1; z >= 0; --z )
		{
			for ( int x = xSize - 1; x >= 0; --x )
			{
				CompactCell cell = compactHeightfield.Cells[x + z * zStride];
				int maxSpanIndex = (int)(cell.Index + cell.Count);
				for ( int spanIndex = (int)cell.Index; spanIndex < maxSpanIndex; ++spanIndex )
				{
					CompactSpan span = compactHeightfield.Spans[spanIndex];

					if ( Utils.GetCon( span, 2 ) != Constants.NOT_CONNECTED )
					{
						// (1,0)
						int aX = x + Utils.GetDirOffsetX( 2 );
						int aZ = z + Utils.GetDirOffsetZ( 2 );
						int aIndex = (int)compactHeightfield.Cells[aX + aZ * xSize].Index + Utils.GetCon( span, 2 );
						CompactSpan aSpan = compactHeightfield.Spans[aIndex];
						byte newDistance = (byte)Math.Min( (int)distanceToBoundary[aIndex] + 2, 255 );
						if ( newDistance < distanceToBoundary[spanIndex] )
						{
							distanceToBoundary[spanIndex] = newDistance;
						}

						// (1,1)
						if ( Utils.GetCon( aSpan, 1 ) != Constants.NOT_CONNECTED )
						{
							int bX = aX + Utils.GetDirOffsetX( 1 );
							int bZ = aZ + Utils.GetDirOffsetZ( 1 );
							int bIndex = (int)compactHeightfield.Cells[bX + bZ * xSize].Index + Utils.GetCon( aSpan, 1 );
							newDistance = (byte)Math.Min( (int)distanceToBoundary[bIndex] + 3, 255 );
							if ( newDistance < distanceToBoundary[spanIndex] )
							{
								distanceToBoundary[spanIndex] = newDistance;
							}
						}
					}

					if ( Utils.GetCon( span, 1 ) != Constants.NOT_CONNECTED )
					{
						// (0,1)
						int aX = x + Utils.GetDirOffsetX( 1 );
						int aZ = z + Utils.GetDirOffsetZ( 1 );
						int aIndex = (int)compactHeightfield.Cells[aX + aZ * xSize].Index + Utils.GetCon( span, 1 );
						CompactSpan aSpan = compactHeightfield.Spans[aIndex];
						byte newDistance = (byte)Math.Min( (int)distanceToBoundary[aIndex] + 2, 255 );
						if ( newDistance < distanceToBoundary[spanIndex] )
						{
							distanceToBoundary[spanIndex] = newDistance;
						}

						// (-1,1)
						if ( Utils.GetCon( aSpan, 0 ) != Constants.NOT_CONNECTED )
						{
							int bX = aX + Utils.GetDirOffsetX( 0 );
							int bZ = aZ + Utils.GetDirOffsetZ( 0 );
							int bIndex = (int)compactHeightfield.Cells[bX + bZ * xSize].Index + Utils.GetCon( aSpan, 0 );
							newDistance = (byte)Math.Min( (int)distanceToBoundary[bIndex] + 3, 255 );
							if ( newDistance < distanceToBoundary[spanIndex] )
							{
								distanceToBoundary[spanIndex] = newDistance;
							}
						}
					}
				}
			}
		}

		// Mark non-walkable areas based on distance
		byte minBoundaryDistance = (byte)(erosionRadius * 2);
		for ( int spanIndex = 0; spanIndex < compactHeightfield.SpanCount; ++spanIndex )
		{
			if ( distanceToBoundary[spanIndex] < minBoundaryDistance )
			{
				compactHeightfield.Areas[spanIndex] = Constants.NULL_AREA;
			}
		}
	}

	/// <summary>
	/// Applies a median filter to walkable area types (based on area id), removing noise.
	/// </summary>
	/// <param name="compactHeightfield">The compact heightfield to filter</param>
	/// <returns>True if the operation completed successfully</returns>
	public static bool MedianFilterWalkableArea( CompactHeightfield compactHeightfield )
	{
		int xSize = compactHeightfield.Width;
		int zSize = compactHeightfield.Height;
		int zStride = xSize; // For readability

		using var pooledAreas = new PooledSpan<int>( compactHeightfield.SpanCount );
		Span<int> areas = pooledAreas.Span;
		areas.Fill( Constants.NULL_AREA );

		Span<int> neighborAreas = stackalloc int[9];

		// Process each cell
		for ( int z = 0; z < zSize; ++z )
		{
			for ( int x = 0; x < xSize; ++x )
			{
				CompactCell cell = compactHeightfield.Cells[x + z * zStride];
				int maxSpanIndex = (int)(cell.Index + cell.Count);

				for ( int spanIndex = (int)cell.Index; spanIndex < maxSpanIndex; ++spanIndex )
				{
					CompactSpan span = compactHeightfield.Spans[spanIndex];

					if ( compactHeightfield.Areas[spanIndex] == Constants.NULL_AREA )
					{
						areas[spanIndex] = compactHeightfield.Areas[spanIndex];
						continue;
					}

					// Create neighborhood samples for median filter
					for ( int neighborIndex = 0; neighborIndex < 9; ++neighborIndex )
					{
						neighborAreas[neighborIndex] = compactHeightfield.Areas[spanIndex];
					}

					// Check each direction
					for ( int dir = 0; dir < 4; ++dir )
					{
						if ( Utils.GetCon( span, dir ) == Constants.NOT_CONNECTED )
							continue;

						int aX = x + Utils.GetDirOffsetX( dir );
						int aZ = z + Utils.GetDirOffsetZ( dir );
						int aIndex = (int)compactHeightfield.Cells[aX + aZ * zStride].Index + Utils.GetCon( span, dir );

						if ( compactHeightfield.Areas[aIndex] != Constants.NULL_AREA )
						{
							neighborAreas[dir * 2 + 0] = compactHeightfield.Areas[aIndex];
						}

						CompactSpan aSpan = compactHeightfield.Spans[aIndex];
						int dir2 = (dir + 1) & 0x3;
						int neighborConnection2 = Utils.GetCon( aSpan, dir2 );

						if ( neighborConnection2 != Constants.NOT_CONNECTED )
						{
							int bX = aX + Utils.GetDirOffsetX( dir2 );
							int bZ = aZ + Utils.GetDirOffsetZ( dir2 );
							int bIndex = (int)compactHeightfield.Cells[bX + bZ * zStride].Index + neighborConnection2;

							if ( compactHeightfield.Areas[bIndex] != Constants.NULL_AREA )
							{
								neighborAreas[dir * 2 + 1] = compactHeightfield.Areas[bIndex];
							}
						}
					}

					// Sort neighborhood and pick the median value
					neighborAreas.Sort();
					areas[spanIndex] = neighborAreas[4]; // Middle value (median)
				}
			}
		}

		// Copy the results back to the heightfield
		areas.CopyTo( compactHeightfield.Areas );

		return true;
	}
}
