namespace Sandbox.Physics;

/// <summary>
/// Spring related settings for joints such as <see cref="FixedJoint"/>.
/// </summary>
[Expose]
public record struct PhysicsSpring
{
	/// <summary>
	/// The stiffness of the spring
	/// </summary>
	public float Frequency { get; set; }

	/// <summary>
	/// The damping ratio of the spring, usually between 0 and 1
	/// </summary>
	public float Damping { get; set; }

	/// <summary>
	/// For weld joints only, maximum force. Not for breaking.
	/// </summary>
	public float Maximum { get; set; }

	public PhysicsSpring( float frequency = 0.0f, float damping = 0.0f, float maximum = -1.0f )
	{
		Frequency = frequency;
		Damping = damping;
		Maximum = maximum < 0.0f ? float.MaxValue : maximum;
	}

	public static implicit operator Vector3( PhysicsSpring s )
	{
		return new Vector3( s.Frequency, s.Damping, s.Maximum );
	}

	public static implicit operator PhysicsSpring( Vector3 s )
	{
		return new PhysicsSpring( s.x, s.y, s.z );
	}
}
