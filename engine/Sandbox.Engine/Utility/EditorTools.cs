using Sandbox.Engine;

namespace Sandbox.Utility;

/// <summary>
/// Functions to interact with the tools system. Does nothing if tools aren't enabled.
/// </summary>
public static partial class EditorTools
{
	/// <summary>
	/// Set the object to be inspected by the inspector in the editor
	/// </summary>
	public static object InspectorObject
	{
		get => IToolsDll.Current?.InspectedObject;
		set
		{
			if ( IToolsDll.Current != null )
				IToolsDll.Current.InspectedObject = value;
		}
	}
}
