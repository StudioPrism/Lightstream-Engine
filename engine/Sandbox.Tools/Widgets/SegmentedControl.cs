using Sandbox.UI;
using System;

namespace Editor;

public class SegmentedControl : Widget
{
	private List<string> options = new();
	private int selectedIndex = 0;

	public int SelectedIndex
	{
		get => selectedIndex;
		set
		{
			selectedIndex = value;
			SelectedChanged();
		}
	}

	public string Selected
	{
		get => options[selectedIndex];
		set
		{
			var index = options.IndexOf( value );
			if ( index < 0 )
				return;

			selectedIndex = index;
			Update();
		}
	}

	public Action<string> OnSelectedChanged;

	bool _showtext = true;

	public bool ShowText
	{
		get => _showtext;

		set
		{
			if ( ShowText == value ) return;

			_showtext = value;

			foreach ( var option in Children.OfType<Option>() )
			{
				option.ShowText = _showtext;
			}
		}
	}

	public SegmentedControl( Widget parent = null ) : base( parent )
	{
		FixedHeight = 24;
		Layout = Layout.Row();
		Layout.Spacing = 1;
		Layout.Margin = 1;
	}

	protected override void DoLayout()
	{
		var first = Children.OfType<Option>().FirstOrDefault();
		foreach ( var option in Children.OfType<Option>() )
		{
			// Only show text if we can show text for all options and we have enough room
			option.ShowText = first.Width > 72f;
		}
	}

	public void AddOption( string name, string icon = null, int? count = null )
	{
		int index = options.Count;

		Layout.Add( new Option( name, icon, () =>
		{
			selectedIndex = index;
			SelectedChanged();
		} )
		{ ShowText = ShowText, Index = index, Count = count }, 1 );

		options.Add( name );
	}

	public bool HasOption( string name )
	{
		return options.Contains( name );
	}

	private void SelectedChanged()
	{
		OnSelectedChanged?.Invoke( Selected );
		Update();
	}

	protected override void OnPaint()
	{
		Paint.ClearPen();
		Paint.ClearBrush();

		Paint.Antialiasing = true;

		Paint.SetBrush( Theme.ControlBackground );
		Paint.DrawRect( LocalRect, 4 );

		var selectedOption = Children.OfType<Option>().FirstOrDefault( x => x.Index == selectedIndex );
		if ( selectedOption is not null )
		{
			var selectedRect = selectedOption.LocalRect;
			selectedRect.Position = selectedOption.Position;

			Paint.SetBrush( Theme.Primary );
			Paint.DrawRect( selectedRect, 4 );
		}
	}

	class Option : Widget
	{
		public string Icon { get; set; }
		public Action OnClick { get; set; }
		public bool ShowText { get; set; }
		public int Index { get; set; }
		public int? Count { get; set; }

		public Option( string name, string icon = null, Action onClick = null, Widget parent = null ) : base( parent )
		{
			Name = name;
			Icon = icon;
			OnClick = onClick;
			Cursor = CursorShape.Finger;
			ToolTip = name;
		}

		protected override Vector2 SizeHint()
		{
			if ( !ShowText )
			{
				return 24;
			}

			var textSize = Paint.MeasureText( Name );
			textSize.x += 16;
			textSize.y += 8;

			if ( !string.IsNullOrWhiteSpace( Icon ) )
			{
				textSize.x += 22 + 8;
			}

			return textSize;
		}

		protected override void OnMouseClick( MouseEvent e )
		{
			base.OnMouseClick( e );
			OnClick?.Invoke();
		}

		protected override void OnPaint()
		{
			Paint.ClearPen();
			Paint.ClearBrush();

			Paint.Antialiasing = true;

			var r = LocalRect;
			Paint.ClearPen();

			float alpha = (Paint.HasMouseOver) ? 0.1f : 0f;

			Paint.SetBrush( Theme.Text.WithAlpha( alpha ) );
			Paint.DrawRect( r, 3 );

			if ( !ShowText )
			{
				Paint.ClearBrush();
				Paint.SetPen( Theme.Text );
				Paint.DrawIcon( r, Icon, 12.0f );
				return;
			}

			Paint.ClearBrush();
			Paint.SetPen( Theme.Text );
			r.Left += 12.0f;

			var nameRect = Paint.DrawText( r, Name );
			r = nameRect;

			if ( Count.HasValue )
			{
				var rect = LocalRect;
				rect.Left = nameRect.Right + 10;
				rect.Top += 1.0f;

				Paint.SetBrush( Theme.Text.WithAlpha( 0.2f ) );
				Paint.ClearPen();
				Paint.DrawTextBox( rect, Count.Value.ToString(), Theme.Text, new Margin( 5, 0 ), 3.0f, TextFlag.LeftCenter );
			}

			if ( !string.IsNullOrEmpty( Icon ) )
			{
				r.Left -= nameRect.Width + 24.0f;
				r.Top += 2.0f;

				Paint.ClearBrush();
				Paint.SetPen( Theme.Text );
				Paint.DrawIcon( r, Icon, 12.0f );
			}
		}
	}
}
