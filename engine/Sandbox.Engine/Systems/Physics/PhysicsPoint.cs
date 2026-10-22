namespace Sandbox.Physics;

/// <summary>
/// Used to describe a point on a physics body. This is used for things like joints where
/// you want to pass in just a body, or sometimes you want to pass in a body with a specific
/// location and rotation to attach to.
/// </summary>
public struct PhysicsPoint
{
	/// <summary>
	/// The physics body this point is attached to.
	/// </summary>
	public PhysicsBody Body;

	/// <summary>
	/// Position offset from the body's position.
	/// </summary>
	public Vector3 LocalPosition;

	/// <summary>
	/// Rotation offset from the body's position.
	/// </summary>
	public Rotation LocalRotation;

	/// <summary>
	/// A transform relative to <see cref="Body"/>, containing <see cref="LocalPosition"/> and <see cref="LocalRotation"/> with scale of 1.
	/// </summary>
	public Transform LocalTransform
	{
		readonly get => new( LocalPosition, LocalRotation );
		set
		{
			LocalPosition = value.Position;
			LocalRotation = value.Rotation;
		}
	}

	/// <summary>
	/// Transform of this point in world space.
	/// </summary>
	public Transform Transform => Body.Transform.ToWorld( LocalTransform );

	public PhysicsPoint( PhysicsBody body, Vector3? localPosition = default, Rotation? localRotation = default )
	{
		Body = body;
		LocalPosition = localPosition ?? Vector3.Zero;
		LocalRotation = localRotation ?? Rotation.Identity;
	}

	public static implicit operator PhysicsPoint( PhysicsBody body )
	{
		return new PhysicsPoint { Body = body, LocalPosition = default, LocalRotation = Rotation.Identity };
	}

	/// <summary>
	/// Describe an attachment using a position/rotation local to the body
	/// </summary>
	public static PhysicsPoint Local( PhysicsBody body, Vector3? localPosition = default, Rotation? localRotation = default )
	{
		return new PhysicsPoint( body, localPosition, localRotation );
	}

	/// <summary>
	/// Describe an attachment using a position/rotation from the world
	/// </summary>
	public static PhysicsPoint World( PhysicsBody body, Vector3? worldPosition = default, Rotation? worldRotation = default )
	{
		var localTx = body.Transform.ToLocal( new Transform( worldPosition ?? body.Transform.Position, worldRotation ?? body.Transform.Rotation ) );

		if ( !worldPosition.HasValue ) localTx.Position = Vector3.Zero;
		if ( !worldRotation.HasValue ) localTx.Rotation = Rotation.Identity;

		return new PhysicsPoint( body, localTx.Position, localTx.Rotation );
	}
}
