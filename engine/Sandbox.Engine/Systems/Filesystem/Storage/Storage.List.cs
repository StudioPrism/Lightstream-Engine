using System.Collections.Concurrent;

namespace Sandbox;

public static partial class Storage
{
	public static Entry[] GetAll( string type )
	{
		return Sandbox.FileSystem.Data.FindDirectory( $"/storage/{type}/", "*" )
			.Select( x => Load( type, x ) )
			.Where( x => x is not null )
			.ToArray();
	}

	static ConcurrentDictionary<string, Entry> _cache = new();

	static Entry Load( string type, string folderName )
	{
		var cacheKey = $"{type}:{folderName}";

		if ( _cache.TryGetValue( cacheKey, out var cached ) )
		{
			return cached;
		}

		try
		{
			var meta = Sandbox.FileSystem.Data.ReadJson<StorageMeta>( $"/storage/{type}/{folderName}/_meta.json" );
			if ( meta is null ) return null;
			if ( meta.Type != type ) return null;

			// force meta name!
			meta.Id = folderName;

			var content = new Entry( meta );
			_cache[cacheKey] = content;
			return content;
		}
		catch ( System.Exception e )
		{
			Log.Warning( e );
			return null;
		}
	}

	internal static void OnDeleted( Entry source )
	{
		foreach ( var kv in _cache.Where( x => x.Value == source ).ToArray() )
		{
			_cache.TryRemove( kv.Key, out _ );
		}
	}
}
