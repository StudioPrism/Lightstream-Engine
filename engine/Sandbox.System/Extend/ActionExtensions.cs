
namespace Sandbox;

public static partial class SandboxSystemExtensions
{
	/// <summary>
	/// Call an action, swallow any exceptions with a warning
	/// </summary>
	public static void InvokeWithWarning( this Action action )
	{
		if ( action is null ) return;

		try
		{
			action.Invoke();
		}
		catch ( System.Exception e )
		{
			Log.Warning( e, $"{e.Message}" );
		}
	}

	/// <summary>
	/// Call an action, swallow any exceptions with a warning
	/// </summary>
	public static void InvokeWithWarning<T>( this Action<T> action, T arg0 )
	{
		if ( action is null ) return;

		try
		{
			action.Invoke( arg0 );
		}
		catch ( System.Exception e )
		{
			Log.Warning( e, $"{e.Message}" );
		}
	}

	/// <summary>
	/// Call an action, swallow any exceptions with a warning
	/// </summary>
	public static void InvokeWithWarning<T1, T2>( this Action<T1, T2> action, T1 arg0, T2 arg1 )
	{
		if ( action is null ) return;

		try
		{
			action.Invoke( arg0, arg1 );
		}
		catch ( System.Exception e )
		{
			Log.Warning( e, $"{e.Message}" );
		}
	}
}
