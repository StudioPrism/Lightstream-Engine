using Sandbox;

public partial struct Vector3
{
	/// <summary>
	/// Smoothly move towards the target vector
	/// </summary>
	public static Vector3 SmoothDamp( in Vector3 current, in Vector3 target, ref Vector3 velocity, float smoothTime, float deltaTime )
	{
		// If smoothing time is zero, directly jump to target (independent of timestep)
		if ( smoothTime <= 0.0f )
		{
			return target;
		}

		// If timestep is zero, stay at current position
		if ( deltaTime <= 0.0f )
		{
			return current;
		}

		// Implicit integration of critically damped spring
		var omega = MathF.Tau / smoothTime;
		var denom = (1.0f + omega * deltaTime);
		velocity = (velocity - (omega * omega) * deltaTime * (current - target)) / (denom * denom);

		return current + velocity * deltaTime;
	}

	/// <summary>
	/// Springly move towards the target vector
	/// </summary>
	public static Vector3 SpringDamp( in Vector3 current, in Vector3 target, ref Vector3 velocity, float deltaTime, float frequency = 2.0f, float damping = 0.5f )
	{
		var displacement = current - target;

		(displacement, velocity) = SpringDamper.FromDamping( frequency, damping )
			.Simulate( displacement, velocity, deltaTime );

		return displacement + target;
	}

	[Obsolete( "Use the overload without the 'smoothTime' parameter instead, as it is no longer needed." )]
	public static Vector3 SpringDamp( in Vector3 current, in Vector3 target, ref Vector3 velocity, float smoothTime, float deltaTime, float frequency = 2.0f, float damping = 0.5f )
	{
		return SpringDamp( in current, in target, ref velocity, deltaTime, frequency, damping );
	}
}
