namespace Sandbox.Engine.Shaders;

/// <summary>
/// The results of compiling a single combo
/// </summary>
class CompiledCombo
{
	private VfxCompiledShaderInfo_t _native;
	private ProgramSource program;
	private ulong staticCombo;
	private ulong dynamicCombo;

	public ulong StaticCombo => staticCombo;
	public ulong DynamicCombo => dynamicCombo;
	public ShaderProgramType ProgramType => program.ProgramType;

	public bool IsSuccess => !_native.compileFailed;
	public string CompilerOutput => _native.compilerOutput;

	public CompiledCombo( VfxCompiledShaderInfo_t result, ProgramSource program, ulong staticCombo, ulong dynamicCombo )
	{
		_native = result;
		this.program = program;
		this.staticCombo = staticCombo;
		this.dynamicCombo = dynamicCombo;
	}

	public VfxCompiledShaderInfo_t GetResult() => _native;

	~CompiledCombo()
	{
		_native.Delete();
		_native = default;
	}
}
