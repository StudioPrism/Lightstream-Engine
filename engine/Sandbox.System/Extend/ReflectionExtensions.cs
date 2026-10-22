using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Facepunch.ActionGraphs;

namespace Sandbox;

public static partial class SandboxSystemExtensions
{
	/// <summary>
	/// Returns true if this member has this attribute
	/// </summary>
	internal static bool HasAttribute( this MemberInfo memberinfo, Type attribute, bool inherit = true )
	{
		return memberinfo?.IsDefined( attribute, inherit ) ?? false;
	}

	/// <summary>
	/// Returns true if this type derives from a type named name
	/// </summary>
	internal static bool HasBaseType( this Type type, string name )
	{
		if ( type.BaseType == null ) return false;
		if ( type.BaseType.FullName == name ) return true;

		return HasBaseType( type.BaseType, name );
	}

	/// <summary>
	/// Gets an attribute on an enum field value
	/// </summary>
	/// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
	/// <param name="enumVal">The enum value</param>
	/// <returns>The attribute of type T that exists on the enum value</returns>
	/// <example><![CDATA[string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;]]></example>
	public static T GetAttributeOfType<T>( this Enum enumVal ) where T : System.Attribute
	{
		var type = enumVal.GetType();
		var memInfo = type.GetMember( enumVal.ToString() );
		var attributes = memInfo[0].GetCustomAttributes( typeof( T ), false );
		return (attributes.Length > 0) ? (T)attributes[0] : null;
	}

	/// <summary>
	/// Returns if this type is based on a given generic type.
	/// </summary>
	/// <param name="src">The type to test.</param>
	/// <param name="test">The type to test against. Typically this will be something like <code>typeof( MyType&lt;&gt; )</code></param>
	public static bool IsBasedOnGenericType( this Type src, Type test )
	{
		if ( !test.IsGenericType ) return false;

		var type = src;
		while ( type != null )
		{
			if ( type.IsGenericType && type.GetGenericTypeDefinition() == test ) return true;
			type = type.BaseType;
		}

		return false;
	}

	/// <summary>
	/// Check all <see cref="ValidationAttribute"/>s on this property, and get the error messages if there are any.
	/// </summary>
	/// <param name="prop">The property whose arguments to test.</param>
	/// <param name="obj">Instance of the object this property is of.</param>
	/// <param name="errors">If returned false, these will be the error messages to display.</param>
	/// <param name="name">Override the property name in error messages.</param>
	/// <returns>Returns true if all checks have passed or there is no attributes to test, false if there were errors.</returns>
	public static bool CheckValidationAttributes( this PropertyInfo prop, object obj, out string[] errors, string name = null )
	{
		if ( prop == null )
			throw new System.ArgumentNullException();

		if ( obj == null )
			throw new System.ArgumentNullException();

		var errorList = new List<string>();

		var attrs = prop.GetCustomAttributes<ValidationAttribute>();
		foreach ( var attr in attrs )
		{
			var valid = attr.IsValid( prop.GetValue( obj ) );
			if ( !valid ) errorList.Add( attr.FormatErrorMessage( name ?? prop.Name ) );
		}

		errors = errorList.ToArray();
		return !errorList.Any();
	}

	/// <summary>
	/// Determine if this property is init-only.
	/// </summary>
	/// <param name="property">The property to test.</param>
	/// <returns>Returns true if the property is init-only, false otherwise.</returns>
	public static bool IsInitOnly( this PropertyInfo property )
	{
		if ( !property.CanWrite ) return false;

		var setMethod = property.SetMethod;
		if ( setMethod == null ) return false;

		// Init-only properties are marked with the IsExternalInit type.
		return setMethod.ReturnParameter.GetRequiredCustomModifiers().Contains( typeof( System.Runtime.CompilerServices.IsExternalInit ) );
	}

