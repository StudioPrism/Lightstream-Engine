namespace Sandbox;

public interface IByteParsable
{
	public readonly ref struct ByteParseOptions
	{

	}

	static abstract object ReadObject( ref ByteStream stream, ByteParseOptions o = default );
	static abstract void WriteObject( ref ByteStream stream, object value, ByteParseOptions o = default );
}

public interface IByteParsable<T> : IByteParsable
{
	public static abstract T Read( ref ByteStream stream, ByteParseOptions o = default );
	public static abstract void Write( ref ByteStream stream, T value, ByteParseOptions o = default );
}
