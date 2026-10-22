namespace Sandbox.UI;

/// <summary>
/// List of CSS pseudo-classes used by the styling system for hover, active, etc.
/// This acts as a bit-flag.
/// </summary>
[System.Flags]
public enum PseudoClass
{
	/// <summary>
	/// No pseudo-class.
	/// </summary>
	None = 0,

	/// <summary>
	/// Unused.
	/// </summary>
	Unknown = 1 << 1,

	/// <summary>
	/// <c>:hover</c> - Any element with the mouse cursor hovering over it.
	/// </summary>
	Hover = 1 << 2,

	/// <summary>
	/// <c>:active</c> - A button that is being pressed down.
	/// </summary>
	Active = 1 << 3,

	/// <summary>
	/// <c>:focus</c> - An element with input focus.
	/// </summary>
	Focus = 1 << 4,

	/// <summary>
	/// <c>:intro</c> - Present on all elements for their first frame. Useful to start CSS transitions on creation.
	/// </summary>
	Intro = 1 << 5,

	/// <summary>
	/// <c>:outro</c> - The element has been marked for deletion, and will be deleted once all CSS transitions on it has stopped.<br/>
	/// Transitions can be started here to gracefully remove the element visually.
	/// </summary>
	Outro = 1 << 6,

	/// <summary>
	/// <c>:empty</c> - Any element that has no children.
	/// </summary>
	Empty = 1 << 7,

	/// <summary>
	/// <c>:first-child</c> - The element is the first element among a group of sibling elements.
	/// </summary>
	FirstChild = 1 << 8,

	/// <summary>
	/// <c>:last-child</c> - The element is the last element among a group of sibling elements.
	/// </summary>
	LastChild = 1 << 9,

	/// <summary>
	/// <c>:only-child</c> - The element is the only child of their parent element.
	/// </summary>
	OnlyChild = 1 << 10,

	/// <summary>
	/// <c>:before</c> - Creates an element on the parent element
	/// </summary>
	Before = 1 << 11,

	/// <summary>
	/// <c>:after</c> - Creates an element on the parent element
	/// </summary>
	After = 1 << 12,
}
