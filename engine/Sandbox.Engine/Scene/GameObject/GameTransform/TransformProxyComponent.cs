namespace Sandbox;

/// <summary>
/// Help to implement a component that completely overrides the transform. This is useful for scenarios
/// where you will want to keep the local transform of a GameObject, but want to offset based on that 
/// for some reason.
/// Having multiple of these on one GameObject is not supported, and will result in weirdness.
/// </summary>
public abstract class TransformProxyComponent : Component
{
	protected override void OnEnabled()
	{
		Transform.Proxy = new ComponentProxy( this );
	}

	protected override void OnDisabled()
	{
		if ( Transform.Proxy is ComponentProxy cp && cp.Target == this )
		{
			Transform.Proxy = null;
		}
	}

	/// <summary>
	/// Override to provide the local transform
	/// </summary>
	public abstract Transform GetLocalTransform();

	/// <summary>
	/// Called when the local transform is being set
	/// </summary>
	public virtual void SetLocalTransform( in Transform value )
	{

	}

	/// <summary>
	/// Override to provide the world transform. The default implementation will calculate it using GetLocalTransform() based on the parent.
	/// </summary>
	public virtual Transform GetWorldTransform()
	{
		if ( GameObject?.Parent is GameObject parent )
		{
			return parent.WorldTransform.ToWorld( GetLocalTransform() );
		}

		return global::Transform.Zero;
	}

	/// <summary>
	/// Called when the world transform is being set
	/// </summary>
	public virtual void SetWorldTransform( Transform value )
	{

	}

	/// <summary>
	/// Tell our other components, and our children that our transform has changed. This will
	/// update things like Renderers to update their render positions.
	/// </summary>
	public void MarkTransformChanged()
	{
		Transform.TransformChanged();
	}
}

file class ComponentProxy : TransformProxy
{
	public TransformProxyComponent Target { get; }

	public ComponentProxy( TransformProxyComponent target )
	{
		Target = target;
	}

	public override Transform GetLocalTransform() => Target.GetLocalTransform();

	public override void SetLocalTransform( in Transform value ) => Target.SetLocalTransform( value );

	public override Transform GetWorldTransform() => Target.GetWorldTransform();

	public override void SetWorldTransform( Transform value ) => Target.SetWorldTransform( value );
}
