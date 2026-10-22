using Sandbox;
using System;

namespace Editor
{
	public class GraphicsLine : GraphicsItem
	{
		Native.CManagedLineGraphicsItem _line;

		public GraphicsLine( GraphicsItem parent = null )
		{
			InteropSystem.Alloc( this );
			NativeInit( Native.CManagedLineGraphicsItem.CreateLine( parent.IsValid() ? parent._graphicsitem : IntPtr.Zero, this ) );
			Parent = parent;
		}

		internal override void NativeInit( Native.QGraphicsItem ptr )
		{
			_line = (Native.CManagedLineGraphicsItem)ptr;
			base.NativeInit( ptr );
		}

		internal override void NativeShutdown()
		{
			_line = default;
			base.NativeShutdown();
		}

		public void Clear() => _line.Clear();
		public void MoveTo( Vector2 point ) => _line.MoveTo( point );
		public void LineTo( Vector2 point ) => _line.LineTo( point );
		public void CubicLineTo( Vector2 c1, Vector2 c2, Vector2 point ) => _line.CubicTo( c1, c2, point );

		public float HitWidth
		{
			get => _line.HitWidth;
			set => _line.HitWidth = value;
		}

		protected override void OnPaint()
		{
			Editor.Paint.SetPen( Color.Red, 5.0f );
			PaintLine();
		}

		protected void PaintLine()
		{
			_line.paint( Editor.Paint.Current );
		}
	}
}
