using System.Text.Json.Nodes;

namespace Sandbox;

public partial class PrefabFile
{
	/// <summary>
	/// Version 1 to 2
	/// - Add "Children" and "Components" array if they don't exist
	/// - Add Flags if they don't exist
	/// </summary>
	[Expose, JsonUpgrader( typeof( PrefabFile ), 2 )]
	internal static void Upgrader_v2( JsonObject obj )
	{
		if ( obj["RootObject"] is JsonObject root )
		{
			Upgrader_v2( root );
		}

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

				Upgrader_v2( jso );
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
	}

}
