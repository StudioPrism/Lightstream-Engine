using System.Runtime.CompilerServices;

namespace Sandbox.Navigation.Generation;

[SkipHotload]
internal static class Utils
{
	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	public static bool VEqual2D( Span<int> a, Span<int> b )
	{
		return a[0] == b[0] && a[2] == b[2];
	}

	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	public static bool Left2D( Span<int> a, Span<int> b, Span<int> c )
	{
		return Area2D( a, b, c ) < 0;
	}

	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	public static bool LeftOn2D( Span<int> a, Span<int> b, Span<int> c )
	{
		return Area2D( a, b, c ) <= 0;
	}

	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	public static bool Collinear2D( Span<int> a, Span<int> b, Span<int> c )
	{
		return Area2D( a, b, c ) == 0;
	}

	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	public static int Area2D( Span<int> a, Span<int> b, Span<int> c )
	{
		return (b[0] - a[0]) * (c[2] - a[2]) - (c[0] - a[0]) * (b[2] - a[2]);
	}

	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	public static bool Xorb( bool x, bool y )
	{
		return !x ^ !y;
	}

	public static bool IntersectProp2D( Span<int> a, Span<int> b, Span<int> c, Span<int> d )
	{
		// Eliminate improper cases
		if ( Collinear2D( a, b, c ) || Collinear2D( a, b, d ) ||
			Collinear2D( c, d, a ) || Collinear2D( c, d, b ) )
			return false;

		return Xorb( Left2D( a, b, c ), Left2D( a, b, d ) ) && Xorb( Left2D( c, d, a ), Left2D( c, d, b ) );
	}

	public static bool Between2D( Span<int> a, Span<int> b, Span<int> c )
	{
		if ( !Collinear2D( a, b, c ) )
			return false;

		// If ab not vertical, check betweenness on x; else on z
		if ( a[0] != b[0] )
			return ((a[0] <= c[0]) && (c[0] <= b[0])) || ((a[0] >= c[0]) && (c[0] >= b[0]));
		else
			return ((a[2] <= c[2]) && (c[2] <= b[2])) || ((a[2] >= c[2]) && (c[2] >= b[2]));
	}

	public static bool Intersect2D( Span<int> a, Span<int> b, Span<int> c, Span<int> d )
	{
		if ( IntersectProp2D( a, b, c, d ) )
			return true;
		else if ( Between2D( a, b, c ) || Between2D( a, b, d ) ||
				 Between2D( c, d, a ) || Between2D( c, d, b ) )
			return true;
		else
			return false;
	}

	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	public static int Prev( int i, int n )
	{
		return i - 1 >= 0 ? i - 1 : n - 1;
	}

	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	public static int Next( int i, int n )
	{
		return i + 1 < n ? i + 1 : 0;
	}

	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	public static int GetCon( CompactSpan span, int dir )
	{
		int shift = dir * 6;
		return (int)((span.Con >> shift) & 0x3Fu);
	}

	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	public static int GetDirOffsetX( int dir )
	{
		switch ( dir & 3 )
		{
			case 0: return -1;
			case 1: return 0;
			case 2: return 1;
			default: return 0;
		}
	}

	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	public static int GetDirOffsetZ( int dir )
	{
		switch ( dir & 3 )
		{
			case 0: return 0;
			case 1: return 1;
			case 2: return 0;
			default: return -1;
		}
	}

	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	public static void SetCon( ref CompactSpan span, int dir, int i )
	{
		int shift = dir * 6;
		span.Con = ((span.Con & ~(0x3f << shift)) | ((i & 0x3f) << shift));
	}

