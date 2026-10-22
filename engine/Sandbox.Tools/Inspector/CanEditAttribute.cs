using System;
using System.Linq;
using System.Reflection;

namespace Editor;

[AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
public class CanEditAttribute : Attribute, ITypeAttribute, IUninheritable
{
	static List<CanEditAttribute> All = new();

	public System.Type TargetType { get; set; }

	void ITypeAttribute.TypeRegister()
	{
		EditorAttributeTypes.Clear();
		EditorAttributes.Clear();

		//
		// If this type has any IEditorAttribute<T>'s, save them off into the lists.
		//
		foreach ( var i in TargetType.GetInterfaces() )
		{
			if ( !i.IsAssignableTo( typeof( Internal.IEditorAttributeBase ) ) ) continue;

			var types = i.GenericTypeArguments;
			if ( types.Length != 1 ) continue;

			EditorAttributeTypes.Add( types[0] );
			EditorAttributes.Add( i );
		}

		All.Add( this );
	}

	void ITypeAttribute.TypeUnregister()
	{
		All.RemoveAll( x => x.TargetType == TargetType );
	}

	public static Widget CreateEditorFor( PropertyInfo property )
	{
		var editor = CreateEditorFor( property.PropertyType, property.GetCustomAttributes() );
		if ( editor == null )
			return null;

		return editor;
	}

	public static Widget CreateEditorFor( Type t, IEnumerable<System.Attribute> attributes = null, Type[] generics = null )
	{
		// If we have override attributes, check those first
		if ( attributes != null )
		{
			var ed = CreateWidget( t, attributes, generics );
			if ( ed != null ) return ed;
		}

		var customAttributes = t.GetCustomAttributes();

		var widget = CreateWidget( t, customAttributes, generics );
		if ( widget != null ) return widget;

		if ( t.IsGenericType && !t.IsGenericTypeDefinition )
		{
			return CreateEditorFor( t.GetGenericTypeDefinition(), attributes, t.GetGenericArguments() );
		}

		if ( t.IsSZArray )
		{
			return CreateEditorFor( "array", t.GetElementType() );
		}

		if ( t.IsValueType && !t.IsPrimitive && !t.IsEnum )
		{
			return CreateEditorFor( "struct" );
		}

		if ( t.BaseType != null )
		{
			return CreateEditorFor( t.BaseType, attributes );
		}

		return null;
	}
	public static Widget CreateEditorFor( string name )
	{
		foreach ( var entry in All )
		{
			if ( !entry.CanEdit( name ) ) continue;

			// try with Widget arg
			try
			{
				return EditorTypeLibrary.Create<Widget>( entry.TargetType, new object[] { null } );
			}
			catch ( System.MissingMethodException )
			{
				// Ignore and move on
			}
			catch ( System.Exception e )
			{
				Log.Warning( e, $"Error creating {entry.TargetType}: {e.Message}" );
			}

			// try with no arg
			try
			{
				return EditorTypeLibrary.Create<Widget>( entry.TargetType, null );
			}
			catch ( System.MissingMethodException )
			{
				// Ignore and move on
			}
			catch ( System.Exception e )
			{
				Log.Warning( e, $"Error creating {entry.TargetType}: {e.Message}" );
			}
		}

		return null;
	}

	internal static Widget CreateEditorFor( string name, Type genericArgument )
	{
		foreach ( var entry in All )
		{
			if ( !entry.CanEdit( name ) ) continue;

			try
			{
				return EditorTypeLibrary.CreateGeneric<Widget>( entry.TargetType, genericArgument, new object[] { null } );
			}
			catch ( System.Exception ) { }
		}

		return null;
	}

	public static Widget CreateEditorFor( Array array )
	{
		if ( array.Length == 0 ) return null;
		if ( array.Length == 1 ) return CreateEditorForObject( array.GetValue( 0 ) );

		try
		{
			var me = new MultiSerializedObject();

			for ( int i = 0; i < array.Length; i++ )
			{
				var so = EditorTypeLibrary.GetSerializedObject( array.GetValue( i ) );
				if ( so is not null )
				{
					me.Add( so );
				}
			}

			me.Rebuild();

			// Find the lowest common type in the array
			Type t = array.OfType<object>().Select( o => o.GetType() ).GetCommonBaseType();

			var attributes = t.GetCustomAttributes<System.Attribute>();

			var widget = CreateWidget( t, attributes, null, me );
			if ( widget != null ) return widget;

			if ( t.IsGenericType && !t.IsGenericTypeDefinition )
			{
				return CreateEditorFor( t.GetGenericTypeDefinition(), null, t.GetGenericArguments() );
			}
		}
		catch ( System.Exception )
		{
			// can't serialize to make it a MultiSerializedObject, so just use the first one
			return CreateEditorForObject( array.GetValue( 0 ) );
		}

		return null;
	}

	public static Widget CreateEditorForObject( object obj )
	{
		if ( obj == null )
			return null;

		if ( obj is Array array )
		{
			return CreateEditorFor( array );
		}

		var t = obj.GetType();

		var attributes = t.GetCustomAttributes<System.Attribute>();

		var widget = CreateWidget( t, attributes, null, obj );
		if ( widget != null ) return widget;

		if ( t.IsGenericType && !t.IsGenericTypeDefinition )
		{
			return CreateEditorFor( t.GetGenericTypeDefinition(), null, t.GetGenericArguments() );
		}

		return null;
	}


	public System.Type Type { get; init; }
	public string TypeName { get; init; }

	/// <summary>
	/// List of attribute types that are generic argument in IEditorAttribute
	/// </summary>
	List<System.Type> EditorAttributeTypes = new();

	/// <summary>
	/// List of the generic IEditorAttribute class types itself
	/// </summary>
	List<System.Type> EditorAttributes = new();

	public CanEditAttribute( System.Type type, string typeName = null )
	{
		Type = type;
		TypeName = typeName;
	}

	public CanEditAttribute( string typeName ) : this( null, typeName )
	{

	}

	/// <summary>
	/// Is <paramref name="t"/> a delegate type that returns void or a plain Task?
	/// </summary>
	private static bool IsActionDelegateType( Type t )
	{
		if ( !typeof( Delegate ).IsAssignableFrom( t ) )
		{
			return false;
		}

		var invoke = t.GetMethod( "Invoke" );

		if ( invoke == null )
		{
			return false;
		}

		return invoke.ReturnType == typeof( void ) || invoke.ReturnType == typeof( Task );
	}

	static Widget CreateForType( Type editType, object target )
	{
		var constructors = editType.GetConstructors();

		if ( constructors.Any( x => x.GetParameters().Count() == 1 && x.GetParameters().Any( x => x.ParameterType == typeof( SerializedObject ) ) ) )
		{
			try
			{
				if ( target is not SerializedObject so )
					so = target.GetSerialized();

				return EditorTypeLibrary.Create<Widget>( editType, new object[] { so } );
			}
			catch ( System.Exception e )
			{
				Log.Warning( e, $"Couldn't create {editType} ({e.Message})" );
				return null;
			}
		}

		if ( constructors.Any( x => x.GetParameters().Count() == 2 && x.GetParameters()[0].ParameterType == typeof( Widget ) ) )
		{
			try
			{
				return EditorTypeLibrary.Create<Widget>( editType, new object[] { null, target } );
			}
			catch ( System.Exception e )
			{
				Log.Warning( e, $"Couldn't create {editType} ({e.Message})" );
				return null;
			}
		}

		if ( constructors.Any( x => x.GetParameters().Count() == 1 && x.GetParameters()[0].ParameterType == typeof( Widget ) ) )
		{
			try
			{
				return EditorTypeLibrary.Create<Widget>( editType, new object[] { null } );
			}
			catch ( System.Exception e )
			{
				Log.Warning( e, $"Couldn't create {editType} ({e.Message})" );
				return null;
			}
		}

		return null;
	}

	private static Widget CreateWidget( Type t, IEnumerable<System.Attribute> attributes, System.Type[] generics = null, object obj = null )
	{
		string name = null;

		if ( IsActionDelegateType( t ) ) name = "action";
		if ( t.IsEnum ) name = "enum";
		if ( t == typeof( bool ) ) name = "bool";

		var ed = All
			.Select( x => new { score = x.CanEdit( t, attributes, name ), value = x } )
			.Where( x => x.score > 0 )
			.OrderByDescending( x => x.score )
			.FirstOrDefault()?.value;

		if ( ed == null ) return null;

		Widget widget = null;
		var editType = ed.TargetType;

		if ( editType.IsGenericTypeDefinition )
		{
			generics ??= new[] { t };
			editType = editType.MakeGenericType( generics );
			generics = null;
		}

		widget = CreateForType( editType, obj );

		if ( widget is null )
		{
			return null;
		}

		//
		// If we're implementing any IEditorAttribute<T> then call their SetEditorAttribute method
		// with any matching attributes.
		//
		for ( int i = 0; i < ed.EditorAttributes.Count; i++ )
		{
			var interfaceType = ed.EditorAttributes[i];
			var attributeType = ed.EditorAttributeTypes[i];

			var attr = attributes.FirstOrDefault( x => x.GetType() == attributeType );
			if ( attr == null ) continue;

			var m = interfaceType.GetMethod( "SetEditorAttribute", BindingFlags.Public | BindingFlags.Instance );
			m.Invoke( widget, new object[] { attr } );

		}


		return widget;
	}

	private int CanEdit( Type t, IEnumerable<System.Attribute> attributes, string name )
	{
		int score = -500;

		//
		// If the editor has IEditorAttribute<T> values, add 10 points for every matching value
		// But subtract one point, so that any other editors with matching types will take priority
		//
		if ( EditorAttributeTypes.Count > 0 )
		{
			score -= 5;
			score += attributes.Count( x => EditorAttributeTypes.Contains( x.GetType() ) ) * 10;
			score += t.CustomAttributes.Count( x => EditorAttributeTypes.Contains( x.AttributeType ) ) * 10;
		}

		// If a property has [Editor( "blahblah" )] and we're "blahblah" we should be given priority
		if ( attributes.OfType<EditorAttribute>().Any( x => x.Value == TypeName ) )
			return score + 1500;

		// Match on Type, but only if TypeName isn't something more specific than Type.Name
		if ( Type != null && Type == t && (string.IsNullOrEmpty( TypeName ) || string.Equals( TypeName, Type.Name, StringComparison.OrdinalIgnoreCase )) )
			return score + 1100;

		if ( !string.IsNullOrEmpty( name ) && string.Equals( TypeName, name, StringComparison.OrdinalIgnoreCase ) )
			return score + 1000;

		// If we're a generic type, which is constraint to one of our base classes, we should work
		if ( Type == null )
		{
			foreach ( var arg in TargetType.GetGenericArguments() )
			{
				var constraints = arg.GetGenericParameterConstraints();
				if ( constraints.Any( x => t.IsAssignableTo( x ) ) )
					return score + 900;
			}
		}

		if ( Type != null && t.IsAssignableTo( Type ) )
			return score + 800;

		return 0;
	}

	private bool CanEdit( string name )
	{
		return string.Equals( TypeName, name, StringComparison.OrdinalIgnoreCase );
	}
}
