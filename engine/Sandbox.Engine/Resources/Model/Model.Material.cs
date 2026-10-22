using System.Collections.Immutable;

namespace Sandbox
{
	public partial class Model
	{
		ImmutableArray<Material> _materials;

		/// <summary>
		/// Number of material groups this model has.
		/// </summary>
		public int MaterialGroupCount => native.GetNumMaterialGroups();

		/// <summary>
		/// Returns name of a material group at given group index.
		/// </summary>
		/// <param name="groupIndex">Group index to get name of, starting at 0.</param>
		/// <returns>Name of the group.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when given index exceeds range of [0,MaterialGroupCount-1]</exception>
		public string GetMaterialGroupName( int groupIndex )
		{
			if ( groupIndex < 0 || groupIndex >= MaterialGroupCount )
			{
				throw new ArgumentOutOfRangeException( "groupIndex", $"Tried to access out of range group index {groupIndex}, range is 0-{MaterialGroupCount - 1}" );
			}

			return native.GetMaterialGroupName( groupIndex );
		}

		/// <summary>
		/// Retrieves the index of a material group given its name.
		/// </summary>
		/// <param name="groupIndex">The name of the material group.</param>
		/// <returns>The index of the material group, or a negative value if the group does not exist.</returns>
		public int GetMaterialGroupIndex( string groupIndex )
		{
			return native.GetMaterialGroupIndex( groupIndex );
		}

		/// <summary>
		/// Retrieves an enumerable collection of all Materials on the meshes.
		/// This is fast, and cached. The order of these items is the same order used in ModelRenderer.Materials etc
		/// </summary>
		/// <returns>An ImmutableArray of Materials.</returns>
		public ImmutableArray<Material> Materials
		{
			get
			{
				if ( _materials.IsDefault )
				{
					_materials = Enumerable.Range( 0, native.GetMaterialIndexCount() )
						.Select( i => Material.FromNative( native.GetMaterialByIndex( i ) ) )
						.ToImmutableArray();
				}

				return _materials;
			}
		}

		/// <summary>
		/// Retrieves an enumerable collection of Materials belonging to a specified group.
		/// </summary>
		/// <param name="groupIndex">The index of the material group. Default value is 0.</param>
		/// <returns>An IEnumerable of Materials in the specified group.</returns>
		public IEnumerable<Material> GetMaterials( int groupIndex )
		{
			if ( groupIndex < 0 || groupIndex >= MaterialGroupCount )
			{
				throw new ArgumentOutOfRangeException( "groupIndex", $"Tried to access out of range group index {groupIndex}, range is 0-{MaterialGroupCount - 1}" );
			}

			int count = native.GetNumMaterialsInGroup( groupIndex );

			for ( int i = 0; i < count; ++i )
			{
				yield return Material.FromNative( native.GetMaterialInGroup( groupIndex, i ) );
			}
		}

		/// <summary>
		/// Retrieves an enumerable collection of Materials belonging to a specified group.
		/// </summary>
		/// <param name="groupName">The name of the material group.</param>
		/// <returns>An IEnumerable of Materials in the specified group.</returns>
		/// <exception cref="ArgumentException">Thrown when the provided group name does not exist.</exception>
		public IEnumerable<Material> GetMaterials( string groupName )
		{
			var groupIndex = GetMaterialGroupIndex( groupName );

			if ( groupIndex < 0 )
			{
				throw new ArgumentException( "groupName", $"The group name '{groupName}' does not exist." );
			}

			return GetMaterials( groupIndex );
		}

		/// <summary>
		/// Used to mark a property as a material group, for the editor
		/// </summary>
		public sealed class MaterialGroupAttribute : System.Attribute
		{
			public string ModelParameter { get; set; } = "Model";
		}

		/// <summary>
		/// Used to mark a property as a material material override dictionary, for the editor
		/// </summary>
		public sealed class MaterialOverrideAttribute : System.Attribute
		{
			public string ModelParameter { get; set; } = "Model";
		}
	}
}
