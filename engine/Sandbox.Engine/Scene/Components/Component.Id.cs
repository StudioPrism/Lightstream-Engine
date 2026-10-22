namespace Sandbox;

public partial class Component
{
	private Guid _id = Guid.NewGuid();
	public Guid Id
	{
		get => _id;
		private set
		{
			if ( _id == value ) return;

			var oldId = _id;
			_id = value;

			Scene?.Directory?.Add( this, oldId );
		}
	}

	/// <summary>
	/// Should only be called by <see cref="GameObjectDirectory.Add( Component )"/>.
	/// </summary>
	internal void ForceChangeId( Guid guid )
	{
		_id = guid;
	}

	/// <summary>
	/// Allows overriding the ID of this object. This should be used sparingly, and only when necessary.
	/// This is generally used for network reasons, to make something deterministic.
	/// </summary>
	internal void SetDeterministicId( Guid id )
	{
		Id = id;
	}
}
