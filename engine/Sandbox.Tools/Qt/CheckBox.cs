using System;

namespace Editor
{
	/// <summary>
	/// Check state of a <see cref="Checkbox"/>.
	/// </summary>
	public enum CheckState
	{
		/// <summary>
		/// The checkbox is not checked.
		/// </summary>
		Off,

		/// <summary>
		/// Partial. This is useful in cases when representing multiple objects,
		/// with a boolean value where some are set to true, and others to false.
		/// </summary>
		Partial,

		/// <summary>
		/// The checkbox is checked.
		/// </summary>
		On
	}

	/// <summary>
	/// A generic checkbox widget.
	/// </summary>
	public class Checkbox : Widget
	{
		Native.QCheckBox _checkbox;

		/// <inheritdoc cref="OnClicked"/>
		public Action Clicked;

		/// <inheritdoc cref="OnPressed"/>
		public Action Pressed;

		/// <inheritdoc cref="OnReleased"/>
		public Action Released;

		/// <inheritdoc cref="OnToggled"/>
		public Action Toggled;

		/// <inheritdoc cref="OnStateChanged"/>
		public Action<CheckState> StateChanged;

		/// <summary>
		/// The checkbox label.
		/// </summary>
		public string Text
		{
			get => _checkbox.text();
			set => _checkbox.setText( value );
		}

		/// <summary>
		/// Whether the checkbox is checked or not.
		/// </summary>
		public bool Value
		{
			get => _checkbox.isChecked();
			set => _checkbox.setChecked( value );
		}

		/// <summary>
		/// Current state of this checkbox.
		/// </summary>
		public CheckState State
		{
			get => _checkbox.checkState();

			set => _checkbox.setCheckState( value );
		}
		/// <summary>
		/// Enable the third state, the half checked half not checked state.
		/// Disabled by default
		/// </summary>
		public bool TriState
		{
			get => _checkbox.isTristate();
			set => _checkbox.setTristate( value );
		}

		internal Checkbox( Native.QPushButton ptr ) : base( false )
		{
			NativeInit( ptr );
		}

		public Checkbox( Widget parent = null ) : base( false )
		{
			Sandbox.InteropSystem.Alloc( this );
			var ptr = CCheckBox.CreateCheckBox( parent?._widget ?? default, this );
			NativeInit( ptr );

			TriState = false;
		}

		public Checkbox( string title, Widget parent = null ) : this( parent )
		{
			if ( title != null )
				Text = title;

		}
		public Checkbox( string title, string icon, Widget parent = null ) : this( title, parent )
		{
			if ( icon != null )
				Icon = icon;
		}

		internal override void NativeInit( IntPtr ptr )
		{
			_checkbox = ptr;
			base.NativeInit( ptr );
		}
		internal override void NativeShutdown()
		{
			base.NativeShutdown();
			_checkbox = default;
		}

		/// <summary>
		/// Called when checkbox was clicked, on release.
		/// </summary>
		protected virtual void OnClicked()
		{
			Clicked?.Invoke();
		}

		/// <summary>
		/// Called when checkbox was pressed down.
		/// </summary>
		protected virtual void OnPressed()
		{
			Pressed?.Invoke();
		}

		/// <summary>
		/// Called when checkbox was released.
		/// </summary>
		protected virtual void OnReleased()
		{
			Released?.Invoke();
		}

		/// <summary>
		/// Called when checkbox gets toggled on or off.
		/// </summary>
		protected virtual void OnToggled()
		{
			Toggled?.Invoke();
			SignalValuesChanged();
		}

		/// <summary>
		/// Called when the <see cref="State"/> of the checkbox states.
		/// </summary>
		protected virtual void OnStateChanged( CheckState state )
		{
			StateChanged?.Invoke( state );
		}

		/// <summary>
		/// Name of a material icon to be drawn in front of the checkbox label.
		/// </summary>
		public string Icon
		{
			set => _checkbox.setIcon( value );
		}

		internal void InternalOnPressed() => OnPressed();
		internal void InternalOnReleased() => OnReleased();
		internal void InternalOnClicked() => OnClicked();
		internal void InternalOnToggled() => OnToggled();
		internal void InternalOnStateChanged() => OnStateChanged( State );
	}
}
