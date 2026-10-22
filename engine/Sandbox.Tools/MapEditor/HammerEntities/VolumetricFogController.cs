namespace Editor.MapEditor.EntityDefinitions;

/// <summary>
/// Controller for volumetric fogging - bounds are extents of fog irradiance volume (for indirect)
/// </summary>
[Library( "env_volumetric_fog_controller" )]
[HammerEntity]
[EditorSprite( "materials/editor/fog_volume_controller.vmat" )]
[Sphere]
[Title( "Volumetric Fog Controller" ), Category( "Fog & Sky" ), Icon( "lens_blur" )]
[SimpleHelper( "volumetric_fog_controller" )]
internal sealed class VolumetricFogController : HammerEntityDefinition
{
	[Property( "FogStrength" ), DefaultValue( "1.0" )]
	public float FogStrength { get; set; }

	[Property( "DrawDistance" ), DefaultValue( "1024.0" )]
	public float DrawDistance { get; set; }

	[Property( "FadeInStart" ), DefaultValue( "20.0" )]
	public float FadeInStart { get; set; }

	[Property( "FadeInEnd" ), DefaultValue( "100.0" )]
	public float FadeInEnd { get; set; }

	[Property( "IndirectVoxelDim", Title = "Fog Volume Resolution" ), DefaultValue( "256.0" )]
	public float FogVolumeResolution { get; set; }

	[Property( "FadeSpeed", Title = "Fade time" ), DefaultValue( "2.0" )]
	public float FadeSpeed { get; set; }

	[Property( "Anisotropy", Title = "Anisotropy" ), DefaultValue( "1.0" )]
	public float Anisotropy { get; set; }

	[Property( "fogirradiancevolume", Title = "Fog Irradiance Texture" ), FGDType( "resource:texture" )]
	public string FogIrradianceVolumeTexture { get; set; }

	public enum IndirectVoxelDim
	{
		[Title( "Very High Resolution (512x)" )]
		VeryHighResolution = 512,
		[Title( "High Resolution (256x)" )]
		HighResolution = 256,
		[Title( "Medium Resolution (128x)" )]
		MediumResolution = 128,
		[Title( "Low Resolution (64x)" )]
		LowResolution = 64,
	}
}
