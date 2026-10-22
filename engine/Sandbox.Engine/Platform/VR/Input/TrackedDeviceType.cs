namespace Sandbox.VR;

public enum TrackedDeviceType
{
	/// <summary>
	/// The ID was not valid.
	/// </summary>
	Invalid = 0,

	/// <summary>
	/// Head-mounted display (your headset)
	/// </summary>
	Hmd = 1,

	/// <summary>
	/// Tracked controllers
	/// </summary>
	Controller = 2,

	/// <summary>
	/// Generic trackers
	/// </summary>
	Tracker = 3,

	/// <summary>
	/// Camera and base stations that serve as tracking reference points
	/// </summary>
	BaseStation = 4,

	/// <summary>
	/// Accessories that aren't necessarily tracked themselves, but may redirect video output from other tracked devices
	/// </summary>
	Redirect = 5
}
