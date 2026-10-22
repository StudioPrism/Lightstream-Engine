namespace Editor;

/// <summary>
/// Holds a current open scene and its edit state
/// </summary>
class PrefabEditorSession : SceneEditorSession
{
	public new PrefabScene Scene => base.Scene as PrefabScene;

	public PrefabEditorSession( PrefabScene scene ) : base( scene )
	{
		scene.SceneWorld.AmbientLightColor = new Color( 0.7f, 0.7f, 0.7f );

		scene.Name = scene.Source.ResourceName;
		Selection.Add( scene );
	}

	protected override void OnEdited()
	{
		if ( Scene is { } prefabScene )
		{
			var prefab = (PrefabFile)prefabScene.Source;

			// write from prefab scene to its jsonobject
			// this doesn't save it to disk
			prefabScene.ToPrefabFile();

			EditorScene.UpdatePrefabInstances( prefab );
		}
	}
}
