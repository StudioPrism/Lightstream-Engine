using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sandbox;

[Expose]
public class ControlModeSettings
{
	[Display( Name = "Keyboard" )]
	public bool Keyboard { get; set; } = false;
	public bool VR { get; set; } = false;
	public bool Gamepad { get; set; } = false;

	[JsonIgnore, Hide]
	public bool IsVROnly => VR && !Keyboard && !Gamepad;
}
