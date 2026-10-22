using Sandbox;

namespace SpaceWars;

class MySpaceShip : BaseSpaceShip
{
	public override void ShootLaser()
	{
		Log.Info( "Shooting a laser on MySpaceShip!" );
	}
}
