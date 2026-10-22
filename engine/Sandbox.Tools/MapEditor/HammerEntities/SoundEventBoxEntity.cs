namespace Editor.MapEditor.EntityDefinitions;

/// <summary>
/// Plays a sound event from a point, passes along the min and max positions of its AABB.
/// </summary>
[Library( "snd_event_alignedbox" ), HammerEntity]
[EditorSprite( "editor/snd_event.vmat" ), VisGroup( VisGroup.Sound )]
[Title( "Sound Event Aligned Box" ), Category( "Sound" ), Icon( "speaker_phone" )]
[BoundsHelper( "mins", "maxs", true, false )]
class SoundEventBoxEntity : HammerEntityDefinition
{
	/// <summary>
	/// Name of the sound to play.
	/// </summary>
	[Property( "soundName" ), FGDType( "sound" )]
	public string SoundName { get; set; }

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

	[Property( "mins", Title = "Box Mins" ), Category( "Box Size" )]
	[DefaultValue( "-32 -32 -32" )]
	public Vector3 Mins { get; set; } = new Vector3( -32, -32, -32 );

	[Property( "maxs", Title = "Box Maxs" ), Category( "Box Size" )]
	[DefaultValue( "32 32 32" )]
	public Vector3 Maxs { get; set; } = new Vector3( 32, 32, 32 );
}
