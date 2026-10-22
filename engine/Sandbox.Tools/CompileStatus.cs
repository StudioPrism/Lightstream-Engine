namespace Editor;


internal static class CompileStatus
{
	static CompileProgressWindow progress;

	static int compileCount;

	internal static void CompileProgress( string statusText )
	{
		if ( progress is not null )
		{
			//progress.Label.Text = statusText;
			//progress.WindowTitle = statusText;
			//progress.Update();
			//progress.Repaint();
		}
	}

	internal static void StartCompile( int id, string text )
	{
		if ( progress is null )
		{
			//progress = new CompileProgressWindow();
		}

		//progress.WindowTitle = text;
		//progress.Update();
		//progress.Repaint();

		compileCount++;
	}

	internal static void EndCompile( int id )
	{
		//progress.Label.Text = "Hello! Done";
		//progress.WindowTitle = "Done!";
		//progress.Update();
		//progress.Repaint();

		compileCount--;
	}

	internal static void CloseProgress()
	{
		if ( compileCount > 0 )
			return;

		if ( progress is not null )
		{
			progress.Destroy();
			progress = null;
		}
	}
}


class CompileProgressWindow : Widget
{
	public Label Label { get; }

	public CompileProgressWindow()
	{
		WindowFlags = WindowFlags.Tool;
		Size = new Vector2( 500, 100 );

		Show();
		AlignToParent( TextFlag.Center );

		Label = new Label( "Hello", this );
		Label.Size = 100;
	}

	protected override void OnPaint()
	{
		Paint.SetBrush( Color.Random );
		Paint.DrawRect( LocalRect );
	}
}
