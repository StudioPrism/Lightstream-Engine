namespace Sandbox.Audio;

/// <summary>
/// Just a test - don't count on this sticking around
/// </summary>
[Expose]
public sealed class HighPassProcessor : AudioProcessor<HighPassProcessor.State>
{
	/// <summary>
	/// Cutoff frequency of the high-pass filter (0 to 1, where 1 is Nyquist frequency).
	/// </summary>
	[Range( 0, 1 )]
	public float Cutoff { get; set; } = 0.5f;

	public class State : ListenerState
	{
		internal PerChannel<float> PreviousInput;
		internal PerChannel<float> PreviousOutput;
	}

	/// <summary>
	/// Processes each channel individually using a simple one-pole high-pass filter.
	/// </summary>
	protected override unsafe void ProcessSingleChannel( AudioChannel channel, Span<float> input )
	{
		if ( input.Length == 0 ) return;

		float alpha = Cutoff;
		float prevIn = CurrentState.PreviousInput.Get( channel );
		float prevOut = CurrentState.PreviousOutput.Get( channel );

		for ( int i = 0; i < input.Length; i++ )
		{
			float current = input[i];
			input[i] = prevOut + alpha * (current - prevIn);
			prevIn = current;
			prevOut = input[i];
		}

		CurrentState.PreviousInput.Set( channel, prevIn );
		CurrentState.PreviousOutput.Set( channel, prevOut );
	}
}
