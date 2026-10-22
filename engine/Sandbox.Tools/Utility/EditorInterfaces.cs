namespace Editor;

/// <summary>
/// An popup editor capable of editing type T. This is created when EditorUtility.OpenControlSheet is called, 
/// which is generally and most commonly done via the GenericControlWidget, which is a ControlWidget used
/// to view and edit generic classes.
/// </summary>
public interface IPopupEditor<T>
{

}
