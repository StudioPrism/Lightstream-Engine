using Sandbox.UI;
using System;
using System.ComponentModel;

namespace Editor
{
	/// <summary>
	/// A generic widget.
	/// </summary>
	public partial class Widget
	{
		bool _debugModeEnabled;

		/// <summary>
		/// Enable debug mode on this widget.
		/// </summary>
		public bool DebugModeEnabled
		{
			get => _debugModeEnabled;

			set
			{
				if ( _debugModeEnabled == value ) return;
				_debugModeEnabled = value;
				Update();
			}
		}

		/// <summary>
		/// If true then this widget has a debug mode that can be activated
		/// </summary>
		public virtual bool ProvidesDebugMode => false;
	}
}
