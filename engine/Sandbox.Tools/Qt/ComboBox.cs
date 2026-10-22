using System;

namespace Editor
{
	public partial class ComboBox : Widget
	{
		Native.QComboBox _combobox;

		public LineEdit LineEdit { get; init; }

		public ComboBox( Widget parent = null ) : base( false )
		{
			Sandbox.InteropSystem.Alloc( this );
			NativeInit( CComboBox.Create( parent?._widget ?? default, this ) );

			LineEdit = new LineEdit( this );
			_combobox.setLineEdit( LineEdit._lineedit );

			Editable = false;
		}

		internal override void NativeInit( IntPtr ptr )
		{
			_combobox = ptr;
			base.NativeInit( ptr );
		}

		internal override void NativeShutdown()
		{
			base.NativeShutdown();
			_combobox = default;
		}

		public string CurrentText
		{
			get => _combobox.currentText();
			set
			{
				_combobox.setEditText( value );

				if ( !changedProgramatically )
				{
					if ( FindIndex( value ) is int index )
						CurrentIndex = index;
				}
			}
		}

		public int CurrentIndex
		{
			get => _combobox.currentIndex();
			set
			{
				if ( CurrentIndex == value )
					return;

				changedProgramatically = true;
				_combobox.setCurrentIndex( value );
				changedProgramatically = false;
			}
		}

		public int Count
		{
			get => _combobox.count();
		}

		bool changedProgramatically = false;

		public int? FindIndex( string text )
		{
			var index = _combobox.findText( text );
			if ( index < 0 ) return null;

			return index;
		}

		public bool TrySelectNamed( string name )
		{
			var i = FindIndex( name );
			if ( i == null ) return false;

			var changed = CurrentIndex != i.Value;
			CurrentIndex = i.Value;
			if ( changed ) InternalIndexChanged();
			return true;
		}

		public void ClearText()
		{
			_combobox.clearEditText();
		}
		public void Clear()
		{
			_combobox.clear();
		}

		public bool AllowDuplicates { get => _combobox.duplicatesEnabled(); set => _combobox.setDuplicatesEnabled( value ); }
		public int MaxVisibleItems { get => _combobox.maxVisibleItems(); set => _combobox.setMaxVisibleItems( value ); }
		public bool Editable
		{
			get => _combobox.isEditable();
			set
			{
				_combobox.setEditable( value );

				_widget.Polish();
				LineEdit?._widget.Polish();
			}
		}
		public InsertMode Insertion { get => _combobox.insertPolicy(); set => _combobox.setInsertPolicy( value ); }
		public Action OnReturn { get; internal set; }


		public enum InsertMode
		{
			Skip,
			AtTop,
			AtCurrent,
			AtBottom,
			AfterCurrent,
			BeforeCurrent,
			Alphabetically
		}

		public event Action TextChanged;
		public event Action ItemChanged;

		protected virtual void OnTextChanged()
		{
			var txt = CurrentText;

			AutoComplete?.OnAutoComplete( txt, ScreenPosition );
			TextChanged?.Invoke();

			InvokeSelected();
		}

		internal void InternalTextChanged() => OnTextChanged();

		Dictionary<string, Action> ItemActions = new();

		public void AddItem( string text, string icon = null, Action onSelected = null, string description = null, bool selected = false, bool enabled = true )
		{
			_combobox.addItem( icon, text );

			if ( selected )
			{
				CurrentIndex = _combobox.count() - 1;
			}

			if ( description != null )
			{
				_combobox.setItemDescription( Count - 1, description );
			}

			if ( !enabled && !selected )
			{
				_combobox.setItemEnabled( Count - 1, enabled );
			}

			if ( onSelected != null )
			{
				ItemActions[text] = onSelected;
			}
		}

		internal void InternalIndexChanged() => OnItemChanged();

		public void InvokeSelected()
		{
			if ( ItemActions.TryGetValue( CurrentText, out var action ) )
			{
				action.Invoke();
			}
		}

		protected virtual void OnItemChanged()
		{
			SaveToStateCookie();

			var txt = CurrentText;

			ItemChanged?.Invoke();

			InvokeSelected();

			if ( !changedProgramatically )
				SignalValuesChanged();
		}

		public void SetAutoComplete( Action<Menu, string> func )
		{
			AutoComplete = new AutoComplete( this );
			AutoComplete.OnOptionSelected = ( o ) => CurrentText = o;
			AutoComplete.OnBuildOptions = func;
		}

		public AutoComplete AutoComplete { get; set; }

		protected override void OnBlur( FocusChangeReason reason )
		{
			AutoComplete?.OnParentBlur();

			base.OnBlur( reason );
		}

		protected override void OnKeyPress( KeyEvent e )
		{
			AutoComplete?.OnParentKeyPress( e );

			base.OnKeyPress( e );
		}
	}
}
