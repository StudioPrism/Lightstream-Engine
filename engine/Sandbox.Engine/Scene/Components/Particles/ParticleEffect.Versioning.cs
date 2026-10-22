using System.Text.Json.Nodes;

namespace Sandbox;

public sealed partial class ParticleEffect
{
	public override int ComponentVersion => 3;

	/// <summary>
	/// v1
	/// - Added ApplyAlpha property if ApplyColor was true.
	/// </summary>
	[Expose, JsonUpgrader( typeof( ParticleEffect ), 1 )]
	static void Upgrader_v1( JsonObject obj )
	{
		if ( obj.TryGetPropertyValue( "ApplyColor", out var propertyNode ) )
		{
			var applyColor = (bool)propertyNode;
			if ( applyColor )
			{
				obj["ApplyAlpha"] = true;
			}
		}
	}

	/// <summary>
	/// v2
	/// - Changed Space from enum to LocalSpace ParticleFloat, where 1 is local, 0 is world space.
	/// </summary>
	[Expose, JsonUpgrader( typeof( ParticleEffect ), 2 )]
	static void Upgrader_v2( JsonObject obj )
	{
		if ( obj.TryGetPropertyValue( "Space", out var space ) && (string)space == "Local" )
		{
			obj[nameof( LocalSpace )] = 1;
		}
	}

	[Expose, JsonUpgrader( typeof( ParticleEffect ), 3 )]
	static void Upgrader_v3( JsonObject obj )
	{
		var hasPitch = obj.TryGetPropertyValue( "Pitch", out var pitch );
		var hasYaw = obj.TryGetPropertyValue( "Yaw", out var yaw );
		var hasRoll = obj.TryGetPropertyValue( "Roll", out var roll );
		if ( hasPitch && hasYaw && hasRoll )
		{
			var originalPitch = pitch.DeepClone();
			var originalYaw = yaw.DeepClone();
			var originalRoll = roll.DeepClone();

			obj[nameof( Roll )] = originalPitch;
			obj[nameof( Pitch )] = originalYaw;
			obj[nameof( Yaw )] = originalRoll;
		}
	}
}
