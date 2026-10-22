using System.Text.Json.Nodes;

namespace Sandbox;

public static partial class SandboxSystemExtensions
{
	/// <summary>
	/// Get a property value by name, from a JsonObject. Return defaultValue if it's not found.
	/// </summary>
	public static T GetPropertyValue<T>( this JsonObject jso, string membername, in T defaultvalue )
	{
		if ( jso is null )
			return defaultvalue;

		if ( !jso.TryGetPropertyValue( membername, out var value ) )
			return defaultvalue;

		if ( value is T tValue )
			return tValue;

		if ( value is JsonNode node )
		{
			try
			{
				return node.Deserialize<T>();
			}
			catch
			{
				return defaultvalue;
			}
		}

		try
		{
			return (T)Convert.ChangeType( value, typeof( T ) );
		}
		catch
		{
			return defaultvalue;
		}

	}
}
