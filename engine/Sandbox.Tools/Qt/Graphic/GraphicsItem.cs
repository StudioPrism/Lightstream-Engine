using Native;
using System;

namespace Editor
{
	public abstract class GraphicsItem : IValid
	{
		internal Native.QGraphicsItem _graphicsitem;
		internal Native.CManagedGraphicsItem _native;

		static Dictionary<IntPtr, GraphicsItem> AllByAddress = new();

		HashSet<GraphicsItem> _children;

		public IEnumerable<GraphicsItem> Children => _children?.AsEnumerable() ?? Enumerable.Empty<GraphicsItem>();

		internal GraphicsItem()
		{
		}

		public GraphicsItem( GraphicsItem parent = null )
		{
			InteropSystem.Alloc( this );
			_native = Native.CManagedGraphicsItem.Create( parent?._graphicsitem ?? IntPtr.Zero, this );
			NativeInit( _native );

			Parent = parent;

			SetFlag( Native.GraphicsItemFlag.ItemNegativeZStacksBehindParent, true ); // ZIndex = -1 means behind parent
			SetFlag( Native.GraphicsItemFlag.ItemSendsGeometryChanges, true ); // make sure OnPositionChanged gets called
		}

		public GraphicsView GraphicsView => Scene?.View;

		GraphicsScene _scene;

		internal GraphicsScene Scene
		{
			get
			{
				if ( _scene is not null ) return _scene;
				if ( _parent is not null ) return _parent.Scene;

				return default;
			}

			set
			{
				_scene = value;
			}
		}

		GraphicsItem _parent;
		public GraphicsItem Parent
		{
			set
			{
				if ( _parent == value ) return;

				// old parent cleanup
				_parent?.RemoveChild( this );

				// change
				_parent = value;

				// new parent
				_parent?.AddChild( this );
				_graphicsitem.setParentItem( _parent?._graphicsitem ?? IntPtr.Zero );

				Scene = _parent?.Scene;
			}
			get { return _parent; }
		}

		public void Destroy()
		{
			if ( !_graphicsitem.IsValid ) return;

			OnDestroy();

			if ( Scene is GraphicsScene scene )
			{
				scene.RemoveInternal( this );

			}

			Scene = null;
			Parent = null;
			NativeShutdown();
		}

		protected virtual void OnDestroy()
		{

		}


		private void AddChild( GraphicsItem i )
		{
			_children ??= new();
			_children.Add( i );
		}

		private void RemoveChild( GraphicsItem i )
		{
			_children?.Remove( i );
		}

		bool IValid.IsValid => IsValid;
		public bool IsValid => _graphicsitem.IsValid;

		internal virtual void NativeInit( QGraphicsItem ptr )
		{
			_graphicsitem = ptr;
			AllByAddress[_graphicsitem] = this;

			EditorShortcuts.Register( this );
		}

		internal virtual void NativeShutdown()
		{
			InteropSystem.Free( this );
			EditorShortcuts.Unregister( this );

			AllByAddress.Remove( _graphicsitem );

			_graphicsitem = default;

			if ( _children == null ) return;

			foreach ( var child in _children )
			{
				child.NativeShutdown();
			}

			_children.Clear();
			_children = null;
		}

		internal static GraphicsItem Get( QGraphicsItem item )
		{
			if ( AllByAddress.TryGetValue( item, out var found ) )
				return found;

			return null;
		}

		public Vector2 Position
		{
			get => (Vector2)_graphicsitem.pos();
			set => _graphicsitem.setPos( value );
		}

		public Vector2 ViewPosition
		{
			get => GraphicsView.FromScene( ToScene( 0 ) );
		}

		public Vector2 Center
		{
			get => Position + Size * 0.5f;
		}

		public Rect LocalRect
		{
			get => new Rect( 0, Size );
		}

		public Rect SceneRect
		{
			get => new Rect( Position, Size );
			set
			{
				Position = value.Position;
				Size = value.Size;
			}
		}

		public float Rotation
		{
			get => _graphicsitem.rotation();
			set => _graphicsitem.setRotation( value );
		}

		public float Scale
		{
			get => _graphicsitem.scale();
			set => _graphicsitem.setScale( value );
		}

		public bool Movable
		{
			get => Has( Native.GraphicsItemFlag.ItemIsMovable );
			set => SetFlag( Native.GraphicsItemFlag.ItemIsMovable, value );
		}

		public bool ClipChildren
		{
			get => Has( Native.GraphicsItemFlag.ItemClipsChildrenToShape );
			set => SetFlag( Native.GraphicsItemFlag.ItemClipsChildrenToShape, value );
		}

		public bool Clip
		{
			get => Has( Native.GraphicsItemFlag.ItemClipsToShape );
			set => SetFlag( Native.GraphicsItemFlag.ItemClipsToShape, value );
		}

		string _toolTip;

