using System.Collections.Concurrent;

namespace Sandbox;

internal interface IWeakInteropHandle
{
	uint InteropHandle { get; set; }
}

internal static class InteropSystem
{
	static uint LastIndex = 1;

	static ConcurrentDictionary<uint, object> Objects = new();
	static ConcurrentDictionary<object, uint> Address = new();

	static ConcurrentQueue<uint> IndexPool = new();

	static bool TryGetObject( uint addr, out object o )
	{
		return Objects.TryGetValue( addr, out o );
	}

	static public bool TryGetObject<T>( uint addr, out T o )
	{
		if ( addr == 0 )
		{
			o = default;
			return true;
		}

		if ( TryGetObject( addr, out var obj ) )
		{
			// If the stored object is a weak reference, try to get the target
			if ( obj is WeakReference weak )
			{
				//
				// Native is trying to access a weak reference that has been collected on the managed side.
				// This can happen in the period between the destructor being called and Dispose being called (when using MainThread.QueueDispose())
				//
				if ( !weak.IsAlive )
				{
					o = default;
					return false;
				}

				obj = weak.Target;
			}

			if ( obj is T tobj )
			{
				o = tobj;
				return true;
			}

			Log.Warning( $"InteropSystem: Tried to get {addr} as {typeof( T ).FullName} - but it is a {obj?.GetType()?.FullName ?? "null"} ({obj})" );
		}

		o = default;
		return false;
	}

	static public bool TryGetAddress<T>( T o, out uint addr )
	{
		return Address.TryGetValue( o, out addr );
	}

	public static T Get<T>( uint address )
	{
		if ( TryGetObject<T>( address, out var obj ) )
			return obj;

		Log.Warning( $"InteropSystem: Tried to get {typeof( T )} at {address} - but not found" );
		return default;
	}

	public static uint GetAddress<T>( T obj, bool complain )
	{
		if ( obj == null )
			return 0;

		if ( obj is IWeakInteropHandle weakHandle )
		{
			return weakHandle.InteropHandle;
		}

		if ( TryGetAddress<T>( obj, out var addr ) )
			return addr;

		if ( complain )
		{
			Log.Warning( $"Tried to send address of object to native but it isn't allocated: {obj} ({obj.GetType()})" );
		}

		return 0;
	}

	static uint TakeIndex()
	{
		// not tested this code :0
		if ( LastIndex >= uint.MaxValue )
		{
			if ( IndexPool.TryDequeue( out var val ) )
				return val;
		}

		return LastIndex++;
	}

	public static void Alloc<T>( T obj )
	{
		ThreadSafe.AssertIsMainThread();

		if ( Address.ContainsKey( obj ) )
		{
			Log.Warning( $"Double Alloc on {typeof( T )}! Tell a grown up!" );
			return;
		}

		if ( obj == null )
		{
			Log.Warning( $"Object {typeof( T )} is null! Tell a grown up!" );
			return;
		}

		var idx = TakeIndex();

		Objects[idx] = obj;
		Address[obj] = idx;

		// Log.Info( $"[{idx}] Alloc {obj} ({typeof( T ).FullName})" );
	}

	public static uint AllocWeak<T>( T obj ) where T : IWeakInteropHandle
	{
		ThreadSafe.AssertIsMainThread();

		if ( obj == null )
		{
			Log.Warning( $"Object {typeof( T )} is null! Tell a grown up!" );
			return 0;
		}

		var idx = TakeIndex();

		Objects[idx] = new WeakReference( obj );

		obj.InteropHandle = idx;

		// Log.Info( $"[{idx}] Alloc Weak {obj} ({typeof( T ).FullName})" );
		return idx;
	}

	public static void Free<T>( T obj )
	{
		ThreadSafe.AssertIsMainThread();

		if ( obj == null ) throw new ArgumentNullException( nameof( obj ) );

		if ( !TryGetAddress( obj, out var idx ) )
			return;

		Objects.Remove( idx, out var _ );
		Address.Remove( obj, out var _ );

		IndexPool.Enqueue( idx );

		// Log.Info( $"Free {obj} ({idx})" );
	}

	public static void FreeWeak( IWeakInteropHandle handle )
	{
		var idx = handle.InteropHandle;
		if ( idx == 0 ) return;

		Objects.Remove( idx, out var _ );
		IndexPool.Enqueue( idx );

		handle.InteropHandle = 0;
	}

	[ConCmd( "interopsystem_status", ConVarFlags.Protected )]
	public static void PrintStatus()
	{
		Log.Info( $"Objects: {Objects.Count}" );
		Log.Info( $"Address: {Address.Count}" );
		Log.Info( $"IndexPool: {IndexPool.Count}" );
	}

	[ConCmd( "interopsystem_dump", ConVarFlags.Protected )]
	public static void PrintAlls()
	{
		foreach ( var item in Objects )
		{
			Log.Info( $"{item.Key}: {item.Value}" );
		}
	}

	static Dictionary<string, int> callCounts = new();

	internal static void Record( string name, string attributes )
	{
		name = $"{name} [{attributes}]";

		lock ( callCounts )
		{
			callCounts[name] = callCounts.GetValueOrDefault( name, 0 ) + 1;
		}
	}

	[ConCmd( "interopsystem_counts", ConVarFlags.Protected )]
	public static void PrintCalLCounts()
	{
		lock ( callCounts )
		{
			foreach ( var c in callCounts.OrderBy( x => x.Value ) )
			{
				if ( c.Key.Contains( "nogc" ) ) continue;

				Log.Info( $"{c.Value}: {c.Key}" );
			}
		}
	}
}
