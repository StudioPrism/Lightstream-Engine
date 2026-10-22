using Sandbox;

namespace NativeEngine
{
	internal struct InputEvent
	{
		public IntPtr m_hWnd;        // Window to which the event is sent
		public InputEventType EventType;                // Type of the event (see InputEventType_t)
		public int m_nTick;                // Tick on which the event occurred
		public ulong m_nData;             // Generic 64-bit data, what it contains depends on the event
		public int m_nData2;               // Generic 32-bit data, what it contains depends on the event
		public int m_nData3;               // Generic 32-bit data, what it contains depends on the event
		public float m_fData;              // Generic float data.
		public float m_fData2;             // Generic float data.

		public ButtonCode Button => (ButtonCode)m_nData;

		public KeyboardModifiers KeyboardModifiers
		{
			get
			{
				KeyboardModifiers m = KeyboardModifiers.None;

				if ( (m_nData2 & 1) == 1 ) m |= KeyboardModifiers.Shift;
				if ( (m_nData2 & 2) == 2 ) m |= KeyboardModifiers.Ctrl;
				if ( (m_nData2 & 4) == 4 ) m |= KeyboardModifiers.Alt;
				//if ( (m_nData2 & 8) == 8 ) m |= KeyboardModifiers.Windows;
				//if ( (m_nData2 & 16) == 8 ) m |= KeyboardModifiers.Finger;

				return m;
			}
		}

		/// <summary>
		/// True if this is as a result of a button being pressed
		/// </summary>
		public bool IsButtonPress
		{
			get
			{
				if ( EventType == NativeEngine.InputEventType.ButtonPressed ) return true;
				if ( EventType == NativeEngine.InputEventType.ButtonReleased ) return true;

				return false;
			}
		}

		public bool IsMouseButton => Button >= ButtonCode.MOUSE_FIRST && Button <= ButtonCode.MOUSE_LAST;

		/// <summary>
		/// We let some button presses skip the UI completely. These can then be used as key binds that
		/// can always run. We mainly do this with the F keys.
		/// </summary>
		public bool IsGameButton
		{
			get
			{
				if ( !IsButtonPress ) return false;

				// If we're a game button, don't let the UI have any of it
				if ( Button >= ButtonCode.KEY_F1 && Button <= ButtonCode.KEY_F12 ) return true;

				return false;
			}
		}
	}

	internal enum InputEventType : int
	{
		ButtonPressed = 0,
		ButtonReleased,      // m_nData contains a ButtonCode_t
		ButtonDoubleClick, // m_nData contains a ButtonCode_t
		IE_AnalogValueChanged,  // m_nData contains an AnalogCode_t, m_nData2 contains the value
		IE_ButtonPressedRepeating,  // m_nData contains a ButtonCode_t.  This is similar to IE_ButtonPressed, but is called every key repeat interval while a key is held down
		CursorPositionChanged,   // m_nData contains mouse X and Y deltas, m_nData2 contains screen X and m_nData3 contains screen Y

		IE_LastStandardEvent,
		// If you add a system event, add to s_pSystemEventNames in inputsystem.cpp
		IE_FirstSystemEvent = 100,

		IE_Quit = IE_FirstSystemEvent,
		IE_ControllerInserted,  // m_nData contains the controller ID
		IE_ControllerUnplugged, // m_nData contains the controller ID
		IE_Close,
		IE_WindowSizeChanged,   // m_nData contains width, m_nData2 contains height, m_nData3 = 0 if not minimized, 1 if minimized
		IE_ActivateApp,         // Tells if any window went foreground. m_hWnd is PLAT_WINDOW_INVALID.    m_nData = 1 -> activated, 0 -> deactivated
		IE_ActivateWindow,      // Tells if a specific window showed up or went away. m_hWindow is valid. m_nData = 1 -> activated, 0 -> deactivated
		IE_WindowMove,          // The window was moved
		IE_CopyData,            // Data to be copied between applications
		IE_MonitorOrientationChanged,   // The given screen orientation changed, such as a phone rotation.  m_nData is the tier0 monitor index, m_nData2 is the new orientation.

		IE_LastSystemEvent,
		// If you add a UI event, add to s_pUIEventNames in inputsystem.cpp
		//	IE_FirstUIEvent = 200,

		KeyTyped = 200,
		KeyCodeTyped,
		IE_KeyCodeReleased,
		IE_InputLanguageChanged,
		IE_IMESetWindow,
		IE_IMEStartComposition,
		IE_IMEComposition,
		IE_IMEEndComposition,
		IE_IMEShowCandidates,
		IE_IMEChangeCandidates,
		IE_IMECloseCandidates,
		IE_IMERecomputeModes,
		IE_MultiTouchData,

		IE_LastUIEvent,
		IE_FirstVguiEvent = 1000,   // Assign ranges for other systems that post user events here
		IE_FirstAppEvent = 2000,
	};
}
