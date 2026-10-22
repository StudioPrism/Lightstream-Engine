namespace Editor.MapEditor.EntityDefinitions;

/// <summary>
/// An entity used to control the 3D Skybox.
/// Its origin is used to determine the 3D Skybox's position relative to the map.
/// Place this entity, in the 3D Skybox, at the point where the origin of the map should be.
/// </summary>
[Library( "sky_camera" )]
[HammerEntity]
[EditorModel( "models/editor/sky_camera.vmdl" )]
[Title( "Sky Camera" ), Category( "Fog & Sky" ), Icon( "photo_camera" )]
class SkyCameraEntity : HammerEntityDefinition
{
	/// <summary>
	/// Scale of the skybox.
	/// </summary>
	[Property( "scale", Title = "3D Skybox Scale" ), DefaultValue( "16" )]
	public float SkyboxScale { get; set; }
}
