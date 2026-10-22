namespace Sandbox.Network;

internal interface IObjectPoolEvent
{
	void OnRented() { }
	void OnReturned() { }
}

internal class NetworkObjectPool<T> where T : IObjectPoolEvent, new()
{
	private readonly Queue<T> _pool = new();

	/// <summary>
	/// Rent an object from the pool.
	/// </summary>
	public T Rent()
	{
		var instance = _pool.Count > 0 ? _pool.Dequeue() : new();
		instance.OnRented();
		return instance;
	}

	/// <summary>
	/// Return an object to the pool.
	/// </summary>
	public void Return( T instance )
	{
		instance.OnReturned();
		_pool.Enqueue( instance );
	}
}
