using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facepunch.ShaderProc
{
	class CppPacker
	{
		private string CppCode;

		private const string EncryptionKey = "AAAAC3NzaC1lZDI1NTE5AAAAIJwmXmv+290eSl+0j87MoGXzlI/hIUeFgxKKU10xe/0t";
		private const bool EncryptPack = true;

		public CppPacker( List<ShaderCode> codeList )
		{
			string output = "";

			output += GetHeader();

			// Shader builder functions
			List<string> processedShaders = new List<string>();
			foreach ( var code in codeList )
			{
				string shaderName = code.ShaderName.Replace( "_", "" ).Replace( ".fxc", "" );
				output += "//---------------------------------------------------------------\n";
				output += $"//{code.ShaderName}\n";
				output += AddShaderFunction( $"Name_{shaderName}", SymmetricEncryptDecrypt( code.ShaderName ), code.ShaderName.Length );
				output += AddShaderFunction( $"Data_{shaderName}", SymmetricEncryptDecrypt( code.ToString() ), code.ToString().Length );
				processedShaders.Add( shaderName );
			}

			// Body
			output += "const EmbeddedShader_t g_pEmbeddedShaders[] = \n{";

			foreach ( var shader in processedShaders )
			{
				output += "	";
				output += "{ ";
				output += $" Name_{shader}(), ";
				output += $" Data_{shader}() }},\n";
			}

			// Close body
			output += "\n}; \n";

			CppCode = output;
		}

		private string AddShaderFunction( string name, string data, int count = 0 )
		{
			string output = "";

			output += $"const CBufferString {name}()\n" +
						"{\n" +
						"	CBufferString o;\n";
			/*foreach( var chunk in BreakIntoChunks(data).Split("\n") )
			{
				output += $"	o.Put( \"{chunk}\", {chunk.Length/4} );\n";
			}*/
			output += $"	const uint8_t data[] = {data};\n";

			//If Decrypt
			output += $"	o = SymmetricEncryptDecrypt( data, {count} );\n";

			output += $"	return o;\n" +
						"}\n\n";

			return output;
		}

		public override string ToString()
		{
			return CppCode;
		}

		internal string GetHeader()
		{
			string output = "";
			output += "#pragma once\n\n";

			//We pass the decryption key so it's synched
			output += "constexpr const char* GetShaderDecryptionKey()\n" +
						"{\n" +
						$"	return \"{EncryptionKey}\";\n" +
						"} \n\n";

			//Pass the decryption function
			output += "// Simple XOR symmetrical Encryption/Decryption method\n" +
						"CBufferString SymmetricEncryptDecrypt( const uint8_t* toEncrypt, size_t count ) {\n" +
						"	constexpr const char* key = GetShaderDecryptionKey();\n" +
						"	const size_t len = strlen( key );\n" +
						"	CBufferString output;\n" +
						"	for ( int i = 0; i < count; i++ )\n" +
						"		output.AppendChar( ((char)(toEncrypt[i] ^ key[ i % len])) );\n" +
						"	return output;\n" +
						"}\n";

			//Pass the struct
			output += "struct EmbeddedShader_t \n" +
						"{ \n" +
						"	const CBufferString pszName;\n" +
						"	const CBufferString pszData;\n" +
						"}; \n";
			return output;
		}

		public byte[] Compress( byte[] data )
		{
			//Prepare for compress
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			System.IO.Compression.GZipStream sw = new System.IO.Compression.GZipStream( ms, System.IO.Compression.CompressionMode.Compress );

			//Compress
			sw.Write( data, 0, data.Length );
			//Close, DO NOT FLUSH cause bytes will go missing...
			sw.Close();

			return ms.ToArray();
		}

		/// <summary>
		/// Break the string into adjacent chunks
		/// Fix for error C2026 in MSVC
		/// </summary>
		private string BreakIntoChunks( string input )
		{
			const int splitEvery = 4096;

			int cursor = splitEvery;

			string output = input;

			while ( cursor < input.Length )
			{
				//Trail a little bit
				while ( output[cursor] != '\\' )
					cursor++;

				output = output.Insert( cursor, "\n" );
				cursor += splitEvery;
			}
			return output;
		}
		internal string ByteToFormattedString( byte[] input )
		{
			return "{ 0x" + BitConverter.ToString( input ).Replace( "-", ", 0x" ) + " }";
		}
		internal string SymmetricEncryptDecrypt( string toEncrypt )
		{
			return ByteToFormattedString( SymmetricEncryptDecrypt( Encoding.UTF8.GetBytes( toEncrypt ) ) );
		}

		internal byte[] SymmetricEncryptDecrypt( byte[] toEncrypt )
		{
			const string key = EncryptionKey;

			byte[] output = new byte[toEncrypt.Length];

			for ( int i = 0; i < toEncrypt.Length; i++ )
				output[i] = (byte)(toEncrypt[i] ^ key[i % key.Length]);

			return output;
		}
	}
}
