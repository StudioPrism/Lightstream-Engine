namespace Editor.MapEditor.EntityDefinitions;

/// <summary>
/// A place to leave notes
/// </summary>
[Library( "info_notepad" )]
[EditorSprite( "editor/info_notepad.vmat" )]
[HammerEntity]
[Title( "Comment" ), Icon( "sticky_note_2" )]
class CommentEntity : HammerEntityDefinition
{
	[Property, FGDType( "text_block" )]
	public string Message { get; set; }
}
