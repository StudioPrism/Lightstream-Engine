namespace Editor.MapEditor.EntityDefinitions;

/// <summary>
/// An entity that places an overlay on the world
/// </summary>
[Library( "info_overlay" )]
[EditorModel( "models/editor/overlay_helper" )]
[HammerEntity]
[Title( "Overlay" ), Icon( "layers" )]
[Sphere( "fademindist" )]
[Sphere( "fademaxdist" )]
[SimpleHelper( "overlay" )]
sealed class InfoOverlayEntity : HammerEntityDefinition
{
	[Property( "width", Title = "Overlay Width" ), DefaultValue( "-1.0" )]
	public float OverlayWidth { get; set; } = -1.0f;

	[Property( "height", Title = "Overlay Height" ), DefaultValue( "-1.0" )]
	public float OverlayHeight { get; set; } = -1.0f;

	[Property( "material", Title = "Material" ), DefaultValue( "materials/decals/decalgraffiti001c.vmat" )]
	public Material Material { get; set; }

	[Property( "RenderOrder", Title = "Render Order" ), DefaultValue( "0" )]
	public int RenderOrder { get; set; } = 0;

	[Property( "sequence", Title = "Sequence Index" ), DefaultValue( "-1" )]
	public int SequenceIndex { get; set; } = -1;

	[Property( "depth", Title = "Overlay Depth" ), DefaultValue( "1.0" )]
	public float OverlayDepth { get; set; } = 1.0f;

	[Property( "startu", Title = "U Start" ), DefaultValue( "0.0" )]
	public float UStart { get; set; } = 0.0f;

	[Property( "endu", Title = "U End" ), DefaultValue( "1.0" )]
	public float UEnd { get; set; } = 1.0f;

	[Property( "startv", Title = "V Start" ), DefaultValue( "0.0" )]
	public float VStart { get; set; } = 0.0f;

	[Property( "endv", Title = "V End" ), DefaultValue( "1.0" )]
	public float VEnd { get; set; } = 1.0f;

	[Property( "centeru", Title = "Center U" ), DefaultValue( "0.5" )]
	public float CenterU { get; set; } = 0.5f;

	[Property( "centerv", Title = "Center V" ), DefaultValue( "0.5" )]
	public float CenterV { get; set; } = 0.5f;

	[Property( "fademindist", Title = "Start Fade Dist" ), DefaultValue( "-1" )]
	public float StartFadeDist { get; set; } = -1.0f;

	[Property( "fademaxdist", Title = "End Fade Dist" ), DefaultValue( "0" )]
	public float EndFadeDist { get; set; } = 0.0f;

	[Property( "rendercolor", Title = "Color (R G B)" ), DefaultValue( "255 255 255" )]
	public Color32 RenderColor { get; set; }
}
