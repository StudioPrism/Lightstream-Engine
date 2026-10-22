using NativeMapDoc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Editor.MapDoc;

/// <summary>
/// MapEntity in Hammer is a type of <see cref="MapNode"/> that has a set of key/value pairs.
/// The keyvalues represent the authoritative state of the entity. 
/// 
/// Entities may have helpers that enhance the presentation and sometimes modification of those keyvalues.
/// The helpers may come and go; it should always be possible to recreate the helpers from
/// the parent entity's keyvalues.
/// 
/// Entities may also have zero or more <see cref="MapMesh"/> children.
/// </summary>
[Display( Name = "Entity" ), Icon( "view_in_ar" )]
public sealed class MapEntity : MapNode
{
	public SerializedObject SerializedObject { get; internal set; }

	public MapClass MapClass { get; internal set; }
	public TypeDescription TypeDescription { get; internal set; }

	internal CMapEntity entityNative;

	internal MapEntity( HandleCreationData _ ) { }

	public MapEntity( MapDocument mapDocument = null )
	{
		ThreadSafe.AssertIsMainThread();

		// Default to the active map document if none specificed
		mapDocument ??= MapEditor.Hammer.ActiveMap;

		Assert.IsValid( mapDocument );

		using ( var h = IHandle.MakeNextHandle( this ) )
		{
			mapDocument.native.CreateEntity( true );
		}

	}

	internal override void OnNativeInit( CMapNode ptr )
	{
		base.OnNativeInit( ptr );

		entityNative = (CMapEntity)ptr;
		SerializedObject = new Sandbox.Internal.MapEntitySerializedObject( this );

		//
		// Don't try to interact with this yet here, wait for OnAddedToDocument
		//
	}

	internal override void OnAddedToWorld( MapWorld world )
	{
		UpdateTypeDescription();
	}

	internal override void OnNativeDestroy()
	{
		base.OnNativeDestroy();

		SerializedObject = null;
		entityNative = default;
	}

	/// <summary>
	/// Entity class name like prop_physics
	/// </summary>
	public string ClassName
	{
		get => entityNative.GetClassName();
		set
		{
			Assert.NotNull( value );
			entityNative.SetClass( value );

			UpdateTypeDescription();
		}
	}

	internal void UpdateTypeDescription()
	{
		MapClass = GameData.EntityClasses.Where( x => x.Name == ClassName ).FirstOrDefault();
		if ( MapClass == null || MapClass.Type == null )
		{
			TypeDescription = null;
			return;
		}

		TypeDescription = EditorTypeLibrary.GetType( MapClass.Type );
	}

	/// <summary>
	/// Gets the value for the key, e.g "model" could return "models/props_c17/oildrum001_explosive.mdl"
	/// </summary>
	public string GetKeyValue( string key )
	{
		ArgumentNullException.ThrowIfNull( key );

		key = key.ToLowerInvariant();

		if ( key == "origin" ) return entityNative.GetOrigin().ToString();
		if ( key == "angles" ) return entityNative.GetAngles().ToString();
		if ( key == "scale" ) return entityNative.GetScales().ToString();

		return entityNative.GetKeyValue( key );
	}

	/// <summary>
	/// Sets the value for the key, e.g "model" could be set to "models/props_c17/oildrum001_explosive.mdl"
	/// </summary>
	public void SetKeyValue( string key, string value )
	{
		ArgumentNullException.ThrowIfNull( key, nameof( key ) );
		ArgumentNullException.ThrowIfNull( value, nameof( value ) );

		key = key.ToLowerInvariant();

		if ( key == "origin" )
		{
			entityNative.SetOrigin( Vector3.Parse( value ) );
			return;
		}

		if ( key == "angles" )
		{
			entityNative.SetAngles( Angles.Parse( value ) );
			return;
		}

		if ( key == "scale" )
		{
			entityNative.SetScales( Vector3.Parse( value ) );
			return;
		}

		entityNative.SetKeyValue( key, value );
	}

	/// <summary>
	/// Sets the default bounds of the entity if it doesn't have a model. By default this is 16x16x16.
	/// </summary>
	/// <remarks>
	/// Refactor out once we have managed helpers I think.
	/// </remarks>
	public void SetDefaultBounds( Vector3 min, Vector3 max ) => entityNative.SetDefaultBounds( min, max );
}
