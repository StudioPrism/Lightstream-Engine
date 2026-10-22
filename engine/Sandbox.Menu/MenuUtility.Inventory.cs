namespace Sandbox;

public static partial class MenuUtility
{
	public static class Inventory
	{
		public static Task<bool> CheckOut( List<Sandbox.Services.Inventory.ItemDefinition> cart )
		{
			return Sandbox.Services.Inventory.CheckOut( cart );
		}
	}
}
