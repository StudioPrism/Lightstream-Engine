namespace Sandbox.Audio;

[Expose]
public sealed class DelayProcessor : AudioProcessor<DelayProcessor.State>
{
	[Range( 0, 1 )]
	public float Delay { get; set; }

	[Range( 0, 1 )]
	public float Volume { get; set; }

	public class State : ListenerState
	{
		internal CAudioProcessor _native;

		private float _delay;
		internal float Delay
		{
			get => _delay;
			set
			{
				if ( value > 3.0f )
					value = 3.0f;

				if ( _delay == value ) return;
				_delay = value;
				_native.SetControlParameter( "delay", value * 1000.0f );
			}
		}

		private float _volume;
		internal float Volume
		{
			get => _volume;
			set
			{
				if ( _volume == value ) return;
				_volume = value;
				_native.SetControlParameter( "gain", _volume.Remap( 0, 1, -20, 0 ) );
			}
		}

		public State()
		{
			_native = CAudioProcessor.CreateDelay( AudioEngine.ChannelCount );

			Delay = 0.2f;
			Volume = 0.5f;
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
		CurrentState.Process( input, output );
	}
}
