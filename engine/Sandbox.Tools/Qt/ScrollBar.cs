using System;

namespace Editor
{
	public class ScrollBar : Widget
	{
		Native.QScrollBar _scrollbar;

		public int Minimum
		{
			get => _scrollbar.minimum();
			set => _scrollbar.setMinimum( value );
		}

		public int Maximum
		{
			get => _scrollbar.maximum();
			set => _scrollbar.setMaximum( value );
		}

		public int Value
		{
			get => _scrollbar.value();
			set => _scrollbar.setValue( value );
		}

		public int SliderPosition
		{
			get => _scrollbar.sliderPosition();
			set => _scrollbar.setSliderPosition( value );
		}

		public int PageStep
		{
			get => _scrollbar.pageStep();
			set => _scrollbar.setPageStep( value );
		}

		public int SingleStep
		{
			get => _scrollbar.singleStep();
			set => _scrollbar.setSingleStep( value );
		}


		public ScrollBar( IntPtr ptr ) : base( false )
		{
			NativeInit( ptr );
		}

		internal override void NativeInit( IntPtr ptr )
		{
			_scrollbar = ptr;

			base.NativeInit( ptr );
		}
		internal override void NativeShutdown()
		{
			base.NativeShutdown();

			_scrollbar = default;
		}
	}
}
