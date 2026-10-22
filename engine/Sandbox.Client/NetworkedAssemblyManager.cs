using Sandbox.Diagnostics;
using Sandbox.Engine;
using Sandbox.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Sandbox;

/// <summary>
/// Handles the loading of assemblies sent from the server via string tables
/// </summary>
internal class NetworkedAssemblyManager : IDisposable
{
	PackageLoader Loader;
	PackageLoader.Enroller Enroller;
	Logger log = new Logger( "GameAssemblyManager" );
	Dictionary<string, ulong> Loaded;
	bool stringTableDirty = false;

	/// <summary>
	/// Called on Game Loop Init
	/// </summary>
	internal NetworkedAssemblyManager()
	{
		Loaded = new();

		Loader = new PackageLoader( "client", "game", GetType().Assembly );
		Loader.HotloadWatch( Game.Assembly );
		Loader.HotloadWatch( typeof( Sandbox.Event ).Assembly );
		Loader.HotloadIgnore( typeof( SkiaSharp.SKBitmap ).Assembly );
		//Loader.HotloadIgnore( typeof( Topten.RichTextKit.StyleRun ).Assembly );
		Loader.HotloadIgnore( IServerDll.Current?.GetType().Assembly );
		Loader.HotloadIgnore( IToolsDll.Current?.GetType().Assembly );
		Loader.OnAfterHotload = () =>
		{
			UISystem.OnHotload();
			Event.Run( "hotloaded" );
		};

		Enroller = Loader.CreateEnroller( "client" );

		Enroller.OnAssemblyAdded += ( a ) =>
		{
			ConsoleSystem.Collection.AddAssembly( a.Assembly );
			Event.RegisterAssembly( a.Assembly );
			Cloud.UpdateTypes( a.Assembly );
			TypeLibrary.AddAssembly( a.Assembly, true );
		};

		Enroller.OnAssemblyRemoved += ( a ) =>
		{
			ConsoleSystem.Collection?.RemoveAssembly( a.Assembly );
			Event.UnregisterAssembly( a.Assembly );
			TypeLibrary?.RemoveAssembly( a.Assembly );
		};
	}

	public void Dispose()
	{
		StringTables.Assemblies.OnStringAddedOrChanged = null;

		Enroller?.Dispose();
		Enroller = null;

		Loader?.Dispose();
		Loader = null;

		Loaded.Clear();
		Loaded = null;

	}

	/// <summary>
	/// The client has ended the loading and downloading session and is now ready to enter the server
	/// </summary>
	public void ClientSignonFull()
	{
		StringTables.Assemblies.OnStringAddedOrChanged = ( i ) => stringTableDirty = true;
		stringTableDirty = true;

		Tick();
	}

	public void Tick()
	{
		if ( stringTableDirty )
		{
			log.Trace( "ReloadStringTableAssemblies" );

			stringTableDirty = false;
			var timer = Stopwatch.StartNew();

			for ( int i = 0; i < StringTables.Assemblies.Count(); i++ )
			{
				OnAssemblyFromServer( i );
			}

			CodeIterate.Hint( "ClientLoad", timer.Elapsed.TotalMilliseconds );
		}

		Loader.Tick();
	}

	/// <summary>
	/// Called by the string table when an assembly is received/added
	/// </summary>
	unsafe void OnAssemblyFromServer( int entryId )
	{
		// Log.Info( $"OnAssemblyFromServer {entryId}" );

		// We index our dictionary by name - so we need this
		var name = StringTables.Assemblies.GetString( entryId );

		//
		// Get a pointer to the data in the string table
		//
		var data = StringTables.Assemblies.GetData( entryId, out var datalen );
		if ( data == IntPtr.Zero || datalen <= 0 ) return;

		//
		// Wrap the data in a stream
		//
		using var stream = new UnmanagedMemoryStream( (byte*)data, datalen );
		var crc = Crc64.FromStream( stream );
		stream.Position = 0;

		log.Trace( $"OnAssemblyFromServer {entryId} [{name}] [crc:{crc}]" );

		if ( Loaded.TryGetValue( name, out var testCrc ) && testCrc == crc )
		{
			log.Trace( $"Skipping loading {name} as it has already been loaded" );
			return;
		}

		Enroller.LoadAssemblyFromStream( name, stream );
		Loaded[name] = crc;
	}
}
