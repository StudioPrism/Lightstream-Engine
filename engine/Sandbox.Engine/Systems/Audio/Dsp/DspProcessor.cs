namespace Sandbox.Audio;

[Expose]
public sealed class DspProcessor : AudioProcessor<DspProcessor.State>
{
	public DspPresetHandle Effect { get; set; }

	public DspProcessor() : this( "tunnel.medium" )
	{

	}

	public DspProcessor( string dspName )
	{
		Effect = dspName;
		UpdateEffect();
	}

	void UpdateEffect()
	{
		CurrentState?.UpdateEffect( Effect );
	}

	internal override void Process( MultiChannelBuffer input, MultiChannelBuffer output )
	{
		UpdateEffect();

		if ( CurrentState.Instance is null )
		{
			output.CopyFrom( input );
			return;
		}

		int channels = Math.Min( input.ChannelCount, output.ChannelCount );

		for ( int channel = 0; channel < channels; channel++ )
		{
			CurrentState.Instance.Process( input.Get( channel ), output.Get( channel ), channel );
		}
	}

	public override string ToString()
	{
		return $"Dsp - {Effect.Name}";
	}

	public class State : ListenerState
	{
		internal DspInstance Instance { get; private set; }

		private DspPreset _preset;

		internal void UpdateEffect( DspPresetHandle effect )
		{
			var currentName = _preset?.Name ?? "";

			if ( effect != currentName )
			{
				var newPreset = DspPreset.FindByName( effect );
				SwitchPreset( newPreset );
			}
		}

		private void SwitchPreset( DspPreset newPreset )
		{
			if ( newPreset == _preset )
				return;

			_preset = newPreset;

			Instance?.Dispose();
			Instance = default;

			if ( _preset is null )
			{
				return;
			}

			Instance = _preset.Instantiate( AudioEngine.ChannelCount );
		}

		protected override void OnDestroy()
		{
			Instance?.Dispose();
			Instance = null;
		}
	}
}
