using Facepunch.ActionGraphs;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Sandbox.ActionGraphs;

/// <summary>
/// Some game logic implemented using visual scripting
/// </summary>
[AssetType( Name = "Action Graph", Extension = "action", Category = "Action Graph", Flags = AssetTypeFlags.NoEmbedding )]
public sealed class ActionGraphResource : GameResource
{
	static ActionGraphResource()
	{
		GraphLoader.OnLoadGraph = path => ResourceLibrary.Get<ActionGraphResource>( path )?.Graph;
	}

	[Hide, JsonIgnore]
	public DisplayInfo DisplayInfo
	{
		get
		{
			if ( Graph == null )
			{
				return new DisplayInfo { Name = "Null" };
			}

			return new DisplayInfo
			{
				Name = Graph.Title ?? "Unnamed Graph",
				Description = Graph.Description,
				Group = Graph.Category,
				Icon = Graph.Icon ?? "account_tree",
				Tags = Graph.Tags
			};
		}
	}

	// Defer actually deserializing the graph until needed, in case types aren't loaded yet

	private ActionGraph _graph;
	private JsonNode _serializedGraph;

	[Hide, JsonPropertyName( "Graph" )]
	public JsonNode SerializedGraph
	{
		get
		{
			if ( _serializedGraph is not null || _graph is null )
			{
				return _serializedGraph;
			}

			using var optionsScope = PushSerializationScope();

			return _serializedGraph = JsonSerializer.SerializeToNode( _graph, Json.options );
		}
		set
		{
			_serializedGraph = value;
			_graph = null;
		}
	}

	[Hide, JsonIgnore]
	public ActionGraph Graph
	{
		get
		{
			if ( _graph is not null )
			{
				return _graph;
			}

			using var optionsScope = PushSerializationScope();

			return _graph = _serializedGraph?.Deserialize<ActionGraph>( Json.options );
		}
		set
		{
			_graph = value;
			_serializedGraph = null;
		}
	}

	[JsonIgnore]
	public string Title
	{
		get => Graph?.Title;
		set => Graph.Title = value;
	}

	[JsonIgnore]
	public string Description
	{
		get => Graph?.Description;
		set => Graph.Description = value;
	}

	[JsonIgnore]
	public string Category
	{
		get => Graph?.Category;
		set => Graph.Category = value;
	}

	[JsonIgnore]
	public string Icon
	{
		get => Graph?.Icon;
		set => Graph.Icon = value;
	}

	[Hide, JsonIgnore]
	protected override Type ActionGraphTargetType => typeof( GameObject );

	[Hide, JsonIgnore]
	protected override object ActionGraphTarget => null;

	protected override Bitmap CreateAssetTypeIcon( int width, int height )
	{
		return CreateSimpleAssetTypeIcon( "account_tree", width, height );
	}
}
