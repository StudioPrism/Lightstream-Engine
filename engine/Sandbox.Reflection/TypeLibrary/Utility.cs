namespace Sandbox.Internal;

public partial class TypeLibrary
{
	/// <summary>
	/// This is used primarily to get GlobalRpcHandler.OnRpc
	/// </summary>
	internal T[] GetStaticMethods<T>( string classname, string methodname ) where T : System.Delegate
	{
		// TODO cache me!

		return Types.Select( x => x.Assembly )
			.Distinct()
			.Select( x => x.GetType( classname, false, false )?.GetMethod( methodname, BindingFlags.Static | BindingFlags.Public ) )
			.Where( x => x != null )
			.Select( x => x.CreateDelegate<T>() )
			.ToArray();
	}

	/// <summary>
	/// This is a hash of loaded assembly names. We can use it to make sure we're using
	/// the same code as the server. This is important when it comes to things like decoding
	/// network messages and datatables - because if the code is different we're going to
	/// get errors, because it could expect different data.
	/// </summary>
	internal int DynamicAssemblyHash { get; private set; }

	internal int GetIdent( Type type )
	{
		if ( type == null )
			return 0;

		if ( typedata.TryGetValue( type, out var data ) )
		{
			return data.Identity;
		}

		return -1;
	}

	internal string GetClassName( Type type )
	{
		if ( typedata.TryGetValue( type, out var data ) )
		{
			return data.ClassName;
		}

		return type.Name;
	}


	internal MethodInfo GetMethod( object target, string name )
	{
		// TODO find by renamed
		return target.GetType().GetMethod( name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy );
	}

	// Special case for map stuff
	internal MethodInfo GetInputMethod( object target, string name )
	{
		var flags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
		var methods = target.GetType().GetMethods( flags ).Where( x =>
		{
			return x.GetCustomAttribute<InputAttribute>() != null;
		} );

		var func = methods.FirstOrDefault( x =>
		{

			var attr = x.GetCustomAttribute<InputAttribute>();
			return attr != null && !string.IsNullOrEmpty( attr.Name ) && attr.Name.Equals( name, StringComparison.CurrentCultureIgnoreCase );

		} );

		// TODO: ClientRPC adds overloads, if used on an [Input] method we now have 2 methods and we don't know which one to choose
		if ( func == null )
		{
			func = methods.FirstOrDefault( x => x.Name.Equals( name, StringComparison.CurrentCultureIgnoreCase ) );
		}

		return func;
	}
}

