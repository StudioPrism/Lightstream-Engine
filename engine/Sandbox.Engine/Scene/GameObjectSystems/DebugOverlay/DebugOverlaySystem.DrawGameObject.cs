
namespace Sandbox;

public partial class DebugOverlaySystem
{
	/// <summary>
	/// Draw a GameObject in the world
	/// </summary>
	public void GameObject( GameObject go, Color color = new Color(), float duration = 0, Transform transform = default, bool overlay = false, bool castShadows = true, Material materialOveride = default )
	{
		if ( transform == default ) transform = Transform.Zero;
		if ( color == default ) color = Color.White;

		foreach ( var renderer in go.GetComponentsInChildren<Renderer>( true ) )
		{
			var localtx = go.WorldTransform.ToLocal( renderer.GameObject.WorldTransform );
			var tx = transform.ToWorld( localtx );

			if ( renderer is ModelRenderer mr )
			{
				AddRendererer( mr, color, duration, tx, overlay, castShadows, materialOveride );
			}

			// TODO - do we want to render others types?
			// animated shit isn't gonna work here because it's created and destroyed same frame
		}
	}

	private void AddRendererer( ModelRenderer mr, Color color, float duration, Transform transform, bool overlay, bool castShadows, Material materialOveride )
	{
		var shadows = castShadows && (mr.RenderType != ModelRenderer.ShadowRenderType.Off);
		var model = mr.Model ?? Sandbox.Model.Load( "models/dev/box.vmdl" );

		Model( model, color * mr.Tint, duration, transform, overlay, shadows, materialOveride: materialOveride );
	}
}
