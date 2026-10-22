using System;
using System.Collections.Generic;

namespace Sandbox.Upgraders
{
	[UpgraderGroup( typeof( ReferenceTypeUpgraderGroup ), GroupOrder.First )]
	public class CachedUpgrader : Hotload.InstanceUpgrader
	{
		private readonly Dictionary<Type, Dictionary<object, object>> ReplaceCache = new Dictionary<Type, Dictionary<object, object>>();

		protected override void OnClearCache()
		{
			ReplaceCache.Clear();
		}

		public bool TryGetCachedInstance( object inst, out object cached )
		{
			var oldType = inst.GetType();

			cached = null;

			if ( !ReplaceCache.TryGetValue( oldType, out var replaced ) )
				return false;

			return replaced.TryGetValue( inst, out cached );
		}

		public new void AddCachedInstance( object inst, object cached )
		{
			var oldType = inst.GetType();

			if ( !ReplaceCache.TryGetValue( oldType, out var replaced ) )
			{
				replaced = new Dictionary<object, object>( ReferenceComparer.Singleton );
				ReplaceCache.Add( oldType, replaced );
			}

			replaced[inst] = cached;
		}

		public override bool ShouldProcessType( Type type )
		{
			return !type.IsValueType;
		}

		protected override bool OnTryCreateNewInstance( object oldInstance, out object newInstance )
		{
			return TryGetCachedInstance( oldInstance, out newInstance );
		}

		protected override bool OnTryUpgradeInstance( object oldInstance, object newInstance, bool createdElsewhere )
		{
			// TODO: might need to handle initonly fields with cached values
			return false;
		}
	}
}
