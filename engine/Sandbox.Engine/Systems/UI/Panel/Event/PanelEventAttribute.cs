namespace Sandbox.UI;

/// <summary>
/// Add an event listener to a <see cref="Panel"/> event with the given name.<br/>
/// See <see cref="Panel.CreateEvent(string, object, float?)"/>.
/// </summary>
[AttributeUsage( AttributeTargets.Method )]
public class PanelEventAttribute : Attribute
{
	/// <summary>
	/// Name of the event to listen to.
	/// </summary>
	public string Name { get; set; }

	public PanelEventAttribute( string name = null )
	{
		Name = name;
	}
}
