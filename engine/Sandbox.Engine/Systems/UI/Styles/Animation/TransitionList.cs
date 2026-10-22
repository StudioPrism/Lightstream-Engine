
namespace Sandbox.UI;

/// <summary>
/// A list of CSS properties that should transition when changed.
///
/// Utility to create a transition by comparing the
/// panel style before and after the scope.
/// </summary>
public class TransitionList
{
	/// <summary>
	/// The actual list of CSS properties that should be transitioned.
	/// </summary>
	public List<TransitionDesc> List = new List<TransitionDesc>();

	internal void AddTransitions( TransitionList transitions )
	{
		foreach ( var t in transitions.List )
		{
			Add( t );
		}
	}

	internal void Add( TransitionDesc t )
	{
		var n = List.FirstOrDefault( x => x.Property == t.Property );

		if ( t.Delay.HasValue ) n.Delay = t.Delay;
		if ( t.TimingFunction != null ) n.TimingFunction = t.TimingFunction;
		if ( t.Property != null ) n.Property = t.Property;
		if ( t.Duration.HasValue ) n.Duration = t.Duration;

		List.RemoveAll( x => x.Property == t.Property );
		List.Add( n );
	}

	/// <summary>
	/// Clear the list of CSS transitions.
	/// </summary>
	public void Clear()
	{
		List.Clear();
	}
}
