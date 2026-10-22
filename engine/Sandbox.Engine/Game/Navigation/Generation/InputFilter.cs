using System.Runtime.CompilerServices;

namespace Sandbox.Navigation.Generation;

internal static class InputFilter
{
	/// <summary>
	/// Marks walkable triangles based on slope angle
	/// </summary>
	public static void MarkWalkableTriangles( float walkableSlopeAngle,
		ReadOnlySpan<Vector3> verts, ReadOnlySpan<int> tris, Span<int> triAreaIds )
	{
		triAreaIds.Fill( Constants.NULL_AREA );

		float walkableThr = MathF.Cos( walkableSlopeAngle / 180.0f * MathF.PI );

		for ( int i = 0; i < tris.Length / 3; i++ )
		{
			int a = tris[i * 3 + 0];
			int b = tris[i * 3 + 1];
			int c = tris[i * 3 + 2];

			CalcTriNormal( verts[a], verts[b], verts[c], out Vector3 normal );

			// Check if the face is walkable based on slope
			if ( normal.y > walkableThr )
			{
				triAreaIds[i] = Constants.WALKABLE_AREA;
			}
		}
	}

	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	private static void CalcTriNormal( in Vector3 v0, in Vector3 v1, in Vector3 v2, out Vector3 normal )
	{
		Vector3 e0 = v1 - v0;
		Vector3 e1 = v2 - v0;
		normal = Vector3.Cross( e0, e1 ).Normal;
	}
}
