using System.Text.Json.Nodes;

namespace Sandbox;

/// <summary>
/// This is added when a component is missing. It will store the json data of the missing component, so we don't lose any data.
/// </summary>
[Expose, Hide]
public class MissingComponent : Component
{
	string json { get; }

	public MissingComponent( JsonObject jso )
	{
		json = jso.ToJsonString();
		Flags |= ComponentFlags.Error;
	}

	/// <summary>
	/// Get the Json data that was deserialized
	/// </summary>
	public JsonObject GetJson()
	{
		return JsonObject.Parse( json ) as JsonObject;
	}
}
