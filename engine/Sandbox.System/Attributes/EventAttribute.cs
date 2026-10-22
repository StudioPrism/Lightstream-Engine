namespace Sandbox;

/// <summary>
/// A generic event listener. You are probably looking for Sandbox.Event.* attributes.
/// </summary>
[AttributeUsage( AttributeTargets.Method, Inherited = true, AllowMultiple = true )]
public class EventAttribute : System.Attribute
{
	/// <summary>
	/// The internal event identifier.
	/// </summary>
	public string EventName { get; set; }

	/// <summary>
	/// Events with lower numbers are run first. This defaults to 0, so setting it to -1 will mean your
	/// event will run before all other events that don't define it. Setting it to 1 would mean it'll
	/// run after all events that don't.
	/// </summary>
	public int Priority { get; set; }

	public EventAttribute( string eventName )
	{
		EventName = eventName;
	}
}
