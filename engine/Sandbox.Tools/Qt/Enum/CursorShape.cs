namespace Editor
{
	/// <summary>
	/// TODO: Make this match whatever we do in game
	/// </summary>
	public enum CursorShape
	{
		/// <summary>
		/// No cursor override.
		/// </summary>
		None = -1,

		Arrow,
		UpArrow,
		Cross,
		Wait,
		IBeam,
		SizeV,
		SizeH,
		SizeBDiag,
		SizeFDiag,
		SizeAll,
		Blank,
		SplitV,
		SplitH,
		Finger,
		Forbidden,
		WhatsThis,
		Busy,
		OpenHand,
		ClosedHand,
		DragCopy,
		DragMove,
		DragLink,

		// LastCursor = DragLink,

		BitmapCursor = 24,
		CustomCursor = 25
	}
}

