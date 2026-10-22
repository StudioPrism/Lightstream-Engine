namespace Sandbox;

/// <summary>
/// Game controller codes, driven from SDL.
/// </summary>
[Expose]
public enum GamepadCode : int
{
	None = -1,

	A = 0,
	B = 1,
	X = 2,
	Y = 3,
	/// <summary>
	/// Normally the small button on the left side of a gamepad
	/// </summary>
	[Title( "Back" )]
	SwitchLeftMenu = 4,
	/// <summary>
	/// The big button in the middle of a gamepad, usually with the logo on it
	/// </summary>
	Guide = 5,
	/// <summary>
	/// This is automatically used as the escape key in all games
	/// </summary>
	[Title( "Start" )]
	SwitchRightMenu = 6,
	/// <summary>
	/// The button when you press down on the stick
	/// </summary>
	[Title( "Left Analog Stick" )]
	LeftJoystickButton = 7,
	/// <summary>
	/// The button when you press down on the stick
	/// </summary>
	[Title( "Right Analog Stick" )]
	RightJoystickButton = 8,
	/// <summary>
	/// Also known as the left bumper, or LB, or L1
	/// </summary>
	[Title( "Left Shoulder" )]
	SwitchLeftBumper = 9,
	/// <summary>
	/// Also known as the right bumper, or RB, or R1
	/// </summary>
	[Title( "Right Shoulder" )]
	SwitchRightBumper = 10,
	[Title( "D-Pad Up" ), Icon( "arrow_circle_up" )]
	DpadNorth = 11,
	[Title( "D-Pad Down" ), Icon( "arrow_circle_down" )]
	DpadSouth = 12,
	[Title( "D-Pad Left" ), Icon( "arrow_circle_left" )]
	DpadWest = 13,
	[Title( "D-Pad Right" ), Icon( "arrow_circle_right" )]
	DpadEast = 14,
	/// <summary>
	/// This is a button that doesn't have a specific name, like the share button on some controllers
	/// </summary>
	[Title( "Misc" )]
	Misc1 = 15,
	/// <summary>
	/// Extra button on the back of some gamepads, like the Xbox Elite
	/// </summary>
	Paddle1 = 16,
	/// <summary>
	/// Extra button on the back of some gamepads, like the Xbox Elite
	/// </summary>
	Paddle2 = 17,
	/// <summary>
	/// Extra button on the back of some gamepads, like the Xbox Elite
	/// </summary>
	Paddle3 = 18,
	/// <summary>
	/// Extra button on the back of some gamepads, like the Xbox Elite
	/// </summary>
	Paddle4 = 19,
	Touchpad = 20,

	[Hide]
	BUTTONS_MAX = 21,

	/// <summary>
	/// Also known as LT, or L2
	/// </summary>
	LeftTrigger = 100,
	/// <summary>
	/// Also known as RT, or R2
	/// </summary>
	RightTrigger = 101
};
