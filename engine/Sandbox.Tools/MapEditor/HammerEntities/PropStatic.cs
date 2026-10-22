namespace Editor.MapEditor.EntityDefinitions;

/// <summary>
/// A static model. It will be embedded into the map on compile therefore cannot be manipulated in-game.
/// It affects performance less than other model entities and can have baked lighting.
/// </summary>
[HammerEntity]
[Library( "prop_static" )]
[Title( "Static Prop" )]
[Sphere( "fademindist", 200, 200, 200 )]
[Sphere( "fademaxdist", 200, 200, 200 )]
[Model( Archetypes = ModelArchetype.static_prop_model )]
sealed class PropStaticEntity : HammerEntityDefinition
{
	[Property( "model", Title = "World Model" )]
	public Model Model { get; set; }

	public enum SolidChoices
	{
		[Title( "Not Solid" )]
		NotSolid = 0,
		[Title( "Use axis-aligned box" )]
		UseAxisAlignedBox = 2,
		[Title( "Use oriented Box" )]
		UseOrientedBox = 3,
		[Title( "Use VPhysics" )]
		UseVPhysics = 6
	}

	[Property( "solid", Title = "Collisions" )]
	[DefaultValue( "6" )]
	public SolidChoices Solid { get; set; }

	[Property( "disableshadows", Title = "Disable Shadows" )]
	[Category( "Rendering" )]
	[DefaultValue( "0" )]
	public bool DisableShadows { get; set; }

	/// <summary>
	/// The method by which the fading distance should be determined.
	/// If 'No', the fade distances is the distance from the player's view to the object, in inches.
	/// If 'Yes', the fade distance is the size of the object onscreen, in pixels.
	/// </summary>
	[Property( "screenspacefade", Title = "Screen Space Fade" )]
	[Category( "Rendering" )]
	[DefaultValue( "0" )]
	public bool ScreenSpaceFade { get; set; }

	/// <summary>
	/// Some models have multiple versions of their textures, called skins.
	/// </summary>
	[Property( "skin", Title = "Skin" )]
	[Category( "Rendering" )]
	[DefaultValue( "default" )]
	public string Skin { get; set; }

	[Property( "bodygroups", Title = "Body Groups" )]
	[Category( "Rendering" )]
	public string BodyGroups { get; set; }

	/// <summary>
	/// LOD level to be displayed in game. Set to Auto for standard automatic selection, or set to a specific level to always use that level.
	/// </summary>
	[Property( "lodlevel", Title = "LOD Level" )]
	[Category( "Rendering" )]
	[DefaultValue( "-1" )]
	public int LodLevel { get; set; }

	[Property( "fademindist", Title = "Start Fade Dist/Pixels" )]
	[Category( "Rendering" )]
	[DefaultValue( "-1" )]
	public float FadeMinDist { get; set; }

	/// <summary>
	/// Maximum distance at which the prop is visible (0 = don't fade out).
	/// If 'Screen Space Fade' is selected, this represents the *minimum* number of pixels wide covered by the prop when it fades.
	/// </summary>
	[Property( "fademaxdist", Title = "End Fade Dist/Pixels" )]
	[Category( "Rendering" )]
	[DefaultValue( "0" )]
	public float FadeMaxDist { get; set; }

	/// <summary>
	/// If true this geometry is not important for precomputed lighting.
	/// </summary>
	[Property( "detailgeometry", Title = "Detail Geometry" )]
	[DefaultValue( "0" )]
	public bool DetailGeometry { get; set; }

	/// <summary>
	/// If true this geometry is used as an occluder for precomputed visibility.
	/// </summary>
	[Property( "visoccluder", Title = "Vis Occluder" )]
	[DefaultValue( "0" )]
	public bool VisOccluder { get; set; }

	/// <summary>
	/// If true this geometry will be baked into the world geometry of the map so that the model is not referenced at runtime.
	/// </summary>
	[Property( "baketoworld", Title = "Bake To World" )]
	[DefaultValue( "0" )]
	public bool BakeToWorld { get; set; }

	/// <summary>
	/// If true this will not be merged with other geometry during map compile (reduces rendering efficiency).
	/// </summary>
	[Property( "disablemeshmerging", Title = "Disable Mesh Merging" )]
	[DefaultValue( "0" )]
	public bool DisableMeshMerging { get; set; }

