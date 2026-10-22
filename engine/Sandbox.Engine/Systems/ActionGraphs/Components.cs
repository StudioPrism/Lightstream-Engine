
using static Sandbox.Component;

namespace Sandbox.ActionGraphs;

/// <summary>
/// A component which allows you to use action in all the usual functions.
/// </summary>
[Title( "Actions Invoker" ), Group( "Actions" ), Icon( "bolt" )]
public sealed class ActionsInvoker : Component
{
	[Property] public Action OnEnabledAction { get; set; }

	protected override void OnEnabled()
	{
		OnEnabledAction?.InvokeWithWarning();
	}

	[Property] public Action OnUpdateAction { get; set; }

	protected override void OnUpdate()
	{
		OnUpdateAction?.InvokeWithWarning();
	}

	[Property] public Action OnFixedUpdateAction { get; set; }

	protected override void OnFixedUpdate()
	{
		OnFixedUpdateAction?.InvokeWithWarning();
	}

	[Property] public Action OnDisabledAction { get; set; }

	protected override void OnDisabled()
	{
		OnDisabledAction?.InvokeWithWarning();
	}

	[Property] public Action OnDestroyAction { get; set; }

	protected override void OnDestroy()
	{
		OnDestroyAction?.InvokeWithWarning();
	}
}

/// <summary>
/// A component that only provides actions to implement with an Action Graph.
/// </summary>
public interface IActionComponent
{
}

/// <summary>
/// These should not exist
/// </summary>
[Obsolete( $"Please use the action properties in {nameof( Component )}." )]
public abstract class SimpleActionComponent : Component, IActionComponent
{
	/// <summary>
	/// ActionGraph to run when the relevant event occurs.
	/// </summary>
	[Property]
	public Action Action { get; set; }
}

/// <inheritdoc cref="Component.OnAwake"/>
[Obsolete( $"Please use \"{nameof( Component )}.{nameof( OnComponentStart )}\"." )]
[Title( "Awake" ), Group( "Actions" ), Icon( "light_mode" )]
public class AwakeActionComponent : SimpleActionComponent
{
	protected override void OnAwake()
	{
		Action?.Invoke();
	}
}

/// <inheritdoc cref="Component.OnStart"/>
[Obsolete( $"Please use \"{nameof( Component )}.{nameof( OnComponentStart )}\"." )]
[Title( "Start" ), Group( "Actions" ), Icon( "sports_score" )]
public class StartActionComponent : SimpleActionComponent
{
	protected override void OnStart()
	{
		Action?.Invoke();
	}
}

/// <inheritdoc cref="Component.OnEnabled"/>
[Obsolete( $"Please use \"{nameof( Component )}.{nameof( OnComponentEnabled )}\"." )]
[Title( "Enabled" ), Group( "Actions" ), Icon( "thumb_up" )]
public class EnabledActionComponent : SimpleActionComponent
{
	protected override void OnEnabled()
	{
		Action?.Invoke();
	}
}

/// <inheritdoc cref="Component.OnDisabled"/>
[Obsolete( $"Please use \"{nameof( Component )}.{nameof( OnComponentDisabled )}\"." )]
[Title( "Disabled" ), Group( "Actions" ), Icon( "thumb_down" )]
public class DisabledActionComponent : SimpleActionComponent
{
	protected override void OnDisabled()
	{
		Action?.Invoke();
	}
}

/// <inheritdoc cref="Component.OnUpdate"/>
[Obsolete( $"Please use \"{nameof( Component )}.{nameof( OnComponentUpdate )}\"." )]
[Title( "Update" ), Group( "Actions" ), Icon( "update" )]
public class UpdateActionComponent : SimpleActionComponent
{
	protected override void OnUpdate()
	{
		Action?.Invoke();
	}
}

/// <inheritdoc cref="Component.OnFixedUpdate"/>
[Obsolete( $"Please use \"{nameof( Component )}.{nameof( OnComponentFixedUpdate )}\"." )]
[Title( "FixedUpdate" ), Group( "Actions" ), Icon( "lock_clock" )]
public class FixedUpdateActionComponent : SimpleActionComponent
{
	protected override void OnFixedUpdate()
	{
		Action?.Invoke();
	}
}

/// <inheritdoc cref="Component.OnDestroy"/>
[Obsolete( $"Please use \"{nameof( Component )}.{nameof( OnComponentDestroy )}\"." )]
[Title( "Destroy" ), Group( "Actions" ), Icon( "delete" )]
public class DestroyActionComponent : SimpleActionComponent
{
	protected override void OnDestroy()
	{
		Action?.Invoke();
	}
}

/// <summary>
/// Reacts to collisions.
/// </summary>
[Obsolete( "TODO: We don't have a replacement for this yet." )]
[Title( "Collision" ), Group( "Actions" ), Icon( "minor_crash" )]
public class CollisionActionComponent : Component, ICollisionListener, IActionComponent
{
	public delegate void CollisionDelegate( Collision other );
	public delegate void CollisionStopDelegate( CollisionStop other );

	/// <inheritdoc cref="Component.ICollisionListener.OnCollisionStart"/>
	[Property]
	public CollisionDelegate CollisionStart { get; set; }

	/// <inheritdoc cref="Component.ICollisionListener.OnCollisionUpdate"/>
	[Property]
	public CollisionDelegate CollisionUpdate { get; set; }

	/// <inheritdoc cref="Component.ICollisionListener.OnCollisionStop"/>
	[Property]
	public CollisionStopDelegate CollisionStop { get; set; }

	void ICollisionListener.OnCollisionStart( Collision other )
	{
		CollisionStart?.Invoke( other );
	}

	void ICollisionListener.OnCollisionUpdate( Collision other )
	{
		CollisionUpdate?.Invoke( other );
	}

	void ICollisionListener.OnCollisionStop( CollisionStop other )
	{
		CollisionStop?.Invoke( other );
	}
}

/// <summary>
/// Reacts to collider triggers.
/// </summary>
[Obsolete( $"Please use \"{nameof( Collider )}.{nameof( Collider.OnTriggerEnter )}\" and \"{nameof( Collider )}.{nameof( Collider.OnTriggerExit )}\"." )]
[Title( "Trigger" ), Group( "Actions" ), Icon( "filter_center_focus" )]
public class TriggerActionComponent : Component, ITriggerListener, IActionComponent
{
	public delegate void TriggerDelegate( Collider other );

	/// <inheritdoc cref="Component.ITriggerListener.OnTriggerEnter(Collider)"/>
	[Property]
	public TriggerDelegate TriggerEnter { get; set; }

	/// <inheritdoc cref="Component.ITriggerListener.OnTriggerExit(Collider)"/>
	[Property]
	public TriggerDelegate TriggerExit { get; set; }

	void ITriggerListener.OnTriggerEnter( Collider other )
	{
		TriggerEnter?.Invoke( other );
	}

	void ITriggerListener.OnTriggerExit( Collider other )
	{
		TriggerExit?.Invoke( other );
	}
}
