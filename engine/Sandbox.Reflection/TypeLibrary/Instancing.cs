namespace Sandbox.Internal;

public partial class TypeLibrary
{
	/// <summary>
	/// Will throw an exception if the type isn't in the whitelist
	/// </summary>
	internal void AssertType( Type t )
	{
		if ( IsAllowedType( t ) ) return;

		throw new System.Exception( $"TypeLibrary: Type '{t}' isn't whitelisted" );
	}

	/// <summary>
	/// We're allowed to use the type if we know about it
	/// </summary>
	internal bool IsAllowedType( Type t )
	{
		if ( t.IsPrimitive ) return true;
		if ( t.IsInterface ) return true;
		if ( t.IsEnum ) return true;
		if ( t == typeof( string ) ) return true;
		if ( typedata.TryGetValue( t, out var _ ) ) return true;

		if ( t.IsConstructedGenericType )
		{
			return IsAllowedType( t.GetGenericTypeDefinition() ) && t.GetGenericArguments().All( IsAllowedType );
		}

		return false;
	}

	/// <summary>
	/// Create a type instance by name and is assignable to given type, with optional arguments for its constructor.
	/// </summary>
	/// <param name="name">Name of the type to create.</param>
	/// <param name="targetType">Type "constraint", as in the type instance must be assignable to this given type.</param>
	/// <param name="args">Optional arguments for the constructor of the selected type.</param>
	public object Create( string name, Type targetType, object[] args = null )
	{
		var foundType = GetType( targetType, name, false );
		if ( foundType == null ) return default;

		var instance = System.Activator.CreateInstance( foundType.TargetType, args );

		return instance;
	}

	/// <summary>
	/// Create type instance from type.
	/// </summary>
	public T Create<T>( Type type, object[] args = null )
	{
		if ( !IsAllowedType( type ) )
		{
			log.Warning( $"Tried to create {type} - but not in library" );
			return default;
		}

		if ( !type.IsAssignableTo( typeof( T ) ) )
		{
			return default;
		}

		return (T)System.Activator.CreateInstance( type, args );
	}

	/// <summary>
	/// Create a type instance by name and is assignable to given type.
	/// </summary>
	/// <param name="name">Name of the type to create.</param>
	/// <param name="complainOnMissing">Display a warning when requested type name was not found.</param>
	/// <typeparam name="T">Type "constraint", as in the type instance must be assignable to this given type.</typeparam>
	public T Create<T>( string name = null, bool complainOnMissing = true ) => Create<T>( name, null, complainOnMissing );

	/// <summary>
	/// Create a type instance by name and is assignable to given type.
	/// </summary>
	/// <param name="name">Name of the type to create.</param>
	/// <param name="complainOnMissing">Display a warning when requested type name was not found.</param>
	/// <param name="args"></param>
	/// <typeparam name="T">Type "constraint", as in the type instance must be assignable to this given type.</typeparam>
	public T Create<T>( string name, object[] args, bool complainOnMissing = true )
	{
		var type = GetType<T>( name );
		if ( type == null )
		{
			if ( complainOnMissing )
				log.Warning( $"Unknown Type {name}" );

			return default;
		}

		return Create<T>( type.TargetType, args );
	}

	/// <summary>
	/// Create a type instance by its identity. See <see cref="TypeLibrary.GetIdent(Type)"/>.
	/// </summary>
	public T Create<T>( int ident )
	{
		var type = GetTypeByIdent( ident );
		if ( type == null )
		{
			log.Warning( $"Unknown Type {ident}" );
			return default;
		}

		return type.Create<T>();
	}

	/// <summary>
	/// Create type by type
	/// </summary>
	internal T CreateGeneric<T>( Type type, Type genericArg1, object[] args = null )
	{
		if ( !IsAllowedType( type ) )
		{
			log.Warning( $"Tried to create {type} - but not in library" );
			return default;
		}

		var t = type.MakeGenericType( genericArg1 );
		return (T)System.Activator.CreateInstance( t, args );
	}
}
