using System;
using System.IO;

namespace Editor;

/// <summary>
/// An editable text box with a button to browse for an arbitrary folder using OS file browser dialog.
/// </summary>
public class FolderEdit : LineEdit
{
	/// <summary>
	/// Path to the user selected folder.
	/// </summary>
	public Action<string> FolderSelected;

	/// <summary>
	/// Title override for the "browse folder" dialog.
	/// </summary>
	public string DialogTitle { get; set; } = "Find Folder";

	public FolderEdit( Widget parent ) : base( parent )
	{
		MinimumSize = Theme.RowHeight;
		MaximumSize = new Vector2( 4096, Theme.RowHeight );

		AddOptionToEnd( new Option( "Browse For Folder", "folder", Browse ) );
	}

	/// <summary>
	/// Open a "browse folder" dialog.
	/// </summary>
	public void Browse()
	{
		var fd = new FileDialog( null )
		{
			Directory = Path.GetDirectoryName( Text ),
			Title = DialogTitle
		};

		fd.SetFindDirectory();

		if ( fd.Execute() )
		{
			Text = fd.SelectedFile;
			FolderSelected?.Invoke( Text );
		}
	}

	protected override void OnMouseEnter()
	{
		base.OnMouseEnter();
		Update();
	}

	protected override void OnMouseLeave()
	{
		base.OnMouseLeave();
		Update();
	}

	protected override void OnPaint()
	{
		base.OnPaint();
	}

	public override void OnDragHover( DragEvent ev )
	{
		if ( !ev.Data.HasFileOrFolder )
			return;

		ev.Action = DropAction.Link;
	}

	public override void OnDragDrop( DragEvent ev )
	{
		if ( !ev.Data.HasFileOrFolder )
			return;

		// TODO - trim filename if it's a file?

		Text = ev.Data.FileOrFolder;
		FolderSelected?.Invoke( Text );

		ev.Action = DropAction.Link;
	}

}
