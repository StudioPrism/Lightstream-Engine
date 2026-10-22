namespace Sandbox.Audio;

[Expose]
public sealed class PitchProcessor : AudioProcessor<PitchProcessor.State>
{
	[Range( 0.5f, 2.0f )]
	public float Pitch { get; set; }

	public class State : ListenerState
	{
		internal CAudioProcessor _native;

		internal PerChannel<float> PreviousInput;
		internal PerChannel<float> PreviousOutput;

		private float _pitch = 1.0f;
		internal float Pitch
		{
			get => _pitch;
			set
			{
				if ( _pitch == value ) return;
				_pitch = value;
				_native.SetControlParameter( "pitchScale", value );
			}
		}

		public State()
		{
			_native = CAudioProcessor.CreatePitchShift( AudioEngine.ChannelCount );
		}

		protected override void OnDestroy()
		{
			MainThread.QueueDispose( _native );
			_native = default;
		}

		internal void Process( MultiChannelBuffer input, MultiChannelBuffer output )
		{
			_native.Process( input._native, output._native, Math.Min( input.ChannelCount, output.ChannelCount ) );
		}
	}

	internal override void Process( MultiChannelBuffer input, MultiChannelBuffer output )
	{
		CurrentState.Pitch = Pitch;
		CurrentState.Process( input, output );
	}
}
