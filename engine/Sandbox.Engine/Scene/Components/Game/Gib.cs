namespace Sandbox;

/// <summary>
/// A gib is a prop that is treated slightly different. It will fade out after a certain amount of time.
/// </summary>
[Expose]
[Title( "Gib" )]
[Category( "Game" )]
[Icon( "broken_image" )]
public class Gib : Prop
{
	public float FadeTime { get; set; }

	protected override void OnEnabled()
	{
		base.OnEnabled();

		if ( FadeTime > 0 && !Scene.IsEditor )
		{
			_ = RunGib();
		}
	}

	async Task RunGib()
	{
		await Task.DelaySeconds( FadeTime + Random.Shared.Float( 0, 2.0f ) );

		if ( !IsValid )
			return;

		var modelComponent = Components.Get<ModelRenderer>();
		if ( modelComponent is not null )
		{
			for ( float f = modelComponent.Tint.a; f > 0.0f; f -= Time.Delta )
			{
				modelComponent.Tint = modelComponent.Tint.WithAlpha( f );
				await Task.Frame();
			}
		}

		GameObject.Destroy();
	}
}
