namespace Editor.MapEditor.EntityDefinitions;

/// <summary>
/// Specifies fog based on an material.
/// </summary>
[Library( "env_cubemap_fog" )]
[HammerEntity]
[EditorSprite( "materials/editor/env_cubemap_fog.vmat" )]
[Title( "Cubemap fog" ), Category( "Fog & Sky" ), Icon( "lens_blur" )]
class HammerCubemapFog : HammerEntityDefinition
{
	/// <summary>
	/// Cubemap material to use for the fog.
	/// </summary>
	[Property( "cubemapfogmaterial" ), Title( "Cubemap Material" ), DefaultValue( "materials/skybox/skybox_day_01.vmat" ), ResourceType( "vmat" )]
	public string CubemapMaterial { get; set; }

	/// <summary>
	/// Adjust how quickly the cubemap blurs out at closer distances. A value of 0.0 always uses the lowest resolution MIP over the entire range, while a value of 1.0 uses the highest.
	/// </summary>
	[Property( "cubemapfoglodbiase" ), Title( "Cubemap LOD (mip) Bias" ), DefaultValue( "0.5" )]
	public float LodBias { get; set; }

	/// <summary>
	/// The distance from the player at which the fog will start to fade in.
	/// </summary>
	[Property( "cubemapfogstartdistance" ), Title( "Fog Start Distance" ), DefaultValue( "0.0" )]
	public float StartDistance { get; set; }

	/// <summary>
	/// The distance from the player at which the fog will be at full strength.
	/// </summary>
	[Property( "cubemapfogenddistance" ), Title( "Fog End Distance" ), DefaultValue( "6000.0" )]
	public float EndDistance { get; set; }

	/// <summary>
	/// Exponent for distance falloff. For example, 2.0 is proportional to square of distance.
	/// </summary>
	[Property( "cubemapfogfalloffexponent" ), Title( "Distance Falloff Exponent" ), DefaultValue( "2.0" )]
	public float FalloffExponent { get; set; }

	/// <summary>
	/// The distance between the start of the height fog and where it is fully opaque. Setting this to 0 will disable height based blending.
	/// </summary>
	[Property( "cubemapfogheightwidth" ), Title( "Height Fog Width" ), DefaultValue( "0.0" )]
	public float HeightWidth { get; set; }

	/// <summary>
	/// The absolute height in the map at which the height fog will start to fade in.
	/// </summary>
	[Property( "cubemapfogheightstart" ), Title( "Height Fog Start" ), DefaultValue( "2000.0" )]
	public float HeightStart { get; set; }

	/// <summary>
	/// Exponent for height falloff. For example, 2.0 is proportional to square of distance.
	/// </summary>
	[Property( "cubemapfogheightexponent" ), Title( "Height Fog Exponent" ), DefaultValue( "2.0" )]
	public float HeightExponent { get; set; }
}
