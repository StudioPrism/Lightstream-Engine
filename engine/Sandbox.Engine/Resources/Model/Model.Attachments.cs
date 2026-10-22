namespace Sandbox;

public class ModelAttachments
{
	public Model Model { get; }

	List<Attachment> _all;

	public IReadOnlyList<Attachment> All => _all;

	public int Count { get; private set; }

	public ModelAttachments( Model model )
	{
		Model = model;

		_all = new();

		var count = Model.native.GetNumAttachments();

		for ( int i = 0; i < count; i++ )
		{
			var a = Model.native.GetAttachment( i );
			if ( a.IsNull )
				continue;

			_all.Add( new Attachment( Model, i, a ) );
		}

		Count = All.Count;
	}

	public class Attachment
	{
		internal Attachment( Model model, int i, CAttachment a )
		{
			Index = i;
			Name = a.m_name;
			LocalTransform = Transform.Zero;
			Model = model;

			if ( a.m_nInfluences > 0 )
			{
				// support ONE fucking bone influence
				var boneName = a.GetInfluenceName( 0 );
				var offset = a.GetInfluenceOffset( 0 );
				var rotation = a.GetInfluenceRotation( 0 );

				LocalTransform = new Transform( offset, rotation );
				Bone = model.Bones.AllBones.FirstOrDefault( x => x.IsNamed( boneName ) );
			}
		}

		public Model Model { get; }
		public int Index { get; }
		public string Name { get; }
		public Transform LocalTransform { get; }
		public BoneCollection.Bone Bone { get; set; }

		/// <summary>
		/// Bone transformed LocalTransform
		/// </summary>
		public Transform WorldTransform
		{
			get
			{
				if ( Bone is null )
					return LocalTransform;

				var tx = Model.GetBoneTransform( Bone.Index );
				return tx.ToWorld( LocalTransform );
			}
		}

		public bool IsNamed( string name )
		{
			return string.Equals( name, Name, StringComparison.OrdinalIgnoreCase );
		}
	}

	internal void Dispose()
	{
		_all.Clear();
	}

	public Attachment Get( string name )
	{
		return All.FirstOrDefault( x => x.IsNamed( name ) );
	}

	public Transform? GetTransform( string name )
	{
		var attachment = Get( name );
		if ( attachment is null ) return default;

		return attachment.WorldTransform;
	}
}

public partial class Model
{
	ModelAttachments _attachments;

	/// <summary>
	/// Access to bones of this model.
	/// </summary>
	public ModelAttachments Attachments
	{
		get
		{
			_attachments ??= new ModelAttachments( this );
			return _attachments;
		}
	}

	/// <summary>
	/// Returns amount of attachment points this model has.
	/// </summary>
	[Obsolete]
	public int AttachmentCount => native.GetNumAttachments();

	/// <summary>
	/// Retrieves attachment transform based on given attachment name.
	/// </summary>
	/// <param name="name">Name of the attachment to retrieve transform of.</param>
	/// <returns>The attachment transform, or null if attachment by given name is not found.</returns>
	[Obsolete]
	public Transform? GetAttachment( string name )
	{
		if ( native.GetAttachmentTransform( name, out var tx ) )
			return tx;

		return null;
	}

	/// <summary>
	/// Retrieves attachment transform based on given attachment index.
	/// </summary>
	/// <param name="index">>Index of the attachment to look up, starting at 0.</param>
	/// <returns>The attachment transform.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when given index exceeds range of [0,AttachmentCount-1]</exception>
	[Obsolete]
	public Transform? GetAttachment( int index )
	{
		if ( index < 0 || index >= AttachmentCount )
			throw new ArgumentOutOfRangeException( nameof( index ) );

		return GetAttachment( native.GetAttachmentNameFromIndex( index ) );
	}

	/// <summary>
	/// Returns name of an attachment at given index.
	/// </summary>
	/// <param name="index">Index of the attachment to look up, starting at 0.</param>
	/// <returns>The name of the attachment at given index.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when given index exceeds range of [0,AttachmentCount-1]</exception>
	[Obsolete]
	public string GetAttachmentName( int index )
	{
		if ( index < 0 || index >= AttachmentCount )
			throw new ArgumentOutOfRangeException( nameof( index ) );

		return native.GetAttachmentNameFromIndex( index );
	}
}
