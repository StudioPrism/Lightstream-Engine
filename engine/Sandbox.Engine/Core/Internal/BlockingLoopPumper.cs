
using System;

namespace Sandbox.Internal;

/// <summary>
/// Sometimes stuff is blocking that doesn't need to be. We want to keep running the main loop
/// while the function is blocking. This usually happens with Qt stuff like the drag and drop.
/// This should allow the main loop to still be pumped while waiting for that blocking function.
/// 
/// - main game loop
/// while ( true )
/// {
///		mainloop();
///		
///		BlockingLoopPumper.Run()
///		{
///			runBlockingFunction();
///		}
/// }
/// 
/// - somewhere else - usually Qt Timer
/// BlockingLoopPumper.Pump();
/// {
///		mainloop();
/// }
/// 
/// </summary>
internal static class BlockingLoopPumper
{
	/// <summary>
	/// Used to call things outside of the main frame
	/// </summary>
	internal static Action PendingFunction { get; set; }

	static Action loopPump;

	/// <summary>
	/// Called outside the main game loop.
	/// </summary>
	/// <param name="pumper">An action to call the main game loop</param>
	public static void Run( Action pumper )
	{
		if ( PendingFunction == null )
			return;

		loopPump = pumper;

		try
		{
			var f = PendingFunction;
			PendingFunction = null;
			f?.Invoke();
		}
		finally
		{
			loopPump = null;
		}
	}

	/// <summary>
	/// Should be called regularly, on the main thread. Generally this is called
	/// automatically by Qt in the timer (search OnQtHeartbeat).
	/// This should do total nothing if we're not actually in a blocking loop -  
	/// because loopPump will be null.
	/// </summary>
	public static void Pump()
	{
		loopPump?.Invoke();
	}
}
