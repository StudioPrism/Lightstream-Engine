using Native;
using System;
using System.Reflection;
using Sandbox;
using System.Text.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Editor;

//
// Methods to convert MapClass to and from native GDclass.
//

public partial class MapClass
{
	internal CGameDataClass ToNative( CGameDataClass gdclass )
	{
		// Clear out existing inputs, outputs and helpers
		gdclass.Reset();

		gdclass.SetName( Name );
		gdclass.SetDescription( Description );
		gdclass.SetClassType( ClassType );

		// TODO: Wank. Hammer can't make it's mind up on which one it wants to use.
		if ( ClassType == GameDataClassType.GenericPointClass )
			gdclass.AddTag( "point" );
		else if ( ClassType == GameDataClassType.GenericSolidClass )
			gdclass.AddTag( "solid" );

		// Add variables ( it has an overwrite mechanic natively )
		if ( Variables != null )
		{
			foreach ( var variable in Variables )
			{
				var gdvar = GameData.NativeGameData.AllocateVar(); // Gives ownership to the right C++ class so it's always cleaned properly.
				variable.ToNative( gdvar );

				gdclass.AddVariable( gdvar );
			}
		}

		foreach ( var tag in Tags ) gdclass.AddTag( tag );
		foreach ( var helper in EditorHelpers )
		{
			var cHelper = CHelperInfo.Create();
			cHelper.SetName( helper.Item1 );

			if ( helper.Item2.Length == 1 && helper.Item2[0] != null && helper.Item2[0].StartsWith( '{' ) )
			{
				// KV3 parameters

				string kv3data = helper.Item2[0];
				var kv3params = NativeEngine.EngineGlue.LoadKeyValues3( kv3data );

				if ( kv3params.IsValid ) cHelper.SetKV3ParameterData( kv3params );
			}
			else
			{
				foreach ( var parameter in helper.Item2 ) cHelper.AddParameter( parameter );
			}
			gdclass.AddHelper( cHelper );
		}

		foreach ( var input in Inputs )
		{
			var cInput = CClassInput.Create();
			cInput.SetName( input.Name );
			cInput.SetDescription( input.Description );
			cInput.SetType( input.Type );

			gdclass.AddInput( cInput );
		}

		foreach ( var output in Outputs )
		{
			var cOutput = CClassOutput.Create();
			cOutput.SetName( output.Name );
			cOutput.SetDescription( output.Description );
			cOutput.SetType( output.Type );

			gdclass.AddOutput( cOutput );
		}

		// Bit of a hack, but nice and simple.
		var json = JsonSerializer.Serialize( Metadata );
		var kv3 = NativeEngine.EngineGlue.JsonToKeyValues3( json );
		gdclass.MergeKV3Metadata( kv3 );

		return gdclass;
	}

	static internal MapClass FromNative( Native.CGameDataClass gdclass )
	{
		//
		// This won't be needed in the future, so we just grab the minimum we need.
		// All entities should be at least defined in C# somehow in the future.
		//

		var mapClass = new MapClass( gdclass.GetName() );
		mapClass.Description = gdclass.GetDescription();
		mapClass.ClassType = gdclass.GetClassType();

		// These are our only native entities, give them alright names
		mapClass.DisplayName = mapClass.Name switch
		{
			"func_nav_markup" => "Nav Markup",
			"worldspawn" => "World",
			"path_model" => "Model Path",
			"path_trajectory" => "Trajectory Path",
			"path_particle_rope" => "Particle Rope",
			"cable_static" => "Static Cable",
			"cable_dynamic" => "Dynamic Cable",
			_ => mapClass.Name
		};

		int count = gdclass.GetInputCount();
		for ( int i = 0; i < count; i++ )
		{
			var input = gdclass.GetInput( i );
			mapClass.Inputs.Add( InputOutputBase.FromNative( input ) as Input );
		}

		int oCount = gdclass.GetOutputCount();
		for ( int i = 0; i < oCount; i++ )
		{
			var output = gdclass.GetOutput( i );
			mapClass.Outputs.Add( InputOutputBase.FromNative( output ) as Output );
		}

		return mapClass;
	}
}
