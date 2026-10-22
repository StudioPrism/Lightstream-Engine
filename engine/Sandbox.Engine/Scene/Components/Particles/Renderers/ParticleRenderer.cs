namespace Sandbox;


/// <summary>
/// Renders a set of particles. Should be attached to a <see cref="ParticleEffect"/>.
/// </summary>
public abstract class ParticleRenderer : Renderer, Component.IHasBounds
{
	[RequireComponent]
	public ParticleEffect ParticleEffect { get; set; }

	protected override void OnEnabled()
	{
		if ( ParticleEffect.IsValid() )
		{
			ParticleEffect.OnParticleCreated += OnParticleCreatedInternal;
			ParticleEffect.OnPostStep += OnPostStepInternal;
		}
	}

	protected override void OnDisabled()
	{
		if ( ParticleEffect.IsValid() )
		{
			ParticleEffect.OnParticleCreated -= OnParticleCreatedInternal;
			ParticleEffect.OnPostStep -= OnPostStepInternal;
			ParticleEffect.OnControllerDisabled( this );
		}
	}

	void OnParticleCreatedInternal( Particle p )
	{
		try
		{
			OnParticleCreated( p );
		}
		catch ( System.Exception e )
		{
			Log.Warning( e );
		}
	}

	void OnPostStepInternal( float time )
	{
		try
		{
			OnPostStep( time );
		}
		catch ( System.Exception e )
		{
			Log.Warning( e );
		}
	}

	protected virtual void OnParticleCreated( Particle p )
	{
		// fill me in
	}

	internal virtual void OnPostStep( float time )
	{
		// fill me in
	}

	/// <summary>
	/// Return the bounds of this renderer in local space.
	/// </summary>
	protected virtual BBox GetLocalBounds()
	{
		if ( ParticleEffect.IsValid() )
		{
			var box = BBox.FromPositionAndSize( 0, 16 );

			foreach ( var p in ParticleEffect.Particles )
			{
				box = box.AddBBox( BBox.FromPositionAndSize( WorldTransform.PointToLocal( p.Position ), 4 ) );
			}

			return box;
		}

		return BBox.FromPositionAndSize( 0, 16 );
	}

	BBox Component.IHasBounds.LocalBounds => GetLocalBounds();
}
