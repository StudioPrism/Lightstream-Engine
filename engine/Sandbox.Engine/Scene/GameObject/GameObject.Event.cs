namespace Sandbox;


public partial class GameObject
{
	/// <summary>
	/// Post event to a specific GameObject (and its descendants by default - you can specify a <see cref="FindMode"/> to control this)
	/// </summary>
	public virtual void RunEvent<T>( Action<T> action, FindMode find = FindMode.EnabledInSelfAndDescendants )
	{
		Components.Execute( action, find );
	}
}
