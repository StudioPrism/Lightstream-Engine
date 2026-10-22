namespace Editor.MapEditor.EntityDefinitions;

[Library( "env_sky" )]
[HammerEntity]
[EditorSprite( "editor/env_sky.vmat" )]
[Skybox]
[Title( "Sky" ), Category( "Fog & Sky" ), Icon( "cloud_circle" )]
class SkyEntity : HammerEntityDefinition
{
	[Property( "skyname" ), Title( "Sky Material" ), ResourceType( "vmat" )]
	public string Skyname { get; set; } = "materials/skybox/skybox_day_01.vmat";

	[Property( "tint_color" ), Title( "Skybox Tint Color" )]
	public Color TintColor { get; set; } = Color.White;

	[Property( "fog_type" ), Title( "Fog Type" )]
	public SceneSkyBox.FogType FogType { get; set; } = SceneSkyBox.FogType.Distance;

	[Property( "angular_fog_min_start" ), Title( "Fog Min Angle Start" ), Category( "Angular Fog" )]
	public float FogMinStart { get; set; } = -25.0f;

	[Property( "angular_fog_min_end" ), Title( "Fog Min Angle End" )]
	public float FogMinEnd { get; set; } = -35.0f;

	[Property( "angular_fog_max_start" ), Title( "Fog Max Angle Start" ), Category( "Angular Fog" )]
	public float FogMaxStart { get; set; } = 25.0f;

	[Property( "angular_fog_max_end" ), Title( "Fog Max Angle End" )]
	public float FogMaxEnd { get; set; } = 35.0f;
	[Property( "ibl" ), Title( "Sky Indirect Lighting" ), Description( "Whether to use the skybox for lighting as an envmap probe" ), DefaultValue( true )]
	public bool SkyIndirectLighting { get; set; } = true;
}
