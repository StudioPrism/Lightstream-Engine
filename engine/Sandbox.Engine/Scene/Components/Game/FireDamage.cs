
namespace Sandbox;

/// <summary>
/// Applies fire damage to any IDamageable in our Root object. 
/// Damage is tagged "fire" and "burn"
/// </summary>
[Expose]
[Title( "Fire Damage" )]
[Category( "Game" )]
[Icon( "local_fire_department" )]
public sealed class FireDamage : Component
{
	const float _damageInterval = 0.2f;

	/// <summary>
	/// How much damage to apply per second
	/// </summary>
	[Property] public float DamagePerSecond = 20;

	TimeSince _timeSinceDamage = 0.1f;

	protected override void OnFixedUpdate()
	{
		if ( IsProxy )
			return;

		if ( _timeSinceDamage > _damageInterval )
		{
			_timeSinceDamage = 0;
			InflictDamage();
		}
	}

	void InflictDamage()
	{
		var damageInfo = new DamageInfo();
		damageInfo.Position = WorldPosition;
		damageInfo.Origin = WorldPosition;
		damageInfo.Tags.Add( "fire" );
		damageInfo.Tags.Add( "burn" );
		damageInfo.Damage = DamagePerSecond * _damageInterval;

		GameObject.Root.RunEvent<IDamageable>( x => x.OnDamage( damageInfo ) );
	}
}
