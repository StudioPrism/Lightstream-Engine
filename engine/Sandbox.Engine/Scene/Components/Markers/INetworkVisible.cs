namespace Sandbox;

public abstract partial class Component
{
	/// <summary>
	/// A <see cref="Component"/> with this interface can determine whether a networked object
	/// should be visible for a specific <see cref="Connection"/>.
	/// </summary>
	public interface INetworkVisible
	{
		/// <summary>
		/// Whether this networked object is visible for the specified <see cref="Connection"/>.
		/// </summary>
		bool IsVisibleToConnection( Connection connection, in BBox worldBounds );
	}
}
