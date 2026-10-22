namespace Facepunch.InteropGen;

[TypeName( "void" )]
public class ArgVoid : Arg
{
	public override string ManagedType => "void";
	public override bool IsVoid => true;
	public override string DefaultValue => "";
}
