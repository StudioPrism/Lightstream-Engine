namespace Editor
{
	// TODO make this internal
	public partial class QStringList
	{
		public List<string> ToList()
		{
			var l = new List<string>();

			for ( int i = 0; i < size(); i++ )
				l.Add( at( i ) );

			return l;
		}
	}
}
