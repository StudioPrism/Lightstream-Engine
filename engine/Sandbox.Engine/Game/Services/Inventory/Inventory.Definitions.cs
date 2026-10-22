using System.Threading;

namespace Sandbox.Services;

/// <summary>
/// Allows access to the Steam Inventory system
/// </summary>
public static partial class Inventory
{
	static Dictionary<int, ItemDefinition> _all = new();

	/// <summary>
	/// All item definitions
	/// </summary>
	public static IReadOnlyCollection<ItemDefinition> Definitions => _all.Values;

	/// <summary>
	/// Find a definition by id
	/// </summary>
	public static ItemDefinition FindDefinition( int definitionId )
	{
		return _all.GetValueOrDefault( definitionId );
	}

	/// <summary>
	/// Wait until the items have been delievered
	/// </summary>
	internal static async Task WaitForSteamInventoryItems( CancellationToken token )
	{
		while ( !gotDefinitions )
		{
			await Task.Delay( 100 );

			if ( token.IsCancellationRequested )
				return;
		}
	}

	static bool gotDefinitions = false;

	internal static async Task LoadDefinitions()
	{
		var c = NativeEngine.SteamInventory.DefinitionCount();

		var all = new Dictionary<int, ItemDefinition>();
		for ( int i = 0; i < c; i++ )
		{
			var id = NativeEngine.SteamInventory.GetDefinitionId( i );
			all[id] = new ItemDefinition( id );
		}

		_all = all;
		gotDefinitions = true;

		// update our actual inventory
		await Refresh( default );

		// wait for the prices to come 
		while ( !NativeEngine.SteamInventory.HasPrices() )
		{
			await Task.Delay( 10 );
		}

		// apply prices to all the definitions
		var currency = NativeEngine.SteamInventory.GetCurrency();
		foreach ( var e in _all.Values )
		{
			e.FetchPriceInformation( currency );
		}
	}
}
