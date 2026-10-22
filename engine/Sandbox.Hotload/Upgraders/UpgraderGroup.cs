using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace Sandbox.Upgraders
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class DisableAutoCreationAttribute : Attribute { }

	public enum GroupOrder
	{
		/// <summary>
		/// Only use <see cref="AttemptBeforeAttribute"/> and <see cref="AttemptAfterAttribute"/> to
		/// determine ordering within a <see cref="UpgraderGroup"/>.
		/// </summary>
		Default,

		/// <summary>
		/// Try to put this upgrader as close to the start of the given group as possible.
		/// </summary>
		First,

		/// <summary>
		/// Try to put this upgrader as close to the end of the given group as possible.
		/// </summary>
		Last
	}

	[AttributeUsage( AttributeTargets.Class )]
	public sealed class UpgraderGroupAttribute : Attribute
	{
		public Type UpgraderGroupType { get; }

		public GroupOrder GroupOrder { get; }

		public UpgraderGroupAttribute( Type upgraderGroupType, GroupOrder groupOrder = default )
		{
			UpgraderGroupType = upgraderGroupType;
			GroupOrder = groupOrder;
		}
	}

	/// <summary>
	/// Use this attribute to specify that a <see cref="Hotload.IInstanceUpgrader"/> should attempt to process
	/// each object before all other specified <see cref="Hotload.IInstanceUpgrader"/> types.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class AttemptBeforeAttribute : Attribute
	{
		/// <summary>
		/// <see cref="Hotload.IInstanceUpgrader"/> types that should attempt to process each object after the type this attribute is on.
		/// </summary>
		public Type[] InstanceUpgraderTypes { get; }

		/// <summary>
		/// Create an instance of <see cref="AttemptBeforeAttribute"/> with a list of <see cref="Hotload.IInstanceUpgrader"/> types.
		/// </summary>
		/// <param name="instanceUpgraderTypes">One or more <see cref="Hotload.IInstanceUpgrader"/> types.</param>
		public AttemptBeforeAttribute( params Type[] instanceUpgraderTypes )
		{
			InstanceUpgraderTypes = instanceUpgraderTypes;
		}
	}

	/// <summary>
	/// Use this attribute to specify that a <see cref="Hotload.IInstanceUpgrader"/> should attempt to process
	/// each object after all other specified <see cref="Hotload.IInstanceUpgrader"/> types.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class AttemptAfterAttribute : Attribute
	{
		/// <summary>
		/// <see cref="Hotload.IInstanceUpgrader"/> types that should attempt to process each object before the type this attribute is on.
		/// </summary>
		public Type[] InstanceUpgraderTypes { get; }

		/// <summary>
		/// Create an instance of <see cref="AttemptAfterAttribute"/> with a list of <see cref="Hotload.IInstanceUpgrader"/> types.
		/// </summary>
		/// <param name="instanceUpgraderTypes">One or more <see cref="Hotload.IInstanceUpgrader"/> types.</param>
		public AttemptAfterAttribute( params Type[] instanceUpgraderTypes )
		{
			InstanceUpgraderTypes = instanceUpgraderTypes;
		}
	}

	/// <summary>
	/// Used to organize <see cref="Sandbox.Hotload.IInstanceUpgrader"/>s into groups that update
	/// in a particular order. Use <see cref="UpgraderGroupAttribute"/> to specify which group an
	/// upgrader should be added to.
	/// </summary>
	public abstract class UpgraderGroup : Hotload.IInstanceUpgrader
	{
		/// <summary>
		/// A list of <see cref="Hotload.IInstanceUpgrader"/>s added to this group, where this group is their immediate parent.
		/// </summary>
		private readonly List<Hotload.IInstanceUpgrader> ChildUpgraders = new List<Hotload.IInstanceUpgrader>();

		/// <summary>
		/// Indices into <see cref="ChildUpgraders"/>, sorted by <see cref="SortUpgraders"/>.
		/// </summary>
		private readonly List<int> ChildUpgraderOrder = new List<int>();

		/// <summary>
		/// For each <see cref="Type"/>, caches which <see cref="Hotload.IInstanceUpgrader"/>s should attempt to process
		/// instances of that type, as given by <see cref="Hotload.IInstanceUpgrader.ShouldProcessType"/>.
		/// </summary>
		private readonly Dictionary<Type, Hotload.IInstanceUpgrader[]> UpgraderCache = new Dictionary<Type, Hotload.IInstanceUpgrader[]>();

		/// <summary>
		/// Should <see cref="ChildUpgraders"/> be sorted?
		/// </summary>
		private bool UpgraderOrderDirty;

		private static void AssertIsUpgraderGroupType( Type type, Type usedByType )
		{
			if ( !typeof( UpgraderGroup ).IsAssignableFrom( type ) )
				throw new Exception( $"Type {type.FullName} was used in an {nameof( UpgraderGroupAttribute )} by {usedByType.FullName}, " +
					$"but does not extend {nameof( UpgraderGroup )}." );
		}

		internal static Type GetUpgraderGroupType( Type upgraderType )
		{
			var attrib = upgraderType.GetCustomAttribute<UpgraderGroupAttribute>( true );
			if ( attrib == null )
				return null;

			AssertIsUpgraderGroupType( attrib.UpgraderGroupType, upgraderType );
			return attrib.UpgraderGroupType;
		}

		private static Type[] GetUpgraderGroupPath( Type upgraderType )
		{
			var groupPath = new List<Type>();

			while ( true )
			{
				upgraderType = GetUpgraderGroupType( upgraderType );

				if ( upgraderType == null )
					break;

				if ( groupPath.Contains( upgraderType ) )
					throw new Exception( $"Found a loop in {nameof( UpgraderGroupAttribute )}s involving {upgraderType.FullName}." );

				groupPath.Add( upgraderType );
			}

			groupPath.Reverse();

			return groupPath.ToArray();
		}

		public void AddUpgrader( Hotload.IInstanceUpgrader upgrader )
		{
			if ( upgrader == null )
				throw new ArgumentNullException( nameof( upgrader ) );

			var groupPath = GetUpgraderGroupPath( upgrader.GetType() );

			// If groupPath is empty, there was no UpgraderGroupAttribute so
			// we should just add the upgrader to this group.
			if ( groupPath.Length == 0 )
			{
				AddUpgraderHere( upgrader );
				return;
			}

			// Find where in the groupPath this group is.
			var thisIndex = Array.IndexOf( groupPath, GetType() );

			if ( thisIndex == -1 )
				throw new Exception( $"Attempted to add an upgrader of type {upgrader.GetType().FullName} " +
					$"to a group of type {GetType().FullName}, violating its {nameof( UpgraderGroupAttribute )}." );

			AddUpgrader( upgrader, groupPath, thisIndex + 1 );
		}

		/// <summary>
		/// Works out which child group to add the given upgrader to, or whether to add it to this group.
		/// </summary>
		protected void AddUpgrader( Hotload.IInstanceUpgrader upgrader, Type[] groupPath, int groupPathIndex )
		{
			if ( groupPathIndex >= groupPath.Length )
			{
				AddUpgraderHere( upgrader );
				return;
			}

			var childGroupType = groupPath[groupPathIndex];
			UpgraderGroup matchingGroup = null;

			foreach ( var instanceUpgrader in ChildUpgraders )
			{
				if ( instanceUpgrader.GetType() == childGroupType && instanceUpgrader is UpgraderGroup group )
				{
					if ( matchingGroup != null )
						throw new Exception( $"Attempted to add an upgrader of type {upgrader.GetType().FullName} " +
							$"to a group of type {groupPath.Last().FullName}, but multiple instances of that group type exist." );

					matchingGroup = group;
				}
			}

			if ( matchingGroup == null )
				throw new Exception( $"Attempted to add an upgrader of type {upgrader.GetType().FullName} " +
					$"to a group of type {groupPath.Last().FullName}, but such a group hasn't been added yet." );

			matchingGroup.AddUpgrader( upgrader, groupPath, groupPathIndex + 1 );
		}

		private void AddUpgraderHere( Hotload.IInstanceUpgrader upgrader )
		{
			UpgraderCache.Clear();
			UpgraderOrderDirty = true;

			ChildUpgraders.Add( upgrader );
		}

		private static void ThrowImpossibleOrdering( Type a, Type b )
		{
			throw new Exception( $"Unable to find a valid ordering between instance upgraders {a.FullName} and {b.FullName}. " +
				$"Please check their {nameof( AttemptBeforeAttribute )}s and {nameof( AttemptAfterAttribute )}s, and the {nameof( GroupOrder )} " +
				$"value for each of their {nameof( UpgraderGroupAttribute )}s." );
		}

		private struct SortInfo : IEquatable<SortInfo>
		{
			public readonly int Index;
			public readonly Type Type;

			public GroupOrder GroupOrder =>
				Type.GetCustomAttribute<UpgraderGroupAttribute>()?.GroupOrder ?? default;

			public IEnumerable<Type> AttemptBefore =>
				Type.GetCustomAttribute<AttemptBeforeAttribute>()?.InstanceUpgraderTypes ?? Enumerable.Empty<Type>();

			public IEnumerable<Type> AttemptAfter =>
				Type.GetCustomAttribute<AttemptAfterAttribute>()?.InstanceUpgraderTypes ?? Enumerable.Empty<Type>();

			public SortInfo( Hotload.IInstanceUpgrader upgrader, int index )
			{
				Index = index;
				Type = upgrader.GetType();
			}

			public bool Equals( SortInfo other ) => Index == other.Index;

			public override bool Equals( object obj ) => obj is SortInfo other && Equals( other );

			public override int GetHashCode() => Index;
		}

		private void SortUpgraders()
		{
			if ( !UpgraderOrderDirty )
				return;

			UpgraderOrderDirty = false;

			var sortInfos = ChildUpgraders
				.Select( ( x, i ) => new SortInfo( x, i ) )
				.ToArray();

			var sortingHelper = new SortingHelper( ChildUpgraders.Count );

			// Add constraints to sorter

			foreach ( var sortInfo in sortInfos )
			{
				foreach ( var otherInfo in sortInfo.AttemptBefore.SelectMany( x => sortInfos.Where( y => y.Type == x ) ) )
				{
					sortingHelper.AddConstraint( sortInfo.Index, otherInfo.Index );
				}

				foreach ( var otherInfo in sortInfo.AttemptAfter.SelectMany( x => sortInfos.Where( y => y.Type == x ) ) )
				{
					sortingHelper.AddConstraint( otherInfo.Index, sortInfo.Index );
				}

				switch ( sortInfo.GroupOrder )
				{
					case GroupOrder.First:
						sortingHelper.AddFirst( sortInfo.Index );
						break;

					case GroupOrder.Last:
						sortingHelper.AddLast( sortInfo.Index );
						break;
				}
			}

			// Sort!

			if ( sortingHelper.Sort( ChildUpgraderOrder, out var invalidConstraint ) )
				return;

			if ( invalidConstraint.IsZero )
				throw new Exception( $"Unable to find a valid ordering for upgraders added to {GetType().FullName}." );

			ThrowImpossibleOrdering(
				sortInfos[invalidConstraint.EarlierIndex].Type,
				sortInfos[invalidConstraint.LaterIndex].Type );
		}

		internal void SetUpgradersForType( Type type, params Hotload.IInstanceUpgrader[] upgraders )
		{
			if ( !UpgraderCache.TryAdd( type, upgraders ) )
			{
				UpgraderCache[type] = upgraders;
			}
		}

		/// <summary>
		/// Returns a flat array of upgraders that can process the given type, in
		/// order of precedence. This array won't contain <see cref="UpgraderGroup"/>s,
		/// but it will contain upgraders found within those groups.
		/// </summary>
		/// <param name="type">Type to find upgraders for.</param>
		internal Hotload.IInstanceUpgrader[] GetUpgradersForType( Type type )
		{
			if ( UpgraderCache.TryGetValue( type, out var cached ) )
				return cached;

			SortUpgraders();

			List<Hotload.IInstanceUpgrader> upgraders = null;

			foreach ( var upgraderIndex in ChildUpgraderOrder )
			{
				var upgrader = ChildUpgraders[upgraderIndex];

				if ( upgrader.ShouldProcessType( type ) )
				{
					upgraders ??= new List<Hotload.IInstanceUpgrader>();

					if ( upgrader is UpgraderGroup childGroup )
					{
						upgraders.AddRange( childGroup.GetUpgradersForType( type ) );
					}
					else
					{
						upgraders.Add( upgrader );
					}
				}
			}

			// We check to see if the key exists again because SetUpgradersForType() might
			// have been called since the last time we looked, in which case we should discard
			// our work
			if ( UpgraderCache.TryGetValue( type, out cached ) )
				return cached;

			if ( upgraders != null )
			{
				cached = upgraders.ToArray();
				UpgraderCache.Add( type, cached );
				return cached;
			}

			return Array.Empty<Hotload.IInstanceUpgrader>();
		}

		public bool IsInitialized => Hotload != null;
		public Hotload Hotload { get; private set; }

		public void Initialize( Hotload hotload )
		{
			Hotload = hotload;
		}

		public void HotloadStart()
		{
			SortUpgraders();

			foreach ( var upgraderIndex in ChildUpgraderOrder )
			{
				var upgrader = ChildUpgraders[upgraderIndex];
				upgrader.HotloadStart();
			}
		}

		public void HotloadComplete()
		{
			SortUpgraders();

			foreach ( var upgraderIndex in ChildUpgraderOrder )
			{
				var upgrader = ChildUpgraders[upgraderIndex];
				upgrader.HotloadComplete();
			}
		}

		public void ClearCache()
		{
			SortUpgraders();

			foreach ( var upgraderIndex in ChildUpgraderOrder )
			{
				var upgrader = ChildUpgraders[upgraderIndex];
				upgrader.ClearCache();
			}

			UpgraderCache.Clear();
		}

		public virtual bool ShouldProcessType( Type type )
		{
			return GetUpgradersForType( type ).Length > 0;
		}

		public bool TryCreateNewInstance( object oldInstance, out object newInstance )
		{
			foreach ( var instanceUpgrader in GetUpgradersForType( oldInstance.GetType() ) )
			{
				if ( instanceUpgrader.TryCreateNewInstance( oldInstance, out newInstance ) )
				{
					return true;
				}
			}

			newInstance = null;
			return false;
		}

		public bool TryUpgradeInstance( object oldInstance, object newInstance )
		{
			foreach ( var instanceUpgrader in GetUpgradersForType( oldInstance.GetType() ) )
			{
				if ( instanceUpgrader.TryUpgradeInstance( oldInstance, newInstance ) )
				{
					return true;
				}
			}

			return false;
		}
	}

	[DisableAutoCreation]
	public sealed class RootUpgraderGroup : UpgraderGroup { }

	[UpgraderGroup( typeof( RootUpgraderGroup ) )]
	public sealed class ReferenceTypeUpgraderGroup : UpgraderGroup
	{
		public override bool ShouldProcessType( Type type )
		{
			return !type.IsValueType && base.ShouldProcessType( type );
		}
	}

	[UpgraderGroup( typeof( ReferenceTypeUpgraderGroup ) )]
	public sealed class CollectionsUpgraderGroup : UpgraderGroup { }
}
