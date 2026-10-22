
namespace Sandbox;

public partial class GameObject
{
	/// <summary>
	/// The local transform of the game object.
	/// </summary>
	[ActionGraphInclude, Group( "Transform/Local" )]
	public Transform LocalTransform
	{
		get => _gameTransform.Local;
		set => _gameTransform.Local = value;
	}

	/// <summary>
	/// The local position of the game object.
	/// </summary>
	[ActionGraphInclude, Group( "Transform/Local" )]
	public Vector3 LocalPosition
	{
		get => LocalTransform.Position;
		set => LocalTransform = LocalTransform.WithPosition( value );
	}

	/// <summary>
	/// The local rotation of the game object.
	/// </summary>
	[ActionGraphInclude, Group( "Transform/Local" )]
	public Rotation LocalRotation
	{
		get => LocalTransform.Rotation;
		set => LocalTransform = LocalTransform.WithRotation( value );
	}

	/// <summary>
	/// The local scale of the game object.
	/// </summary>
	[ActionGraphInclude, Group( "Transform/Local" )]
	public Vector3 LocalScale
	{
		get => LocalTransform.Scale;
		set => LocalTransform = LocalTransform.WithScale( value );
	}
}
