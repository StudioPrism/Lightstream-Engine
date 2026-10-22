using System.Collections.Generic;

namespace Sandbox.Bind;

/// <summary>
/// A proxy wrapped around a PropertyInfo
/// </summary>
internal sealed class PropertyProxy : Proxy
{
	public PropertyInfo PropertyInfo;

	bool isStatic;
	MethodInfo Getter;
	MethodInfo Setter;
	Action OnSetCallback;

	System.Type TargetType;

	public static Proxy Create<T>( T target, string targetName, Action onSet = null )
	{
		if ( targetName == "this" )
		{
			return new MethodProxy<T>( () => target, null );
		}

		if ( targetName.Contains( '.' ) )
		{
			return new DeepPropertyProxy( target, targetName, onSet );
		}

		return new PropertyProxy( target, targetName, onSet );
	}

	private PropertyProxy( object target, string targetName, Action onSet = null )
	{
		Initialize( target, target.GetType().GetProperty( targetName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy ), targetName, onSet );
	}

	public PropertyProxy( object target, PropertyInfo property, string propertyName = null, Action onSet = null )
	{
		Initialize( target, property, propertyName, onSet );
	}

	internal void Initialize( object target, PropertyInfo property, string propertyName = null, Action onSet = null )
	{
		if ( property == null )
			throw new System.MissingMemberException( $"Cannot find property \"{propertyName}\" on {target} ({target.GetType()})" );


		PropertyInfo = property;
		Name = $"{property.DeclaringType.Name}.{property.Name}";
		OnSetCallback = onSet;

		Target = new WeakReference<object>( target, true );

		Getter = property.CanRead ? property.GetGetMethod() : null;
		Setter = property.CanWrite ? property.GetSetMethod() : null;

		isStatic = Getter?.IsStatic ?? Setter?.IsStatic ?? false;

		TargetType = property.PropertyType;
	}

	public override object Value
	{
		get
		{
			if ( Target.TryGetTarget( out object obj ) || isStatic )
			{
				return Getter?.Invoke( obj, null );
			}

			return null;
		}

		set
		{
			if ( !CanWrite )
				return;

			object obj = null;

			if ( !isStatic && !Target.TryGetTarget( out obj ) )
				return;

			if ( !Translation.TryConvert( value, TargetType, out var convertedValue ) )
				return;

			Setter?.Invoke( obj, new[] { convertedValue } );
			OnSetCallback?.Invoke();
		}
	}

	public override bool IsValid
	{
		get
		{
			if ( isStatic ) return true;
			return base.IsValid;
		}
	}

	public override bool CanRead => Getter != null;

	public override bool CanWrite => Setter != null;

	public override string ToString() => $"[{Name}]";

	public bool HasSourceObject( object obj, string property )
	{
		if ( PropertyInfo == null )
			return false;

		if ( property != null && PropertyInfo.Name != property )
			return false;

		if ( !Target.TryGetTarget( out var sourceObject ) )
			return false;

		return obj == sourceObject;

	}
}
