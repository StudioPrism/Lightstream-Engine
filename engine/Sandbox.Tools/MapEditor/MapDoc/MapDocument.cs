using NativeMapDoc;
using System;

namespace Editor.MapDoc;

/// <summary>
/// Represents an open map document. A document has a tree of <see cref="MapNode"/> that represent the world.
/// </summary>
public class MapDocument : IHandle
{
	#region IHandle
	//
	// A pointer to the actual native object
	//
	internal CMapDoc native;

	//
	// IHandle implementation
	//
	void IHandle.HandleInit( IntPtr ptr ) => OnNativeInit( ptr );
	void IHandle.HandleDestroy() => OnNativeDestroy();
	bool IHandle.HandleValid() => !native.IsNull;
	#endregion

	internal MapDocument() { }
	internal MapDocument( HandleCreationData _ ) { }

	internal virtual void OnNativeInit( CMapDoc ptr )
	{
		native = ptr;
	}

	internal virtual void OnNativeDestroy()
	{
		native = IntPtr.Zero;
	}

	/// <summary>
	/// The map file name
	/// </summary>
	public string PathName => native.GetPathName();

	/// <summary>
	/// The world
	/// </summary>
	public MapWorld World => native.GetMapWorld();

	/// <summary>
	/// Removes the node from the world, deletes all children too.
	/// </summary>
	public void DeleteNode( MapNode node )
	{
		ThreadSafe.AssertIsMainThread();

		// TODO: There is a delete that makes it undoable, lets expose that somehow
		native.DeleteObject( node );
	}

	internal void PostLoadDocument()
	{
		CheckNodes();
	}

	void CheckNodes()
	{
		var mapgameobjects = World.Children.OfType<MapGameObject>().ToArray();

		// Does every GameObject have a matching MapGameObject?
		foreach ( var g in World.Scene.Children )
		{
			var mg = mapgameobjects.FirstOrDefault( x => x.GameObject == g );
			if ( mg is not null ) continue;

			Log.Warning( $"Found loose GameObject {g} in the scene that has no map game object, creating... (This shouldn't have happened)" );

			new MapGameObject( this, g );
		}
	}
}
