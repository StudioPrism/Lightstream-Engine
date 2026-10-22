using System;

namespace Editor
{
	public partial class Widget : QObject
	{
		Layout _layout;

		/// <summary>
		/// The widget's internal layout, if any
		/// </summary>
		public Layout Layout
		{
			get => _layout;
			set
			{
				if ( _layout == value ) return;

				_layout = value;
				_widget.setLayout( _layout._layout );
			}
		}

		/// <summary>
		/// Raises this widget to the top of the parent widget's stack.
		/// After this call the widget will be visually in front of any overlapping sibling widgets.
		/// </summary>
		public void Raise()
		{
			_widget.raise();
		}

		/// <summary>
		/// Lowers the widget to the bottom of the parent widget's stack.
		/// After this call the widget will be visually behind (and therefore obscured by) any overlapping sibling widgets.
		/// </summary>
		public void Lower()
		{
			_widget.lower();
		}

	}

}
