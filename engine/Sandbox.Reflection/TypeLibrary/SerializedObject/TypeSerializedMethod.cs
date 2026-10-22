namespace Sandbox.Internal;

class TypeSerializedMethod : SerializedProperty
{
	private TypeSerializedObject typeSerializedObject;
	private MethodDescription method;

	public override bool IsMethod => true;

	public override SerializedObject Parent => typeSerializedObject;
	public override string DisplayName => method.Title;
	public override string Name => method.Name;
	public override string Description => method.Description;
	public override string GroupName => method.Group;
	public override bool IsEditable => !method.ReadOnly;
	public override bool IsPublic => method.IsPublic;
	public override Type PropertyType => method.ReturnType;
	public override string SourceFile => method.SourceFile;
	public override int SourceLine => method.SourceLine;
	public override int Order => method.Order;

	public TypeSerializedMethod( TypeSerializedObject typeSerializedObject, MethodDescription method )
	{
		this.typeSerializedObject = typeSerializedObject;
		this.method = method;
	}

	public override void SetValue<T>( T value )
	{
		// nothing
	}

	public override T GetValue<T>( T defaultValue )
	{
		return default;
	}

	/// <inheritdoc />
	public override IEnumerable<Attribute> GetAttributes()
	{
		return method?.Attributes ?? base.GetAttributes();
	}

	public override bool TryGetAsObject( out SerializedObject obj )
	{
		obj = default;
		return false;
	}

	public override void Invoke()
	{
		if ( method.IsStatic )
		{
			method.Invoke( null );
		}
		else
		{
			method.Invoke( typeSerializedObject.GetTargetObject() );
		}
	}
}
