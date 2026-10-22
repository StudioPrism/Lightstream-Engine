

namespace Sandbox.Bind;

/// <summary>
/// A proxy where set and get are done via functions
/// </summary>
internal class MethodProxy<T> : Proxy
{
	public Func<T> Read { get; set; }
	public Action<T> Write { get; set; }

	public MethodProxy( Func<T> read, Action<T> write )
	{
		Name = "[MethodBind]";
		Read = read;
		Write = write;
	}

	public MethodProxy( object target, Func<T> read, Action<T> write )
	{
		Target = new WeakReference<object>( target );
		Name = "[MethodBind]";
		Read = read;
		Write = write;
	}

	public override object Value
	{
		get => Read.Invoke();

		set
		{
			if ( !CanWrite ) throw new System.NotSupportedException( "Binding is read only" );

			if ( !Translation.TryConvert( value, typeof( T ), out var targetValue ) )
				return;

			Write( (T)targetValue );
		}
	}

	public override bool CanRead => Read != null;
	public override bool CanWrite => Write != null;
	public override bool IsValid => CanRead || CanWrite;

	public override string ToString() => $"[MethodBind]";
}
