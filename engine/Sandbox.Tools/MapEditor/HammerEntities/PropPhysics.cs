namespace Editor.MapEditor.EntityDefinitions;

/// <summary>
/// A prop that physically simulates as a single rigid body. It can be constrained to other physics objects using hinges
/// or other constraints. It can also be configured to break when it takes enough damage.
/// Note that the health of the object will be overridden by the health inside the model, to ensure consistent health game-wide.
/// If the model used by the prop is configured to be used as a prop_animated (i.e. it should not be physically simulated) then it CANNOT be
/// used as a prop_physics. Upon level load it will display a warning in the console and remove itself. Use a prop_animated instead.
/// </summary>
[HammerEntity]
[Library( "prop_physics" )]
[Model, RenderFields, VisGroup( VisGroup.Physics ), PhysicsSimulated]
[Title( "Prop" ), Category( "Gameplay" ), Icon( "chair" )]
sealed class PropPhysicsEntity : HammerEntityDefinition
{
	/// <summary>
	/// If set, the prop will spawn with motion disabled and will act as a navigation blocker until broken.
	/// </summary>
	[Property]
	public bool Static { get; set; } = false;

	/// <summary>
	/// Set during map compile for multi physics body models based on Hammer physics simulation tool.
	/// </summary>
	[Property( "boneTransforms" ), Hide]
	private string BoneTransforms { get; set; }

	/// <summary>
	/// Multiplier for the object's mass.
	/// </summary>
	[Property( "massscale", Title = "Mass Scale" ), Category( "Physics" )]
	private float MassScale { get; set; } = 1.0f;

	/// <summary>
	/// Physics linear damping.
	/// </summary>
	[Property( "lineardamping", Title = "Linear Damping" ), Category( "Physics" )]
	private float LinearDamping { get; set; } = 0.0f;

	/// <summary>
	/// Physics angular damping.
	/// </summary>
	[Property( "angulardamping", Title = "Angular Damping" ), Category( "Physics" )]
	private float AngularDamping { get; set; } = 0.0f;
}
