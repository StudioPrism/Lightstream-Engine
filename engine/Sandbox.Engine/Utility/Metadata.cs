using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sandbox;

/// <summary>
/// A simple class for storing and retrieving metadata values.
/// </summary>
public class Metadata
{
	private Dictionary<string, object> Data { get; set; }

	/// <summary>
	/// Deserialize metadata from a JSON string.
	/// </summary>
	internal static Metadata Deserialize( string json )
	{
		var data = Json.Deserialize<Dictionary<string, object>>( json );
		return new Metadata( data );
	}

	/// <summary>
	/// Serialize the metadata to a JSON string.
	/// </summary>
	internal string Serialize()
	{
		return JsonSerializer.Serialize( Data, Json.options );
	}

	internal Metadata( Dictionary<string, object> data )
	{
		Data = new Dictionary<string, object>( data, StringComparer.OrdinalIgnoreCase );
	}

	public Metadata()
	{
		Data = new( StringComparer.OrdinalIgnoreCase );
	}

	/// <summary>
	/// Set a value with the specified key.
	/// </summary>
	public void SetValue( string key, object value )
	{
		Data[key] = value;
	}

	/// <summary>
	/// Try to get a value of the specified type.
	/// </summary>
	public bool TryGetValue<T>( string key, out T outValue )
	{
		outValue = default;

		if ( !Data.TryGetValue( key, out var value ) )
			return false;

		if ( value is T t )
		{
			outValue = t;
			return true;
		}

		if ( value is JsonElement je )
		{
			try
			{
				outValue = je.Deserialize<T>( Json.options ) ?? default;
			}
			catch ( Exception )
			{
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Get the a value. If it's missing or the wrong type then use the default value.
	/// </summary>
	public T GetValueOrDefault<T>( string key, T defaultValue = default )
	{
		if ( !Data.TryGetValue( key, out var value ) )
			return defaultValue;

		if ( value is T t )
		{
			return t;
		}

		if ( value is JsonElement je )
		{
			try
			{
				return je.Deserialize<T>( Json.options ) ?? defaultValue;
			}
			catch ( Exception )
			{
				return defaultValue;
			}
		}

		return defaultValue;
	}
}
