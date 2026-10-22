using Sandbox;
using System;

namespace Editor
{

	namespace Graphic
	{
		public class SimpleText : GraphicsItem
		{
			Native.QGraphicsSimpleTextItem _simpletext;

			public SimpleText( string text, GraphicsItem parent = null )
			{
				NativeInit( Native.QGraphicsSimpleTextItem.Create( text, parent.IsValid() ? parent._graphicsitem : IntPtr.Zero ) );

				Parent = parent;
			}

			internal override void NativeInit( Native.QGraphicsItem ptr )
			{
				_simpletext = (Native.QGraphicsSimpleTextItem)ptr;
				base.NativeInit( ptr );
			}

			internal override void NativeShutdown()
			{
				_simpletext = default;

				base.NativeShutdown();
			}

			public string Text
			{
				get => _simpletext.text();
				set => _simpletext.setText( value );
			}

		}
	}
}
