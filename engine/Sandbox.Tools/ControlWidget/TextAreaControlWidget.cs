using System;

namespace Editor;

[CustomEditor( typeof( string ), WithAllAttributes = new[] { typeof( Sandbox.TextAreaAttribute ) } )]
public class TextAreaControlWidget : ControlWidget
{
	protected TextEdit TextEdit;
	private bool _editing;

	public override bool IsControlActive => TextEdit.IsFocused;
	public override bool SupportsMultiEdit => true;

	public override bool ReadOnly
	{
		get => base.ReadOnly;
		set
		{
			base.ReadOnly = value;
			TextEdit.ReadOnly = ReadOnly;
		}
	}

	public TextAreaControlWidget( SerializedProperty property ) : base( property )
	{
		TextEdit = new TextEdit( this );
		TextEdit.MaximumSize = new Vector2( 4096, Theme.RowHeight * 4 );
		TextEdit.PlainText = ValueToString();

		// Add event after initial text is set ( QPlainTextEdit has no equivalent textEdited signal )
		TextEdit.TextChanged += ( text ) => property.SetValue( StringToValue( text ) );

		TextEdit.OnEditingFinished += OnEditingFinished;
		TextEdit.OnEditingStarted += OnEditingStarted;

		if ( property.TryGetAttribute<PlaceholderAttribute>( out var placeholder ) )
		{
			TextEdit.PlaceholderText = placeholder.Value;
		}

		FixedHeight = Theme.RowHeight * 4;

		if ( !property.IsEditable )
		{
			ReadOnly = true;
		}
	}

	protected override void DoLayout()
	{
		base.DoLayout();

		TextEdit.Position = 0;
		TextEdit.Size = Size;
	}

	/// <summary>
	/// Change text to pink if we're editing multiple values, and they differ
	/// </summary>
	protected override void OnMultipleDifferentValues( bool state )
	{
		if ( state )
		{
			TextEdit.SetStyles( $"color: {Theme.MultipleValues.Hex}; background-color: transparent;" );
		}
		else
		{
			TextEdit.SetStyles( $"color: {Theme.TextControl.Hex}; background-color: transparent;" );
		}
	}

	protected override void OnValueChanged()
	{
		base.OnValueChanged();

		if ( !_editing )
			return;

		if ( TextEdit.IsFocused )
			return;

		TextEdit.PlainText = ValueToString();
	}

	void OnEditingStarted()
	{
		if ( _editing )
			return;

		PropertyStartEdit();

		_editing = true;
	}

	void OnEditingFinished()
	{
		if ( !_editing )
			return;

		TextEdit.PlainText = ValueToString();
		PropertyFinishEdit();

		_editing = false;
	}

	protected virtual string ValueToString() => SerializedProperty.As.String;
	protected virtual object StringToValue( string text ) => text;
}
