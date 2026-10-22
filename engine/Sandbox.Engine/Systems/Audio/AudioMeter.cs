namespace Sandbox.Audio;

/// <summary>
/// Allows the capture and monitor of an audio source
/// </summary>
public class AudioMeter
{
	public struct Frame
	{
		public float MaxLevelLeft { get; set; }
		public float MaxLevelRight { get; set; }
		public float MaxLevel => Math.Max( MaxLevelLeft, MaxLevelRight );

		/// <summary>
		/// The amount of individual voices playing
		/// </summary>
		public int VoiceCount { get; set; }
	}

	List<Frame> Frames = new( 64 );

	internal AudioMeter()
	{
		Add( new Frame() );
	}

	internal void Add( MultiChannelBuffer mix, int voiceCount )
	{
		Frame f = new Frame();
		f.VoiceCount = voiceCount;
		f.MaxLevelLeft = mix.Get( AudioChannel.Left ).LevelMax;

		if ( mix.ChannelCount > 1 )
		{
			f.MaxLevelRight = mix.Get( AudioChannel.Right ).LevelMax;
		}
		else
		{
			f.MaxLevelLeft = f.MaxLevelRight;
		}

		Add( f );
	}

	void Add( in Frame frame )
	{
		lock ( Frames )
		{
			if ( Frames.Count > 64 )
				Frames.RemoveAt( Frames.Count - 1 );

			Frames.Insert( 0, frame );
		}
	}

	public Frame Current
	{
		get
		{
			lock ( Frames )
			{
				return Frames.LastOrDefault();
			}
		}
	}
}
