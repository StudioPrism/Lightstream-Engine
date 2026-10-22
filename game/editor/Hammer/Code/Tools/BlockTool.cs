using Editor.MeshEditor;
using System.Reflection;

namespace Editor.MapEditor;

/// <summary>
/// Block tool in Hammer, implements an interface called from native.. As our API matures this can be entirely C# and not need any native interface.
/// Any primitives derived from <see cref="PrimitiveBuilder"/> are automatically added to the list.
/// </summary>
partial class BlockTool : IBlockTool
{
	[Event( "hammer.initialized" )]
	public static void Initialize()
	{
		IBlockTool.Instance = new BlockTool();
	}

	PrimitiveBuilder _current = new BlockPrimitive();
	public PrimitiveBuilder Current
	{
		get => _current;
		set
		{
			_current = value;

			if ( SerializedObject is not null )
				SerializedObject.OnPropertyChanged -= OnPropertiesChanged;

			if ( !Properties.IsValid() )
				return;

			Properties.Clear( true );

			var sheet = new ControlSheet();
			SerializedObject = Current.GetSerialized();
			SerializedObject.OnPropertyChanged += OnPropertiesChanged;
			sheet.AddObject( SerializedObject );

			Properties.Add( sheet );
			Properties.AddStretchCell();

			IBlockTool.UpdateTool();
		}
	}

	bool _inProgress;
	public bool InProgress
	{
		get => _inProgress;
		set
		{
			_inProgress = value;
			UpdateStatus();
		}
	}

	string _entityOverride;
	public string EntityOverride
	{
		get => _entityOverride;
		set
		{
			_entityOverride = value;
			UpdateStatus();
		}
	}

	IEnumerable<TypeDescription> GetBuilderTypes() => EditorTypeLibrary.GetTypes<PrimitiveBuilder>().Where( t => !t.IsAbstract );
	public void Update() => IBlockTool.UpdateTool();
}
