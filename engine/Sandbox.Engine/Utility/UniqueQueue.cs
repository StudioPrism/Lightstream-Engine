namespace Sandbox.Utility;

internal class UniqueQueue<T> : IEnumerable<T>
{
	private readonly HashSet<T> set;
	private readonly Queue<T> queue;

	public UniqueQueue()
	{
		set = new HashSet<T>();
		queue = new Queue<T>();
	}

	public int Count => queue.Count;

	public void Enqueue( T item )
	{
		if ( set.Add( item ) )
		{
			queue.Enqueue( item );
		}
	}

	public T Dequeue()
	{
		T item = queue.Dequeue();
		set.Remove( item );
		return item;
	}

	public bool Contains( T item )
	{
		return set.Contains( item );
	}

	public void Clear()
	{
		set.Clear();
		queue.Clear();
	}

	public IEnumerator<T> GetEnumerator()
	{
		return queue.GetEnumerator();
	}

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return queue.GetEnumerator();
	}
}
