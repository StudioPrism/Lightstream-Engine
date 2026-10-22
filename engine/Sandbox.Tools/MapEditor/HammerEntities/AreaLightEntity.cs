namespace Editor.MapEditor.EntityDefinitions;

/// <summary>
/// An omni-directional light entity.
/// </summary>
[Library( "light_capsule" ), HammerEntity]
[EditorModel( "models/editor/tube", "rgb(0, 255, 192)", "rgb(255, 64, 64)" )]
[Sphere( "lightsourceradius", IsLean = true ), Sphere( "range", 255, 255, 0 )]
[Light, VisGroup( VisGroup.Lighting ), CanBeClientsideOnly]
[HideProperty( "enable_shadows" )]
[Title( "Capsule Light" ), Category( "Lighting" ), Icon( "wb_iridescent" ), Description( "A tube-shaped light entity." )]
class CapsuleLightEntity : PointLightEntity
{
	[Property( "lightsourcedim1" ), Category( "Shape" ), DefaultValue( 64.0f ), MinMax( 0, 512 ), Description( "Length of the light." )]
	public float CapsuleLength
	{
		get => default;
		set { }
	}

	[Property( "lightsourcedim0" ), Category( "Shape" ), DefaultValue( 10.0f ), MinMax( 0, 512 ), Description( "Radius of the light." )]
	public float CapsuleRadius
	{
		get => default;
		set { }
	}
}

/// <summary>
/// An omni-directional light entity.
/// </summary>
[Library( "light_rect" ), HammerEntity]
[EditorModel( "models/editor/rect", "rgb(0, 255, 192)", "rgb(255, 64, 64)" )]
[Sphere( "lightsourceradius", IsLean = true ), Sphere( "range", 255, 255, 0 )]
[Light, VisGroup( VisGroup.Lighting ), CanBeClientsideOnly]
[HideProperty( "enable_shadows" )]
[Title( "Rectangle Light" ), Category( "Lighting" ), Icon( "backlight_low" ), Description( "A rectangle/disk-shaped light entity." )]
class RectangleLightEntity : PointLightEntity
{
	public RectangleLightEntity()
	{

	}
	[Property( "lightsourcedim0" ), Category( "Shape" ), DefaultValue( 32.0f ), MinMax( 0, 512 ), Description( "Height of the light." )]
	public float PlaneWidth
	{
		get => default;
		set { }
	}
	[Property( "lightsourcedim1" ), Category( "Shape" ), DefaultValue( 32.0f ), MinMax( 0, 512 ), Description( "Height of the light." )]
	public float PlaneHeight
	{
		get => default;
		set { }
	}
}
