using System.Text.Json.Nodes;
namespace Sandbox;

public partial class SceneFile
{
	/// <summary>
	/// Version 1 to 2
	/// - Title and Description moved to SceneInformation metadata component
	/// </summary>
	[Expose, JsonUpgrader( typeof( SceneFile ), 2 )]
	internal static void Upgrader_v2( JsonObject obj )
	{
		var title = obj.GetPropertyValue<string>( "Title", null );
		var description = obj.GetPropertyValue<string>( "Description", null );

		if ( string.IsNullOrEmpty( title ) && string.IsNullOrEmpty( description ) )
			return;

		//
		// Add them to the metadata
		//
		if ( obj["SceneProperties"] is JsonObject properties )
		{
			var metadata = new JsonObject();
			metadata["Title"] = title;
			metadata["Description"] = description;
			properties["Metadata"] = metadata;
		}

		if ( obj["GameObjects"] is not JsonArray gameObjects )
			return;

		var component = new JsonObject();
		component["__type"] = "SceneInformation";
		component["Title"] = title;
		component["Description"] = description;

		var go = new JsonObject();
		go["Id"] = Guid.NewGuid();
		go["Name"] = "Scene Information";
		go["Enabled"] = true;
		go["Components"] = new JsonArray( component );

		gameObjects.Insert( 0, go );
	}
}
