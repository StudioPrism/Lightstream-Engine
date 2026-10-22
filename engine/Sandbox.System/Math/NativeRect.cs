using Sandbox;

internal struct NativeRect
{
	public int x;
	public int y;
	public int w;
	public int h;

	public NativeRect( in int x, in int y, in int w, in int h )
	{
		this.x = x;
		this.y = y;
		this.w = w;
		this.h = h;
	}

	public static implicit operator NativeRect( in Rect value )
	{
		return new NativeRect { x = (int)value.Left, y = (int)value.Top, w = (int)value.Width, h = (int)value.Height };
	}

	public readonly Rect Rect => new( x, y, w, h );

}
