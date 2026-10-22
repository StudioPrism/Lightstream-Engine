using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sandbox.Upgraders.SpecialCases;

[UpgraderGroup( typeof( CollectionsUpgraderGroup ) )]
internal abstract class BaseSetUpgrader( Type genericTypeDefinition )
	: KeyedCollectionUpgrader<object>( genericTypeDefinition )
{
	private sealed class Wrapper<T>( ISet<T> set ) : ICollectionWrapper
	{
		public IEnumerator<object> GetEnumerator() => set.Cast<object>().GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => set.GetEnumerator();

		public int? Count => set.Count;

		public void Clear() => set.Clear();

		public bool Add( object item ) => set.Add( (T)item );
	}

	protected override object GetKey( object item ) => item;
	protected override ICollectionWrapper Wrap( object collection )
	{
		var elemType = collection.GetType().GetGenericArguments()[0];
		var wrapperType = typeof( Wrapper<> ).MakeGenericType( elemType );

		return (ICollectionWrapper)Activator.CreateInstance( wrapperType, collection )!;
	}
}

[UpgraderGroup( typeof( CollectionsUpgraderGroup ) )]
internal class HashSetUpgrader() : BaseSetUpgrader( typeof( HashSet<> ) );
