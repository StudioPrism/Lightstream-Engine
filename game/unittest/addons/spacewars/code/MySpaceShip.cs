
using Sandbox;

namespace SpaceWars;

class BlueSpaceShip : BaseSpaceShip
{

	public void ExtraMethod()
	{
		Log.Info( "Adding an extra method should mean that Fast Hotload doesn't work.."  );
	}

	public override void ShootLaser()
	{
		Log.Info( "This method is changed! Should just use fast hotload!" );
	}
}

