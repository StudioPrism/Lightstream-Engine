using System.Text.Json.Nodes;
namespace Sandbox;

public partial class SceneFile
{
	/// <summary>
	/// Version 2 to 3
	/// - Add "Children" and "Components" array if they don't exist
	/// - Add Flags if they don't exist
	/// </summary>
	[Expose, JsonUpgrader( typeof( SceneFile ), 3 )]
	internal static void Upgrader_v3( JsonObject obj )
	{
		if ( !obj.TryGetPropertyValue( "Flags", out var _ ) )
		{
			obj["Flags"] = 0;
		}

		if ( obj["Children"] is JsonArray childArray )
		{
			foreach ( var child in childArray )
			{
				if ( child is not JsonObject jso )
					continue;

				Upgrader_v3( jso );
			}
		}
		else
		{
			obj["Children"] = new JsonArray();
		}

		if ( obj["Components"] is not JsonArray componentArray )
		{
			obj["Components"] = new JsonArray();
		}


		if ( obj["GameObjects"] is JsonArray objectArray )
		{
			foreach ( var child in objectArray )
			{
				if ( child is not JsonObject jso )
					continue;

				Upgrader_v3( jso );
			}
		}
	}
}
