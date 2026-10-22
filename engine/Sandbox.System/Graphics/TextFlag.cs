namespace Sandbox;

/// <summary>
/// Flags dictating position of text (and other elements).
/// Default alignment on each axis is Top, Left.
/// Values for each axis can be combined into a single value, conflicting values have undefined behavior.
/// </summary>
[Flags]
public enum TextFlag
{
	None = 0,
	/// <summary>
	/// Align to the left on the X axis.
	/// </summary>
	Left = 0x0001,

	/// <summary>
	/// Align to the right on the X axis.
	/// </summary>
	Right = 0x0002,

	/// <summary>
	/// Align in the center on the X axis.
	/// </summary>
	CenterHorizontally = 0x0004,
	Justify = 0x0008,
	Absolute = 0x0010,

	/// <summary>
	/// Anchor to the top on the Y axis.
	/// </summary>
	Top = 0x0020,

	/// <summary>
	/// Anchor to the bottom on the Y axis.
	/// </summary>
	Bottom = 0x0040,

	/// <summary>
	/// Align in the center on the Y axis.
	/// </summary>
	CenterVertically = 0x0080,

	/// <summary>
	/// Anchor to the top left corner.
	/// </summary>
	LeftTop = Left | Top,

	/// <summary>
	/// Anchor to the left side, center vertically.
	/// </summary>
	LeftCenter = Left | CenterVertically,

	/// <summary>
	/// Anchor to the bottom left corner.
	/// </summary>
	LeftBottom = Left | Bottom,

	/// <summary>
	/// Anchor to the top side, center horizontally.
	/// </summary>
	CenterTop = CenterHorizontally | Top,

	/// <summary>
	/// Align in the center on both axises.
	/// </summary>
	Center = CenterHorizontally | CenterVertically,

	/// <summary>
	/// Anchor to the bottom side, center horizontally.
	/// </summary>
	CenterBottom = CenterHorizontally | Bottom,

	/// <summary>
	/// Anchor to the top right corner.
	/// </summary>
	RightTop = Right | Top,

	/// <summary>
	/// Anchor to the right side, center vertically.
	/// </summary>
	RightCenter = Right | CenterVertically,

	/// <summary>
	/// Anchor to the bottom right corner.
	/// </summary>
	RightBottom = Right | Bottom,

	/// <summary>
	/// Limit the text to a single line. Used in <c>Graphics.DrawText</c> and <c>Graphics.MeasureText</c>.
	/// </summary>
	SingleLine = 0x0100,

	/// <summary>
	/// Do not cutoff text beyond its bounds. Used in <c>Graphics.DrawText</c> and <c>Graphics.MeasureText</c>.
	/// </summary>
	DontClip = 0x0200,

	//ExpandTabs = 0x0400,
	//ShowMnemonic = 0x0800,

	WordWrap = 0x1000,
	WrapAnywhere = 0x2000,

	//DontPrint = 0x4000,
	//IncludeTrailingSpaces = 0x08000000,
	//HideMnemonic = 0x8000,
	//JustificationForced = 0x10000,
	//ForceLeftToRight = 0x20000,
	//ForceRightToLeft = 0x40000,

	// Note: Commenting out stuff that is bullshit
};
