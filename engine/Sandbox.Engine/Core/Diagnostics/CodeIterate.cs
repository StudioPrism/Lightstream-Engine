using NativeEngine;
using Sandbox.Utility;
using System;
using System.Diagnostics;

namespace Sandbox.Diagnostics;

/// <summary>
/// A shared private class with the explicit purpose of recording how long it takes
/// between a developer saving a .cs file, and the changes being available on the client.
/// This is used to diagnose and monitor the code iteration time.
/// </summary>
internal static unsafe class CodeIterate
{
	public static bool Recording { get; set; }

	static Logger log = new Logger( "CodeIterate" );
	static Stopwatch stopwatch;
	static RealTimeSince timeSinceTriggered = 0;

	static Api.Events.EventRecord Event;

	public static void Start()
	{
		if ( Application.IsHeadless )
			return;

		if ( Recording || timeSinceTriggered < 2 )
		{
			Recording = false;
			timeSinceTriggered = -5;
			log.Trace( "Overlap - shut it down" );
			return;
		}

		timeSinceTriggered = 0;
		Recording = true;
		Event = new Api.Events.EventRecord( "CodeIterate" );

		log.Trace( "Start" );

		stopwatch = Stopwatch.StartNew();
	}

	public static void Finish()
	{
		if ( !Recording )
			return;

		log.Trace( $"Finish (total time {stopwatch.Elapsed.TotalSeconds:0.00}s)" );
		Event.SetValue( "Total", stopwatch.Elapsed.TotalMilliseconds );

		foreach ( var hint in Event.Data )
		{
			log.Trace( $"	{hint.Key}: {hint.Value}s" );
		}

		Event.Submit();

		Recording = false;
	}

	internal static void Hint( string v, double totalSeconds )
	{
		if ( !Recording )
			return;

		Event?.SetValue( v, totalSeconds );
	}
}
