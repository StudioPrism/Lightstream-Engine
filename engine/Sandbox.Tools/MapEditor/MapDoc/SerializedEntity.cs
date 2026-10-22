namespace Editor.MapEditor;

public abstract class EditorContext
{
	/// <summary>
	/// The current entity we're rendering gizmos for
	/// </summary>
	public virtual EntityObject Target { get; internal set; }

	/// <summary>
	/// If the current entity we're drawing selected
	/// </summary>
	public virtual bool IsSelected { get; internal set; }

	/// <summary>
	/// All selected entities
	/// </summary>
	public HashSet<EntityObject> Selection { get; internal set; }

	/// <summary>
	/// Given a string name return the first found target
	/// </summary>
	public virtual EntityObject FindTarget( string name ) => null;

	/// <summary>
	/// Given a string name return all found targets
	/// </summary>
	public virtual EntityObject[] FindTargets( string name ) => null;


	public abstract class EntityObject : SerializedObject
	{
		public abstract Transform Transform { get; set; }
		public abstract Vector3 Position { get; set; }
		public abstract Angles Angles { get; set; }
		public abstract Vector3 Scale { get; set; }
	}
}
