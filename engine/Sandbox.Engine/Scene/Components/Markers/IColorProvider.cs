namespace Sandbox;

public partial class Component
{
	/// <summary>
	/// When applied to a <see cref="Component"/>, the component will be able to provide the color to use for certain UI editor elements.
	/// </summary>
	public interface IColorProvider
	{
		Color ComponentColor { get; }
	}

}
