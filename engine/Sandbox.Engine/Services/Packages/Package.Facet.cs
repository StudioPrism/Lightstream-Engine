namespace Sandbox;

public partial class Package
{

	// this would be nice as a struct

	/// <summary>
	/// Describes a facet of a group of items, with a limited
	/// number of each facet with their total item counts
	/// </summary>
	public record class Facet( string Name, string Title, Facet.Entry[] Entries )
	{
		internal static Facet FromDto( Sandbox.Services.PackageFacet input )
		{
			return new Facet( input.Name, input.Title, input.Entries.Select( Entry.FromDto ).ToArray() );
		}

		/// <summary>
		/// A facet entry consists of a name, display information and the number of items inside
		/// </summary>
		public record class Entry( string Name, string Title, string Icon, int Count, List<Entry> Children )
		{
			internal static Entry FromDto( Sandbox.Services.PackageFacet.Entry input )
			{
				return new Entry( input.Name, input.Title, input.Icon, input.Count, input.Children?.Select( FromDto ).ToList() );
			}
		}
	}
}
