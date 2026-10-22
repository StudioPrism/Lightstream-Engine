namespace Editor.MapEditor.EntityDefinitions;

/// <summary>
/// Specifies fog based on a color gradient
/// </summary>
[Library( "env_gradient_fog" ), HammerEntity]
[VisGroup( VisGroup.Lighting ), Global( "gradient_fog" )]
[EditorSprite( "editor/env_gradient_fog.vmat" ), SimpleHelper( "gradientfog" )]
[HideProperty( "enable_shadows" )]
[Title( "Gradient Fog" ), Category( "Fog & Sky" ), Icon( "gradient" )]
class GradientFogEntity : HammerEntityDefinition
{
	/// <summary>
	/// Whether the fog is enabled or not.
	/// </summary>
	[Property( "fogenabled" ), Title( "Fog Enabled" )]
	public bool FogEnabled { get; set; } = true;

	/// <summary>
	/// For start distance.
	/// </summary>
	[Property( "fogstart" ), Title( "Fog Start Distance" )]
	public float FogStartDistance { get; set; } = 0.0f;

	/// <summary>
	/// Fog end distance.
	/// </summary>
	[Property( "fogend" ), Title( "Fog End Distance" )]
	public float FogEndDistance { get; set; } = 4000.0f;

	/// <summary>
	/// Fog start height.
	/// </summary>
	[Property( "fogstartheight" ), Title( "Fog Start Height" )]
	public float FogStartHeight { get; set; } = 0.0f;

	/// <summary>
	/// Fog end height.
	/// </summary>
	[Property( "fogendheight" ), Title( "Fog End Height" )]
	public float FogEndHeight { get; set; } = 200.0f;

	/// <summary>
	/// Set the maximum opacity at the base of the gradient fog.
	/// </summary>
	[Property( "fogmaxopacity" ), Title( "Fog Maximum Opacity" )]
	public float FogMaximumOpacity { get; set; } = 0.5f;

	/// <summary>
	/// Set the gradient fog color.
	/// </summary>
	[Property( "fogcolor" ), Title( "Fog Color (R G B)" )]
	[DefaultValue( "255 255 255 255" )]
	public Color FogColor { get; set; }

	/// <summary>
	/// Fog strength.
	/// </summary>
	[Property( "fogstrength" ), Title( "Fog Strength" )]
	public float FogStrength { get; set; } = 1.0f;

	/// <summary>
	/// Exponent for distance falloff.
	/// </summary>
	[Property( "fogfalloffexponent" ), Title( "Distance Falloff Exponent" )]
	public float FogDistanceFalloffExponent { get; set; } = 2.0f;

	/// <summary>
	/// "Exponent for vertical falloff."
	/// </summary>
	[Property( "fogverticalexponent" ), Title( "Vertical Falloff Exponent" )]
	public float FogVerticalFalloffExponent { get; set; } = 1.0f;

	/// <summary>
	/// How much time it takes to fade in new values.
	/// </summary>
	[Property( "fadetime" ), Title( "Fade Time" )]
	public float FogFadeTime { get; set; } = 1.0f;
}
