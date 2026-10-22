using System;

namespace Editor
{
	public partial class LineEdit
	{
		List<string> historyEntries = new();
		AutoComplete historyMenu { get; set; }

		/// <summary>
		/// True if history menu is visible
		/// </summary>
		public bool HistoryVisible => historyMenu?.Visible ?? false;

		/// <summary>
		/// if set > 1 we will support history items (which you need to add using AddHistory)
		/// </summary>
		public int MaxHistoryItems { get; set; }


		public void AddHistory( string text )
		{
			if ( MaxHistoryItems <= 0 ) return;

			historyEntries.RemoveAll( x => x == text );
			historyEntries.Add( text );

			while ( historyEntries.Count > MaxHistoryItems )
				historyEntries.RemoveAt( 0 );

			SaveHistoryCookie();
		}

		void OpenHistory()
		{
			if ( historyMenu == null )
			{
				historyMenu = new AutoComplete( this );
				historyMenu.OnOptionSelected = ( o ) =>
					{
						historyMenu.Visible = false;
						Text = o;
					};
				historyMenu.OnBuildOptions = BuildHistoryOptions;
			}

			historyMenu?.OnAutoComplete( null, ScreenPosition );
		}

		void BuildHistoryOptions( Menu menu, string partial )
		{
			foreach ( var item in historyEntries )
			{
				menu.AddOption( item );
			}
		}
	}
}
