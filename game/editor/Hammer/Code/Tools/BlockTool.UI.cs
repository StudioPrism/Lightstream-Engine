using Editor.MeshEditor;

namespace Editor.MapEditor;

partial class BlockTool : IBlockTool
{
	Widget Widget { get; set; }
	StatusWidget Header { get; set; }
	Layout Properties { get; set; }

	ComboBox GeometryComboBox { get; set; }

	SerializedObject SerializedObject;

	private void OnPropertiesChanged( SerializedProperty property )
	{
		IBlockTool.UpdateTool();
	}

	public Widget BuildUI()
	{
		Widget = new Widget( null );
		Widget.Layout = Layout.Column();
		Widget.Layout.Margin = 4;

		// Status
		{
			Header = new StatusWidget( Widget );
			UpdateStatus();
			Widget.Layout.Add( Header );
		}

		Widget.Layout.AddSpacingCell( 8 );

		// Selector
		{
			var hLayout = Widget.Layout.AddRow();
			hLayout.Spacing = 4;

			var label = new Label( "Geometry Type" );
			GeometryComboBox = new ComboBox();

			foreach ( var builder in GetBuilderTypes().OrderBy( x => x.Name ) )
			{
				GeometryComboBox.AddItem( builder.Title, builder.Icon ?? "square", () => Current = builder.Create<PrimitiveBuilder>(), null, builder.TargetType == Current.GetType() );
			}

			hLayout.Add( label );
			hLayout.Add( GeometryComboBox, 1 );
		}

		Widget.Layout.AddSpacingCell( 8 );

		// Control sheet
		{
			Properties = Layout.Column();

			var sheet = new ControlSheet();
			SerializedObject = Current.GetSerialized();
			SerializedObject.OnPropertyChanged += OnPropertiesChanged;
			sheet.AddObject( SerializedObject );

			Properties.Add( sheet );
			Properties.AddStretchCell();

			Widget.Layout.Add( Properties );
		}

		Widget.Layout.AddSpacingCell( 8 );

		var checkBox = new Checkbox( "Orient to the 2D view plane direction.", "flip" );
		checkBox.State = IBlockTool.OrientPrimitives ? CheckState.On : CheckState.Off;
		checkBox.StateChanged += ( CheckState state ) => IBlockTool.OrientPrimitives = state == CheckState.On;

		Widget.Layout.Add( checkBox, 1 );

		Widget.Layout.AddStretchCell();

		return Widget;
	}

	private void RefreshCombos()
	{
		foreach ( var builder in GetBuilderTypes() )
		{
			// if ( GeometryComboBox.)
			//
			// var displayInfo = DisplayInfo.ForType( builder );
			// GeometryComboBox.AddItem( displayInfo.Name, displayInfo.Icon ?? "square", () => Current = (PrimitiveBuilder)Activator.CreateInstance( builder ) );
		}
	}

	private void UpdateStatus()
	{
		//
		// Block Tool has been given the responsibility for creating another solid entity
		// this can either be from the entity tool or navmesh tool.
		//
		if ( !string.IsNullOrEmpty( EntityOverride ) )
		{
			Header.Text = InProgress ? $"Placing {EntityOverride}" : $"Create {EntityOverride}";
			Header.LeadText = InProgress ? "Press Enter to complete the entity." : "Drag out a rectangle to create the entity.";
			Header.Color = Color.Lerp( Theme.Yellow, Theme.Red, 0.25f ).Darken( 0.1f );
		}
		else
		{
			Header.Text = $"{(InProgress ? "Placing" : "Create")} Geometry";
			Header.LeadText = InProgress ? "Press Enter to complete the geometry." : "Drag out a rectangle to create the geometry.";
			Header.Color = InProgress ? Theme.Blue : Theme.Green;
		}

		Header.Icon = InProgress ? "check_circle_outline" : "view_in_ar";
		Header.Update();
	}
}

internal class StatusWidget : Widget
{
	public string Icon { get; set; }
	public string Text { get; set; }
	public string LeadText { get; set; }
	public Color Color { get; set; }

	public StatusWidget( Widget parent ) : base( parent )
	{
		MinimumSize = 48;
		SetSizeMode( SizeMode.Default, SizeMode.CanShrink );
	}

	protected override void OnPaint()
	{
		var rect = new Rect( 0, Size );

		Paint.ClearPen();
		Paint.SetBrush( Theme.ControlBackground.Lighten( 0.9f ) );
		Paint.DrawRect( rect );

		rect.Left += 8;

		Paint.SetPen( Color );
		var iconRect = Paint.DrawIcon( rect, Icon, 24, TextFlag.LeftCenter );

		rect.Top += 8;
		rect.Left = iconRect.Right + 8;

		Paint.SetPen( Color );
		Paint.SetDefaultFont( 10, 500 );
		var titleRect = Paint.DrawText( rect, Text, TextFlag.LeftTop );

		rect.Top = titleRect.Bottom + 2;

		Paint.SetPen( Color.WithAlpha( 0.6f ) );
		Paint.SetDefaultFont( 8, 400 );
		Paint.DrawText( rect, LeadText, TextFlag.LeftTop );
	}
}
