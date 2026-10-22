using System.ComponentModel;

namespace Sandbox;

/// <summary>
/// New GameObjects and Components are registered with this class when they're created, and 
/// unregistered when they're removed. This gives us a single place to enforce
/// Id uniqueness in the scene, and allows for fast lookups by Id.
/// </summary>
public sealed class GameObjectDirectory
{
	private Scene scene;

	Dictionary<Guid, Component> componentsById = new();
	Dictionary<Guid, GameObject> objectsById = new();
	Dictionary<Guid, GameObjectSystem> systemsById = new();

	// obsolete me
	public int Count => objectsById.Count;

	public int GameObjectCount => objectsById.Count;
	public int ComponentCount => componentsById.Count;

	internal GameObjectDirectory( Scene scene )
	{
		this.scene = scene;
	}

	internal IEnumerable<GameObject> AllGameObjects => objectsById.Values;
	internal IEnumerable<Component> AllComponents => componentsById.Values;

	internal Action<GameObject> OnGameObjectAdded;

	internal Action<Component> OnComponentAdded;

	internal void Add( GameObjectSystem system )
	{
		if ( systemsById.TryGetValue( system.Id, out var existing ) )
		{
			Log.Warning( $"{system}: Guid {system.Id} is already taken by {existing} - changing" );
			system.ForceChangeId( Guid.NewGuid() );
		}

		systemsById[system.Id] = system;
	}

	internal void Add( Component component )
	{
		if ( componentsById.TryGetValue( component.Id, out var existing ) )
		{
			Log.Warning( $"{component}: Guid {component.Id} is already taken by {existing} - changing" );
			component.ForceChangeId( Guid.NewGuid() );
		}

		if ( objectsById.TryGetValue( component.Id, out var go ) )
		{
			Log.Warning( $"{component}: Guid {component.Id} is already taken by {go} - changing" );
			component.ForceChangeId( Guid.NewGuid() );
		}

		componentsById[component.Id] = component;
		OnComponentAdded?.Invoke( component );
	}

	internal void Add( GameObject go )
	{
		if ( objectsById.TryGetValue( go.Id, out var existing ) )
		{
			Log.Warning( $"{go}: Guid {go.Id} is already taken by {existing} - changing" );
			go.ForceChangeId( Guid.NewGuid() );
		}

		if ( componentsById.TryGetValue( go.Id, out var component ) )
		{
			Log.Warning( $"{go}: Guid {go.Id} is already taken by {component} - changing" );
			go.ForceChangeId( Guid.NewGuid() );
		}

		objectsById[go.Id] = go;
		OnGameObjectAdded?.Invoke( go );
	}

	internal void Add( GameObjectSystem system, Guid previouslyKnownAs )
	{
		if ( systemsById.TryGetValue( previouslyKnownAs, out var existing ) && existing == system )
		{
			systemsById.Remove( previouslyKnownAs );
		}

		Add( system );
	}

	internal void Add( Component component, Guid previouslyKnownAs )
	{
		if ( componentsById.TryGetValue( previouslyKnownAs, out var existing ) && existing == component )
		{
			componentsById.Remove( previouslyKnownAs );
		}

		Add( component );
	}

	internal void Add( GameObject go, Guid previouslyKnownAs )
	{
		if ( go is Scene && go is not PrefabScene ) return;

		if ( objectsById.TryGetValue( previouslyKnownAs, out var existing ) && existing == go )
		{
			objectsById.Remove( previouslyKnownAs );
		}

		Add( go );
	}

	internal void Remove( GameObjectSystem system )
	{
		if ( !systemsById.TryGetValue( system.Id, out var existing ) )
		{
			Log.Warning( $"Tried to unregister unregistered id {system}, {system.Id}" );
			return;
		}

		if ( existing != system )
		{
			Log.Warning( $"Tried to unregister wrong component {system}, {system.Id} (was {existing})" );
			return;
		}

		systemsById.Remove( system.Id );
	}

	internal void Remove( Component component )
	{
		if ( !componentsById.TryGetValue( component.Id, out var existing ) )
		{
			Log.Warning( $"Tried to unregister unregistered id {component}, {component.Id}" );
			return;
		}

		if ( existing != component )
		{
			Log.Warning( $"Tried to unregister wrong component {component}, {component.Id} (was {existing})" );
			return;
		}

		componentsById.Remove( component.Id );
	}

	internal void Remove( GameObject go )
	{
		if ( go is Scene && go is not PrefabScene ) return;

		if ( !objectsById.TryGetValue( go.Id, out var existing ) )
		{
			Log.Warning( $"Tried to unregister unregistered id {go}, {go.Id}" );
			return;
		}

		if ( existing != go )
		{
			Log.Warning( $"Tried to unregister wrong game object {go}, {go.Id} (was {existing})" );
			return;

		}

		objectsById.Remove( go.Id );
	}

	/// <summary>
	/// Find a GameObjectSystem in the scene by Guid. This should be really really fast.
	/// </summary>
	internal GameObjectSystem FindSystemByGuid( Guid guid )
	{
		return systemsById.GetValueOrDefault( guid );
	}

	/// <summary>
	/// Find a Component in the scene by Guid. This should be really really fast.
	/// </summary>
	public Component FindComponentByGuid( Guid guid )
	{
		if ( componentsById.TryGetValue( guid, out var found ) )
			return found;

		return null;
	}

	/// <summary>
	/// Find a GameObject in the scene by Guid. This should be really really fast.
	/// </summary>
	public GameObject FindByGuid( Guid guid )
	{
		if ( objectsById.TryGetValue( guid, out var found ) )
			return found;

		// Do we have a prefabfile with this guid?
		var prefabFile = PrefabFile.FindByGuid( guid );
		if ( prefabFile is not null )
		{
			return SceneUtility.GetPrefabScene( prefabFile );
		}

		return default;
	}

	/// <summary>
	/// Find objects with this name. Not performant.
	/// </summary>
	public IEnumerable<GameObject> FindByName( string name, bool caseinsensitive = true )
	{
		return objectsById.Values.Where( x => string.Equals( x.Name, name, caseinsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal ) );
	}
}
