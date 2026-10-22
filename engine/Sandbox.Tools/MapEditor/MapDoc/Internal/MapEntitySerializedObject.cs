using Editor.MapDoc;
using Editor.MapEditor;

namespace Sandbox.Internal;

/// <summary>
/// An implementation of TypeLibrary which uses TypeLibrary to fill out properties
/// </summary>E
class MapEntitySerializedObject : EditorContext.EntityObject
{
	internal MapEntity Target;

	public override Transform Transform
	{
		get => new Transform( Position, Rotation.From( Angles ), (Scale.x + Scale.y + Scale.z) / 3.0f );
		set
		{
			Position = value.Position;
			Angles = value.Rotation.Angles();
			Scale = value.Scale;
		}
	}

	public override Vector3 Position
	{
		get => Target.Position;
		set => Target.Position = value;
	}

	public override Angles Angles
	{
		get => Target.Angles;
		set => Target.Angles = value;
	}

	public override Vector3 Scale
	{
		get => Target.Scale;
		set => Target.Scale = value;
	}

	public MapEntitySerializedObject( MapEntity entity )
	{
		Target = entity;
		BuildProperties();
	}

	void BuildProperties()
	{
		PropertyList ??= new();
	}

	public override SerializedProperty GetProperty( string v )
	{
		var prop = base.GetProperty( v );
		if ( prop != null ) return prop;

		prop = new TypeSerializedProperty( this, v );
		PropertyList.Add( prop );
		return prop;
	}
}


file class TypeSerializedProperty : SerializedProperty
{
	private MapEntitySerializedObject _parent;

	string PropertyName;

	public override SerializedObject Parent => _parent;
	public override string DisplayName => PropertyName;
	public override string Name => PropertyName;
	public override string Description => PropertyName;

	public TypeSerializedProperty( MapEntitySerializedObject obj, string name )
	{
		_parent = obj;
		PropertyName = name;
	}

	public override void SetValue<T>( T value )
	{
		NotePreChange();
		// should we be json'ing?
		_parent.Target.SetKeyValue( Name, $"{value}" );
		NoteChanged();
	}

	public override T GetValue<T>( T defaultValue )
	{
		var str = _parent.Target.GetKeyValue( Name );

		if ( str == null )
			return default;

		return (T)str.ToType( typeof( T ) );
	}
}
