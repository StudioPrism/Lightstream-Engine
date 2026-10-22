
namespace Sandbox;

public partial class Component
{
	/// <summary>
	/// The world transform of the game object.
	/// </summary>
	[ActionGraphInclude, Group( "Transform/World" )]
	public Transform WorldTransform
	{
		get => GameObject.WorldTransform;
		set => GameObject.WorldTransform = value;
	}

	/// <summary>
	/// The world position of the game object.
	/// </summary>
	[ActionGraphInclude, Group( "Transform/World" )]
	public Vector3 WorldPosition
	{
		get => WorldTransform.Position;
		set => WorldTransform = WorldTransform.WithPosition( value );
	}

	/// <summary>
	/// The world rotation of the game object.
	/// </summary>
	[ActionGraphInclude, Group( "Transform/World" )]
	public Rotation WorldRotation
	{
		get => WorldTransform.Rotation;
		set => WorldTransform = WorldTransform.WithRotation( value );
	}

	/// <summary>
	/// The world scale of the game object.
	/// </summary>
	[ActionGraphInclude, Group( "Transform/World" )]
	public Vector3 WorldScale
	{
		get => WorldTransform.Scale;
		set => WorldTransform = WorldTransform.WithScale( value );
	}
}
