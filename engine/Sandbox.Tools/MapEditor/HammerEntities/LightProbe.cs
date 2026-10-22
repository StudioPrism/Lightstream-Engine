namespace Editor.MapEditor.EntityDefinitions;

/// <summary>
/// A grid of precomputed light probes.
/// </summary>
[ClassName( "env_light_probe_volume" )]
[HammerEntity]
[EditorModel( "models/editor/iv_helper" )]
[BoundsHelper( "box_mins", "box_maxs" )]
[BakeResource( "lightprobetexture", "vtex", "env_light_probe_volume", "ToolObjects/LightProbeVolume" )]
[Title( "Light Probe Volume" ), Category( "Lighting" ), Icon( "all_out" )]
internal sealed class LightProbe : HammerEntityDefinition
{
	public enum IndoorOutdoorLevel
	{
		Lowest = -2,
		Low = -1,
		Normal = 0,
		High = 1,
		Highest = 2,
	}

	public enum StorageType
	{
		Default = -1,
		AmbientCubeRGBM8888Uncompressed = 0,
		AmbientCubeRGBMDXT5Compressed = 1,
	}

	/// <summary>
	/// Name of the light probe texture
	/// </summary>
	[Property( "lightprobetexture" ), Title( "Light Probe Texture" ), ResourceType( "vtex" )]
	public string Texture { get; set; }

	[Property( "lightprobetexture_dli" ), Hide]
	public string TextureIndices { get; set; }

	[Property( "lightprobetexture_dls" ), Hide]
	public string TextureScalars { get; set; }

	/// <summary>
	/// Minimum bounding box for the cubemap
	/// </summary>
	[Property( "box_mins" ), Title( "Box Mins" )]
	[DefaultValue( "-72 -72 -72" )]
	public Vector3 BoxMins { get; set; }

	/// <summary>
	/// Maximum bounding box for the cubemap
	/// </summary>
	[Property( "box_maxs" ), Title( "Box Maxs" )]
	[DefaultValue( "72 72 72" )]
	public Vector3 BoxMaxs { get; set; }

	/// <summary>
	/// Volume resolution.
	/// </summary>
	[Property( "voxel_size" ), Title( "Voxel Size" )]
	[DefaultValue( "48" ), MinMax( 1.5f, 108.0f )]
	public float VoxelSize { get; set; }

	/// <summary>
	/// If multiple volumes contain an object, the highest priority volume takes precedence.
	/// </summary>
	[Property( "indoor_outdoor_level" ), Title( "Priority" )]
	[DefaultValue( "0" )]
	public IndoorOutdoorLevel Priority { get; set; }

	/// <summary>
	/// Ignore Unreachable Space.
	/// </summary>
	[Property( "flood_fill" ), Title( "Ignore Unreachable Space" )]
	[DefaultValue( "1" )]
	public bool FloodFill { get; set; }

	/// <summary>
	/// Ignore Voxelized Solid Space.
	/// </summary>
	[Property( "voxelize" ), Title( "Ignore Voxelized Solid Space" )]
	[DefaultValue( "1" )]
	public bool Voxelize { get; set; }

	/// <summary>
	/// Calculate Diffuse Lighting Using Cubemap.
	/// </summary>
	[Property( "light_probe_volume_from_cubemap" ), Title( "Calculate Diffuse Lighting Using Cubemap" )]
	[DefaultValue( "0" )]
	public bool FromCubemap { get; set; }

	/// <summary>
	/// Semicolon-delimited list of light groups to affect.
	/// </summary>
	[Property( "lightgroup" ), Title( "Light Group" )]
	public string LightGroup { get; set; }

	/// <summary>
	/// How the light probe texture is stored.
	/// </summary>
	[Property( "storage" ), Title( "Storage" )]
	[DefaultValue( "-1" )]
	public StorageType Storage { get; set; }

	/// <summary>
	/// Static objects find the light probe to use with a baked handshake.
	/// </summary>
	[Property( "handshake" ), Hide]
	public int Handshake { get; set; }
}

/// <summary>
/// Combination of an env_cubemap_box and an env_light_probe_volume.
/// </summary>
[ClassName( "env_combined_light_probe_volume" )]
[HammerEntity]
[EditorModel( "models/editor/env_cubemap" )]
[BoundsHelper( "box_mins", "box_maxs" )]
[BakeResource( "lightprobetexture", "vtex", "env_light_probe_volume", "ToolObjects/LightProbeVolume" )]
[BakeResource( "cubemaptexture", "vtex", "env_cubemap", "ToolObjects/CubeMap" )]
[Title( "Light Probe Volume Combined" ), Category( "Lighting" ), Icon( "all_out" )]
internal sealed class CombinedLightProbe : HammerEntityDefinition
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
	public Vector3 BoxMins { get; set; }

	/// <summary>
	/// Maximum bounding box for the cubemap
	/// </summary>
	[Property( "box_maxs" ), Title( "Box Projection Maxs" )]
	[DefaultValue( "72 72 72" )]
	public Vector3 BoxMaxs { get; set; }

	/// <summary>
	/// Amount of feathering to apply to the cubemap edges to blend with other envmaps
	/// </summary>
	[Property( "cubemap_feathering" ), Title( "Blending Feathering" )]
	[DefaultValue( "8.0f" )]
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
	/// Name of the light probe texture
	/// </summary>
	[Property( "lightprobetexture" ), Title( "Light Probe Texture" ), ResourceType( "vtex" )]
	public string Texture { get; set; }

	[Property( "lightprobetexture_dli" ), Hide]
	public string TextureIndices { get; set; }

	[Property( "lightprobetexture_dls" ), Hide]
	public string TextureScalars { get; set; }

	/// <summary>
	/// Volume resolution.
	/// </summary>
	[Property( "voxel_size" ), Title( "Voxel Size" )]
	[DefaultValue( "48" ), MinMax( 1.5f, 108.0f )]
	public float VoxelSize { get; set; }

	/// <summary>
	/// Ignore Unreachable Space.
	/// </summary>
	[Property( "flood_fill" ), Title( "Ignore Unreachable Space" )]
	[DefaultValue( "1" )]
	public bool FloodFill { get; set; }

	/// <summary>
	/// Ignore Voxelized Solid Space.
	/// </summary>
	[Property( "voxelize" ), Title( "Ignore Voxelized Solid Space" )]
	[DefaultValue( "1" )]
	public bool Voxelize { get; set; }

	/// <summary>
	/// Calculate Diffuse Lighting Using Cubemap.
	/// </summary>
	[Property( "light_probe_volume_from_cubemap" ), Title( "Calculate Diffuse Lighting Using Cubemap" )]
	[DefaultValue( "0" )]
	public bool FromCubemap { get; set; }

	/// <summary>
	/// How the light probe texture is stored.
	/// </summary>
	[Property( "storage" ), Title( "Storage" )]
	[DefaultValue( "-1" )]
	public LightProbe.StorageType Storage { get; set; }

	/// <summary>
	/// User provided a custom cubemap texture.
	/// </summary>
	[Property( "customcubemaptexture" ), Hide]
	public bool CustomTexture { get; set; }
}
