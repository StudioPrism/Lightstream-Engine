using System.Threading;

namespace Sandbox.Utility;

/// <summary>
/// Wrappers of the parallel class.
/// </summary>
public static class Parallel
{
	//
	// This is all wrapped so that if it turns out to be a bad idea to expose it, we can 
	// do an update where this just does a regular foreach.
	//

	public static bool ForEach<T>( IEnumerable<T> source, Action<T> body )
	{
		var r = System.Threading.Tasks.Parallel.ForEach( source, body );
		return r.IsCompleted;
	}

	public static bool ForEach<T>( IEnumerable<T> source, CancellationToken token, Action<T> body )
	{
		var r = System.Threading.Tasks.Parallel.ForEach( source, new ParallelOptions { CancellationToken = token }, body );
		return r.IsCompleted;
	}

	public static bool For( int fromInclusive, int toExclusive, Action<int> body )
	{
		var r = System.Threading.Tasks.Parallel.For( fromInclusive, toExclusive, body );
		return r.IsCompleted;
	}

	public static async Task ForAsync( int fromInclusive, int toExclusive, CancellationToken token, Func<int, CancellationToken, ValueTask> body )
	{
		await System.Threading.Tasks.Parallel.ForAsync( fromInclusive, toExclusive, new ParallelOptions { CancellationToken = token }, body );
	}
}
