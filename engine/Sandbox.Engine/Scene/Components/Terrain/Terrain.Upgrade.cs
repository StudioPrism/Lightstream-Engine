using System.Text.Json.Nodes;

namespace Sandbox;

public partial class Terrain
{
	public override int ComponentVersion => 1;

	[Expose, JsonUpgrader( typeof( Terrain ), 1 )]
	static void Upgrader_v1( JsonObject json )
	{
		json.Remove( "TerrainMaterial" ); // No custom shaders for now

		if ( json.Remove( "TerrainDataFile", out var newNode ) )
		{
			json["Storage"] = newNode;
		}
	}
}
