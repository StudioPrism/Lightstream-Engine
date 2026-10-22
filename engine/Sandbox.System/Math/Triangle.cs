using System.Runtime.InteropServices;
namespace Sandbox;

[StructLayout( LayoutKind.Sequential )]
public struct Triangle : System.IEquatable<Triangle>
{
	public Vector3 A;
	public Vector3 B;
	public Vector3 C;

	public float Perimeter => A.Distance( B ) + B.Distance( C ) + C.Distance( A );
	public float Area => Vector3.Cross( B - A, C - A ).Length * 0.5f;
	public bool IsRight => (B - A).Dot( C - A ) == 0.0f;
	public Vector3 Normal => Vector3.Cross( B - A, C - A ).Normal;

	public Triangle( Vector3 a, Vector3 b, Vector3 c )
	{
		A = a;
		B = b;
		C = c;
	}

	public override string ToString()
	{
		return $"{A} / {B} / {C}";
	}

	public readonly Vector3 ClosestPoint( in Vector3 P )
	{
		// voronoi

		Vector3 e0 = B - A;
		Vector3 e1 = C - A;
		Vector3 p0 = P - A;

		// voronoi region of A
		float d1 = e0.Dot( p0 );
		float d2 = e1.Dot( p0 );
		if ( d1 <= 0.0f && d2 <= 0.0f )
			return A;

		// voronoi region of B
		Vector3 p1 = P - B;
		float d3 = e0.Dot( p1 );
		float d4 = e1.Dot( p1 );
		if ( d3 >= 0.0f && d4 <= d3 )
			return B;

		// voronoi region of e0 (A-B)
		float ve2 = d1 * d4 - d3 * d2;
		if ( ve2 <= 0.0f && d1 >= 0.0f && d3 <= 0.0f )
		{
			float v = d1 / (d1 - d3);
			return A + v * e0;
		}

		// voronoi region of C
		Vector3 p2 = P - C;
		float d5 = e0.Dot( p2 );
		float d6 = e1.Dot( p2 );
		if ( d6 >= 0.0f && d5 <= d6 )
			return C;

		// voronoi region of e2
		float ve1 = d5 * d2 - d1 * d6;
		if ( ve1 <= 0.0f && d2 >= 0.0f && d6 <= 0.0f )
		{
			float w = d2 / (d2 - d6);
			return A + w * e1;
		}

		// voronoi region on e1
		float ve0 = d3 * d6 - d5 * d4;
		if ( ve0 <= 0.0f && (d4 - d3) >= 0.0f && (d5 - d6) >= 0.0f )
		{
			float w = (d4 - d3) / ((d4 - d3) + (d5 - d6));
			return B + w * (C - B);
		}

		// voronoi region of ABC triangle
		{
			float denom = 1.0f / (ve0 + ve1 + ve2);
			float v = ve1 * denom;
			float w = ve2 * denom;
			return A + e0 * v + e1 * w;
		}
	}

	#region equality
	public static bool operator ==( Triangle left, Triangle right ) => left.Equals( right );
	public static bool operator !=( Triangle left, Triangle right ) => !(left == right);
	public override bool Equals( object obj ) => obj is Triangle o && Equals( o );
	public readonly bool Equals( Triangle o ) => (A) == (o.A) && (B) == (o.B) && (C) == (o.C);
	public readonly override int GetHashCode() => HashCode.Combine( A, B, C );
	#endregion
}
