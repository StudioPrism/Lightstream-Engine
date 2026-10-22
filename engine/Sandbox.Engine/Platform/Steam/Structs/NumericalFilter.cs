namespace Steamworks.Data
{
	internal struct NumericalFilter
	{
		internal string Key { get; set; }
		internal int Value { get; set; }
		internal LobbyComparison Comparer { get; set; }

		internal NumericalFilter( string k, int v, LobbyComparison c )
		{
			Key = k;
			Value = v;
			Comparer = c;
		}
	}
}
