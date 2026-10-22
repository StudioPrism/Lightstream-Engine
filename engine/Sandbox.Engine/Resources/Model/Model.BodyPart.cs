namespace Sandbox;

public class ModelParts
{
	private readonly Model _model;
	private List<Model.BodyPart> _all;

	/// <summary>
	/// All body parts in this model.
	/// </summary>
	public IReadOnlyList<Model.BodyPart> All
	{
		get
		{
			_all ??= BuildParts();
			return _all;
		}
	}

	/// <summary>
	/// How many body parts there are in this model.
	/// </summary>
	public int Count { get; private set; }

	/// <summary>
	/// Default body groups mask for this model.
	/// </summary>
	public ulong DefaultMask { get; private set; }

	private ModelParts() { }

	public ModelParts( Model model )
	{
		_model = model ?? throw new ArgumentNullException( nameof( model ) );

		Count = _model.native.GetNumBodyParts();
		DefaultMask = _model.native.GetDefaultMeshGroupMask();
	}

	private List<Model.BodyPart> BuildParts()
	{
		var list = new List<Model.BodyPart>( Count );

		for ( int i = 0; i < Count; i++ )
		{
			var meshCount = _model.native.GetNumBodyPartMeshes( i );
			var choices = new List<Model.BodyPart.Choice>( meshCount );

			for ( int m = 0; m < meshCount; m++ )
			{
				var name = _model.native.GetBodyPartMeshName( i, m );
				var mask = _model.native.GetBodyPartMeshMask( i, m );

				choices.Add( new Model.BodyPart.Choice( name, mask ) );
			}

			var part = new Model.BodyPart
			{
				Index = i,
				Name = _model.native.GetBodyPartName( i ),
				Mask = _model.native.GetBodyPartMask( i ),
				Choices = choices.AsReadOnly()
			};

			if ( string.IsNullOrWhiteSpace( part.Name ) )
				part.Name = "empty";

			list.Add( part );
		}

		return list;
	}

	internal void Dispose()
	{
		_all?.Clear();
		_all = null;
	}

	/// <summary>
	/// Get body part by name.
	/// </summary>
	public Model.BodyPart Get( string name )
	{
		return All.FirstOrDefault( x => x.Name == name );
	}
}

partial class Model
{
	ModelParts _parts;

	/// <summary>
	/// Access to body parts of this model.
	/// </summary>
	public ModelParts Parts
	{
		get
		{
			_parts ??= new ModelParts( this );
			return _parts;
		}
	}

	[Obsolete( $"Use {nameof( Parts )}" )]
	public int BodyGroupCount => Parts.Count;

	[Obsolete( $"Use {nameof( Parts )}" )]
	public ulong DefaultBodyGroupMask => Parts.DefaultMask;

	[Obsolete( $"Use {nameof( Parts )}" )]
	public IEnumerable<BodyPart> BodyParts => Parts.All;

	public sealed class BodyPart
	{
		public int Index { get; internal set; }
		public string Name { get; internal set; }
		public ulong Mask { get; internal set; }
		public IReadOnlyList<Choice> Choices { get; internal set; }

		public sealed record Choice( string Name, ulong Mask );

		internal int GetChoiceIndex( string name )
		{
			for ( int i = 0; i < Choices.Count; i++ )
			{
				if ( Choices[i].Name == name ) return i;
			}

			return -1;
		}
	}

	/// <summary>
	/// Used to mark properties as a body group mask, so the correct editor can be used
	/// </summary>
	public sealed class BodyGroupMaskAttribute : System.Attribute
	{
		public string ModelParameter { get; set; } = "Model";
	}
}
