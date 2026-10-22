namespace Sandbox;

partial record class HapticEffect
{
	/// <summary>
	/// A haptic pattern that represents a light, soft impact.
	/// </summary>
	public static HapticEffect SoftImpact => new( HapticPattern.SoftImpact, HapticPattern.SoftImpact, HapticPattern.SoftImpact );

	/// <summary>
	/// A haptic pattern that represents a hard, sudden impact.
	/// </summary>
	public static HapticEffect HardImpact => new( HapticPattern.HardImpact, HapticPattern.HardImpact, HapticPattern.HardImpact );

	/// <summary>
	/// Applies a simple rumble to the controller.
	/// </summary>
	public static HapticEffect Rumble => new( HapticPattern.Rumble );

	/// <summary>
	/// Applies a simple rumble to the left trigger.
	/// </summary>
	public static HapticEffect RumbleLeftTrigger => new( leftTriggerPattern: HapticPattern.Rumble );

	/// <summary>
	/// Applies a simple rumble to the right trigger.
	/// </summary>
	public static HapticEffect RumbleRightTrigger => new( rightTriggerPattern: HapticPattern.Rumble );

	/// <summary>
	/// A haptic effect that feels like a heartbeat.
	/// </summary>
	public static HapticEffect Heartbeat => new( HapticPattern.Heartbeat );
}
