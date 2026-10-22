using Sandbox;
using System;

namespace Editor
{
	public class AutoComplete : Menu
	{
		public AutoComplete( Widget parent ) : base( parent )
		{
			FocusMode = FocusMode.None;
			IsTooltip = true;
			ShowWithoutActivating = true;
			Name = "AutoCompleteMenu";
		}

		public bool HasAutocompleteOptions { get; protected set; }
		public int MinimumLength { get; set; } = 2;
		public Vector2 OpenOffset { get; set; } = new Vector2( 0, -5 );

		/// <summary>
		/// The text has changed - fill in the options
		/// </summary>
		public Action<Menu, string> OnBuildOptions;

		public void OnAutoComplete( string newPrefix, Vector2 screenPosition )
		{
			HasAutocompleteOptions = false;

			using ( var v = new SuspendUpdates( this ) )
			{
				Clear();

				if ( newPrefix == null || newPrefix.Length >= MinimumLength )
				{

					OnBuildOptions?.Invoke( this, newPrefix );
				}
			}

			if ( !HasAutocompleteOptions )
			{
				Visible = false;
				return;
			}

			OpenAbove( screenPosition + OpenOffset );
		}

		/// <summary>
		/// You should hook this up to change the text on your control
		/// </summary>
		public Action<string> OnOptionSelected;

		/// <summary>
		/// Add an option for this autocomplete
		/// </summary>
		public override Option AddOption( string name, string icon = null, Action action = null, string shortcut = null )
		{
			HasAutocompleteOptions = true;

			var act = () =>
			{
				OnOptionSelected?.Invoke( name );
				action?.Invoke();
			};

			return base.AddOption( name, icon, act, shortcut );
		}

		/// <summary>
		/// Open above this position
		/// </summary>
		public void OpenAbove( Vector2 position )
		{
			Visible = true;
			MaximumSize = new Vector2( 1000, position.y - 10 );
			Position = position - new Vector2( 0, Size.y );
		}

		/// <summary>
		/// You should call this from the parent when a key is pressed. Will forward
		/// the appropriate keys to us and accept the event.
		/// </summary>
		public void OnParentKeyPress( KeyEvent e )
		{
			if ( !Visible ) return;

			bool isNavigation = e.Key == KeyCode.Up || e.Key == KeyCode.Down;
			bool isSelect = e.Key == KeyCode.Enter || e.Key == KeyCode.Return;
			bool isTakeAndCarryOn = e.Key == KeyCode.Space;

			if ( e.Key == KeyCode.Escape )
			{
				e.Accepted = true;
				Visible = false;
				return;
			}

			// Up down menu navigation
			if ( isNavigation )
			{
				e.Accepted = true;
				PostKeyEvent( e.Key );
			}

			// Only select the option if we have one selected
			if ( SelectedOption != null && isSelect )
			{
				e.Accepted = true;
				PostKeyEvent( e.Key );
			}

			// Only select the option if we have one selected
			if ( SelectedOption != null && isTakeAndCarryOn )
			{
				// change the text to this
				OnOptionSelected?.Invoke( SelectedOption.Text );
			}
		}

		/// <summary>
		/// Call this when the widget that spawns this blurs, so we can hide ourself
		/// </summary>
		public void OnParentBlur()
		{
			if ( IsActiveWindow ) return;
			Visible = false;
		}

		/// <summary>
		/// Called when the mouse is pressed. Will hide this window if we clicked on anything
		/// except ourselves or our parent control.
		/// </summary>
		[Event( "qt.mousepressed" )]
		public void OnGlobalMousePressed()
		{
			if ( IsUnderMouse ) return;
			if ( Parent.IsValid() && Parent.IsUnderMouse ) return;

			Visible = false;
		}
	}
}
