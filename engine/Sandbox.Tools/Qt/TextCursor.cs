using System;

namespace Editor
{
	public partial class TextCursor
	{

		public int Position
		{
			get => position();
			set => setPosition( value );
		}

		public int BlockNumber => blockNumber();
		public int ColumnNumber => columnNumber();

		public string SelectedText => selectedText();

		public void InsertHtml( string str ) => insertHtml( str );
		public void InsertText( string str ) => insertText( str );

		public bool HasSelection => hasSelection();

		public void RemoveSelectedText() => removeSelectedText();
		public void ClearSelection() => clearSelection();

		public int SelectionStart => selectionStart();
		public int SelectionEnd => selectionEnd();

		public void SelectBlockUnderCursor()
		{
			select( QTextCursorSelectionType.BlockUnderCursor );
		}
	}
}

internal enum QTextCursorSelectionType
{
	WordUnderCursor,
	LineUnderCursor,
	BlockUnderCursor,
	Document
}
