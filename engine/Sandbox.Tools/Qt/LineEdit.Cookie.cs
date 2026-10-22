using System;

namespace Editor
{
	public partial class LineEdit
	{
		string _historyCookie;

		public string HistoryCookie
		{
			get => _historyCookie;
			set
			{
				if ( _historyCookie == value ) return;
				_historyCookie = value;

				RestoreHistoryFromCookie();
			}
		}

		public virtual void RestoreHistoryFromCookie()
		{
			if ( string.IsNullOrWhiteSpace( HistoryCookie ) )
				return;

			historyEntries = EditorCookie.Get( $"LineEdit.{HistoryCookie}.History", historyEntries );

			if ( historyEntries == null )
				historyEntries = new List<string>();
		}

		public virtual void SaveHistoryCookie()
		{
			if ( string.IsNullOrWhiteSpace( HistoryCookie ) )
				return;

			EditorCookie.Set( $"LineEdit.{HistoryCookie}.History", historyEntries );
		}
	}
}