	/// <summary>
	/// Returns this type's name, with nicer formatting for generic types.
	/// </summary>
	public static string ToSimpleString( this Type type, bool includeNamespace = true )
	{
		var sb = new StringBuilder();
		sb.AppendType( type, includeNamespace );
		return sb.ToString();
	}

	/// <summary>
	/// Returns this member's name qualified by its declaring type, with nicer formatting for generics.
	/// </summary>
	public static string ToSimpleString( this MemberInfo member, bool includeNamespace = true )
	{
		if ( member is Type type ) return ToSimpleString( type, includeNamespace );

		if ( member is not MethodInfo { IsGenericMethod: true } method )
		{
			return $"{member.DeclaringType}::{member.Name}";
		}

		var sb = new StringBuilder();
		sb.AppendType( method.DeclaringType, includeNamespace );
		sb.Append( "::" );

		var quoteIndex = method.Name.IndexOf( '`' );

		sb.Append( quoteIndex == -1 ? method.Name : method.Name[..quoteIndex] );
		sb.Append( "<" );

		var first = true;

		foreach ( var arg in method.GetGenericArguments() )
		{
			if ( first ) first = false;
			else sb.Append( ", " );

			sb.AppendType( arg, includeNamespace );
		}

		sb.Append( ">" );

		return sb.ToString();
	}

	/// <summary>
	/// Returns a nice name for the given delegate, based on the method that implements it.
	/// </summary>
	public static string ToSimpleString( this Delegate deleg, bool includeNamespace = true )
	{
		if ( deleg.Method is { Name: "MoveNext", DeclaringType.Name: "AsyncStateMachineBox`1" } )
		{
			var stateMachineType = deleg.Method.DeclaringType.GetGenericArguments()[1];

			return stateMachineType.ToSimpleString();
		}

		return deleg.Method.ToSimpleString();
	}

	private static void AppendType( this StringBuilder sb, Type type, bool includeNamespace )
	{
		if ( type == null )
		{
			sb.Append( "null" );
			return;
		}

		if ( type.IsGenericMethodParameter || type.IsGenericTypeParameter )
		{
			return;
		}

		if ( type.IsArray )
		{
			sb.AppendType( type.GetElementType(), includeNamespace );
			sb.Append( "[" );

			for ( var i = 1; i < type.GetArrayRank(); ++i )
			{
				sb.Append( "," );
			}

			sb.Append( "]" );
			return;
		}

		if ( type.IsByRef )
		{
			sb.Append( "ref " );
			sb.AppendType( type.GetElementType(), includeNamespace );
			return;
		}

		if ( type.IsPointer )
		{
			sb.AppendType( type.GetElementType(), includeNamespace );
			sb.Append( "*" );
			return;
		}

		if ( type.IsNested )
		{
			sb.AppendType( type.DeclaringType, includeNamespace );
			sb.Append( "." );
		}
		else if ( type.Namespace != null && includeNamespace )
		{
			sb.Append( type.Namespace );
			sb.Append( "." );
		}

		if ( type.IsGenericType )
		{
			if ( Either.IsEitherType( type ) )
			{
				var options = Either.Unwrap( type );

				var first = true;

				foreach ( var option in options )
				{
					if ( first ) first = false;
					else sb.Append( " | " );

					sb.AppendType( option, includeNamespace );
				}
			}
			else if ( Nullable.GetUnderlyingType( type ) is { } elemType )
			{
				sb.AppendType( elemType, includeNamespace );
				return;
			}
			else
			{
				var quoteIndex = type.Name.IndexOf( '`' );

				sb.Append( quoteIndex == -1 ? type.Name : type.Name[..quoteIndex] );
				sb.Append( "<" );

				var first = true;

				foreach ( var arg in type.GetGenericArguments() )
				{
					if ( first ) first = false;
					else sb.Append( ", " );

					sb.AppendType( arg, includeNamespace );
				}

				sb.Append( ">" );
			}

			return;
		}

		sb.Append( type.Name );
	}
}
