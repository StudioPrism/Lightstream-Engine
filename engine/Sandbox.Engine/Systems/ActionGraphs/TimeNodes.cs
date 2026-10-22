using Facepunch.ActionGraphs;
using System.Threading;

namespace Sandbox.ActionGraphs;

internal static class TimeNodes
{
	/// <summary>
	/// A task that does nothing for a given amount of time in seconds. This will continue even if the
	/// object containing this graph is destroyed, you probably want <see cref="GameObject"/> → Time → Delay instead.
	/// </summary>
	/// <param name="seconds">Time to wait in seconds.</param>
	/// <param name="ct">Token for cancelling the delay.</param>
	[ActionGraphNode( "time.delay" ), Title( "Scene Delay" ), Category( "Time" ), Icon( "schedule" ), Tags( "common" )]
	public static Task Delay( float seconds, CancellationToken? ct = null )
	{
		return ct is null ? GameTask.DelaySeconds( seconds ) : GameTask.DelaySeconds( seconds, ct.Value );
	}

	/// <summary>
	/// A task that does nothing for a given amount of time in seconds, and cancels automatically if the target <see cref="GameObject"/>
	/// is destroyed.
	/// </summary>
	/// <param name="target">Cancel the delay if this object is destroyed.</param>
	/// <param name="seconds">Time to wait in seconds.</param>
	/// <param name="ct">Token for cancelling the delay.</param>
	[ActionGraphNode( "time.delayobj" ), Category( "Time" ), Icon( "schedule" ), Tags( "common" )]
	public static Task Delay( [Target] GameObject target, float seconds, CancellationToken? ct = null )
	{
		return ct is null ? target.Task.DelaySeconds( seconds ) : target.Task.DelaySeconds( seconds, ct.Value );
	}

	/// <inheritdoc cref="Time.Delta"/>
	[ActionGraphNode( "time.delta" ), Pure, Category( "Time" ), Icon( "Δ" ), Tags( "common" )]
	public static float Delta()
	{
		return Time.Delta;
	}

	/// <inheritdoc cref="Time.Now"/>
	[ActionGraphNode( "time.now" ), Pure, Category( "Time" ), Icon( "Δ" ), Tags( "common" )]
	public static float Now()
	{
		return Time.Now;
	}
}
