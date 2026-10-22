namespace Editor;

public class PopupDialogWidget : Widget
{
	public Label MessageLabel { get; set; }

	public Layout ButtonLayout { get; private set; }

	public PopupDialogWidget( string icon = "⚠️" ) : base( null, true )
	{
		WindowFlags = WindowFlags.Window | WindowFlags.Customized | WindowFlags.WindowTitle | WindowFlags.MSWindowsFixedSizeDialogHint;
		WindowTitle = "title";

		FixedWidth = 650;

		Layout = Layout.Row();
		Layout.Margin = 16;

		var iconColumn = Layout.AddColumn();

		iconColumn.Margin = 0;
		iconColumn.Add( new IconButton( icon ) { IconSize = 48, FixedHeight = 64, FixedWidth = 64, Background = Color.Transparent, TransparentForMouseEvents = true } );
		iconColumn.AddStretchCell();

		Layout.Spacing = 32;

		var column = Layout.AddColumn();

		column.AddSpacingCell( 16 );

		MessageLabel = column.Add( new Label() );
		MessageLabel.WordWrap = true;
		MessageLabel.MinimumWidth = 600;
		MessageLabel.TextSelectable = true;

		column.AddSpacingCell( 16 );
		column.AddStretchCell();

		ButtonLayout = column.AddRow();
		ButtonLayout.Spacing = 8;
	}

	protected override bool OnClose()
	{
		return true;
	}

	protected override void OnPaint()
	{
		base.OnPaint();

		Paint.SetBrushAndPen( Theme.WindowBackground );
		Paint.DrawRect( LocalRect );
	}
}
