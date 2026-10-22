namespace Editor.MapEditor.EntityDefinitions;

/// <summary>
/// An env_cubemap with box projection.
/// </summary>
[ClassName( "env_cubemap_box" )]
[HammerEntity]
[EditorModel( "models/editor/env_cubemap" )]
[BoundsHelper( "box_mins", "box_maxs" )]
[BakeResource( "cubemaptexture", "vtex", "env_cubemap", "ToolObjects/CubeMap" )]
[Title( "Cubemap Box" ), Category( "Lighting" ), Icon( "all_out" )]
class CubemapBox : BaseCubemap
{
}

/// <summary>
/// Cubemap for sampling indirect specular reflection.
/// </summary>
[ClassName( "env_cubemap" )]
[HammerEntity]
[EditorModel( "models/editor/env_cubemap" )]
[Sphere( "influenceradius", 128, 128, 255 )]
[BakeResource( "cubemaptexture", "vtex", "env_cubemap", "ToolObjects/CubeMap" )]
[Title( "Cubemap" ), Category( "Lighting" ), Icon( "all_out" )]
class Cubemap : BaseCubemap
{
	/// <summary>
	/// The radius of influence for this cubemap
	/// </summary>
	[Property( "influenceradius" ), Title( "Influence Radius" )]
	[DefaultValue( "512" )]
	public float Radius { get; set; }
}

internal abstract class BaseCubemap : HammerEntityDefinition
{
	/// <summary>
	/// Name of the cubemap texture
	/// </summary>
	[Property( "cubemaptexture" ), Title( "Cubemap Texture" ), ResourceType( "vtex" )]
	public string CubemapTexture { get; set; }

	public enum IndoorOutdoorLevel
	{
		Lowest = -2,
		Low = -1,
		Normal = 0,
		High = 1,
		Highest = 2,
	}

	/// <summary>
	/// If multiple volumes contain an object, the highest priority volume takes precedence.
	/// </summary>
	[Property( "indoor_outdoor_level" ), Title( "Priority" )]
	[DefaultValue( "0" )]
	public IndoorOutdoorLevel Priority { get; set; }

	/// <summary>
	/// Near clip plane used for the camera when baking the cube map
	/// </summary>
	[Property( "bakenearz" ), Title( "Bake Near Z" )]
	[DefaultValue( "2" )]
	public float BakeNearZ { get; set; }


	/// <summary>
	/// Far clip plane used for the camera when baking the cube map
	/// </summary>
	[Property( "bakefarz" ), Title( "Bake Far Z" )]
	[DefaultValue( "4096" )]
	public float BakeFarZ { get; set; }

	/// <summary>
	/// Semicolon-delimited list of light groups to affect.
	/// </summary>
	[Property( "lightgroup" ), Title( "Light Group" )]
	public string LightGroup { get; set; }

	/// <summary>
	/// Minimum bounding box for the cubemap
	/// </summary>
	[Property( "box_mins" ), Title( "Box Projection Mins" )]
	[DefaultValue( "-72 -72 -72" )]
	public Vector3 BoxProjectMins { get; set; }

	/// <summary>
	/// Maximum bounding box for the cubemap
	/// </summary>
	[Property( "box_maxs" ), Title( "Box Projection Maxs" )]
	[DefaultValue( "72 72 72" )]
	public Vector3 BoxProjectMaxs { get; set; }

	/// <summary>
	/// Amount of feathering to apply to the cubemap edges to blend with other envmaps
	/// </summary>
	[Property( "cubemap_feathering" ), Title( "Blending Feathering" )]
	[DefaultValue( "8.0" )]
	public float Feathering { get; set; }

	/// <summary>
	/// Index into the cubemap texture array
	/// </summary>
	[Property( "array_index" ), Hide]
	public int ArrayIndex { get; set; }

	/// <summary>
	/// Static objects find the cubemap to use with a baked handshake.
	/// </summary>
	[Property( "handshake" ), Hide]
	public int Handshake { get; set; }

	/// <summary>
	/// User provided a custom cubemap texture.
	/// </summary>
	[Property( "customcubemaptexture" ), Hide]
	public bool CustomTexture { get; set; }
}
