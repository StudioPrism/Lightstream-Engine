using System.Text.Json.Nodes;

namespace Sandbox;

public sealed partial class ParticleSpriteRenderer
{
	public override int ComponentVersion => 2;

	/// <summary>
	/// v2
	/// - Use embedded Sprite resource instead of a single Texture
	/// </summary>
	[Expose, JsonUpgrader( typeof( ParticleSpriteRenderer ), 2 )]
	static void Upgrader_v2( JsonObject obj )
	{
		string pivotValue = null;
		if ( obj.TryGetPropertyValue( "Pivot", out var pivotNode ) )
		{
			pivotValue = (string)pivotNode;
		}
		if ( obj.TryGetPropertyValue( "Texture", out var textureNode ) )
		{
			var arrAnimations = new JsonArray();
			var arrFrames = new JsonArray();

			var frame = Json.SerializeAsObject( new Sprite.Frame() );
			frame["Texture"] = textureNode.DeepClone();
			arrFrames.Add( frame );

			var objAnimation = new JsonObject()
			{
				["Name"] = "Default",
				["Frames"] = arrFrames
			};
			if ( pivotNode != null )
			{
				objAnimation["Pivot"] = pivotValue;
			}
			arrAnimations.Add( objAnimation );
			var objSprite = new JsonObject()
			{
				["Animations"] = arrAnimations
			};

			obj["Sprite"] = new JsonObject()
			{
				["$compiler"] = "embed",
				["$source"] = null,
				["data"] = objSprite,
				["compiled"] = null
			};
		}
	}
}
