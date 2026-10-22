using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Sandbox.Upgraders.SpecialCases
{
	/// <summary>
	/// Custom handling for <see cref="ConcurrentQueue{T}"/> to reduce hotload processing time.
	/// </summary>
	[UpgraderGroup( typeof( CollectionsUpgraderGroup ) )]
	internal class ConcurrentQueueUpgrader : Hotload.InstanceUpgrader
	{
		public override bool ShouldProcessType( Type type )
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof( ConcurrentQueue<> );
		}

		protected override bool OnTryCreateNewInstance( object oldInstance, out object newInstance )
		{
			var newType = GetNewType( oldInstance.GetType() );

			newInstance = Activator.CreateInstance( newType );

			return true;
		}

		protected override bool OnTryUpgradeInstance( object oldInstance, object newInstance, bool createdElsewhere )
		{
			AddCachedInstance( oldInstance, newInstance );
			SuppressFinalize( oldInstance, newInstance );
			ScheduleProcessInstance( oldInstance, newInstance );

			return true;
		}

		protected override int OnProcessInstance( object oldInstance, object newInstance )
		{
			var oldType = oldInstance.GetType();
			var newType = newInstance.GetType();

			var elementType = newType.GetGenericArguments()[0];
			var paramTypes = new[] { elementType };
			var enqueueMethod = newType.GetMethod( nameof( ConcurrentQueue<object>.Enqueue ), paramTypes );
			var toArrayMethod = oldType.GetMethod( nameof( ConcurrentQueue<object>.ToArray ), Array.Empty<Type>() );

			var oldItems = (IEnumerable)toArrayMethod.Invoke( oldInstance, Array.Empty<object>() );

			if ( ReferenceEquals( oldInstance, newInstance ) )
			{
				// If the instance was in an InitOnly static field of an assembly
				// that wasn't swapped, oldInstance == newInstance so we have to
				// edit in-place

				var clearMethod = newType.GetMethod( nameof( ConcurrentQueue<object>.Clear ), Array.Empty<Type>() );

				clearMethod.Invoke( newInstance, Array.Empty<object>() );
			}

			var args = new object[1];
			foreach ( var oldItem in oldItems )
			{
				var newItem = GetNewInstance( oldItem );

				args[0] = newItem;
				enqueueMethod.Invoke( newInstance, args );
			}

			return 1;
		}
	}
}
