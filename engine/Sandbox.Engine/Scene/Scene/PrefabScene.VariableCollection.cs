using System.Collections;

namespace Sandbox;

public partial class PrefabScene
{
	/// <summary>
	/// A collection of variabnles that have been configured for this scene
	/// </summary>
	[Obsolete]
	public class VariableCollection : IEnumerable<PrefabVariable>
	{
		public bool IsVariable( SerializedProperty property )
		{
			var key = ConstructKey( property );

			foreach ( var entry in _all )
			{
				if ( entry.Targets.Any( x => x == key ) ) return true;
			}

			return false;
		}

		List<PrefabVariable> _all { get; } = new();

		public PrefabVariable Create( string name )
		{
			var created = new PrefabVariable
			{
				Id = name,
				Title = name,
				Targets = new(),
			};

			_all.Add( created );
			return created;
		}

		public void Remove( PrefabVariable variable )
		{
			_all.Remove( variable );
		}

		string ConstructKey( Guid guid, string property )
		{
			return $"{guid}/{property}";
		}

		public static (Guid gameobject, Guid component, string propertyName) DeconstructKey( string property )
		{
			var parts = property.Split( '/' );
			if ( parts.Length == 3 )
			{
				return (Guid.Parse( parts[0] ), Guid.Parse( parts[1] ), parts[2]);
			}

			if ( parts.Length == 2 )
			{
				return (Guid.Parse( parts[0] ), Guid.Empty, parts[1]);
			}

			return default;
		}

		PrefabVariable.PrefabVariableTarget ConstructKey( SerializedProperty prop )
		{
			var parent = prop.Parent;

			if ( parent.Targets.First() is Component component )
			{
				return new PrefabVariable.PrefabVariableTarget { Id = component.Id, Property = prop.Name };
			}

			if ( parent.Targets.First() is GameObject go )
			{
				return new PrefabVariable.PrefabVariableTarget { Id = go.Id, Property = prop.Name };
			}

			return default;

		}

		public void ClearVariable( SerializedProperty property )
		{
			var key = ConstructKey( property );

			foreach ( var entry in _all )
			{
				entry.Targets.RemoveAll( x => x == key );
			}
		}

		internal List<PrefabVariable> Serialize() => _all;

		internal void Deserialize( List<PrefabVariable> json )
		{
			_all.Clear();

			if ( json is null )
				return;

			_all.AddRange( json );
		}

		public IEnumerator<PrefabVariable> GetEnumerator() => ((IEnumerable<PrefabVariable>)_all).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_all).GetEnumerator();
	}

}


