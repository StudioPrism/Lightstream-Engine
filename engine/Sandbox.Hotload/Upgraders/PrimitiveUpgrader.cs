using System;

namespace Sandbox.Upgraders
{
	[UpgraderGroup( typeof( RootUpgraderGroup ), GroupOrder.First )]
	internal class PrimitiveUpgrader : Hotload.InstanceUpgrader
	{
		public override bool ShouldProcessType( Type type )
		{
			return type.IsPrimitive;
		}

		protected override bool OnTryCreateNewInstance( object oldInstance, out object newInstance )
		{
			newInstance = oldInstance;

			return true;
		}

		protected override bool OnTryUpgradeInstance( object oldInstance, object newInstance, bool createdElsewhere )
		{
			return true;
		}
	}
}
