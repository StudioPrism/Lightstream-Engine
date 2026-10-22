using System.Diagnostics.CodeAnalysis;
using Facepunch.ActionGraphs;

namespace Sandbox.ActionGraphs
{
	internal static class LogNodes
	{
		private static string Format( [AllowNull] string format, params object[] args )
		{
			return format == null ? string.Join( ",", args ) : string.Format( format, args );
		}

		[ActionGraphNode( "log.info" ), Category( "Debug" ), Title( "Log Info" ), Description( "Print a message to the console." ), Icon( "info" ), Tags( "common" )]
		public static void Info( string format = null, params object[] args )
		{
			Log.Info( Format( format, args ) );
		}

		[ActionGraphNode( "log.warning" ), Category( "Debug" ), Title( "Log Warning" ), Description( "Print a warning message to the console." ), Icon( "warning" ), Tags( "common" )]
		public static void Warning( string format = null, params object[] args )
		{
			Log.Warning( Format( format, args ) );
		}

		[ActionGraphNode( "log.error" ), Category( "Debug" ), Title( "Log Error" ), Description( "Print an error message to the console." ), Icon( "error" ), Tags( "common" )]
		public static void Error( string format = null, params object[] args )
		{
			Log.Error( Format( format, args ) );
		}
	}
}
