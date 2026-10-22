namespace Sandbox;

public partial class GameObject
{
	/// <summary>
	/// Add a component to this GameObject
	/// </summary>
	public T AddComponent<T>( bool startEnabled = true ) where T : Component, new() => Components.Create<T>( startEnabled );

	/// <summary>
	/// Add a component to this GameObject
	/// </summary>
	public T GetOrAddComponent<T>( bool startEnabled = true ) where T : Component, new()
	{
		var t = GetComponent<T>( true );
		if ( t is not null ) return t;

		t = AddComponent<T>( startEnabled );
		return t;
	}

	/// <summary>
	/// Get a component on this GameObject
	/// </summary>
	public T GetComponent<T>( bool includeDisabled = false ) => Components.Get<T>( includeDisabled );

	/// <summary>
	/// Get components on this GameObject
	/// </summary>
	public IEnumerable<T> GetComponents<T>( bool includeDisabled = false ) => Components.GetAll<T>( includeDisabled ? (FindMode.InSelf | FindMode.Enabled | FindMode.Disabled) : FindMode.InSelf | FindMode.Enabled );

	/// <summary>
	/// Get components on this GameObject and on descendant GameObjects
	/// </summary>
	public IEnumerable<T> GetComponentsInChildren<T>( bool includeDisabled = false, bool includeSelf = true ) => Components.GetAll<T>( includeDisabled ? ((includeSelf ? FindMode.InSelf : default) | FindMode.InDescendants | FindMode.Enabled | FindMode.Disabled) : (includeSelf ? FindMode.InSelf : default) | FindMode.InDescendants | FindMode.Enabled );

	/// <summary>
	/// Get component on this GameObject or on descendant GameObjects
	/// </summary>
	public T GetComponentInChildren<T>( bool includeDisabled = false, bool includeSelf = true ) => Components.Get<T>( includeDisabled ? ((includeSelf ? FindMode.InSelf : default) | FindMode.InDescendants | FindMode.Enabled | FindMode.Disabled) : (includeSelf ? FindMode.InSelf : default) | FindMode.InDescendants | FindMode.Enabled );

	/// <summary>
	/// Get components on this GameObject and on ancestor GameObjects
	/// </summary>
	public IEnumerable<T> GetComponentsInParent<T>( bool includeDisabled = false, bool includeSelf = true ) => Components.GetAll<T>( includeDisabled ? ((includeSelf ? FindMode.InSelf : default) | FindMode.InAncestors | FindMode.Enabled | FindMode.Disabled) : (includeSelf ? FindMode.InSelf : default) | FindMode.InAncestors | FindMode.Enabled );

	/// <summary>
	/// Get component on this GameObject and on ancestor GameObjects
	/// </summary>
	public T GetComponentInParent<T>( bool includeDisabled = false, bool includeSelf = true ) => Components.Get<T>( includeDisabled ? ((includeSelf ? FindMode.InSelf : default) | FindMode.InAncestors | FindMode.Enabled | FindMode.Disabled) : (includeSelf ? FindMode.InSelf : default) | FindMode.InAncestors | FindMode.Enabled );
}
