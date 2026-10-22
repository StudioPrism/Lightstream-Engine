namespace Sandbox.Audio;

/// <summary>
/// Defines a DSP preset. This is a collection of DSP processors that can be applied to a sound.
/// These originate from Half-Life 1's DSP system, and give that iconic Source Engine Sound.
/// </summary>
class DspPreset
{
	public string Name { get; }

	global::DspPreset _native;
	bool isSealed = false;

	public DspPreset( string name )
	{
		Name = name;
		_native = global::DspPreset.Create( Name );

		DspFactory.Register( this );
	}

	public static DspPreset FindByName( string name )
	{
		if ( DspFactory.All.TryGetValue( name, out var preset ) )
			return preset;

		return null;
	}

	internal DspInstance Instantiate( int channels )
	{
		if ( !isSealed )
		{
			isSealed = true;
			_native.FinishBuilding();
		}

		Assert.True( _native.IsValid );
		return new DspInstance( this, channels );
	}

	internal global::DspPreset GetNative() => _native;

	internal unsafe void AddProcessor( int v, float[] values )
	{
		fixed ( float* ptr = values )
		{
			_native.AddProcessor( v, ptr, (uint)values.Length );
		}
	}

	internal void AddReverb( float msDelayMax = 80, float msDelayMin = 30, float delayCount = 4, float feedack = 0.85f, float gain = 1.1f, float cutoffFreq = 4000, bool parallel = true, float modDepthMs = 0, float modRate = 0 )
	{
		AddProcessor( 2, new[] { msDelayMax, msDelayMin, delayCount, feedack, gain, cutoffFreq, parallel ? 1 : 0, modDepthMs, modRate, 0, 0, 0, 0, 0, 0, 0 } );
	}

	internal void AddDiffusor( float delayScale, float delayCount, float feedbackScale, float gain )
	{
		AddProcessor( 10, new[] { delayScale, delayCount, feedbackScale, gain } );
	}

	internal void AddAmplifier( float gain, float distortionThreshold, float distortionMix, float distortionFeedback, float modRate, float modDepth, float modGlide, bool randomMod = false )
	{
		AddProcessor( 11, new[] { gain, distortionThreshold, distortionMix, distortionFeedback, modRate, modDepth, modGlide, randomMod ? 1 : 0 } );
	}

	public enum DelayType
	{
		Plain,
		AllPass,
		LowPass,
		Linear,
		FilteredLinear,
		LowPass4Tap,
		Paint4Tap
	}

	public enum FilterType
	{
		LowPass,
		HighPass,
		BandPass
	}

	public enum Quality
	{
		Low,
		Medium,
		High,
		VeryHigh
	}

	internal void AddModDelay( DelayType delayType, float msDelay, float feedback, float gain, FilterType filterType, float cutoff, float width, Quality quality, float modRate, float modDepth, float modGlide, float mix = 1.0f )
	{
		AddProcessor( 9, new[] { (float)delayType, msDelay, feedback, gain, (float)filterType, cutoff, width, (float)quality, modRate, modDepth, modGlide, mix } );
	}

	internal void AddDelay( DelayType delayType, float msDelay, float feedback, float gain, FilterType filterType, float cutoff, float width = 0, Quality quality = Quality.Low, float tap1 = 0.0f, float tap2 = 0.0f, float tap3 = 0.0f )
	{
		AddProcessor( 1, new[] { (float)delayType, msDelay, feedback, gain, (float)filterType, cutoff, width, (float)quality, tap1, tap2, tap3 } );
	}

	internal void AddFilter( FilterType ftype, float cutoff, float width, Quality quality, float gain )
	{
		AddProcessor( 3, new[] { (float)ftype, cutoff, width, (float)quality, gain } );
	}
}
