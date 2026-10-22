using System;

namespace Editor
{
	/// <summary>
	/// A single line text entry. See <see cref="TextEdit"/> for multi line version.
	/// </summary>
	public partial class LineEdit : Widget
	{
		internal Native.QLineEdit _lineedit;
		internal CLineEdit _clineedit;

		/// <inheritdoc cref="OnTextChanged"/>
		public event Action<string> TextChanged;

		/// <inheritdoc cref="OnTextEdited"/>
		public event Action<string> TextEdited;

		/// <inheritdoc cref="OnTextEdited"/>
		public event Action ReturnPressed;

		/// <inheritdoc cref="OnEditingFinished"/>
		public event Action EditingFinished;

		/// <summary>
		/// The text entry received keyboard focus.
		/// </summary>
		public event Action EditingStarted;

		/*
		public event Action SelectionChanged;
		public event Action InputRejected;
		public event Action CursorPositionChanged;
		*/

		/// <summary>
		/// Forward up, down and enter keys to this control. This is useful if you have a
		/// search box that you want to also allow to navigate a list of items.
		/// </summary>
		public Widget ForwardNavigationEvents { get; set; }

		/// <summary>
		/// Alias of <see cref="Text"/>, except disallows setting text when <see cref="Widget.IsFocused"/> is <see langword="true"/>.
		/// </summary>
		public virtual string Value
		{
			get => Text;
			set
			{
				if ( IsFocused )
					return;

				Text = value;
			}
		}

		/// <summary>
		/// The text of this text entry.
		/// </summary>
		public string Text
		{
			get => _lineedit.text();
			set => _lineedit.setText( value );
		}

		public string DisplayText
		{
			get => _lineedit.displayText();
		}

		/// <summary>
		/// The placeholder text, it will be displayed only when the text entry is empty.
		/// Typically used to as a short description of the expected input, or as an example input.
		/// </summary>
		public string PlaceholderText
		{
			get => _lineedit.placeholderText();
			set => _lineedit.setPlaceholderText( value );
		}

		/// <summary>
		/// User entered text can never be longer than this many characters (not bytes).
		/// </summary>
		public int MaxLength
		{
			get => _lineedit.maxLength();
			set => _lineedit.setMaxLength( value );
		}

		/// <summary>
		/// Whether the user has any text selected within this text entry.
		/// </summary>
		public bool HasSelectedText => _lineedit.hasSelectedText();

		/// <summary>
		/// Character at which the text selection begins, or -1 if there is no selection.
		/// </summary>
		public int SelectionStart
		{
			get => _lineedit.selectionStart();
		}

		/// <summary>
		/// Character at which the text selection ends, or -1 if there is no selection.
		/// </summary>
		public int SelectionEnd
		{
			get => _lineedit.selectionEnd();
		}

		/// <summary>
		/// Show a button to clear the text input when it is not empty.
		/// </summary>
		public bool ClearButtonEnabled
		{
			get => _lineedit.isClearButtonEnabled();
			set => _lineedit.setClearButtonEnabled( value );
		}

		public override bool ReadOnly
		{
			get => _lineedit.isReadOnly();
			set => _lineedit.setReadOnly( value );
		}

		/// <summary>
		/// The selected text, if any.
		/// </summary>
		public string SelectedText => _lineedit.selectedText();

		/// <summary>
		/// Position of the text cursor, at which newly typed letters will be inserted.
		/// </summary>
		public int CursorPosition
		{
			get => _lineedit.cursorPosition();
			set => _lineedit.setCursorPosition( value );
		}

		/// <summary>
		/// Clear the text.
		/// </summary>
		public void Clear() => _lineedit.clear();

		/// <summary>
		/// Select all of the text.
		/// </summary>
		public void SelectAll() => _lineedit.selectAll();

		/// <summary>
		/// Set the selected text region.
		/// </summary>
		public void SetSelection( int start, int length ) => _lineedit.setSelection( start, length );

		/// <summary>
		/// De-select all of the text.
		/// </summary>
		public void Deselect() => _lineedit.deselect();

		public void Undo() => _lineedit.undo();
		public void Redo() => _lineedit.redo();
		public void Cut() => _lineedit.cut();
		public void Copy() => _lineedit.copy();
		public void Paste() => _lineedit.paste();
		public void Insert( string val ) => _lineedit.insert( val );

		public void SetValidator( string str )
		{
			if ( _clineedit.IsNull ) return;
			_clineedit.SetValidation( str );
		}

		internal LineEdit( IntPtr ptr ) : base( false )
		{
			NativeInit( ptr );
		}
		public LineEdit( Widget parent = null ) : base( false )
		{
			Sandbox.InteropSystem.Alloc( this );
			_clineedit = CLineEdit.Create( parent?._widget ?? default, this );
			NativeInit( _clineedit );
		}

		public LineEdit( string title, Widget parent = null ) : this( parent )
		{
			Text = title;
		}

