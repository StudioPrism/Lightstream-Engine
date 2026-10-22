namespace Sandbox;

/// <summary>
/// Weld two physics objects together
/// </summary>
[Expose]
[Title( "Fixed Joint" )]
[Category( "Physics" )]
[Icon( "join_inner" )]
[EditorHandle( "materials/gizmo/pinned.png" )]
public sealed class FixedJoint : Joint
{
	[Property, Title( "Frequency" ), Group( "Linear" )]
	public float LinearFrequency
	{
		get;
		set
		{
			if ( value == field )
				return;

			field = value;

			if ( _joint.IsValid() )
			{
				_joint.SpringLinear = new PhysicsSpring( field, LinearDamping );
				_joint.WakeBodies();
			}
		}
	} = 10;

	[Property, Title( "Damping" ), Group( "Linear" )]
	public float LinearDamping
	{
		get;
		set
		{
			if ( value == field )
				return;

			field = value;

			if ( _joint.IsValid() )
			{
				_joint.SpringLinear = new PhysicsSpring( LinearFrequency, field );
				_joint.WakeBodies();
			}
		}
	} = 1;

	[Property, Title( "Frequency" ), Group( "Angular" )]
	public float AngularFrequency
	{
		get;
		set
		{
			if ( value == field )
				return;

			field = value;

			if ( _joint.IsValid() )
			{
				_joint.SpringAngular = new PhysicsSpring( field, AngularDamping );
				_joint.WakeBodies();
			}
		}
	} = 10;

	[Property, Title( "Damping" ), Group( "Angular" )]
	public float AngularDamping
	{
		get;
		set
		{
			if ( value == field )
				return;

			field = value;

			if ( _joint.IsValid() )
			{
				_joint.SpringAngular = new PhysicsSpring( AngularFrequency, field );
				_joint.WakeBodies();
			}
		}
	} = 1;

	Physics.FixedJoint _joint;

	protected override PhysicsJoint CreateJoint( PhysicsPoint point1, PhysicsPoint point2 )
	{
		var localFrame1 = LocalFrame1;
		var localFrame2 = LocalFrame2;

		if ( Attachment == AttachmentMode.Auto )
		{
			// Anchor point is the midpoint between the two world-space attachment points.
			var anchor = 0.5f * (point1.Transform.Position + point2.Transform.Position);

			// Convert anchor to body1's local space.
			localFrame1 = new Transform( point1.Body.Transform.PointToLocal( anchor ) );

			// Convert anchor to body2's local space and apply the rotation offset from body2 to body1,
			// so the joint preserves their initial relative orientation.
			localFrame2 = new Transform( point2.Body.Transform.PointToLocal( anchor ), point2.Body.Rotation.Conjugate * point1.Body.Rotation );
		}

		if ( !Scene.IsEditor )
		{
			LocalFrame1 = localFrame1;
			LocalFrame2 = localFrame2;

			Attachment = AttachmentMode.LocalFrames;
		}

		point1 = new PhysicsPoint( point1.Body, localFrame1.Position, localFrame1.Rotation );
		point2 = new PhysicsPoint( point2.Body, localFrame2.Position, localFrame2.Rotation );

		_joint = PhysicsJoint.CreateFixed( point1, point2 );
		_joint.SpringLinear = new PhysicsSpring( LinearFrequency, LinearDamping );
		_joint.SpringAngular = new PhysicsSpring( AngularFrequency, AngularDamping );

		_joint.WakeBodies();

		return _joint;
	}
}
