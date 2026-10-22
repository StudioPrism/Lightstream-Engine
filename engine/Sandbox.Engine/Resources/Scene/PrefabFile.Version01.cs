using System.Text.Json.Nodes;

namespace Sandbox;

public partial class PrefabFile
{
	/// <summary>
	/// Version 0 to 1
	/// - "Id" changed to "__guid"
	/// </summary>
	[Expose, JsonUpgrader( typeof( PrefabFile ), 1 )]
	internal static void Upgrader_v1( JsonObject obj )
	{
		if ( obj["RootObject"] is JsonObject root )
		{
			Upgrader_v1( root );
		}

		if ( obj.TryGetPropertyValue( "Id", out var id ) )
		{
			try
			{
				obj["__guid"] = (Guid)id;
				obj.Remove( "Id" );
			}
			catch ( System.Exception e )
			{
				Log.Warning( e );
			}
		}

		if ( obj["Children"] is JsonArray childArray )
		{
			foreach ( var child in childArray )
			{
				if ( child is not JsonObject jso )
					continue;

				Upgrader_v1( jso );
			}
		}

		if ( obj["Components"] is JsonArray componentArray )
		{
			foreach ( var child in componentArray )
			{
				if ( child is not JsonObject jso )
					continue;

				Upgrader_v1( jso );
			}
		}
	}

}
