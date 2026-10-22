namespace Sandbox.VR;

/// <summary>
/// Information about a tracked device - vendor info, serial number, battery data
/// </summary>
internal record struct TrackedDeviceInfo
{
	/// <summary>
	/// Which tracking system does this use (e.g. "oculus")? <br/>
	/// Represents the value given by <c>Prop_TrackingSystemName_String</c>.
	/// </summary>
	public string VendorName = "unknown";

	/// <summary>
	/// Who manufactured this device (e.g. "Oculus")? <br />
	/// Represents the value given by <c>Prop_ManufacturerName_String</c>.
	/// </summary>
	public string ManufacturerName = "unknown";

	/// <summary>
	/// What is this device called (e.g. "Oculus Rift S (Left Controller)")? <br/>
	/// Represents the value given by <c>Prop_ModelNumber_String</c>.
	/// </summary>
	public string DisplayName = "unknown";

	/// <summary>
	/// Which render model should this use (e.g. "oculus_rifts_controller_left")? <br/>
	/// Represents the value given by <c>Prop_RenderModelName_String</c>.
	/// </summary>
	public string RenderModelName = "unknown";

	/// <summary>
	/// What is the serial number for this device (e.g. "1WMGH---------_Controller_Left")? <br/>
	/// Represents the value given by <c>Prop_SerialNumber_String</c>.
	/// </summary>
	public string SerialNumber = "unknown";

	/// <summary>
	/// Battery percentage from 0 to 100
	/// </summary>
	public float BatteryPercentage = -1;

	/// <summary>
	/// If this is a controller, then represents the value given by <c>Prop_ControllerType_String</c>, otherwise "unknown".
	/// </summary>
	public string TypeString = "unknown";

	public TrackedDeviceInfo()
	{

	}
}
