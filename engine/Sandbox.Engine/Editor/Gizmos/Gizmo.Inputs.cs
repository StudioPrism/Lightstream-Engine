namespace Sandbox;

public static partial class Gizmo
{
	/// <summary>
	/// The input state, allows interaction with Gizmos
	/// </summary>
	public struct Inputs
	{
		public Vector2 CursorPosition { get; set; }
		public Ray CursorRay { get; set; }
		public bool LeftMouse { get; set; }
		public bool RightMouse { get; set; }
		public bool DoubleClick { get; set; }
		public KeyboardModifiers Modifiers { get; set; }
		public SceneCamera Camera { get; set; }

		/// <summary>
		/// True if the scene is being hovered by the mouse. False if the cursor is being used somewhere else
		/// </summary>
		public bool IsHovered { get; set; }
	}
}