		[Obsolete( $"Use {nameof( ToolTip )}" )]
		public string Tooltip
		{
			get => ToolTip;
			set => ToolTip = value;
		}

		public string ToolTip
		{
			get => _toolTip;
			set
			{
				if ( _toolTip == value )
					return;

				_toolTip = value;
				_graphicsitem.setToolTip( _toolTip );
			}
		}

		bool _selected;
		public bool Selected
		{
			get => _selected;
			set
			{
				if ( _selected == value ) return;
				_graphicsitem.setSelected( value );
			}
		}

		public bool Selectable
		{
			get => Has( Native.GraphicsItemFlag.ItemIsSelectable );
			set => SetFlag( Native.GraphicsItemFlag.ItemIsSelectable, value );
		}

		/// <summary>
		/// Gets keyboard input
		/// </summary>
		public bool Focusable
		{
			get => Has( Native.GraphicsItemFlag.ItemIsFocusable );
			set => SetFlag( Native.GraphicsItemFlag.ItemIsFocusable, value );
		}

		public bool HoverEvents
		{
			get => _graphicsitem.acceptHoverEvents();
			set => _graphicsitem.setAcceptHoverEvents( value );
		}

		public float ZIndex
		{
			get => _graphicsitem.zValue();
			set => _graphicsitem.setZValue( value );
		}

		private bool Has( GraphicsItemFlag f )
		{
			return (_graphicsitem.flags() & f) == f;
		}

		private void SetFlag( GraphicsItemFlag f, bool b )
		{
			_graphicsitem.setFlag( f, b );
		}

		internal void InternalPaint( QPainter painter, int state )
		{
			using ( Editor.Paint.Start( painter, (StateFlag)state ) )
			{
				OnPaint();
			}
		}

		protected virtual void OnPaint()
		{

		}

		internal Rect InternalGetBoundingRect()
		{
			return BoundingRect;
		}

		internal Rect InternalGetPaintRect()
		{
			return new( -HandlePosition * Size, Size );
		}

		/// <summary>
		/// The outer bounds of the item as a rectangle; all painting must be restricted to inside an item's bounding rect.
		/// </summary>
		public virtual Rect BoundingRect => new( -HandlePosition * Size, Size );

		public virtual Vector2 Size
		{
			get;
			set;
		}

		public float Width
		{
			get => Size.x;
			set => Size = Size.WithX( value );
		}

		public float Height
		{
			get => Size.y;
			set => Size = Size.WithY( value );
		}

		/// <summary>
		/// 0,0 means top left, 1,1 means bottom right
		/// </summary>
		public virtual Vector2 HandlePosition
		{
			get;
			set;
		}

		internal void InternalMouseReleaseEvent( QGraphicsSceneMouseEvent e ) => OnMouseReleased( new GraphicsMouseEvent( e ) );
		protected virtual void OnMouseReleased( GraphicsMouseEvent e )
		{

		}

		internal void InternalMousePressEvent( QGraphicsSceneMouseEvent e ) => OnMousePressed( new GraphicsMouseEvent( e ) );
		protected virtual void OnMousePressed( GraphicsMouseEvent e )
		{

		}

		internal void InternalMouseMoveEvent( QGraphicsSceneMouseEvent e ) => OnMouseMove( new GraphicsMouseEvent( e ) );
		protected virtual void OnMouseMove( GraphicsMouseEvent e )
		{

		}

		public bool Hovered { get; internal set; }

		internal void InternalHoverEnterEvent( QGraphicsSceneHoverEvent e )
		{
			Hovered = true;
			OnHoverEnter( new GraphicsHoverEvent { ptr = e } );
		}

		protected virtual void OnHoverEnter( GraphicsHoverEvent e )
		{

		}

		internal void InternalHoverMoveEvent( QGraphicsSceneHoverEvent e )
		{
			OnHoverMove( new GraphicsHoverEvent { ptr = e } );
		}

		protected virtual void OnHoverMove( GraphicsHoverEvent e )
		{

		}

		internal void InternalHoverLeaveEvent( QGraphicsSceneHoverEvent e )
		{
			Hovered = false;
			OnHoverLeave( new GraphicsHoverEvent { ptr = e } );
		}

		protected virtual void OnHoverLeave( GraphicsHoverEvent e )
		{

		}

		public void Update()
		{
			_graphicsitem.update();
		}

		/// <summary>
		/// Usually called before resizing items so they paint properly.
		/// </summary>
		public void PrepareGeometryChange()
		{
			if ( _native.IsValid )
				_native.PrepareGeometryChange();
		}

		public Vector2 ToScene( Vector2 pos ) => (Vector2)_graphicsitem.mapToScene( pos );
		public Vector2 FromScene( Vector2 pos ) => (Vector2)_graphicsitem.mapFromScene( pos );

		public Vector2 ToParent( Vector2 pos ) => (Vector2)_graphicsitem.mapToParent( pos );
		public Vector2 FromParent( Vector2 pos ) => (Vector2)_graphicsitem.mapFromParent( pos );

