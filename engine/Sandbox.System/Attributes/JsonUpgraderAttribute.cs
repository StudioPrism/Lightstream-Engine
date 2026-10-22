namespace Sandbox;

/// <summary>
/// An attribute that describes a version update for a JSON object.
/// </summary>
[AttributeUsage( AttributeTargets.Method )]
public class JsonUpgraderAttribute : Attribute
{
	/// <summary>
	/// The version of this upgrade.
	/// </summary>
	public int Version { get; set; }

	/// <summary>
	/// The type we're targeting for this upgrade.
	/// </summary>
	public Type Type { get; set; }

	public JsonUpgraderAttribute( Type type, int version )
	{
		Type = type;
		Version = version;
	}
}
