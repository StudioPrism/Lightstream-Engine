namespace Facepunch.InteropGen;

[TypeName( "ulong" )]
public class ArgULong : Arg
{
	public override string ManagedType => "ulong";
	public override string NativeType => "uint64";
}

[TypeName( "long" )]
public class ArgLong : Arg
{
	public override string ManagedType => "long";
	public override string NativeType => "int64";
}
