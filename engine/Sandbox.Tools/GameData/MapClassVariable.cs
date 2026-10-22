using System;

namespace Editor;

/// <summary>
/// Represents a variable.
/// </summary>
public partial class MapClassVariable
{
	/// <summary>
	/// The internal name.
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// The user friendly name for UI.
	/// </summary>
	public string LongName { get; set; }

	/// <summary>
	/// Description for this variable.
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// Category or group for this variable.
	/// </summary>
	public string GroupName { get; set; }

	/// <summary>
	/// Data type for this variable.
	/// </summary>
	public Type PropertyType { get; set; }

	/// <summary>
	/// Default value for this variable.
	/// </summary>
	public object DefaultValue { get; set; }

	/// <summary>
	/// Internal, used to override the type to one the tools understand.
	/// </summary>
	public string PropertyTypeOverride { get; set; }

	/// <summary>
	/// General purpose key-value store to alter functionality of UI, map compilation, editor helpers, etc.
	/// </summary>
	public Dictionary<string, string> Metadata { get; set; } = new();

	// Min/Max, Hidden, Important, Randomizable

	public override string ToString()
	{
		return $"MapClassVariable( {Name} )";
	}
}
