using NativeEngine;

namespace Sandbox;

internal class ShaderProgram
{
	Shader shader;
	ShaderProgramType programtype;

	public ShaderProgramType ProgramType => programtype;

	public ShaderProgram( Shader shader, ShaderProgramType program )
	{
		this.shader = shader;
		this.programtype = program;
	}

	CVfxProgramData GetData()
	{
		return this.shader.native.GetProgramData( programtype );
	}

	public bool WasLoadedFromCompiled => GetData().m_bLoadedFromVcsFile;

	public record struct Combo( ulong Static, ulong Dynamic );

	internal IEnumerable<Combo> EnumerateCombos( ShaderProgramType program )
	{
		var iter = shader.native.GetIterator( program );
		try
		{
			for ( ulong s = iter.FirstStaticCombo(); s != iter.InvalidIndex(); s = iter.NextStaticCombo() )
			{
				for ( ulong d = iter.FirstDynamicCombo(); d != iter.InvalidIndex(); d = iter.NextDynamicCombo() )
				{
					yield return new Combo( s, d );
				}
			}
		}
		finally
		{
			iter.Delete();
		}

	}


}
