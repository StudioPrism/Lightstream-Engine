using Facepunch.ActionGraphs;

namespace Sandbox.ActionGraphs
{
	internal static class CollectionNodes
	{
		[ActionGraphNode( "array.new" ), Pure, Title( "New Array of {T|Type}" ), Icon( "data_array" ), Category( "Collections" ), Tags( "common" )]
		public static T[] NewArray<T>( int count )
		{
			return count == 0 ? Array.Empty<T>() : new T[count];
		}

		[ActionGraphNode( "list.new" ), Pure, Title( "New List of {T|Type}" ), Icon( "data_array" ), Category( "Collections" ), Tags( "common" )]
		public static List<T> NewList<T>()
		{
			return new List<T>();
		}

		[ActionGraphNode( "list.new" ), Pure, Title( "New List of {T|Type}" ), Icon( "data_array" ), Category( "Collections" ), Tags( "common" )]
		public static List<T> NewList<T>( params T[] items )
		{
			return new List<T>( items );
		}

		[ActionGraphNode( "set.new" ), Pure, Title( "New Set of {T|Type}" ), Icon( "data_object" ), Category( "Collections" ), Tags( "common" )]
		public static HashSet<T> NewSet<T>()
		{
			return new HashSet<T>();
		}

		[ActionGraphNode( "set.new" ), Pure, Title( "New Set of {T|Type}" ), Icon( "data_object" ), Category( "Collections" ), Tags( "common" )]
		public static HashSet<T> NewSet<T>( params T[] items )
		{
			return new HashSet<T>( items );
		}

		[ActionGraphNode( "dict.new" ), Pure, Title( "New Dictionary from {TKey|Key} to {TValue|Value}" ), Icon( "toc" ), Category( "Collections" ), Tags( "common" )]
		public static Dictionary<TKey, TValue> NewDictionary<TKey, TValue>()
		{
			return new Dictionary<TKey, TValue>();
		}

		[ActionGraphNode( "list.get" ), Pure, Title( "Get Item" ), Category( "Collections" ), Icon( "logout" ), Tags( "common" )]
		public static T GetItem<T>( IReadOnlyList<T> list, int index )
		{
			return list[index];
		}

		[ActionGraphNode( "list.set" ), Title( "Set Item" ), Category( "Collections" ), Icon( "login" ), Tags( "common" )]
		public static void SetItem<T>( IList<T> list, int index, T value )
		{
			list[index] = value;
		}
	}
}
