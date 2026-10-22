using Sandbox;

public static partial class SandboxSystemExtensions
{
	/// <summary>
	/// Returns false if model has no valid render meshes, or is null.
	/// </summary>
	public static bool HasRenderMeshes( this Model model )
	{
		if ( model is null ) return false;
		if ( !model.native.IsValid ) return false;
		if ( model.MeshCount <= 0 ) return false;
		if ( model.native.HasSceneObjects() == false ) return false;

		return true;
	}
}
