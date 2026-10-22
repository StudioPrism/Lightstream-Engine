namespace Sandbox;

partial class Model
{
	/// <summary>
	/// List of sequence names this model has.
	/// </summary>
	internal IReadOnlyList<string> SequenceNames => _sequenceNames ??= GetSequenceNames();
	private List<string> _sequenceNames;

	private List<string> GetSequenceNames()
	{
		var list = NativeEngine.CUtlVectorString.Create( 4, 4 );
		native.GetSequenceNames( list );

		var names = new List<string>();
		for ( int i = 0; i < list.Count(); i++ )
		{
			var name = list.Element( i );
			if ( string.IsNullOrWhiteSpace( name ) )
				continue;

			names.Add( name );
		}

		list.DeleteThis();

		return names;
	}
}
