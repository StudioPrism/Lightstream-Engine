namespace Sandbox.Audio;

/// <summary>
/// A wrapper around CAudioMixer, which is used in c++ to read from a wav etc.
/// This has two parts. 
/// 
///  o Sample - reads the samples, advances the index
///  o GetSamples - returns the samples.
///  
/// The reason it's coded like this is because each mix frame we read all samples, then
/// the mixers can do whatever they want with them. So rather than have the mixers fight
/// over reading and advancing, they can all get the samples if they want to.
/// </summary>
internal class AudioSampler : IDisposable
{
	CAudioMixer _native;
	MultiChannelBuffer buffer;

	internal AudioSampler( CAudioMixer native )
	{
		Assert.NotNull( native );
		_native = native;

		ChannelCount = native.GetChannelCount();
		Assert.True( ChannelCount > 0 );
		Assert.True( ChannelCount <= 8 );

		buffer = new MultiChannelBuffer( ChannelCount );
	}

	public void Dispose()
	{
		MainThread.QueueDispose( buffer );
		buffer = null;

		MainThread.QueueDispose( _native );
		_native = default;
	}

	public int ChannelCount { get; }

	public bool IsReadyToMix => _native.IsReadyToMix();

	public bool ShouldContinueMixing => _native.ShouldContinueMixing();

	public int SamplePosition
	{
		get => _native.GetSamplePosition();
		set => _native.SetSamplePosition( value );
	}

	internal void DelayOrSkipSamples( int numSamples )
	{
		_native.DelayOrSkipSamples( numSamples );
	}

	/// <summary>
	/// Read samples to our internal buffer and advance the index. 
	/// Pitch should be default 1.
	/// </summary>
	public void Sample( float pitch )
	{
		if ( !IsReadyToMix || !ShouldContinueMixing )
		{
			buffer.Silence();
			return;
		}

		Assert.AreEqual( ChannelCount, _native.GetChannelCount() );

		_native.ReadToBuffer( pitch, buffer._native );
	}

	/// <summary>
	/// Get the last read samples. This is called by the mixers.
	/// </summary>
	public MultiChannelBuffer GetLastReadSamples()
	{
		return buffer;
	}
}
