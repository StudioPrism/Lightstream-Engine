namespace Editor.MapEditor.EntityDefinitions;

/// <summary>
/// Plays a sound event from a point. The point can be this entity or a specified entity's position.
/// </summary>
[Library( "snd_event_point" ), HammerEntity]
[EditorSprite( "editor/snd_event.vmat" ), VisGroup( VisGroup.Sound )]
[Title( "Sound Event" ), Category( "Sound" ), Icon( "volume_up" )]
class SoundEventEntity : HammerEntityDefinition
{
	/// <summary>
	/// Name of the sound to play.
	/// </summary>
	[Property( "soundName" ), FGDType( "sound" )]
	public string SoundName { get; set; }

	/// <summary>
	/// The entity to use as the origin of the sound playback. If not set, will play from this snd_event_point.
	/// </summary>
	[Property( "sourceEntityName" ), FGDType( "target_destination" )]
	public string SourceEntityName { get; set; }

	/// <summary>
	/// Start the sound on spawn
	/// </summary>
	[Property( "startOnSpawn" )]
	public bool StartOnSpawn { get; set; }

	/// <summary>
	/// Stop the sound before starting to play it again
	/// </summary>
	[Property( "stopOnNew" ), Title( "Stop before repeat" )]
	public bool StopOnNew { get; set; }

	/// <summary>
	/// Setting this to true will override default sound parameters
	/// </summary>
	[Property( "overrideParams" ), Title( "Override Default" ), Category( "Sound Parameters" )]
	public bool OverrideSoundParams { get; set; } = false;

	/// <summary>
	/// Set the volume of the sound
	/// </summary>
	[Property( "soundVolume" ), Title( "Volume" ), Category( "Sound Parameters" )]
	[MinMax( 0.0f, 1.0f )]
	public float SoundVolume { get; set; } = 1.0f;

	/// <summary>
	/// Set the pitch of the sound
	/// </summary>
	[Property( "soundPitch" ), Title( "Pitch" ), Category( "Sound Parameters" )]
	[MinMax( 0.0f, 2.0f )]
	public float SoundPitch { get; set; } = 1.0f;
}
