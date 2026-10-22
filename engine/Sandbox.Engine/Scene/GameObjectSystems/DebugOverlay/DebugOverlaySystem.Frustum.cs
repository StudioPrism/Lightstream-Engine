namespace Sandbox;

public partial class DebugOverlaySystem
{
	/// <summary>
	/// Draw a frustum
	/// </summary>
	public void Frustum( Frustum frustum, Color color = new Color(), float duration = 0, Transform transform = default, bool overlay = false )
	{
		if ( transform == default ) transform = Transform.Zero;
		if ( color == default ) color = Color.White;

		var c0 = frustum.GetCorner( 0 ) ?? Vector3.Zero;
		var c1 = frustum.GetCorner( 1 ) ?? Vector3.Zero;
		var c2 = frustum.GetCorner( 2 ) ?? Vector3.Zero;
		var c3 = frustum.GetCorner( 3 ) ?? Vector3.Zero;
		var c4 = frustum.GetCorner( 4 ) ?? Vector3.Zero;
		var c5 = frustum.GetCorner( 5 ) ?? Vector3.Zero;
		var c6 = frustum.GetCorner( 6 ) ?? Vector3.Zero;
		var c7 = frustum.GetCorner( 7 ) ?? Vector3.Zero;

		var so = new SceneDynamicObject( Scene.SceneWorld );
		so.Transform = transform;
		so.Material = LineMaterial;
		so.Flags.CastShadows = false;
		so.RenderLayer = overlay ? SceneRenderLayer.OverlayWithoutDepth : SceneRenderLayer.OverlayWithDepth;
		so.Init( Graphics.PrimitiveType.Lines );

		so.AddVertex( new Vertex( c0, color ) );
		so.AddVertex( new Vertex( c1, color ) );
		so.AddVertex( new Vertex( c1, color ) );
		so.AddVertex( new Vertex( c2, color ) );
		so.AddVertex( new Vertex( c2, color ) );
		so.AddVertex( new Vertex( c3, color ) );
		so.AddVertex( new Vertex( c3, color ) );
		so.AddVertex( new Vertex( c0, color ) );
		so.AddVertex( new Vertex( c4, color ) );
		so.AddVertex( new Vertex( c5, color ) );
		so.AddVertex( new Vertex( c5, color ) );
		so.AddVertex( new Vertex( c6, color ) );
		so.AddVertex( new Vertex( c6, color ) );
		so.AddVertex( new Vertex( c7, color ) );
		so.AddVertex( new Vertex( c7, color ) );
		so.AddVertex( new Vertex( c4, color ) );
		so.AddVertex( new Vertex( c0, color ) );
		so.AddVertex( new Vertex( c4, color ) );
		so.AddVertex( new Vertex( c1, color ) );
		so.AddVertex( new Vertex( c5, color ) );
		so.AddVertex( new Vertex( c2, color ) );
		so.AddVertex( new Vertex( c6, color ) );
		so.AddVertex( new Vertex( c3, color ) );
		so.AddVertex( new Vertex( c7, color ) );

		Add( duration, so );
	}
}
