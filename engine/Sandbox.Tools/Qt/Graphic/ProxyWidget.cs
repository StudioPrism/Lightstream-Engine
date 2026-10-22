using System;

namespace Editor
{
	public class GraphicsWidget : GraphicsItem
	{
		Native.QGraphicsProxyWidget _proxyWidget;


		Widget embeddedWidget;
		public Widget Widget
		{
			get => embeddedWidget;
			set
			{
				embeddedWidget = value;
				_proxyWidget.setWidget( embeddedWidget?._widget ?? IntPtr.Zero );
			}
		}

		public GraphicsWidget( Widget widget, GraphicsItem parent = null )
		{
			InteropSystem.Alloc( this );
			NativeInit( WidgetUtil.CreateGraphicsProxy( parent.IsValid() ? parent._graphicsitem : IntPtr.Zero, this ) );

			Widget = widget;
			Parent = parent;
			NoSystemBackground = true;
		}

		public GraphicsWidget( GraphicsItem parent = null )
		{
			InteropSystem.Alloc( this );
			NativeInit( WidgetUtil.CreateGraphicsProxy( parent.IsValid() ? parent._graphicsitem : IntPtr.Zero, this ) );

			Parent = parent;
			NoSystemBackground = true;
		}

		internal override void NativeInit( Native.QGraphicsItem ptr )
		{
			_proxyWidget = (Native.QGraphicsProxyWidget)ptr;
			base.NativeInit( ptr );
		}

		internal override void NativeShutdown()
		{
			_proxyWidget = default;
			base.NativeShutdown();
		}

		public override Vector2 Size
		{
			get => (Vector2)_proxyWidget.size();
			set => _proxyWidget.resize( value );
		}

		public bool TranslucentBackground
		{
			get => HasFlag( Widget.Flag.WA_TranslucentBackground );
			set => SetFlag( Widget.Flag.WA_TranslucentBackground, value );
		}

		public bool NoSystemBackground
		{
			get => HasFlag( Widget.Flag.WA_NoSystemBackground );
			set => SetFlag( Widget.Flag.WA_NoSystemBackground, value );
		}

		internal void SetFlag( Widget.Flag f, bool b )
		{
			_proxyWidget.setAttribute( f, b );
		}

		internal bool HasFlag( Widget.Flag f )
		{
			return _proxyWidget.testAttribute( f );
		}

		internal void Poop()
		{
			throw new NotImplementedException();
		}
	}
}