		internal override void NativeInit( IntPtr ptr )
		{
			_lineedit = ptr;

			base.NativeInit( ptr );
		}
		internal override void NativeShutdown()
		{
			base.NativeShutdown();

			_lineedit = default;
		}

		/// <summary>
		/// Called when the input text changes.
		/// </summary>
		protected virtual void OnTextChanged( string value )
		{
			AutoComplete?.OnAutoComplete( value, ScreenPosition );
			TextChanged?.Invoke( value );
		}

		/// <summary>
		/// Called when the text was edited.
		/// </summary>
		protected virtual void OnTextEdited( string value )
		{
			if ( _focusedButNotEdited )
			{
				EditingStarted?.Invoke();
				_focusedButNotEdited = false;
			}
			TextEdited?.Invoke( value );

			SignalValuesChanged();
		}

		/// <summary>
		/// Called when the user presses the return (Enter) key.
		/// </summary>
		protected virtual void OnReturnPressed()
		{
			ReturnPressed?.Invoke();
		}

		/// <summary>
		/// The text entry lost keyboard focus.
		/// </summary>
		protected virtual void OnEditingFinished()
		{
			EditingFinished?.Invoke();
		}

		/*
		protected virtual void OnSelectionChanged()
		{
			SelectionChanged?.Invoke();
		}

		protected virtual void OnInputRejected()
		{
			InputRejected?.Invoke();
		}

		protected virtual void OnCursorPositionChanged()
		{
			CursorPositionChanged?.Invoke();
		}*/

		internal void InternalTextChanged( string value ) => OnTextChanged( value );
		internal void InternalTextEdited( string value ) => OnTextEdited( value );
		internal void InternalReturnPressed() => OnReturnPressed();
		internal void InternalEditingFinished() => OnEditingFinished();


		public void SetAutoComplete( Action<Menu, string> func )
		{
			AutoComplete = new AutoComplete( this );
			AutoComplete.OnOptionSelected = ( o ) => Text = o;
			AutoComplete.OnBuildOptions = func;
		}

		public AutoComplete AutoComplete { get; set; }


		protected override void OnBlur( FocusChangeReason reason )
		{
			AutoComplete?.OnParentBlur();

			base.OnBlur( reason );
		}

		protected override void OnFocus( FocusChangeReason reason )
		{
			// EditingStarted?.Invoke();
			_focusedButNotEdited = true;

			base.OnFocus( reason );
		}

		private bool _focusedButNotEdited = false;

		/// <summary>
		/// Whether the <see cref="AutoComplete">auto complete</see> <see cref="Menu"/> is visible or not.
		/// </summary>
		public bool AutoCompleteVisible => AutoComplete?.Visible ?? false;

		/// <summary>
		/// If we have our menus open, let use tab/shift tab to navigate instead of switching to next control
		/// </summary>
		protected override bool FocusNext() => HistoryVisible || AutoCompleteVisible;

		/// <summary>
		/// If we have our menus open, let use tab/shift tab to navigate instead of switching to next control
		/// </summary>
		protected override bool FocusPrevious() => HistoryVisible || AutoCompleteVisible;

		protected override void OnKeyPress( KeyEvent e )
		{
			if ( e.Key == KeyCode.Escape || e.Key == KeyCode.Enter )
			{
				Blur();
				return;
			}

			if ( !AutoCompleteVisible && !HistoryVisible && MaxHistoryItems > 0 && (e.Key == KeyCode.Up || e.Key == KeyCode.Down) )
			{
				OpenHistory();
			}

			if ( HistoryVisible || AutoCompleteVisible )
			{
				if ( e.Key == KeyCode.Tab ) e.Key = KeyCode.Down;
				if ( e.Key == KeyCode.Backtab ) e.Key = KeyCode.Up;
			}

			if ( HistoryVisible )
			{
				historyMenu?.OnParentKeyPress( e );
			}
			else
			{
				AutoComplete?.OnParentKeyPress( e );
			}

			if ( HistoryVisible && !e.Accepted )
			{
				historyMenu.Visible = false;
			}

			if ( ForwardNavigationEvents != null && (e.Key == KeyCode.Up || e.Key == KeyCode.Down || e.Key == KeyCode.Enter) )
			{
				ForwardNavigationEvents.InternalKeyPressEvent( e.ptr );
			}

			base.OnKeyPress( e );
		}

		public Option AddOptionToFront( Option option )
		{
			Assert.NotNull( option );
			_lineedit.addAction( option._action, 0 );
			option.SetParent( this );
			return option;
		}

		public Option AddOptionToEnd( Option option )
		{
			Assert.NotNull( option );
			_lineedit.addAction( option._action, 1 );
			option.SetParent( this );
			return option;
		}

		public TextFlag Alignment
		{
			get => (TextFlag)_lineedit.alignment();
			set => _lineedit.setAlignment( (int)value );
		}

		public string RegexValidator
		{
			set
			{
				_lineedit.setRegexValidator( value );
			}
		}

		public Rect CursorRect
		{
			get
			{
				if ( _clineedit.IsNull ) return default;
				return _clineedit.cursorRect().Rect;
			}
		}

	}
}
