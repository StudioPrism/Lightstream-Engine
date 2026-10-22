using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Sandbox.Upgraders.SpecialCases;

#nullable enable

/// <summary>
/// Base upgrader for <see cref="IDictionary"/> implementations.
/// </summary>
internal abstract class BaseDictionaryUpgrader( Type genericTypeDefinition )
	: KeyedCollectionUpgrader<DictionaryEntry>( genericTypeDefinition )
{
	private sealed class Wrapper<TKey, TValue>( IDictionary<TKey, TValue> dict ) : ICollectionWrapper
	{
		public IEnumerator<DictionaryEntry> GetEnumerator() => dict
			.Select( x => new DictionaryEntry( x.Key!, x.Value ) )
			.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public int? Count => dict.Count;
		public void Clear() => dict.Clear();

		public bool Add( DictionaryEntry item )
		{
			dict.Add( (TKey)item.Key, (TValue)item.Value! );
			return true;
		}
	}

	protected override object GetKey( DictionaryEntry item ) => item.Key;
	protected override ICollectionWrapper Wrap( object collection )
	{
		var typeArgs = collection.GetType().GetGenericArguments();
		var wrapperType = typeof( Wrapper<,> ).MakeGenericType( typeArgs );

		return (ICollectionWrapper)Activator.CreateInstance( wrapperType, collection )!;
	}
}

[UpgraderGroup( typeof( CollectionsUpgraderGroup ) )]
internal class DictionaryUpgrader() : BaseDictionaryUpgrader( typeof( Dictionary<,> ) );

[UpgraderGroup( typeof( CollectionsUpgraderGroup ) )]
internal class SortedDictionaryUpgrader() : BaseDictionaryUpgrader( typeof( SortedDictionary<,> ) );

[UpgraderGroup( typeof( CollectionsUpgraderGroup ) )]
internal class ConcurrentDictionaryUpgrader() : BaseDictionaryUpgrader( typeof( ConcurrentDictionary<,> ) );
