namespace Sandbox;

public abstract partial class Component
{
	/// <summary>
	/// A component that can be damaged by something.
	/// </summary>
	public interface IDamageable
	{
		void OnDamage( in DamageInfo damage );
	}
}
