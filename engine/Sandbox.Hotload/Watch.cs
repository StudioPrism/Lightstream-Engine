using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sandbox
{
	public partial class Hotload
	{
		private readonly Dictionary<Assembly, Func<Type, bool>> _watchedAssemblies = new();
		private readonly HashSet<object> _watchedInstances = new();

		/// <summary>
		/// Currently watched assemblies, to enumerate the static fields of. This will contain assemblies added with <see cref="WatchAssembly(System.Reflection.Assembly,Func{Type,bool})"/>,
		/// along with (after a hotload) the most recent replacing assemblies passed to <see cref="ReplacingAssembly"/>.
		/// </summary>
		public IEnumerable<Assembly> WatchedAssemblies => _watchedAssemblies.Keys;

		/// <summary>
		/// Currently watched object instances. Use <see cref="WatchInstance{T}"/> to add to this set.
		/// </summary>
		public IEnumerable<object> WatchedInstances => _watchedInstances;

		/// <summary>
		/// Look for instances to replace in the static fields of types defined in the given assembly.
		/// </summary>
		/// <param name="a">Assembly to watch the static fields of.</param>
		/// <param name="filter">Only test static fields in types that pass this filter.</param>
		public void WatchAssembly( Assembly a, Func<Type, bool> filter = null )
		{
			lock ( _watchedAssemblies )
			{
				if ( _watchedAssemblies.ContainsKey( a ) )
					return;

				if ( _watchedAssemblies.Keys.Any( x =>
					string.Equals( x.GetName().Name, a.GetName().Name,
						StringComparison.OrdinalIgnoreCase ) ) )
				{
					Log( HotloadEntryType.Warning, $"Already watching static fields in a version of this assembly ({a})" );
				}

				_watchedAssemblies.Add( a, filter );
			}
		}

		/// <summary>
		/// Look for instances to replace in the static fields of types defined in 
		/// the defining assembly of <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">Type defined in the assembly to watch the static fields of.</typeparam>
		public void WatchAssembly<T>()
		{
			WatchAssembly( typeof( T ).GetTypeInfo().Assembly );
		}

		/// <summary>
		/// Look for instances to replace in the static fields of types defined in the given assembly.
		/// </summary>
		/// <param name="assemblyName">Name of the assembly to watch the static fields of.</param>
		/// <param name="filter">Only test static fields in types that pass this filter.</param>
		public void WatchAssembly( string assemblyName, Func<Type, bool> filter = null )
		{
			var assembly = AppDomain.CurrentDomain
				.GetAssemblies()
				.Single( assembly => assembly.GetName().Name == assemblyName );

			WatchAssembly( assembly, filter );
		}

		/// <summary>
		/// Stop watching static fields of types defined in the given assembly.
		/// </summary>
		/// <param name="a">Assembly to stop watching the static fields of.</param>
		public void UnwatchAssembly( Assembly a )
		{
			lock ( _watchedAssemblies )
			{
				_watchedAssemblies.Remove( a );
			}
		}

		/// <summary>
		/// Look for instances to replace in the fields of the given object.
		/// </summary>
		/// <param name="obj">Object to watch the fields of.</param>
		public void WatchInstance<T>( T obj )
			where T : class
		{
			if ( obj == null ) throw new ArgumentNullException( nameof( obj ) );

			lock ( _watchedInstances )
			{
				_watchedInstances.Add( obj );
			}
		}

		/// <summary>
		/// Stop looking for instances to replace in the fields of the given object.
		/// </summary>
		/// <param name="obj">Object to stop watching the fields of.</param>
		public void UnwatchInstance<T>( T obj )
			where T : class
		{
			if ( obj == null ) throw new ArgumentNullException( nameof( obj ) );

			lock ( _watchedInstances )
			{
				_watchedInstances.Remove( obj );
			}
		}
	}
}
