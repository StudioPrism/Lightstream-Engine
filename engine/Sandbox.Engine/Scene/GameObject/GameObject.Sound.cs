
namespace Sandbox;

public partial class GameObject
{
	/// <summary>
	/// Play this sound on this GameObject. The sound will follow the position of the GameObject.
	/// You'll be able to use GameObject.StopAllSounds to stop all sounds that are following this GameObject.
	/// </summary>
	[ActionGraphInclude]
	public SoundHandle PlaySound( SoundEvent sound, Vector3 positionOffset = default )
	{
		if ( !sound.IsValid() )
			return default;

		var sh = Sound.Play( sound, WorldPosition );

		if ( sh is null )
			return default;

		sh.Parent = this;
		sh.FollowParent = true;
		sh.LocalTransform = new Transform( positionOffset );

		return sh;
	}

	/// <summary>
	/// Stop any sounds playing on this GameObject
	/// </summary>
	[ActionGraphInclude]
	public void StopAllSounds( float fadeOutTime = 0.0f )
	{
		SoundHandle.StopAllWithParent( this, fadeOutTime );
	}

}
