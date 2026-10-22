namespace Sandbox;

public partial class Material
{
	/// <summary>
	/// Set a feature flag on the material. This is usually used to enable/disable shader permutations.
	/// This is kind of a define, also known as a combo.
	/// </summary>
	public void SetFeature( string name, int value )
	{
		if ( GetFeature( name ) == value )
			return;

		native.Set( name, new Vector4( value, 0, 0, 0 ) );
		native.ReloadStaticCombos();
	}

	/// <summary>
	/// Get a feature flag on the material. This is usually used to enable/disable shader permutations.
	/// </summary>
	public int GetFeature( string name )
	{
		var val = native.GetVector4( name );
		return val.x.CeilToInt();
	}
}
