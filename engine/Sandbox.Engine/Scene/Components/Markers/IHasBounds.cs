namespace Sandbox;

public abstract partial class Component
{
	/// <summary>
	/// A component that has bounds
	/// </summary>
	public interface IHasBounds
	{
		/// <summary>
		/// Returns bounds, in local space
		/// </summary>
		BBox LocalBounds { get; }
	}
}
