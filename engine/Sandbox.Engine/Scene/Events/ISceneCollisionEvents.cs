namespace Sandbox;

/// <summary>
/// Listen to all collision events that happen during a physics step.
/// </summary>
public interface ISceneCollisionEvents
{
	/// <summary>
	/// Called when a collider/rigidbody starts touching another collider.
	/// </summary>
	void OnCollisionStart( Collision collision ) { }

	/// <summary>
	/// Called once per physics step for every collider being touched.
	/// </summary>
	void OnCollisionUpdate( Collision collision ) { }

	/// <summary>
	/// Called when a collider/rigidbody stops touching another collider.
	/// </summary>
	void OnCollisionStop( CollisionStop collision ) { }

	/// <summary>
	/// Called when a collider/rigidbody hits another collider, including repeated hits
	/// on the same shape while they are already touching.
	/// </summary>
	void OnCollisionHit( Collision collision ) { }
}
