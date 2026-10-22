namespace Sandbox;

internal static partial class ConVarSystem
{
	[ConCmd( "find", ConVarFlags.Protected )]
	public static void CmdFind( string partial )
	{
		var results = Members.Values
							.Where( x => !x.IsHidden )
							.Where( x => x.Name.Contains( partial, System.StringComparison.OrdinalIgnoreCase ) )
							.ToArray();

		foreach ( var c in results.OrderBy( x => x.Name ) )
		{
			Log.Info( $"{c.Name} - {c.BuildDescription()}" );
		}
	}
}
