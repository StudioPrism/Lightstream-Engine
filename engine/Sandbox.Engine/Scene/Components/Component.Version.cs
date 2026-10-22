using System.Text.Json.Nodes;

namespace Sandbox;

public partial class Component
{
	/// <summary>
	/// The version of the component. Used by <see cref="JsonUpgrader"/>.
	/// </summary>
	public virtual int ComponentVersion => 0;
}
