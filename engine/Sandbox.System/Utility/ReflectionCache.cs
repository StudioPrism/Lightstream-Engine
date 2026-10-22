using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;

namespace Sandbox;

/// <summary>
/// Lazily performs expensive reflection, caching the result.
/// Clears itself during hotloads.
/// </summary>
internal sealed class ReflectionCache<TKey, TValue> : IHotloadManaged
	where TKey : MemberInfo
{
	[SkipHotload]
	private readonly ConcurrentDictionary<TKey, TValue> _cache = new();
	private readonly Func<TKey, TValue> _getValue;
	private readonly KeyValuePair<TKey, TValue>[] _defaultItems;

	/// <summary>
	/// Creates a <see cref="ReflectionCache{TKey,TValue}"/>, wrapping a function to get a <typeparamref name="TValue"/>
	/// lazily from a <typeparamref name="TKey"/>.
	/// </summary>
	/// <param name="getValue">Expensive reflection to lazily run once per <typeparamref name="TKey"/>.</param>
	/// <param name="defaultItems">Items to initialize the cache with. Will be added again after each hotload.</param>
	public ReflectionCache( Func<TKey, TValue> getValue, params KeyValuePair<TKey, TValue>[] defaultItems )
	{
		_getValue = getValue;
		_defaultItems = defaultItems;

		AddDefaultItems();
	}

	/// <summary>
	/// Attempts to add the specified key and value to the <see cref="ReflectionCache{TKey,TValue}"/>.
	/// </summary>
	/// <returns><see langword="true"></see> if the item was added, <see langword="false"></see> if an item already had a matching key.</returns>
	public bool TryAdd( TKey key, TValue value ) => _cache.TryAdd( key, value );

	/// <summary>
	/// Gets a cached value for the given <paramref name="key"/> if it exists, otherwise
	/// runs the expensive function wrapped by this instance and stores it.
	/// </summary>
	public TValue this[TKey key] => _cache.GetOrAdd( key, _getValue );

	private void AddDefaultItems()
	{
		foreach ( var item in _defaultItems )
		{
			_cache.TryAdd( item.Key, item.Value );
		}
	}

	void IHotloadManaged.Persisted()
	{
		_cache.Clear();
		AddDefaultItems();
	}
}