		public Vector2 ToItem( GraphicsItem item, Vector2 pos ) => (Vector2)_graphicsitem.mapToItem( item._graphicsitem, pos );
		public Vector2 FromItem( GraphicsItem item, Vector2 pos ) => (Vector2)_graphicsitem.mapFromItem( item._graphicsitem, pos );

		internal void InternalItemChange( int change )
		{
			switch ( (GraphicsItemChange)change )
			{
				case GraphicsItemChange.ItemPositionHasChanged:
				case GraphicsItemChange.ItemTransformHasChanged:
					{
						OnPositionChanged();
						return;
					}
				case GraphicsItemChange.ItemMoved:
					{
						OnMoved();
						return;
					}

				case GraphicsItemChange.ItemSelectedHasChanged:
					{
						OnSelectionChanged();
						return;
					}

			}
		}

		protected virtual void OnPositionChanged()
		{

		}

		/// <summary>
		/// Item has been moved by the user dragging it
		/// </summary>
		protected virtual void OnMoved()
		{

		}

		protected virtual void OnSelectionChanged()
		{
			_selected = _graphicsitem.isSelected();

			Scene?.OnItemSelectionChangedInternal( this, _selected );
		}

		CursorShape _cursor = CursorShape.None;
		public CursorShape Cursor
		{
			get => _cursor;
			set
			{
				_cursor = value;

				if ( Cursor == CursorShape.None )
				{
					_graphicsitem.unsetCursor();
					return;
				}

				_graphicsitem.setCursor( Cursor );
			}
		}

		public Sandbox.Bind.Builder Bind( string targetName )
		{
			var bb = new Sandbox.Bind.Builder
			{
				system = Sandbox.Internal.GlobalToolsNamespace.BindSystem
			};

			return bb.Set( this, targetName );
		}

		internal void InternalKeyPressEvent( QKeyEvent e ) => OnKeyPress( new KeyEvent( e ) );

		/// <summary>
		/// A key has been pressed.
		/// </summary>
		protected virtual void OnKeyPress( KeyEvent e )
		{

		}

		internal void InternalKeyReleaseEvent( QKeyEvent e ) => OnKeyRelease( new KeyEvent( e ) );

		/// <summary>
		/// A key has been released.
		/// </summary>
		protected virtual void OnKeyRelease( KeyEvent e )
		{

		}

		public virtual bool Contains( Vector2 localPos )
		{
			return true;
		}

	}

}



namespace Native
{
	enum GraphicsItemFlag
	{
		ItemIsMovable = 0x1,
		ItemIsSelectable = 0x2,
		ItemIsFocusable = 0x4,
		ItemClipsToShape = 0x8,
		ItemClipsChildrenToShape = 0x10,
		ItemIgnoresTransformations = 0x20,
		ItemIgnoresParentOpacity = 0x40,
		ItemDoesntPropagateOpacityToChildren = 0x80,
		ItemStacksBehindParent = 0x100,
		ItemUsesExtendedStyleOption = 0x200,
		ItemHasNoContents = 0x400,
		ItemSendsGeometryChanges = 0x800,
		ItemAcceptsInputMethod = 0x1000,
		ItemNegativeZStacksBehindParent = 0x2000,
		ItemIsPanel = 0x4000,
		ItemIsFocusScope = 0x8000, // internal
		ItemSendsScenePositionChanges = 0x10000,
		ItemStopsClickFocusPropagation = 0x20000,
		ItemStopsFocusHandling = 0x40000,
		ItemContainsChildrenInShape = 0x80000
		// NB! Don't forget to increase the d_ptr->flags bit field by 1 when adding a new flag.
	};

	enum GraphicsItemChange
	{
		ItemPositionChange,

		ItemVisibleChange = 2,
		ItemEnabledChange,
		ItemSelectedChange,
		ItemParentChange,
		ItemChildAddedChange,
		ItemChildRemovedChange,
		ItemTransformChange,
		ItemPositionHasChanged,
		ItemTransformHasChanged,
		ItemSceneChange,
		ItemVisibleHasChanged,
		ItemEnabledHasChanged,
		ItemSelectedHasChanged,
		ItemParentHasChanged,
		ItemSceneHasChanged,
		ItemCursorChange,
		ItemCursorHasChanged,
		ItemToolTipChange,
		ItemToolTipHasChanged,
		ItemFlagsChange,
		ItemFlagsHaveChanged,
		ItemZValueChange,
		ItemZValueHasChanged,
		ItemOpacityChange,
		ItemOpacityHasChanged,
		ItemScenePositionHasChanged,
		ItemRotationChange,
		ItemRotationHasChanged,
		ItemScaleChange,
		ItemScaleHasChanged,
		ItemTransformOriginPointChange,
		ItemTransformOriginPointHasChanged,
		ItemMoved
	}
}
