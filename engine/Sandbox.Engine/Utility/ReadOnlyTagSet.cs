namespace Sandbox;

internal class ReadOnlyTagSet : ITagSet
{
	private HashSet<string> Tags { get; set; } = new( StringComparer.OrdinalIgnoreCase );

	public bool IsEmpty => Tags.Count == 0;

	public ReadOnlyTagSet( IEnumerable<string> tags )
	{
		foreach ( var tag in tags )
		{
			Tags.Add( tag );
		}
	}

	public override void Add( string tag )
	{
		// Do nothing.
	}

	public override IEnumerable<string> TryGetAll() => Tags.AsEnumerable();

	public override bool Has( string tag ) => Tags.Contains( tag );

	public override void Remove( string tag )
	{
		// Do nothing.
	}

	public override void RemoveAll()
	{
		// Do nothing.
	}
}
