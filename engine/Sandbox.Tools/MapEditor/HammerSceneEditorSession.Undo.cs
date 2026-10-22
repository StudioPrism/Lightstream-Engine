using System;

namespace Editor.MapEditor;

public partial class HammerSceneEditorSession
{
	public ISceneUndoScope UndoScope( string name )
	{
		return new HammerSceneUndoScope( this, name );
	}
}

// Sol: we should handle these properly but just stubbing this out for now
// instead of throwing a NotImplementedException which was obviously breaking things
internal class HammerSceneUndoScope : ISceneUndoScope
{
	internal HammerSceneEditorSession Session { get; }

	public HammerSceneUndoScope( HammerSceneEditorSession session, string name )
	{
		Session = session;
	}

	public IDisposable Push()
	{
		return default;
	}

	public ISceneUndoScope WithGameObjectCreations()
	{
		return this;
	}
	public ISceneUndoScope WithGameObjectDestructions( IEnumerable<GameObject> gameObjects )
	{
		return this;
	}
	public ISceneUndoScope WithGameObjectDestructions( GameObject gameObject )
	{
		return this;
	}
	public ISceneUndoScope WithGameObjectChanges( IEnumerable<GameObject> objects, GameObjectUndoFlags flags )
	{
		return this;
	}
	public ISceneUndoScope WithGameObjectChanges( GameObject gameObject, GameObjectUndoFlags flags )
	{
		return this;
	}
	public ISceneUndoScope WithComponentCreations()
	{
		return this;
	}
	public ISceneUndoScope WithComponentDestructions( IEnumerable<Component> components )
	{
		return this;
	}
	public ISceneUndoScope WithComponentDestructions( Component component )
	{
		return this;
	}
	public ISceneUndoScope WithComponentChanges( IEnumerable<Component> components )
	{
		return this;
	}
	public ISceneUndoScope WithComponentChanges( Component component )
	{
		return this;
	}
}
