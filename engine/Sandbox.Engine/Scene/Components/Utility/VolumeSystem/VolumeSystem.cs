namespace Sandbox.Volumes;

/// <summary>
/// A base GameObjectSystem for handling of IVolume components. You can use this to find volume components
/// by position.
/// </summary>
public sealed class VolumeSystem : GameObjectSystem<VolumeSystem>
{
	public VolumeSystem( Scene scene ) : base( scene )
	{

	}

	/// <summary>
	/// Find a volume of this type, at this point. Will return null if none.
	/// </summary>
	public T FindSingle<T>( Vector3 position ) where T : IVolume
	{
		return FindAll<T>( position ).FirstOrDefault();
	}

	/// <summary>
	/// Find all volumes of this type, at this point
	/// </summary>
	public IEnumerable<T> FindAll<T>( Vector3 position ) where T : IVolume
	{
		// TODO - this won't scale - some kind of spacial lookup in the future
		// where we get a callback when a component is enabled
		// and if it's a volume, keep track of it somehow
		// and then be able to find all volumes in an oct tree or some shit

		foreach ( var volume in Scene.GetAll<T>() )
		{
			if ( !volume.Test( position ) )
				continue;

			yield return volume;
		}
	}

	public interface IVolume
	{
		SceneVolume GetVolume();

		bool Test( Vector3 worldPosition );
		bool Test( BBox worldBBox );
		bool Test( Sphere worldSphere );
	}
}
