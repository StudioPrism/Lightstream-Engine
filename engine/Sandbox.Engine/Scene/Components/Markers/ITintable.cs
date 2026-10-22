namespace Sandbox;

public abstract partial class Component
{
	/// <summary>
	/// A <see cref="Component"/> that lets you change its color.
	/// </summary>
	public interface ITintable
	{
		public Color Color { get; set; }
	}
}
