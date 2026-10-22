using Sandbox;
using Facepunch.ActionGraphs;
using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.UI;

namespace Editor.ActionGraphs;

[CustomEditor( typeof( Either ) )]
internal class EitherControlWidget : ControlWidget
{
	public class TypeValueWrapper : SerializedObject
	{
		private static Type[] PreferredTypes { get; } = { typeof( string ), typeof( float ), typeof( Vector3 ), typeof( int ) };

		public SerializedProperty Inner { get; }
		public Type ValueType { get; set; }

		public Type[] Options { get; }

		private readonly SerializedProperty _typeProperty;
		private readonly SerializedProperty _valueProperty;

		public TypeValueWrapper( SerializedProperty inner, Type[] options, Type preferred = null )
		{
			Inner = inner;
			Options = options;

			ValueType = inner.GetValue<object>()?.GetType() ?? preferred ?? PreferredTypes.FirstOrDefault( options.Contains ) ?? options.First();

			_typeProperty = new TypeProperty( this );
			_valueProperty = new ValueProperty( this );

			PropertyList = new List<SerializedProperty> { _typeProperty, _valueProperty };
		}

		public override SerializedProperty GetProperty( string v )
		{
			return v switch
			{
				"Type" => _typeProperty,
				"Value" => _valueProperty,
				_ => null
			};
		}

		private class TypeProperty : SerializedProperty
		{
			public override string Name => "Type";
			public override string Description => "Value type for this property.";
			public override Type PropertyType => typeof( Type );

			public TypeValueWrapper Wrapper { get; }

			public override SerializedObject Parent => Wrapper;

			public TypeProperty( TypeValueWrapper wrapper )
			{
				Wrapper = wrapper;
			}

			public override void SetValue<T>( T value )
			{
				if ( value is null )
				{
					Wrapper.ValueType = typeof( object );
					Wrapper.Inner.SetValue<object>( null );

					NoteChanged();
				}
				else if ( value is Type type )
				{
					var oldValue = Wrapper.Inner.GetValue<object>();
					var newValue = type.IsInstanceOfType( oldValue )
						? oldValue
						: type.IsValueType
							? Activator.CreateInstance( type )
							: null;

					Wrapper.ValueType = type;
					Wrapper.Inner.SetValue( newValue );

					NoteChanged();
				}
			}

			public override T GetValue<T>( T defaultValue = default( T ) )
			{
				return Wrapper.ValueType is T value ? value : defaultValue;
			}
		}

		private class ValueProperty : SerializedProperty
		{
			public override string Name => Wrapper.Inner.Name;
			public override string Description => Wrapper.Inner.Description;
			public override Type PropertyType => Wrapper.ValueType;

			public TypeValueWrapper Wrapper { get; }

			public ValueProperty( TypeValueWrapper wrapper )
			{
				Wrapper = wrapper;
			}

			public override void SetValue<T>( T value )
			{
				Wrapper.Inner.SetValue( value );
				NoteChanged();
			}

			public override T GetValue<T>( T defaultValue = default( T ) )
			{
				return Wrapper.Inner.GetValue( defaultValue );
			}

			public override bool TryGetAsObject( out SerializedObject obj )
			{
				if ( Wrapper.Inner.TryGetAsObject( out obj ) )
				{
					return true;
				}

				if ( PropertyType.IsValueType )
				{
					obj = EditorTypeLibrary.GetSerializedObject( Activator.CreateInstance( PropertyType ) );
					obj.ParentProperty = this;
					return true;
				}

				return false;
			}
		}
	}

	private readonly TypeValueWrapper _wrapper;
	private ControlWidget _valueControl;

	public EitherControlWidget( SerializedProperty property )
		: base( property )
	{
		Layout = Layout.Row( reversed: true );
		Layout.Spacing = 2;

		var types = Either.Unwrap( property.PropertyType ).ToArray();
		var preferred =
			Either.IsEitherType( property.PropertyType ) &&
			Either.IsEitherType( property.PropertyType.GetGenericArguments()[1] )
				? types[0]
				: null;

		_wrapper = new TypeValueWrapper( property, types, preferred );

		var typeProperty = _wrapper.GetProperty( "Type" );
		var typeControl = Layout.Add( new TypeControlWidget( typeProperty ), 1 );

		typeControl.FixedWidth = Theme.RowHeight * 4f;

		UpdateValueControl();

		_wrapper.OnPropertyChanged += changedProperty =>
		{
			if ( changedProperty == typeProperty )
			{
				UpdateValueControl();
			}
		};
	}

	private void UpdateValueControl()
	{
		_valueControl?.Destroy();

		var valueProperty = _wrapper.GetProperty( "Value" );

		_valueControl = Layout.Add( Create( valueProperty ), 3 );

		_valueControl.MinimumWidth = Theme.RowHeight * 4f;
		_valueControl.HorizontalSizeMode = SizeMode.CanGrow | SizeMode.Expand;
	}

	public override void StartEditing()
	{
		_valueControl?.StartEditing();
	}

	protected override void OnPaint()
	{
		// nothing
	}
}
