namespace Editor
{
	/// <summary>
	/// A wrapper to more easily create dialog windows.
	/// </summary>
	public partial class Dialog : Widget
	{
		/// <summary>
		/// The created parent window for this dialog.
		/// </summary>
		public Window Window { get; init; }

		public Dialog( Widget parent = null, bool initAsDialog = true ) : base( null )
		{
			Window = new Window( parent );
			Window.Size = new Vector2( 500, 500 );

			if ( initAsDialog )
			{
				Window.IsDialog = true;
				Window.StatusBar = null;
			}

			Window.Canvas = this;
			Window.DeleteOnClose = true;
		}

		public override void Close() => Window.Close();
		public override void Show() => Window.Show();
		public override void Hide() => Window.Hide();
	}
}
