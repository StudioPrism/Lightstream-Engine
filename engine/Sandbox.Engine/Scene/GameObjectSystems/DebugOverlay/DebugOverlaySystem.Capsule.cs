namespace Sandbox;

public partial class DebugOverlaySystem
{
	/// <summary>
	/// Draw a wireframe capsule, simple cylinder with 2 hemispheres.
	/// </summary>
	public void Capsule( Capsule capsule, Color color = default, float duration = 0, Transform transform = default, bool overlay = false, int segments = 12 )
	{
		if ( transform == default ) transform = Transform.Zero;
		if ( color == default ) color = Color.White;

		var diff = capsule.CenterB - capsule.CenterA;
		if ( diff.IsNearZeroLength )
		{
			Sphere( new Sphere( capsule.CenterA, capsule.Radius ), color, duration, transform, overlay );
			return;
		}

		var sceneObject = new SceneDynamicObject( Scene.SceneWorld )
		{
			Transform = transform,
			Material = LineMaterial,
			RenderLayer = overlay ? SceneRenderLayer.OverlayWithoutDepth : SceneRenderLayer.OverlayWithDepth
		};

		sceneObject.Flags.CastShadows = false;
		sceneObject.Init( Graphics.PrimitiveType.Lines );

		var rotation = Rotation.LookAt( diff );

		CylinderWire( sceneObject, capsule.CenterA, capsule.CenterB, capsule.Radius, capsule.Radius, color, segments, rotation );
		CircleArc( sceneObject, capsule.CenterA, rotation.Left, rotation.Forward, capsule.Radius, 90, 180, color, segments );
		CircleArc( sceneObject, capsule.CenterB, rotation.Left, rotation.Forward, capsule.Radius, 270, 180, color, segments );
		CircleArc( sceneObject, capsule.CenterA, rotation.Up, rotation.Forward, capsule.Radius, 90, 180, color, segments );
		CircleArc( sceneObject, capsule.CenterB, rotation.Up, rotation.Forward, capsule.Radius, 270, 180, color, segments );

		Add( duration, sceneObject );
	}

	static void CylinderWire( SceneDynamicObject so, Vector3 start, Vector3 end, float startRadius, float endRadius, Color color, int segments, Rotation rotation )
	{
		var left = rotation.Left;
		var up = rotation.Up;
		var angle = 0f;
		var step = MathF.Tau / segments;
		var verts = new Vector3[segments * 2];

		for ( var i = 0; i < segments; angle += step, i++ )
		{
			var offset = left * MathF.Sin( angle ) + up * MathF.Cos( angle );
			verts[i] = start + startRadius * offset;
			verts[i + segments] = end + endRadius * offset;
		}

		for ( var i = 0; i < segments; i++ )
		{
			var prev = i == 0 ? segments - 1 : i - 1;
			so.AddVertex( new Vertex( verts[prev], color ) );
			so.AddVertex( new Vertex( verts[i], color ) );
			so.AddVertex( new Vertex( verts[prev + segments], color ) );
			so.AddVertex( new Vertex( verts[i + segments], color ) );

			var interval = segments / Math.Min( 4, segments );
			if ( i % interval == 0 )
			{
				so.AddVertex( new Vertex( verts[i], color ) );
				so.AddVertex( new Vertex( verts[i + segments], color ) );
			}
		}
	}

	static void CircleArc( SceneDynamicObject so, Vector3 center, Vector3 forward, Vector3 up, float radius, float startDegrees, float sweepDegrees, Color color, int sections )
	{
		var right = Vector3.Cross( forward, up ).Normal;
		right *= radius;
		up *= radius;

		var start = startDegrees.DegreeToRadian();
		var sweep = sweepDegrees.DegreeToRadian();
		var prev = center + MathF.Sin( start ) * right + MathF.Cos( start ) * up;

		for ( var i = 0; i < sections; i++ )
		{
			var f = start + ((i + 1) / (float)sections) * sweep;
			var next = center + MathF.Sin( f ) * right + MathF.Cos( f ) * up;
			so.AddVertex( new Vertex( prev, color ) );
			so.AddVertex( new Vertex( next, color ) );
			prev = next;
		}
	}
}
