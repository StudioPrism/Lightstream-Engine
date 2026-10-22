namespace Sandbox.UI.Layout;

/// <summary>
/// Layout objects in a grid
/// </summary>
internal class GridLayout
{
	private float _itemWidth = 100f;
	private float _itemHeight = 100f;

	private Rect _rect;        // inner/content (local)
	private Rect _outerRect;   // outer/viewport (local)
	private float _scrollOffset;

	private Vector2 _cellSize;
	private int _columns;
	private int _updateHash;

	/// <summary>
	/// Fixed item width (≥ 1). Values set below 1 are clamped to 1.
	/// </summary>
	public float ItemWidth
	{
		get => _itemWidth;
		set => _itemWidth = MathF.Max( 1f, value );
	}

	/// <summary>
	/// Fixed item height (≥ 1). Values set below 1 are clamped to 1.
	/// </summary>
	public float ItemHeight
	{
		get => _itemHeight;
		set => _itemHeight = MathF.Max( 1f, value );
	}

	/// <summary>
	/// Gaps between cells. X = column gap, Y = row gap.
	/// </summary>
	public Vector2 Spacing { get; set; } = 0;


	/// <summary>
	/// If true, stretches cell width to be flush on the right edge and
	/// preserves aspect ratio by scaling height accordingly.
	/// </summary>
	public bool ScaleUp { get; set; } = true;

	/// <summary>
	/// Update layout state from the given box, scale, scroll, and justification.
	/// Returns true when internal state changed (i.e., layout is dirty).
	/// </summary>
	public bool Update( Box box, float scaleFromScreen, float scrollOffset )
	{
		var hash = HashCode.Combine( box.RectInner, scaleFromScreen, scrollOffset, _itemWidth, _itemHeight, Spacing );
		if ( hash == _updateHash ) return false;
		_updateHash = hash;

		_cellSize = new Vector2( _itemWidth, _itemHeight );

		// Inner/content rect in local space (offset by outer->inner), plus outer/viewport
		var inner = box.RectInner;
		inner.Position = box.RectInner.Position - box.Rect.Position;

		_rect = inner * scaleFromScreen;
		_outerRect = box.Rect * scaleFromScreen;

		_scrollOffset = scrollOffset;

		// Columns including gaps
		float stepX = _cellSize.x + Spacing.x;
		_columns = stepX > 0f ? ((_rect.Width + Spacing.x) / stepX).FloorToInt() : 1;
		if ( _columns < 1 ) _columns = 1;

		// Stretch X to fill; preserve aspect (Y scales with X)
		if ( ScaleUp )
		{
			float totalSpacing = (_columns - 1) * Spacing.x;
			_cellSize.x = (_rect.Width - totalSpacing) / _columns;

			float aspect = _itemHeight / _itemWidth; // both ≥ 1
			_cellSize.y = MathF.Max( 1f, _cellSize.x * aspect );
		}

		return true;
	}

	/// <summary>
	/// Compute the visible index range [firstIndex, lastIndex).
	/// Uses the outer viewport height and compensates for inner top offset
	/// so top padding does not cull early rows.
	/// </summary>
	public void GetVisibleRange( out int firstIndex, out int lastIndex )
	{
		float rowStep = MathF.Max( 1f, _cellSize.y + Spacing.y );

		int topRow = ((_scrollOffset - _rect.Top) / rowStep).FloorToInt();
		if ( topRow < 0 ) topRow = 0;

		int rowsFit = (_outerRect.Height / rowStep).CeilToInt() + 1; // buffer row

		firstIndex = Math.Max( 0, topRow * _columns );
		lastIndex = firstIndex + rowsFit * _columns; // exclusive
	}

	/// <summary>
	/// Get the rectangle for the cell at the given index.
	/// </summary>
	public Rect GetPosition( int index )
	{
		int col = index % _columns;
		int row = index / _columns;

		float stepX = _cellSize.x + Spacing.x;
		float stepY = _cellSize.y + Spacing.y;

		return new Rect( _rect.Left + col * stepX, _rect.Top + row * stepY, _cellSize.x, _cellSize.y );
	}

	/// <summary>
	/// Apply the cell rectangle for <paramref name="index"/> to <paramref name="panel"/>.
	/// </summary>
	public void Position( int index, Panel panel )
	{
		var r = GetPosition( index );
		panel.Style.Left = r.Left;
		panel.Style.Top = r.Top;
		panel.Style.Width = r.Width;
		panel.Style.Height = r.Height;
		panel.Style.Dirty();
	}

	/// <summary>
	/// Calculate total content height for the given item count.
	/// </summary>
	public float GetHeight( int count )
	{
		float rowStep = _cellSize.y + Spacing.y;
		if ( rowStep <= 0f ) return MathF.Max( 0f, _outerRect.Height - _rect.Height );

		float rows = MathF.Ceiling( count / (float)_columns );
		float paddingY = _outerRect.Height - _rect.Height; // bottom padding in local

		return rows * rowStep + MathF.Max( 0f, paddingY );
	}
}
