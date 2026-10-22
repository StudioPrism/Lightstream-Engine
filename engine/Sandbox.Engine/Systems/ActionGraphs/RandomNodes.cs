using Facepunch.ActionGraphs;

namespace Sandbox.ActionGraphs
{
	internal static class RandomNodes
	{
		[ActionGraphNode( "random.int" ), Pure, Title( "Random Integer" ), Description( "Generates a random value between min (inclusive) and max (exclusive)." ), Category( "Math/Random" ), Icon( "casino" ), Tags( "common" )]
		public static int Int( int min = 0, int max = 100 )
		{
			return Random.Shared.Next( min, max );
		}

		[ActionGraphNode( "random.float" ), Pure, Title( "Random Float" ), Description( "Generates a random value between min and max." ), Category( "Math/Random" ), Icon( "casino" ), Tags( "common" )]
		public static float Float( float min = 0f, float max = 1f )
		{
			return min + Random.Shared.NextSingle() * (max - min);
		}

		[ActionGraphNode( "random.chance" ), Pure, Title( "Random Chance" ), Description( "Returns true with the given probability." ), Icon( "casino" ), Category( "Math/Random" )]
		public static bool Chance( float probability = 0.5f )
		{
			return Random.Shared.NextSingle() <= probability;
		}

		/// <summary>
		/// Returns a color with a random hue.
		/// </summary>
		/// <param name="saturation">Saturation of the generated color, from 0 to 1.</param>
		/// <param name="lightness">Lightness of the generated color, from 0 to 1.</param>
		/// <returns></returns>
		[ActionGraphNode( "random.color" ), Pure, Title( "Random Color" ), Icon( "casino" ), Category( "Graphics/Color" )]
		public static Color Color( [Facepunch.ActionGraphs.Property] float saturation = 1f, [Facepunch.ActionGraphs.Property] float lightness = 1f )
		{
			var hue = Random.Shared.Float( 0f, 360f );

			return new ColorHsv( hue, saturation, lightness );
		}

		[Obsolete, ActionGraphNode( "random.vector2" ), Pure, Title( "Random Vector2" ), Description( " Uniformly samples a 2D position from all points with distance at most 1 from the origin." ), Icon( "casino" ), Category( "Random" )]
		public static Vector2 Vector2()
		{
			return global::Vector2.Random;
		}

		[Obsolete, ActionGraphNode( "random.vector3" ), Pure, Title( "Random Vector3" ), Description( " Uniformly samples a 3D position from all points with distance at most 1 from the origin." ), Icon( "casino" ), Category( "Random" )]
		public static Vector3 Vector3()
		{
			return global::Vector3.Random;
		}
	}
}
