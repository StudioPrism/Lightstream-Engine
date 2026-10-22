namespace Editor.MapEditor.EntityDefinitions;

/// <summary>
/// A static prop that can play animations. If a door is wanted, please use the door entity.
/// </summary>
[Library( "prop_animated" )]
[Model( Archetypes = ModelArchetype.animated_model ), RenderFields, VisGroup( VisGroup.Dynamic ), Tag( "PropDynamic" )]
[Title( "Animated Entity" ), Category( "Gameplay" ), Icon( "animation" )]
class PropAnimated : HammerEntityDefinition
{
	/// <summary>
	/// The name of the idle animation that this prop will revert to whenever it finishes a random or forced animation.
	/// </summary>
	[Property, FGDType( "sequence" )] public string DefaultAnimation { set; get; }

	/// <summary>
	/// Allow this entity to use its animgraph
	/// </summary>
	[Property] public bool UseAnimationGraph { get; set; } = true;

	/// <summary>
	/// If set, the prop will not loop its animation, but hold the last frame.
	/// </summary>
	[Property] public bool HoldAnimation { get; set; } = true;

	/// <summary>
	/// Whether the animated prop should have collisions.
	/// </summary>
	[Property] public bool Collisions { get; set; } = true;

	/// <summary>
	/// If the model supports break pieces and has prop_data with health, this option can be used to allow the door to break like a normal prop would.
	/// </summary>
	[Property] public bool Breakable { get; set; } = true;

	/// <summary>
	/// Initial animation playback rate.
	/// </summary>
	[Property, MinMax( 0, 5 )] public bool AnimationSpeed { get; set; } = true;
}
