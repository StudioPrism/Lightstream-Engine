namespace Sandbox.Diagnostics;

/// <summary>
/// A lightweight, high-resolution timer for performance measurement.
/// More efficient than <see cref="Stopwatch"/> with a simpler API.
/// </summary>
/// <example>
/// var timer = FastTimer.StartNew();
/// // Do work...
/// Log.Info( $"Took {timer.ElapsedMilliSeconds}ms" );
/// </example>
public struct FastTimer
{
	/// <summary>
	/// Creates and starts a new FastTimer.
	/// </summary>
	/// <returns>A started FastTimer</returns>
	public static FastTimer StartNew()
	{
		var ft = new FastTimer();
		ft.Start();
		return ft;
	}

	long startTimestamp;

	/// <summary>
	/// Starts or restarts the timer.
	/// </summary>
	public void Start()
	{
		startTimestamp = Stopwatch.GetTimestamp();
	}

	/// <summary>
	/// Gets the timestamp when the timer was started.
	/// </summary>
	public readonly long StartTick => startTimestamp;

	/// <summary>
	/// Gets the number of ticks elapsed since the timer was started.
	/// </summary>
	public readonly long ElapsedTicks => (Stopwatch.GetTimestamp() - startTimestamp);

	/// <summary>
	/// Gets the number of microseconds elapsed since the timer was started.
	/// </summary>
	public readonly double ElapsedMicroSeconds => ElapsedTicks * (1_000_000.0 / Stopwatch.Frequency);

	/// <summary>
	/// Gets the number of milliseconds elapsed since the timer was started.
	/// </summary>
	public readonly double ElapsedMilliSeconds => ElapsedTicks * (1_000.0 / Stopwatch.Frequency);

	/// <summary>
	/// Gets the number of seconds elapsed since the timer was started.
	/// </summary>
	public readonly double ElapsedSeconds => ElapsedTicks * (1.0 / Stopwatch.Frequency);

	/// <summary>
	/// Gets the time elapsed since the timer was started as a TimeSpan.
	/// </summary>
	public readonly TimeSpan Elapsed => TimeSpan.FromTicks( ElapsedTicks );
}
