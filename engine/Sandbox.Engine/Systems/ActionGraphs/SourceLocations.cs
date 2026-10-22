using System.IO;
using Facepunch.ActionGraphs;

namespace Sandbox.ActionGraphs;

/// <summary>
/// A <see cref="ISourceLocation"/> that provides <see cref="Facepunch.ActionGraphs.SerializationOptions"/>.
/// </summary>
public interface ISerializationOptionProvider : ISourceLocation
{
	SerializationOptions SerializationOptions { get; }
}

/// <summary>
/// Source location for action graphs that belong to a Hammer map. This is used for stack
/// traces, and for knowing which map to save when editing a graph.
/// </summary>
public sealed class MapSourceLocation : ISerializationOptionProvider
{
	private static Dictionary<string, MapSourceLocation> Cached { get; } =
		new( StringComparer.OrdinalIgnoreCase );

	/// <summary>
	/// Gets a <see cref="MapSourceLocation"/> from a path name.
	/// </summary>
	/// <param name="mapPathName">Project-relative map path ending with ".vmap" or ".vpk".</param>
	public static MapSourceLocation Get( string mapPathName )
	{
		ArgumentNullException.ThrowIfNull( mapPathName, nameof( mapPathName ) );

		mapPathName = NormalizeMapPathName( mapPathName );

		if ( Path.IsPathRooted( mapPathName ) )
		{
			throw new ArgumentException( "Expected a relative path.", nameof( mapPathName ) );
		}

		if ( Cached.TryGetValue( mapPathName, out var cached ) ) return cached;

		return Cached[mapPathName] = cached = new MapSourceLocation( mapPathName );
	}

	private static string NormalizeMapPathName( string mapPathName )
	{
		if ( mapPathName.EndsWith( ".vmap", StringComparison.OrdinalIgnoreCase ) )
		{
			mapPathName = Path.ChangeExtension( mapPathName, ".vpk" );
		}

		mapPathName = mapPathName.NormalizeFilename( false, false );

		return mapPathName;
	}

	public string MapPathName { get; }

	public SerializationOptions SerializationOptions { get; }

	private MapSourceLocation( string mapPathName )
	{
		MapPathName = mapPathName;

		SerializationOptions = new(
			Cache: new ActionGraphCache(),
			WriteCacheReferences: false,
			ForceUpdateCached: true,
			SourceLocation: this,
			ImpliedTarget: null );
	}

	public override string ToString()
	{
		return $"Map:{MapPathName}";
	}
}

/// <summary>
/// Source location for action graphs that belong to a <see cref="GameResource"/>.
/// These can include scenes and prefabs, or custom resources. This is used for stack
/// traces, and for knowing which asset to save when editing a graph.
/// </summary>
/// <param name="Resource">Resource that contains action graphs.</param>
public record GameResourceSourceLocation( GameResource Resource ) : ISourceLocation
{
	public override string ToString()
	{
		return Resource.ToString();
	}
}
