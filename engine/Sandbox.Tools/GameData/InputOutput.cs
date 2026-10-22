using Native;

namespace Editor;

public enum InputOutputType
{
	Invalid = -1,
	Void,
	Int,
	Bool,
	String,
	Float,
	Vector,
	EHandle,
	Color,
	Script
}

/// <summary>
/// Represents a variable
/// </summary>
public partial class InputOutputBase
{
	public string Name { get; set; }
	public string Description { get; set; }
	public InputOutputType Type { get; set; } // TODO: Use C# Type?

	internal static InputOutputBase FromNative( CClassInputOutputBase native )
	{
		InputOutputBase inOut = null;
		if ( native.IsInput() ) inOut = new Input();
		if ( native.IsOutput() ) inOut = new Output();
		if ( inOut == null ) return null;

		inOut.Name = native.GetName();
		inOut.Description = native.GetDescription();
		inOut.Type = native.GetType_Native();
		return inOut;
	}
}

public partial class Input : InputOutputBase { }
public partial class Output : InputOutputBase { }
