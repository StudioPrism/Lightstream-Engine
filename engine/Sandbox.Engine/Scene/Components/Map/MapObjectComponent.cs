
namespace Sandbox;

[Hide]
[Title( "Map Objects" )]
[Category( "World" )]
[Tag( "development" )]
[Icon( "maps_home_work" )]
public class MapObjectComponent : Component, Component.ExecuteInEditor
{
	List<SceneObject> objects = new List<SceneObject>();

	public Action RecreateMapObjects;

	internal void AddSceneObjects( IEnumerable<SceneObject> sceneObjects )
	{
		Tags.RemoveAll();

		// Copy tags from each SceneObject to this GameObject.
		foreach ( var obj in sceneObjects )
		{
			Tags.Add( obj.Tags );
			objects.Add( obj );
		}
	}

	protected override void OnEnabled()
	{
		RecreateMapObjects?.Invoke();

		if ( !objects.Any() )
		{
			GameObject.Flags |= GameObjectFlags.Error;
		}

		Transform.OnTransformChanged += OnTransformChanged;
	}

	protected override void OnDisabled()
	{
		Transform.OnTransformChanged -= OnTransformChanged;

		foreach ( var obj in objects )
		{
			obj.Delete();
		}

		objects.Clear();
	}

	private void OnTransformChanged()
	{
		var origin = WorldTransform;

		foreach ( var obj in objects )
		{
			if ( !obj.IsValid() )
				continue;

			obj.Transform = origin.WithScale( obj.Transform.Scale );
		}
	}
}
