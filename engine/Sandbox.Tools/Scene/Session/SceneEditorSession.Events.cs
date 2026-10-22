using Sandbox.Navigation;

namespace Editor;

public partial class SceneEditorSession : ResourceLibrary.IEventListener, NavMesh.IEventListener
{
	void ResourceLibrary.IEventListener.OnRegister( GameResource resource )
	{
		if ( resource is not PrefabFile prefab ) return;

		EditorScene.UpdatePrefabInstancesInScene( Scene, prefab );
	}

	void ResourceLibrary.IEventListener.OnUnregister( GameResource resource )
	{
		if ( resource is not PrefabFile prefab ) return;

		EditorScene.UpdatePrefabInstancesInScene( Scene, prefab );
	}

	void ResourceLibrary.IEventListener.OnExternalChangesPostLoad( GameResource resource )
	{
		if ( resource is not PrefabFile prefab ) return;

		EditorScene.UpdatePrefabInstancesInScene( Scene, prefab );
	}

	void NavMesh.IEventListener.OnAreaDefinitionChanged()
	{
		Scene.NavMesh?.UpdateAreaIds();
	}
}
