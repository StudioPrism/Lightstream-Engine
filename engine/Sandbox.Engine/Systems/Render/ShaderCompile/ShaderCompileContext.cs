namespace Sandbox.Engine.Shaders;

/// <summary>
/// Passed to shader compiles to provide a shared context between the compiles.
/// This provides the source code to the compile, but it also gives an opportunity
/// for the threaded, individual compiles, to share and cache information between them.
/// </summary>
class ShaderCompileContext : IDisposable
{
	private IShaderCompileContext _native;
	private string _maskedSource;

	internal ShaderCompileContext( IShaderCompileContext shaderCompileContext )
	{
		_native = shaderCompileContext;
	}

	~ShaderCompileContext()
	{
		_native.Delete();
		_native = default;
	}

	public void Dispose()
	{
		_native.Delete();
		_native = default;

		GC.SuppressFinalize( this );
	}

	internal IShaderCompileContext GetNative() => _native;

	public string MaskedSource
	{
		get => _maskedSource;
		set
		{
			_maskedSource = value;
			_native.SetMaskedCode( _maskedSource );
		}
	}
}
