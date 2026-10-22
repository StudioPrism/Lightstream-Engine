namespace Editor.MapEditor.EntityDefinitions;

/// <summary>
/// Sets a user bounding volume for volumetric fog - if one of these are in the map then all fog will get clamped to these entities.
/// Requires a env_volumetric_fog_controller present to work.
/// </summary>
[Library( "env_volumetric_fog_volume" )]
[HammerEntity]
[EditorSprite( "materials/editor/fog_volume.vmat" )]
[BoundsHelper( "box_mins", "box_maxs", true )]
[Title( "Volumetric Fog Volume" ), Category( "Fog & Sky" ), Icon( "lens_blur" )]
internal sealed class HammerVolumetricFogVolume : HammerEntityDefinition
{
	[Property( "box_mins" ), DefaultValue( "-64 -64 -64" )]
	public Vector3 BoxMins { get; set; }

	[Property( "box_maxs" ), DefaultValue( "64 64 64" )]
	public Vector3 BoxMaxs { get; set; }

	public BBox BoundingBox => new( BoxMins, BoxMaxs );

	[Property( "FogStrength" ), DefaultValue( "1.0" )]
	public float FogStrength { get; set; }

	[Property( "FalloffExponent" ), DefaultValue( "1.0" )]
	public float FalloffExponent { get; set; }

	[Property, DefaultValue( "0" )]
	public FalloffShape Shape { get; set; }

	public enum FalloffShape
	{
		[Title( "Linear (Box) Falloff" )]
		Linear = 0,
		[Title( "Radial (Sphere) Falloff" )]
		Radial = 1,
	}
}
