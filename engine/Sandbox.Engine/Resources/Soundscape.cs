
namespace Sandbox;

/// <summary>
/// A soundscape is used for environmental ambiance of a map by playing a set of random sounds at given intervals.
/// </summary>
[AssetType( Name = "Soundscape", Extension = "sndscape", Category = "Sounds" )]
public class Soundscape : GameResource
{
	/// <summary>
	/// All sound volumes in this soundscape will be scaled by this value.
	/// </summary>
	[Range( 0.0f, 2.0f )]
	public RangedFloat MasterVolume { get; set; } = 1;

	/// <summary>
	/// Sounds that are played constantly on a loop.
	/// </summary>
	public List<LoopedSound> LoopedSounds { get; set; } = new();

	/// <summary>
	/// Sounds that are played at intervals.
	/// </summary>
	public List<StingSound> StingSounds { get; set; } = new();

	[Editor( "class" )]
	public class LoopedSound
	{
		/// <summary>
		/// The sound to play. It should have the looped flag set.
		/// </summary>
		public SoundFile SoundFile { get; set; }

		/// <summary>
		/// Sound volume.
		/// </summary>
		public RangedFloat Volume { get; set; } = 1;

		/// <summary>
		/// If true then the sound will come from a random direction in the world
		/// </summary>
		//public PositionMode PositionMode { get; set; }

		public override string ToString()
		{
			if ( SoundFile != null )
				return SoundFile.ResourceName;

			return "Looped Sound";
		}
	}

	[Editor( "class" )]
	public class StingSound
	{
		/// <summary>
		/// The sound event to play.
		/// </summary>
		public SoundEvent SoundFile { get; set; }

		/// <summary>
		/// How many instances of this sting should exist.
		/// </summary>
		public int InstanceCount { get; set; } = 1;

		/// <summary>
		/// How often should this sound be repeated.
		/// </summary>
		public RangedFloat RepeatTime { get; set; } = new RangedFloat( 1, 2 );

		/// <summary>
		/// How far away from the camera should the sound play.
		/// </summary>
		public RangedFloat Distance { get; set; } = new RangedFloat( 50, 100 );

		//public Vector3 PositionOffset { get; set; }

		public override string ToString()
		{
			if ( SoundFile != null )
				return SoundFile.ResourceName;

			return "Sting Sound";
		}
	}

	protected override Bitmap CreateAssetTypeIcon( int width, int height )
	{
		return CreateSimpleAssetTypeIcon( "spatial_tracking", width, height );
	}
}


