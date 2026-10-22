using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Sandbox.Upgraders.SpecialCases
{
	/// <summary>
	/// System.Text.Json keeps a bunch of generated methods cached that we can't upgrade properly.
	/// Let's just clear the cache, and warn if the cache got populated again during the hotload.
	/// </summary>
	[UpgraderGroup( typeof( ReferenceTypeUpgraderGroup ) )]
	internal class JsonSerializerOptionsUpgrader : Hotload.InstanceUpgrader
	{
		private readonly HashSet<Type> _foundTypeInfos = new();

		private void ClearCache()
		{
			var updateHandlerType = typeof( JsonSerializerOptions ).Assembly
				.GetType( "System.Text.Json.JsonSerializerOptionsUpdateHandler", true );

			var clearCacheMethod = updateHandlerType!.GetMethod( "ClearCache", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic );

			Assert.NotNull( clearCacheMethod );

			clearCacheMethod!.Invoke( null, new object[] { null } );
		}

		protected override void OnHotloadStart()
		{
			_foundTypeInfos.Clear();

			ClearCache();
		}

		protected override void OnHotloadComplete()
		{
			if ( _foundTypeInfos.Count == 0 ) return;

			var typeInfos = string.Join( Environment.NewLine, _foundTypeInfos.Select( x => $"  {x.ToSimpleString()}" ) );
			_foundTypeInfos.Clear();

			Log( HotloadEntryType.Warning, $"Encountered a {nameof( JsonTypeInfo )}, did Json serialization occur during hotload?" );
			Log( HotloadEntryType.Trace, $"JsonTypeInfo types:{Environment.NewLine}{typeInfos}" );

			// Clear at the end too, in case some serialization happened during the hotload

			ClearCache();
		}

		public override bool ShouldProcessType( Type type )
		{
			// We shouldn't find any JsonTypeInfo during hotload because we cleared the cache

			return type.IsAssignableTo( typeof( JsonTypeInfo ) );
		}

		protected override bool OnTryCreateNewInstance( object oldInstance, out object newInstance )
		{
			newInstance = null;
			return true;
		}

		protected override bool OnTryUpgradeInstance( object oldInstance, object newInstance, bool createdElsewhere )
		{
			AddCachedInstance( oldInstance, newInstance );

			if ( oldInstance is JsonTypeInfo { Type: { } type } )
			{
				_foundTypeInfos.Add( type );
			}

			return true;
		}
	}
}
