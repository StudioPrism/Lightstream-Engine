using Sandbox.MovieMaker;

namespace Editor.MovieMaker;

#nullable enable

[CustomEditor( typeof( IMovieResource ) )]
public sealed class MovieResourceControlWidget : ResourceControlWidget
{
	public MovieResourceControlWidget( SerializedProperty property )
		: base( new WrapperSerializedProperty( property ) )
	{

	}
}

file sealed class WrapperSerializedProperty( SerializedProperty inner ) : SerializedProperty
{
	public override Type PropertyType => typeof( MovieResource );
	public override SerializedObject Parent => inner.Parent;
	public override string Name => inner.Name;
	public override bool IsValid => inner.IsValid;
	public override string DisplayName => inner.DisplayName;
	public override string Description => inner.Description;
	public override bool HasChanges => inner.HasChanges;
	public override string GroupName => inner.GroupName;
	public override int Order => inner.Order;

	public override bool IsEditable =>
		inner.GetValue<IMovieResource>() is not EmbeddedMovieResource;

	public override void SetValue<T>( T value )
	{
		inner.SetValue( value );
	}

	public override T GetValue<T>( T defaultValue = default! )
	{
		return inner.GetValue<IMovieResource>() is T resource
			? resource
			: default!;
	}
}
