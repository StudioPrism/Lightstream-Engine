using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.Concurrent;

namespace Sandbox;

public class CaseInsensitiveDictionary<T> : Dictionary<string, T>
{
	public CaseInsensitiveDictionary() : base( StringComparer.OrdinalIgnoreCase ) { }
	public CaseInsensitiveDictionary( int capacity ) : base( capacity, StringComparer.OrdinalIgnoreCase ) { }
	public CaseInsensitiveDictionary( IEqualityComparer<string> comparer ) : base( comparer ) { }
	public CaseInsensitiveDictionary( int capacity, IEqualityComparer<string> comparer ) : base( capacity, comparer ) { }
	public CaseInsensitiveDictionary( IDictionary<string, T> dictionary ) : base( dictionary, StringComparer.OrdinalIgnoreCase ) { }
	public CaseInsensitiveDictionary( IDictionary<string, T> dictionary, IEqualityComparer<string> comparer ) : base( dictionary, comparer ) { }
}

public class CaseInsensitiveConcurrentDictionary<T> : ConcurrentDictionary<string, T>
{
	public CaseInsensitiveConcurrentDictionary() : base( StringComparer.OrdinalIgnoreCase ) { }
	public CaseInsensitiveConcurrentDictionary( int concurrencyLevel, int capacity ) : base( concurrencyLevel, capacity, StringComparer.OrdinalIgnoreCase ) { }
	public CaseInsensitiveConcurrentDictionary( IEnumerable<KeyValuePair<string, T>> collection ) : base( collection, StringComparer.OrdinalIgnoreCase ) { }
	public CaseInsensitiveConcurrentDictionary( IEqualityComparer<string> comparer ) : base( comparer ) { }
	public CaseInsensitiveConcurrentDictionary( int concurrencyLevel, IEnumerable<KeyValuePair<string, T>> collection, IEqualityComparer<string> comparer ) : base( concurrencyLevel, collection, comparer ) { }
	public CaseInsensitiveConcurrentDictionary( int concurrencyLevel, int capacity, IEqualityComparer<string> comparer ) : base( concurrencyLevel, capacity, comparer ) { }
}
