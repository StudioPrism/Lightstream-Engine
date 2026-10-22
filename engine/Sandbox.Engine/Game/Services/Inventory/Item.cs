namespace Sandbox.Services;

public static partial class Inventory
{
	/// <summary>
	/// Describes a type of item that can be in the inventory
	/// </summary>
	public sealed class Item
	{
		public ulong ItemId { get; private set; }
		public int DefinitionId { get; private set; }

		public ItemDefinition Definition => FindDefinition( DefinitionId );

		internal Item( CSteamItemInstance instance )
		{
			// Note that instance is destroyed after calling this
			// so don't fucking store it for fuck sake

			ItemId = instance.ItemId();
			DefinitionId = (int)instance.DefinitionId();
		}
	}
}
