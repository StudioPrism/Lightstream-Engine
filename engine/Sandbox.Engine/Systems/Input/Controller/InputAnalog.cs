using NativeEngine;

namespace Sandbox;

/// <summary>
/// An analog input, when fetched, is between -1 and 1 (0 being default)
/// </summary>
public enum InputAnalog : int
{
	[Title( "Right Analog Stick - X Axis" )]
	RightStickX,
	[Title( "Right Analog Stick - Y Axis" )]
	RightStickY,
	[Title( "Left Analog Stick - X Axis" )]
	LeftStickX,
	[Title( "Left Analog Stick - Y Axis" )]
	LeftStickY,
	[Title( "Left Trigger" )]
	LeftTrigger,
	[Title( "Right Trigger" )]
	RightTrigger
}

internal static partial class InputAnalogExtensions
{
	internal static GameControllerAxis ToAxis( this InputAnalog x )
	{
		return x switch
		{
			InputAnalog.LeftStickX => GameControllerAxis.LeftX,
			InputAnalog.LeftStickY => GameControllerAxis.LeftY,
			InputAnalog.RightStickX => GameControllerAxis.RightX,
			InputAnalog.RightStickY => GameControllerAxis.RightY,
			InputAnalog.LeftTrigger => GameControllerAxis.TriggerLeft,
			InputAnalog.RightTrigger => GameControllerAxis.TriggerRight,
			_ => default
		};
	}
}
