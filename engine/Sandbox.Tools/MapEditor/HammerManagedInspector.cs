using Editor.MapDoc;
using Native;

namespace Editor.MapEditor;

public class HammerManagedInspector : Widget
{
	public HammerManagedInspector( Widget parent ) : base( parent )
	{
		Layout = Layout.Column();
	}

	// TODO: We could handle an array of mapnodes easy
	public bool Inspect( MapNode mapNode )
	{
		using var sx = SuspendUpdates.For( this );

		Layout.Clear( true );

		if ( mapNode is null )
			return false;

		// Maybe [Inspector] wrappers for MapNodes would be good
		if ( mapNode is not MapGameObject mgo || mgo.GameObject is null )
			return false;

		var serialized = mgo.GameObject.GetSerialized();
		serialized.OnPropertyChanged += p => OnChange( mgo, p );

		var inspector = InspectorWidget.Create( serialized );
		if ( !inspector.IsValid() )
			return false;

		Layout.Add( inspector, 1 );
		return true;
	}

	/// <summary>
	/// Let Hammer know when shit changes so we can update misc stuff.
	/// We mark the map as edited in a few places, but this seems even more reliable.
	/// </summary>
	private void OnChange( MapGameObject node, SerializedProperty property )
	{
		if ( node is null ) return;
		node.native.SetModifiedFlag();

		// Emit EventMapNodeDescriptionChanged_t if name changes, for native outliner
		if ( property.Parent.TypeName == nameof( GameObject ) && property.Name == nameof( GameObject.Name ) )
		{
			node.native.DescriptionChanged();
		}
	}

	// Native integration
	internal static HammerManagedInspector Create( QWidget parent ) => new HammerManagedInspector( new Widget( parent ) );
	internal QWidget GetWidget() => _widget;
}
