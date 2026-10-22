namespace Editor
{
	public enum SizeConstraint
	{
		/// <summary>
		/// The main widget enforces minimum size
		/// </summary>
		SetDefaultConstraint,

		/// <summary>
		/// The widget size ignores minimum and maximum size
		/// </summary>
		SetNoConstraint,

		/// <summary>
		/// The main widget is set to minimum size and cannot be smaller
		/// </summary>
		SetMinimumSize,

		/// <summary>
		/// The main widget is fixed to the layout's size and won't resize at all
		/// </summary>
		SetFixedSize,

		/// <summary>
		/// The main widget is set to maximum size and cannot be smaller
		/// </summary>
		SetMaximumSize,

		/// <summary>
		/// Size between minimum and maximum size
		/// </summary>
		SetMinAndMaxSize
	}

}

