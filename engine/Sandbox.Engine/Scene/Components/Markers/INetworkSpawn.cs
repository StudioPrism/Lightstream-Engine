namespace Sandbox;

public abstract partial class Component
{
	/// <summary>
	/// A <see cref="Component"/> with this interface can listen for when a GameObject
	/// in its ancestors has been network spawned.
	/// </summary>
	public interface INetworkSpawn
	{
		/// <summary>
		/// Called when this object is spawned on the network.
		/// </summary>
		public void OnNetworkSpawn( Connection owner );
	}
}
