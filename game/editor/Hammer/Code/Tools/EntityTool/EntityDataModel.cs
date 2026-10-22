namespace Editor.MapEditor;

public class EntityDataNode : TreeNode<MapClass>
{
	// Shit way to pass this info
	public bool PreferClassNames { get; set; }
	public bool ShowGameIcon { get; set; }

	public EntityDataNode( MapClass mapClass ) : base( mapClass )
	{
	}

	public override string GetTooltip()
	{
		List<string> extras = new();
		if ( Value.IsSolidClass )
			extras.Add( "<span style=\"color:#dd6;\">This entity requires a mesh to function.</span>" );
		else if ( Value.Tags.Contains( "SupportsSolids" ) )
			extras.Add( "<span style=\"color:#6d6;\">This entity supports being a mesh.</span>" );

		string name = $"<span style=\"font-size: 16px;font-weight: 900;\">{Value.DisplayName}</span>";
		//name += $"<br><span style=\"color:#888;text-align: right;\">{Value.Name}</span>";

		string description = (Value.Description ?? "No description given.");

		return $"{name}<br>{description}{(extras.Count > 0 ? "<br><br>" + string.Join( "<br>", extras ) : "")}";
	}

	public override void OnPaint( VirtualWidget item )
	{
		PaintSelection( item );

		var rect = item.Rect.Shrink( 0, 0, 4, 0 );

		// the indentation on the treeview doesn't work so adjust the rect ourselves
		rect.Left = 8;

		Paint.Antialiasing = true;

		Color fg = Color.White.Darken( 0.1f );

		if ( Paint.HasSelected )
		{
			fg = Color.White;
			Paint.ClearPen();
			Paint.SetBrush( Theme.Primary.WithAlpha( 0.9f ) );
			// Paint.DrawRect( rect, 2 );

			Paint.SetPen( Theme.Text );
		}
		else
		{
			Paint.SetDefaultFont();
			Paint.SetPen( Theme.Text.Darken( 0.3f ) );
		}

		// Show an icon if the entity supports being a mesh in Hammer
		if ( Value.IsSolidClass || Value.Tags.Contains( "SupportsSolids" ) )
		{
			var thumbRect = rect.Shrink( 8, 4 );
			thumbRect.Left = thumbRect.Right - thumbRect.Height;
			Paint.DrawIcon( thumbRect, Value.IsSolidClass ? "crop_free" : "center_focus_strong", 14 );
		}

		// Only makes sense to show this icon on Recent or All screens
		if ( ShowGameIcon && Value.Package != null )
		{
			var thumbRect = rect.Shrink( 8, 4 );
			thumbRect.Right -= 20;
			thumbRect.Left = thumbRect.Right - thumbRect.Height;
			Paint.Draw( thumbRect, Value.Package.Thumb );
		}

		var iconRect = rect.Shrink( 4, 4 );
		iconRect.Width = iconRect.Height;
		if ( !string.IsNullOrEmpty( Value.Icon ) )
		{
			Paint.DrawIcon( iconRect, Value.Icon, 14 );
		}
		else
		{
			Paint.DrawIcon( iconRect, Value.IsSolidClass ? "View_In_Ar" : "Control_Point", 14 );
		}

		var textRect = rect.Shrink( 4, 2 );
		textRect.Left = iconRect.Right + 6;

		Paint.SetDefaultFont();
		Paint.SetPen( fg );
		Paint.DrawText( textRect, PreferClassNames ? Value.Name : Value.DisplayName, TextFlag.LeftCenter );
	}

	public override bool OnContextMenu()
	{
		if ( !Value.IsSolidClass && !Value.Tags.Contains( "SupportsSolids" ) )
			return false;

		var menu = new ContextMenu( null );
		menu.AddOption( "Create with Block Tool", "open_in_new", () => IEntityTool.StartBlockEntityCreation( Value.Name ) );
		menu.OpenAtCursor();
		return true;
	}
}

