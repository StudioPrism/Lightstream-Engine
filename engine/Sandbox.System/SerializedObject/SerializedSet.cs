using System.Reflection;

namespace Sandbox;

internal class SerializedSet : SerializedCollection
{
	// Unfortunately, there is not non-generic ISet interface, so we have to use reflection
	internal object set;
	private MethodInfo containsMethod;
	private MethodInfo addMethod;
	private MethodInfo removeMethod;
	private PropertyInfo countProperty;

	public override void SetTargetObject( object obj, SerializedProperty property )
	{
		base.SetTargetObject( obj, property );

		set = obj;
		ArgumentNullException.ThrowIfNull( set );

		// Cache reflection methods
		Type setType = set.GetType();
		containsMethod = setType.GetMethod( "Contains", [KeyType] );
		addMethod = setType.GetMethod( "Add", [KeyType] );
		removeMethod = setType.GetMethod( "Remove", [KeyType] );
		countProperty = setType.GetProperty( "Count" );
	}

	protected override void PrepareEnumerator()
	{
		// Get count through reflection
		int count = (int)countProperty.GetValue( set );

		if ( PropertyList is not null && PropertyList.Count == count )
			return;

		PropertyList = new List<SerializedProperty>();

		foreach ( var element in (System.Collections.IEnumerable)set )
		{
			PropertyList.Add( new SerializedSetProperty( this, element ) );
		}
	}

	public override bool Remove( SerializedProperty property )
	{
		if ( property is not SerializedSetProperty p )
			return false;

		return RemoveAt( p.Element );
	}

	public override bool RemoveAt( object element )
	{
		if ( !Translation.TryConvert( ref element, KeyType ) )
			return false;

		if ( !Contains( element ) )
			return false;

		NotePreChange();

		// Use reflection to call Remove
		bool removed = (bool)removeMethod.Invoke( set, [element] );
		if ( !removed )
			return false;

		if ( PropertyList is not null )
		{
			var prop = PropertyList.OfType<SerializedSetProperty>().FirstOrDefault( x => Equals( x.Element, element ) );
			if ( prop is not null )
				PropertyList.Remove( prop );
		}

		OnEntryRemoved?.Invoke();
		NoteChanged();
		return true;
	}

	public override bool Add( object element )
	{
		if ( !Translation.TryConvert( ref element, KeyType ) )
			return false;

		// Use reflection to call Add
		bool added = (bool)addMethod.Invoke( set, [element] );
		if ( !added )
			return false; // duplicate

		var prop = new SerializedSetProperty( this, element );
		NotePreChange( prop );

		PropertyList?.Add( prop );
		OnEntryAdded?.Invoke();

		NoteChanged( prop );
		return true;
	}

	public bool Contains( object element )
	{
		if ( !Translation.TryConvert( ref element, KeyType ) )
			return false;

		// Use reflection to call Contains
		return (bool)containsMethod.Invoke( set, [element] );
	}
}

file class SerializedSetProperty : SerializedProperty
{
	private readonly SerializedSet owner;
	public object Element { get; internal set; }

	public override string Name => $"{Element}";
	public override Type PropertyType => owner.KeyType;
	public override SerializedObject Parent => owner;

	public SerializedSetProperty( SerializedSet owner, object element )
	{
		this.owner = owner;
		Element = element;
	}

	// For sets, the element itself is the "key"
	public override SerializedProperty GetKey() => this;

	public override IEnumerable<Attribute> GetAttributes()
	{
		return owner?.ParentProperty?.GetAttributes() ?? Enumerable.Empty<Attribute>();
	}

	public override T GetValue<T>( T defaultValue = default )
	{
		return ValueToType<T>( Element, defaultValue );
	}

	public override void SetValue<T>( T value )
	{
		if ( !Translation.TryConvert( value, PropertyType, out var converted ) )
			return;

		if ( Equals( Element, converted ) )
			return;

		owner.NotePreChange( this );

		// Use reflection instead of dynamic
		owner.RemoveAt( Element );
		owner.Add( converted );

		Element = converted;

		owner.NoteChanged( this );
	}

	public override bool TryGetAsObject( out SerializedObject obj )
	{
		obj = owner.PropertyToObject?.Invoke( this ) ?? null;
		return obj is not null;
	}
}
