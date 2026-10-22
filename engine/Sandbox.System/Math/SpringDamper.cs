namespace Sandbox;

/// <summary>
/// Models a unit mass attached to a spring and a damper, with a particular
/// <see cref="Frequency"/> and <see cref="DecayRate"/>.
/// </summary>
internal readonly struct SpringDamper
{
	/// <summary>
	/// Create a model of a damped spring from a <paramref name="frequency"/> and <paramref name="damping"/> per oscillation.
	/// </summary>
	/// <param name="frequency">How many times the spring oscillates per second.</param>
	/// <param name="damping">How much damping to apply each oscillation, as with the legacy <see cref="Vector3.SpringDamp(in Vector3, in Vector3, ref Vector3, float, float, float)"/>.</param>
	public static SpringDamper FromDamping( float frequency = 2f, float damping = 0.5f ) =>
		new( frequency, damping * frequency * MathF.PI * 2f );

	/// <summary>
	/// Create a critically damped model with a given <paramref name="smoothingTime"/>, for movement that doesn't oscillate but smoothly
	/// settles to the target value.
	/// </summary>
	/// <param name="smoothingTime">Time until the spring has settled.</param>
	public static SpringDamper FromSmoothingTime( float smoothingTime ) =>
		smoothingTime <= 0f
			? FromDamping( 1f, float.PositiveInfinity )
			: FromDamping( 1f / smoothingTime, 1f );

	/// <summary>
	/// How many times the spring oscillates per second.
	/// </summary>
	public float Frequency { get; }

	/// <summary>
	/// Exponential decay constant λ, higher values decay faster.
	/// </summary>
	public float DecayRate { get; }

	/// <summary>
	/// Angular frequency (radians per second), with decay accounted for.
	/// </summary>
	private readonly float _omega;

	/// <summary>
	/// Models a unit mass attached to a spring and a damper, with a particular
	/// <paramref name="frequency"/> and <paramref name="decayRate"/>.
	/// </summary>
	public SpringDamper( float frequency, float decayRate )
	{
		Frequency = frequency;
		DecayRate = decayRate;

		// Natural angular frequency, without damping
		var omega0 = Frequency * MathF.PI * 2f;

		// Damped angular frequency
		_omega = MathF.Sqrt( Math.Max( 0f, omega0 * omega0 - DecayRate * DecayRate ) );
	}

	#region Simulate

	/// <summary>
	/// Simulate the evolution of a unit mass with given <paramref name="position"/> and <paramref name="velocity"/>
	/// being affected by this system, <paramref name="deltaTime"/> seconds into the future.
	/// </summary>
	/// <param name="position">Current displacement of the mass from the spring rest position.</param>
	/// <param name="velocity">Current velocity of the mass.</param>
	/// <param name="deltaTime">How far to simulate into the future.</param>
	public (float Position, float Velocity) Simulate( float position, float velocity, float deltaTime )
	{
		if ( deltaTime <= 0.0f ) return (position, velocity);
		if ( float.IsPositiveInfinity( DecayRate ) ) return (0f, 0f);

		// Correct for what our velocity would be without damping, so we can extrapolate the oscillation
		var velocityWithoutDecay = velocity + DecayRate * position;

		// Simulate spring without decay
		(position, velocityWithoutDecay) = SimulateOscillator( position, velocityWithoutDecay, deltaTime );

		// Apply exponential decay
		(position, velocity) = SimulateDecay( position, velocityWithoutDecay, deltaTime );

		return (position, velocity);
	}

	/// <inheritdoc cref="Simulate(float,float,float)"/>
	public (Vector2 Position, Vector2 Velocity) Simulate( Vector2 position, Vector2 velocity, float deltaTime )
	{
		(position.x, velocity.x) = Simulate( position.x, velocity.x, deltaTime );
		(position.y, velocity.y) = Simulate( position.y, velocity.y, deltaTime );

		return (position, velocity);
	}

	/// <inheritdoc cref="Simulate(float,float,float)"/>
	public (Vector3 Position, Vector3 Velocity) Simulate( Vector3 position, Vector3 velocity, float deltaTime )
	{
		(position.x, velocity.x) = Simulate( position.x, velocity.x, deltaTime );
		(position.y, velocity.y) = Simulate( position.y, velocity.y, deltaTime );
		(position.z, velocity.z) = Simulate( position.z, velocity.z, deltaTime );

		return (position, velocity);
	}

	#endregion

	#region Private

	private (float MaxPosition, float MaxVelocity, float Phase) FindOscillationParameters( float position, float velocity )
	{
		// Total energy (kinetic + potential) x 2, assuming unit mass
		var energy2 = velocity * velocity + _omega * _omega * position * position;

		// Snap to 0 if energy is super low, so we don't wobble forever
		if ( energy2 <= 0.001f ) return (0f, 0f, 0f);

		// Find maximum velocity by turning all energy into kinetic
		var vMax = MathF.Sqrt( energy2 );

		// Find maximum amplitude by turning all energy into potential
		var amplitude = vMax / _omega;

		// Where are we in the oscillation
		var phase = MathF.Atan2( -velocity, position * _omega );

		return (amplitude, vMax, phase);
	}

	private (float Position, float Velocity) SimulateOscillator( float position, float velocity, float deltaTime )
	{
		if ( _omega <= 0.0001f )
		{
			// We're not oscillating, just moving at constant velocity

			return (position + velocity * deltaTime, velocity);
		}

		// Work out where we are in the oscillation (the phase), and what its amplitude / max velocity is

		var (xMax, vMax, phase) = FindOscillationParameters( position, velocity );

		if ( xMax <= 0f )
		{
			// Fast path if we're at equilibrium

			return (0f, 0f);
		}

		// Project it into the future

		return (MathF.Cos( deltaTime * _omega + phase ) * xMax,
			-MathF.Sin( deltaTime * _omega + phase ) * vMax);
	}

	private (float Position, float Velocity) SimulateDecay( float position, float velocity, float deltaTime )
	{
		var scale = float.IsPositiveInfinity( DecayRate ) ? 0f : MathF.Exp( -deltaTime * DecayRate );

		// Apply exponential decay
		position *= scale;
		velocity *= scale;

		// Apply gradient of exponential decay
		velocity -= DecayRate * position;

		return (position, velocity);
	}

	#endregion
}