	public static bool DiagonalIe( int i, int j, int n, Span<int> verts, Span<int> indices )
	{
		Span<int> d0 = verts.Slice( (indices[i] & 0x0fffffff) * 4, 4 );
		Span<int> d1 = verts.Slice( (indices[j] & 0x0fffffff) * 4, 4 );

		// For each edge (k,k+1) of P
		for ( int k = 0; k < n; k++ )
		{
			int k1 = Next( k, n );
			// Skip edges incident to i or j
			if ( !((k == i) || (k1 == i) || (k == j) || (k1 == j)) )
			{
				Span<int> p0 = verts.Slice( (indices[k] & 0x0fffffff) * 4, 4 );
				Span<int> p1 = verts.Slice( (indices[k1] & 0x0fffffff) * 4, 4 );

				if ( VEqual2D( d0, p0 ) || VEqual2D( d1, p0 ) || VEqual2D( d0, p1 ) || VEqual2D( d1, p1 ) )
					continue;

				if ( Intersect2D( d0, d1, p0, p1 ) )
					return false;
			}
		}
		return true;
	}

	public static bool InCone2D( int i, int j, int n, Span<int> verts, Span<int> indices )
	{
		Span<int> pi = verts.Slice( (indices[i] & 0x0fffffff) * 4, 4 );
		Span<int> pj = verts.Slice( (indices[j] & 0x0fffffff) * 4, 4 );
		Span<int> pi1 = verts.Slice( (indices[Next( i, n )] & 0x0fffffff) * 4, 4 );
		Span<int> pin1 = verts.Slice( (indices[Prev( i, n )] & 0x0fffffff) * 4, 4 );

		// If P[i] is a convex vertex [ i+1 left or on (i-1,i) ]
		if ( LeftOn2D( pin1, pi, pi1 ) )
			return Left2D( pi, pj, pin1 ) && Left2D( pj, pi, pi1 );

		// Else P[i] is reflex
		return !(LeftOn2D( pi, pj, pi1 ) && LeftOn2D( pj, pi, pin1 ));
	}

	public static bool Diagonal( int i, int j, int n, Span<int> verts, Span<int> indices )
	{
		return InCone2D( i, j, n, verts, indices ) && DiagonalIe( i, j, n, verts, indices );
	}

	public static bool DiagonalIeLoose( int i, int j, int n, Span<int> verts, Span<int> indices )
	{
		Span<int> d0 = verts.Slice( (indices[i] & 0x0fffffff) * 4, 4 );
		Span<int> d1 = verts.Slice( (indices[j] & 0x0fffffff) * 4, 4 );

		// For each edge (k,k+1) of P
		for ( int k = 0; k < n; k++ )
		{
			int k1 = Next( k, n );
			// Skip edges incident to i or j
			if ( !((k == i) || (k1 == i) || (k == j) || (k1 == j)) )
			{
				Span<int> p0 = verts.Slice( (indices[k] & 0x0fffffff) * 4, 4 );
				Span<int> p1 = verts.Slice( (indices[k1] & 0x0fffffff) * 4, 4 );

				if ( VEqual2D( d0, p0 ) || VEqual2D( d1, p0 ) || VEqual2D( d0, p1 ) || VEqual2D( d1, p1 ) )
					continue;

				if ( IntersectProp2D( d0, d1, p0, p1 ) )
					return false;
			}
		}
		return true;
	}

	public static bool InConeLoose( int i, int j, int n, Span<int> verts, Span<int> indices )
	{
		Span<int> pi = verts.Slice( (indices[i] & 0x0fffffff) * 4, 4 );
		Span<int> pj = verts.Slice( (indices[j] & 0x0fffffff) * 4, 4 );
		Span<int> pi1 = verts.Slice( (indices[Next( i, n )] & 0x0fffffff) * 4, 4 );
		Span<int> pin1 = verts.Slice( (indices[Prev( i, n )] & 0x0fffffff) * 4, 4 );

		// If P[i] is a convex vertex [ i+1 left or on (i-1,i) ]
		if ( LeftOn2D( pin1, pi, pi1 ) )
			return LeftOn2D( pi, pj, pin1 ) && LeftOn2D( pj, pi, pi1 );

		// Else P[i] is reflex
		return !(LeftOn2D( pi, pj, pi1 ) && LeftOn2D( pj, pi, pin1 ));
	}

	public static bool DiagonalLoose( int i, int j, int n, Span<int> verts, Span<int> indices )
	{
		return InConeLoose( i, j, n, verts, indices ) && DiagonalIeLoose( i, j, n, verts, indices );
	}
}
