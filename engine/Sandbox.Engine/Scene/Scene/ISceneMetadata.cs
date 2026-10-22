namespace Sandbox;

/// <summary>
/// Allows components to add metadata to the scene/prefab file, which is accessible before loading it.
/// </summary>
public interface ISceneMetadata
{
	Dictionary<string, string> GetMetadata();
}
