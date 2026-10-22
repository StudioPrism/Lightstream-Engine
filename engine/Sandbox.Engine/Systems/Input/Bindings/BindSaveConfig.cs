using Microsoft.Extensions.Options;
using System;
using static Sandbox.Engine.BindCollection;

namespace Sandbox.Engine;

/// <summary>
/// A struct which is serialized/deserialized to save binds to a file (in a more readable format)
/// </summary>
internal class BindSaveConfig
{
	/// <summary>
	/// To allow us to cater for changes in schema
	/// </summary>
	public int Schema { get; set; } = 1;

	/// <summary>
	/// A list of strings that describe the binds
	///     "jump": "space;m",
	///		"run": "shift",
	///		"walk": "alt",
	/// </summary>
	public CaseInsensitiveDictionary<string> Binds { get; set; }

	/// <summary>
	/// Load a serialized collection from disk
	/// </summary>
	internal static void Load( string configPath, BindCollection bindCollection )
	{
		var data = EngineFileSystem.Config.ReadJsonOrDefault<BindSaveConfig>( configPath, null );
		if ( data == null ) return;

		foreach ( var bind in data.Binds )
		{
			var actionName = bind.Key.ToLowerInvariant().Trim();
			var parts = bind.Value.Split( ';', StringSplitOptions.RemoveEmptyEntries );

			for ( int i = 0; i < parts.Length; i++ )
			{
				bindCollection.Set( actionName, i, parts[i] );
			}
		}
	}

	/// <summary>
	/// Save a serialized collection to disk
	/// </summary>
	internal static void Save( string configPath, BindCollection bindCollection )
	{
		var data = new BindSaveConfig();
		data.Binds = new();

		foreach ( var bind in bindCollection.Actions )
		{
			string builtString = $"{bind.Value.Get( 0 ).FullString};{bind.Value.Get( 1 ).FullString}".Trim( ' ', ';' ).ToLowerInvariant();
			data.Binds[bind.Key.ToLowerInvariant()] = builtString;
		}

		EngineFileSystem.Config.CreateDirectory( "/input/" );
		EngineFileSystem.Config.WriteJson( configPath, data );
	}
}

