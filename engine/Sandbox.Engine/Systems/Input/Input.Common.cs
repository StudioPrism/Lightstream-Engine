namespace Sandbox.Engine;

/// <summary>
/// Temporary housing for common inputs
/// Games that don't define any input actions will get a bunch of default actions given to them
/// </summary>
internal static partial class Input
{
	internal static List<InputAction> CommonInputs { get; } = new List<InputAction>()
	{
		//
		// Movement
		//
		new InputAction( "Forward", "W" ) { GroupName = "Movement" },
		new InputAction( "Backward", "S" ) { GroupName = "Movement" },
		new InputAction( "Left", "A" ) { GroupName = "Movement" },
		new InputAction( "Right", "D" ) { GroupName = "Movement" },

		new InputAction( "Jump", "space", GamepadCode.A ) { GroupName = "Movement" },
		new InputAction( "Run", "shift", GamepadCode.LeftJoystickButton ) { GroupName = "Movement" },
		new InputAction( "Walk", "alt" ) { GroupName = "Movement" },
		new InputAction( "Duck", "ctrl", GamepadCode.B ) { GroupName = "Movement" },

		//
		// Actions
		//
		new InputAction( "Attack1", "mouse1", GamepadCode.RightTrigger ) { GroupName = "Actions", Title = "Primary Attack" },
		new InputAction( "Attack2", "mouse2", GamepadCode.LeftTrigger ) { GroupName = "Actions", Title = "Secondary Attack" },
		new InputAction( "Reload", "r", GamepadCode.X ){ GroupName = "Actions" },
		new InputAction( "Use", "e", GamepadCode.Y ){ GroupName = "Actions" },

		//
		// Inventory
		//
		new InputAction( "Slot1", "1", GamepadCode.DpadWest ) { GroupName = "Inventory", Title = "Slot #1" },
		new InputAction( "Slot2", "2", GamepadCode.DpadEast ) { GroupName = "Inventory", Title = "Slot #2" },
		new InputAction( "Slot3", "3", GamepadCode.DpadSouth ) { GroupName = "Inventory", Title = "Slot #3" },
		new InputAction( "Slot4", "4" ) { GroupName = "Inventory", Title = "Slot #4" },
		new InputAction( "Slot5", "5" ) { GroupName = "Inventory", Title = "Slot #5" },
		new InputAction( "Slot6", "6" ) { GroupName = "Inventory", Title = "Slot #6" },
		new InputAction( "Slot7", "7" ) { GroupName = "Inventory", Title = "Slot #7" },
		new InputAction( "Slot8", "8" ) { GroupName = "Inventory", Title = "Slot #8" },
		new InputAction( "Slot9", "9" ) { GroupName = "Inventory", Title = "Slot #9" },
		new InputAction( "Slot0", "0" ) { GroupName = "Inventory", Title = "Slot #0" },

		new InputAction( "SlotPrev", "mouse4", GamepadCode.SwitchLeftBumper ) { GroupName = "Inventory", Title = "Previous Slot" },
		new InputAction( "SlotNext", "mouse5", GamepadCode.SwitchRightBumper ) { GroupName = "Inventory", Title = "Next Slot" },

		//
		// Misc
		//
		new InputAction( "View", "C", GamepadCode.RightJoystickButton ),
		new InputAction( "Voice", "v" ),
		new InputAction( "Drop", "g" ),
		new InputAction( "Flashlight", "f", GamepadCode.DpadNorth ),
		new InputAction( "Score", "tab", GamepadCode.SwitchLeftMenu ) { Title = "Scoreboard" },
		new InputAction( "Menu", "Q", GamepadCode.SwitchRightMenu ),
		new InputAction( "Chat", "enter" ),
	};
}
