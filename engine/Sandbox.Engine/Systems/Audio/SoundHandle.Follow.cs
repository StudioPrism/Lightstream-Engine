namespace Sandbox;

public partial class SoundHandle
{
	/// <summary>
	/// Update our position every frame relative to our parent
	/// </summary>
	public bool FollowParent { get; set; }

	/// <summary>
	/// If we're following a parent, our position will be this relative to them.
	/// </summary>
	public Transform LocalTransform { get; set; }

	/// <summary>
	/// If set with a parent and <cref name="FollowParent"/> is true, we will update our position to match the parent's world position. You can use <cref name="LocalTransform"/> to set an offset from the parent's position.
	/// Setting a parent also allows you to use GameObject.StopAllSounds on the parent to stop all sounds that are following it.
	/// This is set automatically when calling <cref name="GameObject.PlaySound"/> on a GameObject, but you can set it manually if you want to change the parent of an existing sound handle.
	/// </summary>
	public GameObject Parent { get; set; }

	void UpdateFollower()
	{
		if ( !FollowParent ) return;

		if ( !Parent.IsValid() )
		{
			Parent = default;
			FollowParent = false;
			return;
		}

		var parentTx = Parent.WorldTransform;
		Transform = parentTx.ToWorld( LocalTransform );
	}

	/// <summary>
	/// Clear our parent - stop following
	/// </summary>
	[Obsolete( "Just use Parent property directly" )]
	public void ClearParent()
	{
		Parent = default;
	}

	/// <summary>
	/// Tell the SoundHandle to follow this GameObject's position
	/// </summary>
	[Obsolete( "Just use Parent property directly" )]
	public void SetParent( GameObject obj )
	{
		Parent = obj;
	}
}

