using System.Text.Json.Nodes;

namespace Sandbox;

public static partial class Cloud
{
	/// <summary>
	/// Resolve a primary asset to a loaded package
	/// </summary>
	public static Package ResolvePrimaryAsset( string assetPath )
	{
		assetPath = assetPath.NormalizeFilename( false );
		assetPath = assetPath.TrimStart( '/' );

		foreach ( var package in PackageManager.ActivePackages )
		{
			// compare paths in a case insensitive, slash insensitive way
			if ( string.Equals( package.Package.PrimaryAsset, assetPath, StringComparison.OrdinalIgnoreCase ) )
			{
				return package.Package;
			}
		}

		return null;
	}

	/// <summary>
	/// Given a json value, walk it and find paths, resolve them to packages
	/// </summary>
	public static Package[] ResolvePrimaryAssetsFromJson( JsonNode jso )
	{
		HashSet<Package> packages = [];

		Sandbox.Json.WalkJsonTree( jso, ( k, v ) =>
		{
			if ( !v.TryGetValue<string>( out var path ) )
				return v;

			// not a path if it has any of these characters in it
			if ( path.Contains( ',' ) ) return v;
			if ( path.Contains( ':' ) ) return v;
			if ( path.Contains( '\"' ) ) return v;
			if ( path.Contains( '\'' ) ) return v;

			var lastPeriod = path.LastIndexOf( '.' );
			if ( lastPeriod < 0 ) return v;
			if ( lastPeriod < path.Length - 9 ) return v;

			var package = ResolvePrimaryAsset( path );
			if ( package is null ) return v;

			packages.Add( package );

			return v;
		} );

		return packages.ToArray();
	}

	/// <summary>
	/// Given a json string, walk it and find paths, resolve them to packages
	/// </summary>
	public static Package[] ResolvePrimaryAssetsFromJson( string json )
	{
		return ResolvePrimaryAssetsFromJson( JsonNode.Parse( json ) );
	}
}
