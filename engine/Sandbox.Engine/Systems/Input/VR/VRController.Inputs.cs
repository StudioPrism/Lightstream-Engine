namespace Sandbox.VR;

partial record VRController
{
	/// <summary>
	/// The trigger input on this controller
	/// </summary>
	public AnalogInput Trigger { get; internal set; }

	/// <summary>
	/// The grip input on this controller
	/// </summary>
	public AnalogInput Grip { get; internal set; }

	/// <summary>
	/// The primary joystick input on this controller
	/// </summary>
	public AnalogInput2D Joystick { get; internal set; }

	/// <summary>
	/// The primary joystick press on this controller
	/// </summary>
	public DigitalInput JoystickPress { get; internal set; }

	/// <summary>
	/// The primary button on this controller (Usually A, can be X for Oculus Touch)
	/// </summary>
	public DigitalInput ButtonA { get; internal set; }

	/// <summary>
	/// The secondary button on this controller (Usually B, can be Y for Oculus Touch)
	/// </summary>
	public DigitalInput ButtonB { get; internal set; }
}
