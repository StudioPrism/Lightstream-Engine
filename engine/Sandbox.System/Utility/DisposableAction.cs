namespace Sandbox.Utility;

/// <summary>
/// A simple IDisposable that invokes an action when disposed.
/// Useful for creating using-blocks with cleanup logic.
/// </summary>
/// <example>
/// using ( DisposeAction.Create( () => Console.WriteLine( "Cleanup!" ) ) )
/// {
///     // Do work
/// } // "Cleanup!" is printed here
/// </example>
public readonly struct DisposeAction : IDisposable
{
	private readonly Action action;

	/// <summary>
	/// Creates a new DisposeAction that will invoke the specified action on disposal.
	/// </summary>
	/// <param name="action">The action to invoke when disposed</param>
	public DisposeAction( Action action )
	{
		this.action = action;
	}

	/// <summary>
	/// Invokes the action specified in the constructor.
	/// </summary>
	public void Dispose()
	{
		action?.Invoke();
	}

	/// <summary>
	/// Factory method to create a DisposeAction as an IDisposable.
	/// </summary>
	/// <param name="action">The action to invoke when disposed</param>
	/// <returns>A disposable object that will invoke the action</returns>
	public static IDisposable Create( Action action )
	{
		return new DisposeAction( action );
	}
}

/// <summary>
/// Like regular dispose action but with a state parameter, allowing for allocation free calls.
/// </summary>
internal readonly unsafe struct DisposeAction<A> : IDisposable
{
	private readonly delegate*< A, void > action;
	private readonly A state;

	public DisposeAction( delegate*< A, void > action, A state )
	{
		this.action = action;
		this.state = state;
	}

	public void Dispose()
	{
		if ( action != null )
			action( state );
	}

	public static DisposeAction<A> Create( delegate*< A, void > action, A state )
	{
		return new DisposeAction<A>( action, state );
	}
}

/// <summary>
/// Like regular dispose action but with a state parameter, allowing for allocation free calls.
/// </summary>
internal readonly unsafe struct DisposeAction<A, B> : IDisposable
{
	private readonly delegate*< A, B, void > action;
	private readonly A a;
	private readonly B b;

	public DisposeAction( delegate*< A, B, void > action, A a, B b )
	{
		this.action = action;
		this.a = a;
		this.b = b;
	}

	public void Dispose()
	{
		if ( action != null )
			action( a, b );
	}

	public static DisposeAction<A, B> Create( delegate*< A, B, void > action, A a, B b )
	{
		return new DisposeAction<A, B>( action, a, b );
	}
}
