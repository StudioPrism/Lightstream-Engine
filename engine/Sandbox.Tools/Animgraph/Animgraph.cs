using Native;

namespace Editor;

internal static class Animgraph
{
	private class ModelPicker : Widget
	{
		private NativeAnimgraph.CQAnimGraphPreviewDockWidget parent;

		public Model Model
		{
			get
			{
				var path = parent.GetPreviewModel();
				return string.IsNullOrWhiteSpace( path ) ? null : Model.Load( path );
			}
			set => parent.SetPreviewModel( value?.ResourcePath );
		}

		public ModelPicker( NativeAnimgraph.CQAnimGraphPreviewDockWidget parent )
		{
			this.parent = parent;

			Layout = Layout.Column();

			MinimumSize = 40;
			MaximumHeight = MinimumHeight;

			var so = this.GetSerialized();
			var property = so.GetProperty( nameof( Model ) );

			var type = EditorTypeLibrary.GetType<ControlWidget>( "ResourceControlWidget" );
			var widget = Layout.Add( type.Create<ControlWidget>( new[] { property } ) );
			widget.HorizontalSizeMode = SizeMode.Flexible;
			widget.MinimumWidth = 0;
		}
	}

	internal static QWidget CreateModelPicker( NativeAnimgraph.CQAnimGraphPreviewDockWidget parent )
	{
		var w = new ModelPicker( parent );
		return w._widget;
	}
}
