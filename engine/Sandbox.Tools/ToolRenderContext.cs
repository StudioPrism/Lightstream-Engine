using Sandbox;

namespace Editor
{
	/// <summary>
	/// Renders basic stuff for tool views
	/// </summary>
	public static class ToolRender
	{
		internal static CToolRenderContext native;

		internal static void AssertValid( [System.Runtime.CompilerServices.CallerMemberName] string memberName = "" )
		{
			if ( native.IsValid ) return;

			throw new System.Exception( $"ToolRender.{memberName} must be called within a tool render context" );
		}

		// public bool IsScreenSpace => native.IsScreenSpace();
		// public bool IsWireframe => native.IsWireframe();
		// public bool Is3D => native.Is3D();
		// public bool IsOrthographic => native.IsOrthographic();

		public static bool IsActiveView
		{
			get
			{
				AssertValid();
				return native.IsActiveView();
			}
		}

		public static void DrawScreenText( string text, Vector2 pos, Color color )
		{
			AssertValid();
			native.DrawScreenText( text, pos, color );
		}

		public static void DrawWorldSpaceText( string text, Vector3 pos, Vector2 pixelOffset2D, Color color, float minZoomLevelToRender )
		{
			AssertValid();
			native.DrawWorldSpaceText( text, pos, pixelOffset2D, color.ToColor32(), minZoomLevelToRender );
		}

		public static void DrawLine( Vector3 start, Vector3 end, Color startColor, Color endColor )
		{
			AssertValid();
			native.DrawLine( start, end, startColor.ToColor32(), endColor.ToColor32() );
		}

		public static void DrawLine( Vector3 start, Vector3 end, Color color )
		{
			AssertValid();
			native.DrawLine( start, end, color.ToColor32(), color.ToColor32() );
		}

		public static void DrawBox( Vector3 mins, Vector3 maxs, Color color )
		{
			AssertValid();
			native.DrawBox( mins, maxs, color.ToColor32() );
		}

		public static void Draw2DRectangleFilled( Vector2 topLeft, Vector2 bottomRight, Color color )
		{
			native.Draw2DRectangleFilled( topLeft, bottomRight, color.ToColor32() );
		}

		public static void Draw2DRectangleOutlined( Vector2 topLeft, Vector2 bottomRight, Color color )
		{
			AssertValid();
			native.Draw2DRectangleOutline( topLeft, bottomRight, color.ToColor32() );
		}

		public static void Draw2DCircle( Vector2 center, float radius, int segments, Color color )
		{
			AssertValid();
			native.Draw2DCircle( center, radius, segments, color.ToColor32() );
		}

		public static void Draw2DCross( Vector2 topLeft, Vector2 bottomRight, Color color )
		{
			AssertValid();
			native.Draw2DCross( topLeft, bottomRight, color.ToColor32() );
		}

		public static void Draw2DRectangleTextured( Vector2 topLeft, Vector2 bottomRight, Texture texture, bool alpha = true, bool srgb = true )
		{
			AssertValid();
			native.Draw2DRectangleTextured( topLeft, bottomRight, texture.native, alpha, srgb );
		}
	}
}
