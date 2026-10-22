namespace Sandbox;

/// <summary>
/// Try to keep an object a set distance away from another object. Like a spring connecting two objects.
/// </summary>
[Expose]
[Title( "Spring Joint" )]
[Category( "Physics" )]
[Icon( "waves" )]
[EditorHandle( "materials/gizmo/spring.png" )]
public sealed class SpringJoint : Joint
{
	/// <summary>
	/// The stiffness of the spring
	/// </summary>
	[Property]
	public float Frequency
	{
		get;
		set
		{
			if ( value == field )
				return;

			field = value;

			if ( _joint.IsValid() )
			{
				_joint.SpringLinear = new PhysicsSpring( field, Damping, RestLength );
				_joint.WakeBodies();
			}
		}
	} = 5;

	/// <summary>
	/// The damping ratio of the spring, usually between 0 and 1
	/// </summary>
	[Property]
	public float Damping
	{
		get;
		set
		{
			if ( value == field )
				return;

			field = value;

			if ( _joint.IsValid() )
			{
				_joint.SpringLinear = new PhysicsSpring( Frequency, field, RestLength );
				_joint.WakeBodies();
			}
		}
	} = 0.7f;

	/// <summary>
	/// Minimum length it should be allowed to go
	/// </summary>
	[Property]
	public float MinLength
	{
		get;
		set
		{
			if ( value == field )
				return;

			field = value;

			if ( _joint.IsValid() )
			{
				_joint.MinLength = value;
				_joint.WakeBodies();
			}
		}
	} = 0;

	/// <summary>
	/// Maximum length it should be allowed to go
	/// </summary>
	[Property]
	public float MaxLength
	{
		get;
		set
		{
			if ( value == field )
				return;

			field = value;

			if ( _joint.IsValid() )
			{
				_joint.MaxLength = value;
				_joint.WakeBodies();
			}
		}
	} = 100;

	/// <summary>
	/// Length of the spring at rest.
	/// </summary>
	[Property]
	public float RestLength
	{
		get;
		set
		{
			if ( value == field )
				return;

			field = value;

			if ( _joint.IsValid() )
			{
				_joint.SpringLinear = new PhysicsSpring( Frequency, Damping, field );
				_joint.WakeBodies();
			}
		}
	} = 50;

	public enum SpringForceMode
	{
		Pull,
		Push,
		Both
	}

	private SpringForceMode forceMode = SpringForceMode.Both;

	/// <summary>
	/// Determines which way the spring applies force.
	/// Pull = only when stretched,
	/// Push = only when compressed,
	/// Both = acts in both directions.
	/// </summary>
	[Property]
	public SpringForceMode ForceMode
	{
		get => forceMode;
		set
		{
			if ( value == forceMode )
				return;

			forceMode = value;

			if ( _joint.IsValid() )
			{
				UpdateForce();

				_joint.WakeBodies();
			}
		}
	}

	Physics.SpringJoint _joint;

	protected override PhysicsJoint CreateJoint( PhysicsPoint point1, PhysicsPoint point2 )
	{
		var localFrame1 = LocalFrame1;
		var localFrame2 = LocalFrame2;

		if ( Attachment == AttachmentMode.Auto )
		{
			localFrame1 = point1.LocalTransform;
			localFrame2 = point2.LocalTransform;
		}

		if ( !Scene.IsEditor )
		{
			LocalFrame1 = localFrame1;
			LocalFrame2 = localFrame2;

			Attachment = AttachmentMode.LocalFrames;
		}

		point1.LocalTransform = localFrame1;
		point2.LocalTransform = localFrame2;

		_joint = PhysicsJoint.CreateSpring( point1, point2, MinLength, MaxLength );
		_joint.SpringLinear = new PhysicsSpring( Frequency, Damping, RestLength );

		UpdateForce();

		_joint.WakeBodies();

		return _joint;
	}

	private void UpdateForce()
	{
		switch ( forceMode )
		{
			case SpringForceMode.Pull:
				_joint.MinForce = float.MinValue;
				_joint.MaxForce = 0.0f;
				break;

			case SpringForceMode.Push:
				_joint.MinForce = 0.0f;
				_joint.MaxForce = float.MaxValue;
				break;

			case SpringForceMode.Both:
				_joint.MinForce = float.MinValue;
				_joint.MaxForce = float.MaxValue;
				break;
		}
	}
}
