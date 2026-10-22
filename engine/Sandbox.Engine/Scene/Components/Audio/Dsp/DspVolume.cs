using Sandbox.Audio;

namespace Sandbox;

[Title( "Dsp Volume" )]
[Group( "Audio" )]
[Icon( "graphic_eq" )]
[Tint( EditorTint.Green )]
public class DspVolume : Sandbox.Volumes.VolumeComponent
{
	[Property]
	public DspPresetHandle Dsp { get; set; }

	[Property]
	public MixerHandle TargetMixer { get; set; } = new MixerHandle { Name = "Game" };

	[Property]
	public int Priority { get; set; }
}

