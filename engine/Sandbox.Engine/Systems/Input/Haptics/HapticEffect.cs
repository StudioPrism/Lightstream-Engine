namespace Sandbox;

/// <summary>
/// Contains a haptic effect, which consists of patterns for the controller and triggers.
/// </summary>
public partial record class HapticEffect
{
	public HapticPattern ControllerPattern;
	public HapticPattern LeftTriggerPattern;
	public HapticPattern RightTriggerPattern;

	public float AmplitudeScale
	{
		set
		{
			if ( ControllerPattern != null ) ControllerPattern.AmplitudeScale = value;
			if ( LeftTriggerPattern != null ) LeftTriggerPattern.AmplitudeScale = value;
			if ( RightTriggerPattern != null ) RightTriggerPattern.AmplitudeScale = value;
		}
	}

	public float FrequencyScale
	{
		set
		{
			if ( ControllerPattern != null ) ControllerPattern.FrequencyScale = value;
			if ( LeftTriggerPattern != null ) LeftTriggerPattern.FrequencyScale = value;
			if ( RightTriggerPattern != null ) RightTriggerPattern.FrequencyScale = value;
		}
	}

	public float LengthScale
	{
		set
		{
			if ( ControllerPattern != null ) ControllerPattern.LengthScale = value;
			if ( LeftTriggerPattern != null ) LeftTriggerPattern.LengthScale = value;
			if ( RightTriggerPattern != null ) RightTriggerPattern.LengthScale = value;
		}
	}

	public HapticEffect( HapticPattern controllerPattern = null, HapticPattern leftTriggerPattern = null, HapticPattern rightTriggerPattern = null )
	{
		ControllerPattern = controllerPattern;
		LeftTriggerPattern = leftTriggerPattern;
		RightTriggerPattern = rightTriggerPattern;
	}

	internal void Reset()
	{
		ControllerPattern?.Reset();
		LeftTriggerPattern?.Reset();
		RightTriggerPattern?.Reset();
	}

	internal void Update( Controller controller )
	{
		float leftMotor = 0f, rightMotor = 0f, leftTrigger = 0f, rightTrigger = 0f;

		//
		// Fetch motor values
		//
		{
			if ( ControllerPattern != null )
			{
				ControllerPattern.Update( out var amplitude, out var frequency );
				GenerateMotorValues( amplitude, frequency, out leftMotor, out rightMotor );
			}

			LeftTriggerPattern?.Update( out leftTrigger, out _ );
			RightTriggerPattern?.Update( out rightTrigger, out _ );
		}

		const int duration = 100;

		controller.Rumble(
			leftMotor.Remap( 0, 1, 0, 0xFFFF, true ).CeilToInt(),
			rightMotor.Remap( 0, 1, 0, 0xFFFF, true ).CeilToInt(),
			duration
		);

		controller.RumbleTriggers(
			leftTrigger.Remap( 0, 1, 0, 0xFFFF, true ).CeilToInt(),
			rightTrigger.Remap( 0, 1, 0, 0xFFFF, true ).CeilToInt(),
			duration
		);
	}

	private static void GenerateMotorValues( float amplitude, float frequency, out float leftMotor, out float rightMotor )
	{
		if ( amplitude < 0 || amplitude > 1 ) amplitude = amplitude.Clamp( 0, 1 );
		if ( frequency < 0 ) frequency = 0;

		if ( frequency <= 0.5f ) leftMotor = 1.0f - (frequency * 2.0f);
		else leftMotor = 0.0f;

		if ( frequency >= 0.5f ) rightMotor = (frequency - 0.5f) * 2.0f;
		else rightMotor = 0.0f;

		leftMotor *= amplitude;
		rightMotor *= amplitude;

		leftMotor = leftMotor.Clamp( 0, 1 );
		rightMotor = rightMotor.Clamp( 0, 1 );
	}
}
