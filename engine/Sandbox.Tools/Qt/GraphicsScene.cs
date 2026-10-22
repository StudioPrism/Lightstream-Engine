using System;

namespace Editor
{

	internal class GraphicsScene : QObject
	{
		internal GraphicsView View;

		internal Native.QGraphicsScene _scene;
		Dictionary<IntPtr, GraphicsItem> itemMap = new();
		public IEnumerable<GraphicsItem> Items => itemMap.Values;
		HashSet<GraphicsItem> Selection = new();
		public IEnumerable<GraphicsItem> SelectedItems => Selection.AsEnumerable();
		public Action OnSelectionChanged { get; set; }

		internal GraphicsScene( IntPtr widget )
		{
			NativeInit( widget );
		}

		internal GraphicsScene( GraphicsView parent )
		{
			View = parent;
			Sandbox.InteropSystem.Alloc( this );
			var ptr = ManagedGraphicsScene.Create( parent._object, this );
			NativeInit( ptr );
		}

		internal override void NativeInit( IntPtr ptr )
		{
			_scene = ptr;

			base.NativeInit( ptr );
		}

		internal override void NativeShutdown()
		{
			_scene = default;

			Sandbox.InteropSystem.Free( this );
			base.NativeShutdown();

			foreach ( var item in itemMap )
			{
				item.Value.NativeShutdown();
			}

			itemMap.Clear();
		}

		public void Add( GraphicsItem t )
		{
			t.Scene = this;
			_scene.addItem( t._graphicsitem );
			itemMap[t._graphicsitem] = t;
		}

		internal void RemoveInternal( GraphicsItem item )
		{
			_scene.removeItem( item._graphicsitem );
			itemMap.Remove( item._graphicsitem );
			Selection.RemoveWhere( x => x == item || !x.IsValid );
		}

		public GraphicsWidget Add( Widget t )
		{
			var pw = new GraphicsWidget( t );
			Add( pw );
			return pw;
		}

		internal void OnItemSelectionChangedInternal( GraphicsItem v, bool selected )
		{
			if ( selected )
			{
				if ( !Selection.Add( v ) )
					return;
			}
			else
			{
				if ( !Selection.Remove( v ) )
					return;
			}

			OnSelectionChanged?.Invoke();
		}
	}
}
