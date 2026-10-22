using Sandbox;

namespace Editor;

public static partial class InputSystem
{
	public static IReadOnlyList<InputAction> GetCommonInputs() => Sandbox.Engine.Input.CommonInputs;
}
