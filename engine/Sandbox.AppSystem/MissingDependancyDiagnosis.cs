using System.Runtime.InteropServices;

namespace Sandbox;

/// <summary>
/// Poke a bunch of native dlls that are required and try to work out if they exist, and can load.
/// </summary>
static class MissingDependancyDiagnosis
{
	public static void Run()
	{
		// leafiest first, all the dlls we're going to need to load

		// most likely to fail
		TestAssembly( "MSVCRT.dll" );
		TestAssembly( "vcruntime140.dll" ); // Visual Studio 2015, 2017
		TestAssembly( "vcruntime140_1.dll" ); // Visual Studio 2019
		TestAssembly( "msvcp140.dll" );
		TestAssembly( "dbghelp.dll" );

		TestAssembly( "kernel32.dll" );
		TestAssembly( "user32.dll" );
		TestAssembly( "advapi32.dll" );
		TestAssembly( "gdi32.dll" );
		TestAssembly( "ws2_32.dll" );
		TestAssembly( "rpcrt4.dll" );
		TestAssembly( "ole32.dll" );
		TestAssembly( "SHLWAPI.dll" );
		TestAssembly( "WINMM.dll" );
		TestAssembly( "IMM32.dll" );

		TestAssembly( "sentry.dll" );

		TestAssembly( "steam_api64.dll" );
		TestAssembly( "tier0.dll" );
	}

	private static void TestAssembly( string assemblyName )
	{
		if ( NativeLibrary.TryLoad( assemblyName, out var handle ) )
		{
			NativeLibrary.Free( handle );
			return;
		}

		throw new System.Exception( $"Native dll not found, or can't load: {assemblyName}" );
	}
}
