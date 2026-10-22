namespace Sandbox;

[AttributeUsage( AttributeTargets.Method )]
public class ConCmdAttribute : ConVarAttribute
{
	public struct AutoCompleteResult
	{
		public string Command { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
	};

	public ConCmdAttribute( string name = null, ConVarFlags flags = default ) : base( name, flags )
	{

	}
}