	[Property( "rendercolor", Title = "Color (R G B A)" )]
	[DefaultValue( "255 255 255 255" )]
	public Color RenderColor { get; set; }

	/// <summary>
	/// Select a an entity to specify a location to sample lighting from, instead of using this entity's bounding box center.
	/// </summary>
	[Property( "lightingorigin", Title = "Lighting Origin" )]
	[Category( "Rendering" )]
	[FGDType( "target_destination" )]
	public string LightingOrigin { get; set; }

	/// <summary>
	/// Will only be lit by lights affecting this group.
	/// </summary>
	[Property( "lightgroup", Title = "Light Group" )]
	[Category( "Rendering" )]
	public string LightGroup { get; set; }

	/// <summary>
	/// If true, this geometry renders into baked cube maps.
	/// </summary>
	[Property( "rendertocubemaps", Title = "Render to Cubemaps" )]
	[Category( "Rendering" )]
	[DefaultValue( "1" )]
	public bool RenderToCubemaps { get; set; }

	/// <summary>
	/// Pre-compute environment map and light probe volume used on this object.
	/// </summary>
	[Property( "precomputelightprobes", Title = "Precompute Light Probes" )]
	[Category( "Rendering" )]
	[DefaultValue( "1" )]
	public bool PrecomputeLightProbes { get; set; }

	[Property( "materialoverride", Title = "Override Material" )]
	[Category( "Rendering" )]
	[FGDType( "material" )]
	public string MaterialOverride { get; set; }

	/// <summary>
	/// Do not render this prop when using the lowest quality video settings, it is an non-essential detail.
	/// </summary>
	[Property( "disableinlowquality", Title = "Disable in low quality mode" )]
	[Category( "Rendering" )]
	[DefaultValue( "0" )]
	public bool DisableInLowQuality { get; set; }

	public enum LightmapScaleBiasChoices
	{
		[Title( "Scale down by 256" )]
		ScaleDownBy256 = -8,
		[Title( "Scale down by 128" )]
		ScaleDownBy128 = -7,
		[Title( "Scale down by 64" )]
		ScaleDownBy64 = -6,
		[Title( "Scale down by 32" )]
		ScaleDownBy32 = -5,
		[Title( "Scale down by 16" )]
		ScaleDownBy16 = -4,
		[Title( "Scale down by 8" )]
		ScaleDownBy8 = -3,
		[Title( "Scale down by 4" )]
		ScaleDownBy4 = -2,
		[Title( "Scale down by 2" )]
		ScaleDownBy2 = -1,
		[Title( "Default (no scale)" )]
		Default = 0,
		[Title( "Scale up by 2" )]
		ScaleUpBy2 = 1,
		[Title( "Scale up by 4" )]
		ScaleUpBy4 = 2,
		[Title( "Scale up by 8" )]
		ScaleUpBy8 = 3,
		[Title( "Scale up by 16" )]
		ScaleUpBy16 = 4,
		[Title( "Scale up by 32" )]
		ScaleUpBy32 = 5,
		[Title( "Scale up by 64" )]
		ScaleUpBy64 = 6,
		[Title( "Scale up by 128" )]
		ScaleUpBy128 = 7,
		[Title( "Scale up by 256" )]
		ScaleUpBy256 = 8
	}

	/// <summary>
	/// Use to scale the resolution of the lightmap for this object.
	/// </summary>
	[Property( "lightmapscalebias", Title = "Lightmap Scale Bias" )]
	[Category( "Rendering" )]
	[DefaultValue( "0" )]
	public LightmapScaleBiasChoices LightmapScaleBias { get; set; }

	public enum BakeLightingChoices
	{
		Default = -1,
		No = 0,
		Yes = 1
	}

	[Property( "bakelighting", Title = "Bake Lighting" )]
	[Category( "Rendering" )]
	[DefaultValue( "-1" )]
	public BakeLightingChoices BakeLighting { get; set; }

	/// <summary>
	/// Render this object with other dynamic objects.
	/// </summary>
	[Property( "renderwithdynamic", Title = "Render with Dynamic Objects" )]
	[Category( "Rendering" )]
	[DefaultValue( "0" )]
	public bool RenderWithDynamic { get; set; }
}
