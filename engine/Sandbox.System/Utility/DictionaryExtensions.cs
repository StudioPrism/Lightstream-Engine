using System.Collections.Immutable;

namespace Sandbox;

/// <summary>
/// Extension methods for working with immutable dictionaries.
/// Provides functional-style operations for creating modified copies.
/// </summary>
internal static class DictionaryExtensions
{
	/// <summary>
	/// Returns a new dictionary with the specified key-value pair added or updated.
	/// If the value already exists and is equal, returns the original dictionary.
	/// </summary>
	public static IReadOnlyDictionary<TKey, TValue> With<TKey, TValue>( this IReadOnlyDictionary<TKey, TValue> dict, TKey key, TValue value )
		where TKey : IEquatable<TKey>
	{
		if ( dict.TryGetValue( key, out var existing ) && Equals( existing, value ) )
		{
			return dict;
		}

		var builder = ImmutableDictionary.CreateBuilder<TKey, TValue>();

		builder.AddRange( dict );
		builder[key] = value;

		return builder.ToImmutable();
	}

	/// <summary>
	/// Returns a new dictionary with the specified key removed.
	/// If the key doesn't exist, returns the original dictionary.
	/// </summary>
	public static IReadOnlyDictionary<TKey, TValue> Without<TKey, TValue>( this IReadOnlyDictionary<TKey, TValue> dict, TKey key )
		where TKey : IEquatable<TKey>
	{
		if ( !dict.ContainsKey( key ) )
		{
			return dict;
		}

		var builder = ImmutableDictionary.CreateBuilder<TKey, TValue>();

		builder.AddRange( dict.Where( x => !x.Key.Equals( key ) ) );

		return builder.ToImmutable();
	}
}
