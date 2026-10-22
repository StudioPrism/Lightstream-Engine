using System.Collections;

namespace Sandbox.Services;

public static partial class Stats
{
	/// <summary>
	/// Static cache of all the global stats
	/// </summary>
	static CaseInsensitiveDictionary<GlobalStats> _globalStats = new CaseInsensitiveDictionary<GlobalStats>();

	/// <summary>
	/// Get the global stats for the calling package
	/// </summary>
	public static GlobalStats Global
	{
		get
		{
			var package = Application.GameIdent;
			if ( package is null ) return default;

			return GetGlobalStats( package );
		}
	}

	/// <summary>
	/// Get the global stats for this package
	/// </summary>
	public static GlobalStats GetGlobalStats( string packageIdent )
	{
		if ( _globalStats.TryGetValue( packageIdent, out var s ) )
			return s;

		s = new GlobalStats( packageIdent );
		_ = s.Refresh();
		_globalStats[packageIdent] = s;
		return s;
	}

	public sealed class GlobalStats : IEnumerable<GlobalStat>
	{
		private string _packageIdent;
		private Dictionary<string, GlobalStat> stats = new Dictionary<string, GlobalStat>( StringComparer.OrdinalIgnoreCase );

		/// <summary>
		/// True if we're currently fetching new stats
		/// </summary>
		public bool IsRefreshing { get; private set; }

		/// <summary>
		/// The UTC datetime when we last fetched new stats
		/// </summary>
		public DateTime LastRefresh { get; private set; }


		internal GlobalStats( string packageIdent )
		{
			this._packageIdent = packageIdent;
		}

		/// <summary>
		/// Make a copy of this class. Allows you to store the stats from a point in time.
		/// </summary>
		public GlobalStats Copy()
		{
			var pc = new GlobalStats( _packageIdent );
			pc.LastRefresh = LastRefresh;

			foreach ( var (k, v) in stats )
			{
				pc.stats.Add( k, v );
			}

			return pc;
		}

		/// <summary>
		/// Get a stat by name. Will return an empty stat if not found
		/// </summary>
		public GlobalStat Get( string name )
		{
			if ( TryGet( name, out var stat ) )
				return stat;

			return default;
		}

		/// <summary>
		/// Get a stat by name, returns true if found
		/// </summary>
		public bool TryGet( string name, out GlobalStat stat )
		{
			return stats.TryGetValue( name, out stat );
		}

		/// <summary>
		/// Refresh these global stats - grab the latest values
		/// </summary>
		public async Task Refresh()
		{
			if ( Backend.Stats is null )
				return;

			// if we're refreshing, wait for it to finish and return
			if ( IsRefreshing )
			{
				while ( IsRefreshing )
					await Task.Delay( 100 );

				return;
			}

			// Don't allow refreshing so often
			if ( LastRefresh > DateTime.UtcNow.AddSeconds( -10 ) )
				return;

			if ( _packageIdent.StartsWith( "local." ) )
				return;

			try
			{
				IsRefreshing = true;

				var stats = await Backend.Stats.GetGlobalPackageStats( _packageIdent );
				if ( stats is null ) return;

				this.stats.Clear();

				foreach ( var entry in stats )
				{
					this.stats[entry.Name] = new GlobalStat( entry );
				}

				LastRefresh = DateTime.UtcNow;
			}
			catch ( System.Exception e )
			{
				if ( !Application.IsEditor )
				{
					Log.Warning( e, "Error when fetching global stats" );
				}
			}
			finally
			{
				IsRefreshing = false;
			}

		}

		public IEnumerator<GlobalStat> GetEnumerator() => stats.Values.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => stats.Values.GetEnumerator();

		public GlobalStat this[string propertyName]
		{
			get => Get( propertyName );
		}
	}

	[Expose]
	public readonly struct GlobalStat
	{
		internal GlobalStat( Sandbox.Services.GlobalStat entry )
		{
			Name = entry.Name;
			Title = entry.Title;
			Description = entry.Description;
			Unit = entry.Unit;
			Velocity = entry.Velocity;
			Value = entry.Value;
			ValueString = entry.ValueString;
			Players = entry.Players;
			Max = entry.Max;
			Avg = entry.Avg;
			Min = entry.Min;
			Sum = entry.Sum;
		}

		/// <summary>
		/// The programatic name for this stat. This should probably be called Ident
		/// </summary>
		public string Name { get; init; }

		/// <summary>
		/// The title of this stat, as defined on the backend
		/// </summary>
		public string Title { get; init; }

		/// <summary>
		/// The description of this stat, as defined on the backend
		/// </summary>
		public string Description { get; init; }

		/// <summary>
		/// The unit of this stat as defined on the backend
		/// </summary>
		public string Unit { get; init; }

		/// <summary>
		/// The change in this stat in units per hour
		/// </summary>
		public double Velocity { get; init; }

		/// <summary>
		/// The current stat value
		/// </summary>
		public double Value { get; init; }


		/// <summary>
		/// The current value formatted using Unit
		/// </summary>
		public string ValueString { get; init; }

		/// <summary>
		/// The amount of players that have this stat
		/// </summary>
		public long Players { get; init; }

		/// <summary>
		/// The maximum value
		/// </summary>
		public double Max { get; init; }

		/// <summary>
		/// The minimum value
		/// </summary>
		public double Min { get; init; }

		/// <summary>
		/// The average value
		/// </summary>
		public double Avg { get; init; }

		/// <summary>
		/// The sum of all values
		/// </summary>
		public double Sum { get; init; }
	}
}
