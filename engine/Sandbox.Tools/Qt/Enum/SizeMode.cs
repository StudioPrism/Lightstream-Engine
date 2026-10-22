namespace Editor
{
	public enum SizeMode
	{
		/// <summary>
		/// Can grow beyond its size hint if necessary
		/// </summary>
		CanGrow = 1,

		/// <summary>
		/// Should get as much space as possible.
		/// </summary>
		Expand = 2,

		/// <summary>
		/// can shrink below its size hint if necessary.
		/// </summary>
		CanShrink = 4,

		/// <summary>
		/// Widget size is ignored, will get as much space as possible.
		/// </summary>
		Ignore = 8,


		/// <summary>
		/// Default size mode - CanGrow and CanShrink
		/// </summary>
		Default = CanGrow | CanShrink,

		/// <summary>
		/// Ignores the size hint - just expand as large as possible
		/// </summary>
		Flexible = CanGrow | Expand | CanShrink
	}

}

