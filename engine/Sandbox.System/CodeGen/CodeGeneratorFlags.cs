namespace Sandbox;

/// <summary>
/// Used to specify what type of code generation to perform.
/// </summary>
[Flags]
public enum CodeGeneratorFlags
{
	/// <summary>
	/// Wrap the get accessor of a property.
	/// </summary>
	WrapPropertyGet = 1,

	/// <summary>
	/// Wrap the set accessor of a property.
	/// </summary>
	WrapPropertySet = 2,

	/// <summary>
	/// Wrap a method call.
	/// </summary>
	WrapMethod = 4,

	/// <summary>
	/// Apply this to a static property or method.
	/// </summary>
	Static = 8,

	/// <summary>
	/// Apply this to an instance property or method.
	/// </summary>
	Instance = 16
}
