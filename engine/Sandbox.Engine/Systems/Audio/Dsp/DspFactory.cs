using static Sandbox.Audio.DspPreset;

namespace Sandbox.Audio;

static class DspFactory
{
	internal static Dictionary<string, DspPreset> All = new Dictionary<string, DspPreset>( StringComparer.OrdinalIgnoreCase );

	internal static void Register( DspPreset dspPreset )
	{
		All[dspPreset.Name] = dspPreset;

		// Keep the DspName array up to date
		Sound.DspNames = All.Keys.Order().ToArray();
	}

	internal static void CreateBuiltIn()
	{
		{
			var dsp = new DspPreset( "metallic.small" );
			dsp.AddReverb( 80, 30, 4, 0.85f, 1.1f, 4000 );
		}

		{
			var dsp = new DspPreset( "metallic.medium" );
			dsp.AddReverb( 80, 30, 4, 0.9f, 1.4f, 4000 );
		}

		{
			var dsp = new DspPreset( "metallic.large" );
			dsp.AddDiffusor( 1.0f, 3.0f, 0.1483f, 0.0f );
			dsp.AddReverb( 100, 30, 4, 0.95f, 1.8f, 4000 );
		}

		{
			var dsp = new DspPreset( "tunnel.small" );
			dsp.AddReverb( 50.0f, 8.0f, 2.0f, 0.92f, 1.1f, 6000 );
		}

		{
			var dsp = new DspPreset( "tunnel.medium" );
			dsp.AddDiffusor( 1.0f, 2.0f, 0.15f, 0.0f );
			dsp.AddReverb( 100.0f, 15.0f, 2.0f, 0.92f, 1.1f, 5000.0f );
		}

		{
			var dsp = new DspPreset( "tunnel.large" );
			dsp.AddDiffusor( 1.0f, 3.0f, 0.15f, 0.0f );
			dsp.AddReverb( 120.0f, 25.0f, 2.0f, 0.95f, 1.1f, 4000.0f );
		}

		{
			var dsp = new DspPreset( "chamber.small" );
			dsp.AddDiffusor( 1.0f, 2.0f, 0.15f, 0.0f );
			dsp.AddReverb( 50.0f, 20.0f, 6.0f, 0.9f, 1.4f, 5000.0f, true, 4.0f, 3.48f );
		}

		{
			var dsp = new DspPreset( "chamber.medium" );
			dsp.AddDiffusor( 1.0f, 2.0f, 0.15f, 0.0f );
			dsp.AddReverb( 50.0f, 20.0f, 6.0f, 0.9f, 1.4f, 6000.0f, true, 4.0f, 3.48f );
		}

		{
			var dsp = new DspPreset( "chamber.large" );
			dsp.AddDiffusor( 1.0f, 2.0f, 0.15f, 0.0f );
			dsp.AddReverb( 50.0f, 20.0f, 9.0f, 0.9f, 1.4f, 6000.0f, true, 4.0f, 3.48f );
		}

		{
			var dsp = new DspPreset( "brite.small" );
			dsp.AddReverb( 50.0f, 20.0f, 3.0f, 0.9f, 1.0f, 5000.0f );
		}

		{
			var dsp = new DspPreset( "brite.medium" );
			dsp.AddReverb( 50.0f, 20.0f, 5.0f, 0.9f, 1.0f, 5000.0f );
		}

		{
			var dsp = new DspPreset( "brite.large" );
			dsp.AddReverb( 50.0f, 20.0f, 6.0f, 0.9f, 1.0f, 6000.0f, false );
		}

		{
			var dsp = new DspPreset( "water.small" );
			dsp.AddDiffusor( 1.0f, 3.0f, 0.15f, 0.0f );
			dsp.AddAmplifier( 1.0f, 0.0f, 0.0f, 0.0f, 10.0f, 0.6f, 80.0f );
			dsp.AddReverb( 82.0f, 59.0f, 2.0f, 0.4f, 2.0f, 1800.0f, false, 10.0f, 3.0f );
		}

		{
			var dsp = new DspPreset( "water.medium" );
			dsp.AddDiffusor( 1.0f, 2.0f, 0.15f, 0.0f );
			dsp.AddReverb( 50.0f, 20.0f, 5.0f, 0.9f, 1.4f, 1000.0f, false, 4.0f, 3.48f );
		}

		{
			var dsp = new DspPreset( "water.large" );
			dsp.AddDiffusor( 1.0f, 2.0f, 0.15f, 0.0f );
			dsp.AddReverb( 50.0f, 20.0f, 7.0f, 0.9f, 1.0f, 1000.0f, false, 4.0f, 3.48f );
			dsp.AddModDelay( DspPreset.DelayType.Plain, 500.0f, 0.4f, 1.0f, DspPreset.FilterType.LowPass, 0.0f, 0.0f, DspPreset.Quality.Low, 2.0f, 0.01f, 15.0f, 1.0f );
		}

		{
			var dsp = new DspPreset( "concrete.small" );
			dsp.AddDiffusor( 1.0f, 2.0f, 0.15f, 0.0f );
			dsp.AddReverb( 50.0f, 20.0f, 6.0f, 0.9f, 1.4f, 4000.0f, true, 4.0f, 3.48f );
		}

		{
			var dsp = new DspPreset( "concrete.medium" );
			dsp.AddDiffusor( 1.0f, 2.0f, 0.15f, 0.0f );
			dsp.AddReverb( 50.0f, 20.0f, 7.0f, 0.9f, 1.4f, 3500.0f, true, 4.0f, 3.48f );
		}

		{
			var dsp = new DspPreset( "concrete.large" );
			dsp.AddDiffusor( 1.0f, 2.0f, 0.15f, 0.0f );
			dsp.AddReverb( 50.0f, 20.0f, 8.0f, 0.9f, 1.4f, 3000.0f, true, 4.0f, 3.48f );
		}

		{
			var dsp = new DspPreset( "outside.small" );
			dsp.AddDiffusor( 1.0f, 2.0f, 0.15f, 0.0f );
			dsp.AddDelay( DspPreset.DelayType.LowPass, 300.0f, 0.2f, 0.84f, DspPreset.FilterType.LowPass, 2000.0f );
		}

		{
			var dsp = new DspPreset( "outside.medium" );
			dsp.AddDiffusor( 1.0f, 2.0f, 0.15f, 0.0f );
			dsp.AddDelay( DspPreset.DelayType.LowPass, 400.0f, 0.4f, 0.84f, DspPreset.FilterType.LowPass, 1500.0f );
		}

		{
			var dsp = new DspPreset( "outside.large" );
			dsp.AddDiffusor( 1.0f, 2.0f, 0.15f, 0.0f );
			dsp.AddDelay( DspPreset.DelayType.LowPass, 750.0f, 0.5f, 0.84f, DspPreset.FilterType.LowPass, 1000.0f );
		}

		{
			var dsp = new DspPreset( "cavern.small" );
			dsp.AddDelay( DelayType.LowPass, 150.0f, 0.5f, 0.84f, FilterType.LowPass, 3000.0f, 0.0f, Quality.Low );
			dsp.AddReverb( 50.0f, 20.0f, 1.3f, 0.9f, 1.0f, 1500.0f, true, 4.0f, 3.48f );
		}

		{
			var dsp = new DspPreset( "cavern.medium" );
			dsp.AddDelay( DelayType.LowPass, 200.0f, 0.7f, 0.6f, FilterType.LowPass, 3000.0f, 0.0f, Quality.Low );
			dsp.AddReverb( 50.0f, 20.0f, 7.0f, 0.9f, 1.0f, 1500.0f, true, 4.0f, 3.48f );
		}

		{
			var dsp = new DspPreset( "cavern.large" );
			dsp.AddDiffusor( 1.0f, 2.0f, 0.15f, 0.0f );
			dsp.AddDelay( DelayType.LowPass, 300.0f, 0.7f, 0.6f, FilterType.LowPass, 3000.0f, 0.0f, Quality.Low );
			dsp.AddReverb( 50.0f, 20.0f, 9.0f, 0.9f, 1.0f, 1500.0f, true, 4.0f, 3.48f );
		}

		{
			var dsp = new DspPreset( "weird.1" );
			dsp.AddDelay( DelayType.LowPass, 400.0f, 0.5f, 0.6f, FilterType.LowPass, 1500.0f, 0.0f, Quality.Low );
			dsp.AddDiffusor( 1.0f, 2.0f, 0.15f, 0.0f );
		}

		{
			var dsp = new DspPreset( "weird.2" );
			dsp.AddDelay( DelayType.LowPass, 400.0f, 0.5f, 0.6f, FilterType.LowPass, 1500.0f, 0.0f, Quality.Low );
			dsp.AddDiffusor( 1.0f, 2.0f, 0.15f, 0.0f );
		}

		{
			var dsp = new DspPreset( "weird.3" );
			dsp.AddDelay( DelayType.LowPass, 400.0f, 0.5f, 0.6f, FilterType.LowPass, 1500.0f, 0.0f, Quality.Low );
			dsp.AddDiffusor( 1.0f, 2.0f, 0.15f, 0.0f );
		}

		{
			var dsp = new DspPreset( "weird.4" );
			dsp.AddDelay( DelayType.LowPass, 400.0f, 0.5f, 0.6f, FilterType.LowPass, 1500.0f, 0.0f, Quality.Low );
			dsp.AddDiffusor( 1.0f, 2.0f, 0.15f, 0.0f );
		}

		{
			var dsp = new DspPreset( "lowpass.facing_away" );
			dsp.AddFilter( FilterType.LowPass, 3000.0f, 0.0f, Quality.Medium, 1.0f );
		}

		{
			var dsp = new DspPreset( "lowpass.facing_away_with_delay" );
			dsp.AddFilter( FilterType.LowPass, 1000.0f, 0.0f, Quality.Medium, 1.0f );
			dsp.AddDelay( DelayType.Linear, 80.0f, 0.0f, 1.0f, 0, 0.0f, 0.0f, Quality.Low );
		}

		{
			var dsp = new DspPreset( "room.empty.small" );
			dsp.AddReverb( 80.0f, 30.0f, 2.0f, 0.78f, 1.1f, 6000.0f, true, 0.0f, 0.0f );
		}

		{
			var dsp = new DspPreset( "room.empty.huge" );
			dsp.AddDiffusor( 1.0f, 3.0f, 0.15f, 0.0f );
			dsp.AddReverb( 240.0f, 50.0f, 10.0f, 0.97f, 2.4f, 1800.0f, true, 0.0f, 0.0f );
		}

		{
			var dsp = new DspPreset( "room.diffuse.small" );
			dsp.AddDiffusor( 1.0f, 2.0f, 0.15f, 0.0f );
			dsp.AddReverb( 80.0f, 30.0f, 3.0f, 0.78f, 1.4f, 5000.0f, true, 4.0f, 2.0f );
		}

		{
			var dsp = new DspPreset( "room.diffuse.huge" );
			dsp.AddDiffusor( 1.0f, 3.0f, 0.15f, 0.0f );
			dsp.AddReverb( 240.0f, 50.0f, 12.0f, 0.97f, 2.4f, 1600.0f, true, 6.0f, 2.0f );
		}

		{
			var dsp = new DspPreset( "duct.empty.small" );
			dsp.AddReverb( 150.0f, 10.0f, 2.0f, 0.90f, 2.0f, 6000.0f, true, 0.0f, 0.0f );
		}

		{
			var dsp = new DspPreset( "duct.empty.huge" );
			dsp.AddDiffusor( 1.0f, 2.0f, 0.1483f, 0.0f );
			dsp.AddReverb( 300.0f, 12.0f, 3.0f, 0.95f, 2.0f, 2000.0f, true, 0.0f, 0.0f );
		}

		{
			var dsp = new DspPreset( "duct.diffuse.small" );
			dsp.AddReverb( 150.0f, 10.0f, 2.0f, 0.90f, 2.0f, 6000.0f, true, 0.0f, 0.0f );
		}

		{
			var dsp = new DspPreset( "duct.diffuse.huge" );
			dsp.AddDiffusor( 1.0f, 2.0f, 0.1483f, 0.0f );
			dsp.AddReverb( 300.0f, 12.0f, 3.0f, 0.95f, 2.0f, 2000.0f, true, 0.0f, 0.0f );
		}

		{
			var dsp = new DspPreset( "robotic.voice" );
			dsp.AddReverb( 60.0f, 5.0f, 1.0f, 0.98f, 1.0f, 1000.0f, true, 0.1f, 0.1f );
		}

	}
}
