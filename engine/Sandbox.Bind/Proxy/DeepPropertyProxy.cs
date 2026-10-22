using Sandbox.Diagnostics;

namespace Sandbox.Bind;

/// <summary>
/// A proxy that can access properties using a deep path, ie "School.Teacher.Name".
/// Slower than the regular proxy because we don't do any caching.
/// </summary>
internal sealed class DeepPropertyProxy : Proxy
{
	string[] Path;
	Action OnSetCallback;

	internal DeepPropertyProxy( object target, string targetName, Action onSet = null )
	{
		Name = $"{target}.{targetName}";

		Target = new WeakReference<object>( target, true );
		Path = targetName.Split( '.' );
		OnSetCallback = onSet;

		Assert.NotNull( Path );
		Assert.True( Path.Length > 1 );
	}

	(object target, PropertyInfo property)? Resolve( object target, Span<string> parts )
	{
		if ( target == null )
			return null;

		//
		// Try to get the property named the first part
		//
		var propertyInfo = target.GetType().GetProperty( parts[0], BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy );
		if ( propertyInfo == null )
			return null;

		//
		// This was the last string, return this property
		//
		if ( parts.Length == 1 )
		{
			return (target, propertyInfo);
		}

		//
		// Get the value of this property, then get the property of that
		//
		var get = propertyInfo.GetMethod;

		if ( get == null )
			return null;

		return Resolve( get.Invoke( target, null ), parts[1..] );
	}

	public override object Value
	{
		get
		{
			if ( !Target.TryGetTarget( out object obj ) )
				return null;

			var info = Resolve( obj, Path );
			if ( info == null ) return null;

			return info.Value.property.GetValue( info.Value.target );
		}

		set
		{
			if ( !Target.TryGetTarget( out object obj ) )
				return;

			var info = Resolve( obj, Path );
			if ( info == null )
				return;

			if ( !Translation.TryConvert( value, info.Value.property.PropertyType, out var convertedValue ) )
				return;

			info.Value.property.SetValue( info.Value.target, convertedValue );
			OnSetCallback?.Invoke();

		}
	}

	public override bool CanRead => true; // probably

	public override bool CanWrite => true; // probably

	public override string ToString() => $"[{Name}]";
}
