using Microsoft.AspNetCore.Components;
using Sandbox.UI.Layout;

namespace Sandbox.UI;

/// <summary>
/// A virtualized, scrollable grid panel that only creates item panels when visible.
/// </summary>
public sealed class VirtualGrid : BaseVirtualPanel
{
	/// <summary>
	/// Grid layout used to position/measure items. (Swappable later if needed.)
	/// </summary>
	internal GridLayout Layout { get; } = new();

	/// <summary>
	/// Fixed width of each item. If &lt; 0, layout may stretch to fill width.
	/// </summary>
	[Parameter]
	public Vector2 ItemSize
	{
		get => new( Layout.ItemWidth, Layout.ItemHeight );
		set
		{
			Layout.ItemWidth = value.x;
			Layout.ItemHeight = value.y;
		}
	}

	protected override void UpdateLayoutSpacing( Vector2 spacing )
	{
		Layout.Spacing = spacing;
	}

	protected override bool UpdateLayout()
	{
		return Layout.Update( Box, ScaleFromScreen, ScrollOffset.y * ScaleFromScreen );
	}

	protected override void GetVisibleRange( out int first, out int pastEnd )
	{
		Layout.GetVisibleRange( out first, out pastEnd );
	}

	protected override void PositionPanel( int index, Panel panel )
	{
		Layout.Position( index, panel );
	}

	protected override float GetTotalHeight( int itemCount )
	{
		return Layout.GetHeight( itemCount );
	}
}
