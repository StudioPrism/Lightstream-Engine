namespace Sandbox.Internal;

class TypeSerializedField : SerializedProperty
{
	private TypeSerializedObject typeSerializedObject;
	private FieldDescription field;

	public override bool IsField => true;

	public override SerializedObject Parent => typeSerializedObject;
	public override string DisplayName => @field.Title;
	public override string Name => @field.Name;
	public override string Description => @field.Description;
	public override string GroupName => @field.Group;
	public override bool IsEditable => !@field.ReadOnly;
	public override bool IsPublic => @field.IsPublic;
	public override Type PropertyType => @field.FieldType;
	public override string SourceFile => @field.SourceFile;
	public override int SourceLine => @field.SourceLine;
	public override int Order => @field.Order;

	public TypeSerializedField( TypeSerializedObject typeSerializedObject, FieldDescription field )
	{
		this.typeSerializedObject = typeSerializedObject;
		this.field = field;
	}

	public override void SetValue<T>( T value )
	{
		try
		{
			NotePreChange();
			field.SetValue( typeSerializedObject.GetTargetObject(), value );
			NoteChanged();

		}
		catch ( System.Exception e )
		{
			var l = new Logger( "TypeSerializedProperty" );
			l.Warning( e, $"Error setting {PropertyType} to {value} ({value.GetType()})" );
		}
	}

	public override T GetValue<T>( T defaultValue )
	{
		try
		{
			var value = field.GetValue( typeSerializedObject.GetTargetObject() );
			return ValueToType( value, defaultValue );
		}
		catch ( System.Exception )
		{
			return defaultValue;
		}
	}

	/// <inheritdoc />
	public override IEnumerable<Attribute> GetAttributes()
	{
		return field?.Attributes ?? base.GetAttributes();
	}

	private bool TryGetAsContainer( out SerializedObject obj, TypeLibrary library )
	{
		obj = null;

		var so = SerializedCollection.Create( PropertyType );
		if ( so is not null )
		{
			so.ParentProperty = this;

			var targetObject = GetValue<object>( null );
			if ( targetObject is null )
			{
				targetObject = Activator.CreateInstance( PropertyType );
				SetValue( targetObject );
			}

			so.SetTargetObject( targetObject, this );
			so.PropertyToObject = library.PropertyToObject;
			obj = so;
			return true;
		}

		return false;
	}

	/// <inheritdoc/>
	public override bool TryGetAsObject( out SerializedObject obj )
	{
		obj = null;

		if ( Parent is not TypeSerializedObject szObj )
			return false;

		if ( TryGetAsContainer( out obj, szObj.TypeDescription.library ) )
			return true;

		var targetType = szObj.TypeDescription.library.GetType( PropertyType );

		if ( targetType is null )
			return false;

		obj = new TypeSerializedObject( () => field.GetValue( typeSerializedObject.GetTargetObject() ), targetType, this );
		return obj is not null;
	}
}
