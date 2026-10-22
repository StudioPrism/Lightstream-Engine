namespace Editor
{
	public class Separator : Widget
	{
		public Color Color { get; set; }

		float size;

		public Separator( float size ) : base( null )
		{
			this.size = size;
			FixedHeight = size;
		}

		protected override Vector2 SizeHint() => size;

		protected override void OnPaint()
		{
			Paint.Antialiasing = true;
			Paint.SetBrushAndPen( Color );
			Paint.DrawRect( LocalRect );
		}
	}

}


