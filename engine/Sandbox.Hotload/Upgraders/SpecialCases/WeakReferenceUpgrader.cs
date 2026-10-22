using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Sandbox.Upgraders.SpecialCases;

[UpgraderGroup( typeof( ReferenceTypeUpgraderGroup ) )]
internal class WeakReferenceUpgrader : Hotload.InstanceUpgrader
{
	public override bool ShouldProcessType( Type type )
	{
		return type == typeof( WeakReference ) || type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof( WeakReference<> );
	}

	protected override bool OnTryCreateNewInstance( object oldInstance, out object newInstance )
	{
		if ( oldInstance is WeakReference )
		{
			newInstance = oldInstance;
			return true;
		}

		newInstance = null;

		// For WeakReference<T>, we have to do all the processing now in case
		// we need to call the constructor "new WeakReference<T>(T value)"

		var oldType = oldInstance.GetType();
		var newType = GetNewType( oldType );

		if ( !newType.IsConstructedGenericType || newType.GetGenericTypeDefinition() != typeof( WeakReference<> ) )
			return false;

		var tryGetTargetMethod = oldType.GetMethod( nameof( WeakReference<object>.TryGetTarget ),
			BindingFlags.Instance | BindingFlags.Public );

		if ( tryGetTargetMethod == null )
			return false;

		var args = new object[1];

		// Check if target has been collected, or is null
		if ( !(bool)tryGetTargetMethod.Invoke( oldInstance, args ) || args[0] == null )
		{
			if ( newType == oldType )
			{
				newInstance = oldInstance;
				return true;
			}

			args[0] = null;
			newInstance = Activator.CreateInstance( newType, args );
			return true;
		}

		// Otherwise, replace target
		args[0] = GetNewInstance( args[0] );

		if ( newType == oldType )
		{
			var setTargetMethod = newType.GetMethod( nameof( WeakReference<object>.SetTarget ),
				BindingFlags.Instance | BindingFlags.Public );

			setTargetMethod.Invoke( oldInstance, args );

			newInstance = oldInstance;
			return true;
		}

		newInstance = Activator.CreateInstance( newType, args );
		return true;
	}

	protected override bool OnTryUpgradeInstance( object oldInstance, object newInstance, bool createdElsewhere )
	{
		AddCachedInstance( oldInstance, newInstance );

		if ( oldInstance is WeakReference )
		{
			ScheduleProcessInstance( oldInstance, newInstance );
			return true;
		}

		return true;
	}

	protected override int OnProcessInstance( object oldInstance, object newInstance )
	{
		if ( oldInstance != newInstance || !(oldInstance is WeakReference weakRef) )
			return 0;

		if ( !weakRef.IsAlive )
			return 1;

		var target = weakRef.Target;
		if ( target == null )
			return 1;

		weakRef.Target = GetNewInstance( weakRef.Target );
		return 1;
	}
}

[UpgraderGroup( typeof( ReferenceTypeUpgraderGroup ) )]
internal class ConditionalWeakTableUpgrader() : KeyedCollectionUpgrader<DictionaryEntry>( typeof( ConditionalWeakTable<,> ) )
{
	private sealed class Wrapper<TKey, TValue>( ConditionalWeakTable<TKey, TValue> table ) : ICollectionWrapper
		where TKey : class
		where TValue : class
	{
		public IEnumerator<DictionaryEntry> GetEnumerator() => table
			.Select( x => new DictionaryEntry( x.Key, x.Value ) )
			.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		/// <summary>
		/// Can't get the count of a <see cref="ConditionalWeakTable{TKey,TValue}"/>, so returns <see langword="null"/>.
		/// </summary>
		public int? Count => null;
		public void Clear() => table.Clear();

		public bool Add( DictionaryEntry item ) => table.TryAdd( (TKey)item.Key, (TValue)item.Value );
	}

	protected override object GetKey( DictionaryEntry item ) => item.Key;
	protected override ICollectionWrapper Wrap( object collection )
	{
		var typeArgs = collection.GetType().GetGenericArguments();
		var wrapperType = typeof( Wrapper<,> ).MakeGenericType( typeArgs );

		return (ICollectionWrapper)Activator.CreateInstance( wrapperType, collection )!;
	}
}
