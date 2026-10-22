using System.Collections;

namespace Sandbox;

[SkipHotload]
public class EnumDescription : IEnumerable<EnumDescription.Entry>
{
	private Entry[] Entries { get; }
	public Entry[] Unique { get; }

	public IEnumerator<Entry> GetEnumerator() => Entries.AsEnumerable().GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	internal EnumDescription( Type t )
	{
		Assert.True( t.IsEnum );

		var values = t.GetEnumValues();
		var names = t.GetEnumNames();
		var displayInfo = DisplayInfo.ForEnumValues( t );

		Entries = new Entry[values.Length];

		for ( var i = 0; i < values.Length; i++ )
		{
			var memberInfo = t.GetMember( names[i] );

			var e = new Entry { ObjectValue = values.GetValue( i ) };
			e.IntegerValue = (long)Convert.ChangeType( e.ObjectValue, typeof( long ) )!;
			e.Name = names[i];
			e.Icon = displayInfo[i].Icon;
			e.Group = displayInfo[i].Group;
			e.Title = displayInfo[i].Name;
			e.Description = displayInfo[i].Description;
			e.Browsable = displayInfo[i].Browsable;

			Entries[i] = e;
			// class name etc
		}

		var sorted = Entries
			.Where( x => x.IntegerValue > 0 )
			.OrderBy( x => x.IntegerValue )
			.ToArray();

		var unique = new List<Entry>();
		var mask = 0L;

		foreach ( var entry in sorted )
		{
			if ( (entry.IntegerValue & mask) == entry.IntegerValue )
			{
				continue;
			}

			mask |= entry.IntegerValue;
			unique.Add( entry );
		}

		Unique = unique.ToArray();
	}

	public Entry GetEntry( object value )
	{
		return Entries.FirstOrDefault( x => Equals( x.ObjectValue, value ) );
	}

	public Entry GetEntry( long value )
	{
		return Entries.FirstOrDefault( x => x.IntegerValue == value );
	}

	public Entry[] GetEntries( long value )
	{
		return Unique.Where( x => (x.IntegerValue & value) == x.IntegerValue ).ToArray();
	}

	public struct Entry
	{
		public object ObjectValue { get; set; }
		public long IntegerValue { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public string Icon { get; set; }
		public string Group { get; set; }
		public string Description { get; set; }
		public bool Browsable { get; set; }
	}
}
