
using Facepunch.ActionGraphs;

namespace Sandbox.ActionGraphs
{
	internal static class ConstantNodes
	{
		public const string ObsoleteMessage = "Use the properties panel to directly set constant values for inputs.";

		[Obsolete( ObsoleteMessage ), ActionGraphNode( "const.string" ), Pure, Category( "Constant" ), Title( "String" ), Description( "A constant piece of text." ), Icon( "format_quote" ), Tags( "common" )]
		public static string String( [Facepunch.ActionGraphs.Property] string name = null, [Facepunch.ActionGraphs.Property] string value = "" )
		{
			return value;
		}

		[Obsolete( ObsoleteMessage ), ActionGraphNode( "const.int32" ), Pure, Category( "Constant" ), Title( "Int" ), Description( "A constant 32-bit integer." ), Icon( "123" ), Tags( "common" )]
		public static int Int32( [Facepunch.ActionGraphs.Property] string name = null, [Facepunch.ActionGraphs.Property] int value = default )
		{
			return value;
		}

		[Obsolete( ObsoleteMessage ), ActionGraphNode( "const.float" ), Pure, Category( "Constant" ), Title( "Float" ), Description( "A constant 32-bit floating-point number." ), Icon( "tune" ), Tags( "common" )]
		public static float Float( [Facepunch.ActionGraphs.Property] string name = null, [Facepunch.ActionGraphs.Property] float value = default )
		{
			return value;
		}

		[Obsolete( ObsoleteMessage ), ActionGraphNode( "const.bool" ), Pure, Category( "Constant" ), Title( "Boolean" ), Description( "A constant true or false value." ), Icon( "check_box" ), Tags( "common" )]
		public static bool Boolean( [Facepunch.ActionGraphs.Property] string name = null, [Facepunch.ActionGraphs.Property] bool value = default )
		{
			return value;
		}

		[Obsolete( ObsoleteMessage ), ActionGraphNode( "const.vec2" ), Pure, Category( "Constant" ), Title( "Vector2" ), Description( "A constant 2D vector." ), Icon( "tune" ), Tags( "common" )]
		public static Vector2 Vector2( [Facepunch.ActionGraphs.Property] string name = null, [Facepunch.ActionGraphs.Property] Vector2 value = default )
		{
			return value;
		}

		[Obsolete( ObsoleteMessage ), ActionGraphNode( "const.vec3" ), Pure, Category( "Constant" ), Title( "Vector3" ), Description( "A constant 3D vector." ), Icon( "tune" ), Tags( "common" )]
		public static Vector3 Vector3( [Facepunch.ActionGraphs.Property] string name = null, [Facepunch.ActionGraphs.Property] Vector3 value = default )
		{
			return value;
		}

		[Obsolete( ObsoleteMessage ), ActionGraphNode( "const.rotation" ), Pure, Category( "Constant" ), Title( "Rotation" ), Description( "A constant rotation." ), Icon( "360" ), Tags( "common" )]
		public static Rotation Rotation( [Facepunch.ActionGraphs.Property] string name = null, [Facepunch.ActionGraphs.Property] Rotation value = default )
		{
			return value;
		}

		[Obsolete( ObsoleteMessage ), ActionGraphNode( "const.color" ), Pure, Category( "Constant" ), Title( "Color" ), Description( "A constant color." ), Icon( "palette" ), Tags( "common" )]
		public static Color Color( [Facepunch.ActionGraphs.Property] string name = null, [Facepunch.ActionGraphs.Property] Color value = default )
		{
			return value;
		}

		[Obsolete( ObsoleteMessage ), ActionGraphNode( "const.enum" ), Pure, Category( "Constant" ), Title( "Enum" ), Description( "A constant enum value." ), Icon( "format_list_numbered" ), Tags( "common" )]
		public static T Enum<T>( [Facepunch.ActionGraphs.Property] string name = null, [Facepunch.ActionGraphs.Property] T value = default )
			where T : Enum
		{
			return value;
		}

		[Obsolete( ObsoleteMessage ), ActionGraphNode( "op.null" ), Pure, Category( "Constant" ), Title( "Null" ), Description( "A null reference." ), Icon( "∅" ), Tags( "common" )]
		public static Null Null()
		{
			return default;
		}
	}
}
