using NativeEngine;
using System.Diagnostics.CodeAnalysis;

namespace Sandbox.Engine;

internal static partial class InputBinds
{
	static CaseInsensitiveDictionary<BindCollection> Collections = new CaseInsensitiveDictionary<BindCollection>();

	/// <summary>
	/// Find a bind collection by name. The name is generally the ident of the current game.
	/// We'll try to load the binds from /config/input/*.json - if we fail then we'll serve
	/// the default.
	/// </summary>
	public static BindCollection FindCollection( string name )
	{
		ArgumentNullException.ThrowIfNull( name, "name" );

		if ( name is not null && Collections.TryGetValue( name, out var collection ) )
			return collection;

		collection = new BindCollection( name );
		Collections[name] = collection;

		if ( name != "common" )
		{
			collection.Base = FindCollection( "common" );
		}

		return collection;
	}
}

