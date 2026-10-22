using static Editor.QPalette;

namespace Editor;

internal class QPalette
{
	public enum ColorGroup
	{
		Active,
		Disabled,
		Inactive,

		NColorGroups = Inactive + 1
	}

	public enum ColorRole
	{
		WindowText,
		Button,
		Light,
		Midlight,
		Dark,
		Mid,
		Text,
		BrightText,
		ButtonText,
		Base,
		Window,
		Shadow,
		Highlight,
		HighlightedText,
		Link,
		LinkVisited,
		AlternateBase,
		NoRole,
		ToolTipBase,
		ToolTipText,
		PlaceholderText,

		NColorRoles_Qt = PlaceholderText + 1,

		Grid,

		DarkSplitterHandle,
		LightSplitterHandle,

		ScrollbarBackground,

		HeaderSectionBackground,
		HeaderSectionLine,

		Black,
		Red,
		Green,
		Blue,

		CheckmarkPossible,
		FlatSelection,
		InnerHighlight,

		LighterLine,
		UpperLine,
		LowerLine,

		DarkSplitter,

		MenuBackground,
		MenuBarBackground,

		ToolButtonBackground,
		ToolButtonBorder,
		ToolButtonActiveBackground,
		ToolButtonActiveBorder,

		ToolBarBorderLight,
		ToolBarBorderDark,
		ToolBarBackground,

		ViewportToolBarBorderLight,
		ViewportToolBarBorderDark,

		ButtonBackground,
		ButtonBorder,
		ButtonHighlight,
		ButtonPressed,

		ViewportBackground,
		ViewportText,

		DockWidgetTitleBackground,

		DefaultButtonOuterBorder,
		DefaultButtonInnerBorder,
		DefaultButtonBackground,

		TabBarBackgroundNotSelected,
		TabTextInactive,
		TabBarBackground,

		NumColorRoles
	}
}
