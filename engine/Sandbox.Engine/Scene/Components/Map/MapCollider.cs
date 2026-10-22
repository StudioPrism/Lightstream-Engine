namespace Sandbox;

[Expose]
[Hide]
[Title( "Collider - Map" )]
[Category( "Physics" )]
[Icon( "panorama_fish_eye" )]
public class MapCollider : Collider
{
	protected override IEnumerable<PhysicsShape> CreatePhysicsShapes( PhysicsBody targetBody, Transform local )
	{
		yield break;
	}
}
