using System.Text.Json.Nodes;

namespace Sandbox;

public sealed partial class AmbientOcclusion
{
	[Obsolete]
	public enum SampleQuality
	{
		[Icon( "power_off" )]
		Off = -1,
		/// <summary>
		/// 9 samples
		/// </summary>
		[Icon( "workspaces" )]
		Low = 0,
		/// <summary>
		/// 16 samples
		/// </summary>
		[Icon( "grain" )]
		Medium = 1,
		/// <summary>
		/// 25 samples
		/// </summary>
		[Icon( "blur_on" )]
		High = 2
	}

	[Obsolete]
	public SampleQuality Quality { get; set; }

	[JsonUpgrader( typeof( AmbientOcclusion ), 2 )]
	static void Upgrader_v2( JsonObject obj )
	{
		obj.Remove( "Quality" );
	}

	[Expose, JsonUpgrader( typeof( AmbientOcclusion ), 1 )]
	static void Upgrader_v1( JsonObject obj )
	{
		// Remove old settings, we just want to use the defaults
		// no point trying to find equivilents here
		obj.Remove( "Radius" );
		obj.Remove( "Intensity" );
	}
}
