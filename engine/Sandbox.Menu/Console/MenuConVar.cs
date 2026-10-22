namespace Sandbox;

public class MenuConVarAttribute : ConVarAttribute
{
	public MenuConVarAttribute( string name ) : base( name )
	{
		Context = "menu";
	}
}

public class MenuConCmdAttribute : ConCmdAttribute
{
	public MenuConCmdAttribute( string name ) : base( name )
	{
		Context = "menu";
	}

	public MenuConCmdAttribute( string name, ConVarFlags flags ) : base( name, flags )
	{
		Context = "menu";
	}
}
