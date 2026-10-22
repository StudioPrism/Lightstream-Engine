namespace Sandbox;

public static partial class Input
{
	/// <summary>
	/// Trigger a haptic event on supported controllers including Xbox trigger impulse rumble.
	/// </summary>
	/// <remarks>
	/// SDL will translate these commands into haptic pulses that should work on all controller types.
	/// </remarks>
	/// <param name="leftMotor">The speed of the left motor, between 0.0 and 1.0.</param>
	/// <param name="rightMotor">The speed of the right motor, between 0.0 and 1.0.</param>
	/// <param name="leftTrigger">(Xbox One controller only) The speed of the left trigger motor, between 0.0 and 1.0.</param>
	/// <param name="rightTrigger">(Xbox One controller only) The speed of the right trigger motor, between 0.0 and 1.0.</param>
	/// <param name="duration">How long (in milliseconds) should we apply this for?</param>
	public static void TriggerHaptics( float leftMotor, float rightMotor, float leftTrigger = 0.0f, float rightTrigger = 0.0f, int duration = 500 )
	{
		var controller = Input.CurrentController;
		if ( controller is null ) return;

		if ( controller.ActiveHapticEffect != null )
		{
			controller.StopAllVibrations();
		}

		if ( leftMotor > 0f || rightMotor > 0f )
		{
			controller.Rumble( leftMotor.Remap( 0, 1, 0, 0xFFFF, true ).CeilToInt(), rightMotor.Remap( 0, 1, 0, 0xFFFF, true ).CeilToInt(), duration );
		}

		if ( leftTrigger > 0f || rightTrigger > 0f )
		{
			controller.RumbleTriggers( leftTrigger.Remap( 0, 1, 0, 0xFFFF, true ).CeilToInt(), rightTrigger.Remap( 0, 1, 0, 0xFFFF, true ).CeilToInt(), duration );
		}
	}

	/// <summary>
	/// Trigger haptics based on a predefined <see cref="HapticEffect"/>.
	/// All <see cref="HapticEffect"/>s are normalized (start at 0, peak at 1).
	/// </summary>
	/// <param name="pattern">The pattern to use</param>
	/// <param name="lengthScale">The amount to scale the pattern's length by.</param>
	/// <param name="frequencyScale">The amount to scale the pattern's frequency by.</param>
	/// <param name="amplitudeScale">The amount to scale the pattern's amplitude by.</param>
	public static void TriggerHaptics( HapticEffect pattern, float lengthScale = 1.0f, float frequencyScale = 1.0f, float amplitudeScale = 1.0f )
	{
		var controller = Input.CurrentController;
		if ( controller is null ) return;

		controller.TriggerHapticEffect( pattern, lengthScale, frequencyScale, amplitudeScale );
	}

	/// <summary>
	/// Trigger haptics based on a predefined <see cref="HapticEffect"/>.
	/// All <see cref="HapticEffect"/>s are normalized (start at 0, peak at 1).
	/// </summary>
	/// <param name="pattern">The pattern to use</param>
	/// <param name="frequencyScale">The amount to scale the pattern's frequency by.</param>
	/// <param name="amplitudeScale">The amount to scale the pattern's amplitude by.</param>
	public static void TriggerHaptics( HapticEffect pattern, float frequencyScale, float amplitudeScale )
	{
		var controller = Input.CurrentController;
		if ( controller is null ) return;

		controller.TriggerHapticEffect( pattern, 1.0f, frequencyScale, amplitudeScale );
	}

	/// <summary>
	/// Stop all vibration events on the current controller.
	/// </summary>
	public static void StopAllHaptics()
	{
		var controller = Input.CurrentController;
		if ( controller is null ) return;

		controller.StopAllHaptics();
	}
}
