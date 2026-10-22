namespace Sandbox;

internal static partial class PackageManager
{
	/// <summary>
	/// List all currently active packages
	/// </summary>
	[ConCmd( "package_list", ConVarFlags.Protected )]
	public static void CmdList()
	{
		Log.Info( $"{ActivePackages.Count():n0} active packages" );

		foreach ( var p in ActivePackages )
		{
			Log.Info( $"{p.Package.FullIdent} [{string.Join( ";", p.Tags )}]" );
		}
	}

	/// <summary>
	/// Install a package in the specific context
	/// </summary>
	/// <param name="package">The package ident</param>
	/// <param name="context">The context (ie, client, server)</param>
	[ConCmd( "package_install", ConVarFlags.Protected )]
	public static async Task CmdAdd( string package, string context = "console" )
	{
		try
		{
			Log.Info( $"Installing package {package}" );
			var fs = await PackageManager.InstallAsync( new PackageLoadOptions() { PackageIdent = package, ContextTag = context } );
			Log.Info( $"Installed {fs}" );
		}
		catch ( Exception ex )
		{
			Log.Warning( ex, $"Error installing package {package} ({ex.Message})" );
		}
	}

	/// <summary>
	/// Unmount all packages that use a specific tag. This is usually done on leaving a game for client and server
	/// </summary>
	[ConCmd( "package_unmount_tag", ConVarFlags.Protected )]
	public static void CmdWipe( string context )
	{
		Log.Info( $"Unmounting packages with tag [{context}]" );
		PackageManager.UnmountTagged( context );
	}
}
