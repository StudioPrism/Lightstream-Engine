using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json;


#nullable enable

namespace Sandbox
{
	static partial class Json
	{
		/// <summary>
		/// Represents a JSON Pointer as defined in RFC 6901.
		/// </summary>
		[JsonConverter( typeof( PointerJsonConverter ) )]
		public class Pointer : IEquatable<Pointer>
		{
			private const string TokenSeparator = "/";

			/// <summary>
			/// The reference tokens that make up the JSON Pointer.
			/// </summary>
			public ImmutableArray<string> ReferenceTokens { get; }

			/// <summary>
			/// A static instance representing the root JSON Pointer (i.e., "/").
			/// </summary>
			public static readonly Pointer Root = new Pointer( ImmutableArray<string>.Empty );

			public bool IsRoot => ReferenceTokens.IsEmpty;

			/// <summary>
			/// Initializes a new instance of the <see cref="Pointer"/> class with the specified string.
			/// </summary>
			/// <param name="value">The string value of the JSON Pointer.</param>
			public Pointer( string value )
			{
				ReferenceTokens = ImmutableArray.CreateRange( Parse( value ) );
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="Pointer"/> class with the specified tokens.
			/// </summary>
			/// <param name="tokens">The tokens that make up the JSON Pointer.</param>
			private Pointer( ImmutableArray<string> tokens )
			{
				ReferenceTokens = tokens;
			}

			/// <summary>
			/// Appends a token to the JSON Pointer and returns a new <see cref="Pointer"/>.
			/// </summary>
			/// <param name="token">The token to append.</param>
			/// <returns>A new <see cref="Pointer"/> with the appended token.</returns>
			public Pointer Append( string token )
			{
				var newTokens = ReferenceTokens.Add( token );
				return new Pointer( newTokens );
			}

			/// <summary>
			/// Appends an integer index as a token to the JSON Pointer and returns a new <see cref="Pointer"/>.
			/// </summary>
			/// <param name="index">The integer index to append.</param>
			/// <returns>A new <see cref="Pointer"/> with the appended index.</returns>
			public Pointer Append( int index )
			{
				return Append( index.ToString( CultureInfo.InvariantCulture ) );
			}

			/// <summary>
			/// Returns a new <see cref="Pointer"/> representing the parent of the current pointer.
			/// </summary>
			/// <returns>A new <see cref="Pointer"/> for the parent path.</returns>
			public Pointer GetParent()
			{
				if ( ReferenceTokens.IsEmpty )
				{
					return this; // Already at root
				}

				var newTokens = ReferenceTokens.RemoveAt( ReferenceTokens.Length - 1 );
				return new Pointer( newTokens );
			}

			public JsonNode? Evaluate( JsonNode? document )
			{
				JsonNode? current = document;
				StringBuilder pathBuilder = new StringBuilder();

				foreach ( string referenceToken in ReferenceTokens )
				{
					string unescapedToken = Unescape( referenceToken );
					current = EvaluateToken( unescapedToken, current );

					if ( current == null )
					{
						throw new InvalidOperationException(
							$"Path '{ToString()}' is invalid at '{pathBuilder}'" );
					}

					pathBuilder.Append( TokenSeparator ).Append( Escape( referenceToken ) );
				}

				return current;
			}

			private JsonNode? EvaluateToken( string token, JsonNode? current )
			{
				if ( current is JsonObject jsonObject )
				{
					if ( jsonObject.TryGetPropertyValue( token, out JsonNode? value ) )
					{
						return value;
					}
					else
					{
						return null;
					}
				}
				else if ( current is JsonArray jsonArray )
				{
					if ( int.TryParse( token, NumberStyles.None, CultureInfo.InvariantCulture, out int index ) )
					{
						if ( index >= 0 && index < jsonArray.Count )
						{
							return jsonArray[index];
						}
					}
					return null;
				}
				else
				{
					return null;
				}
			}

			private static readonly Regex PointerRegex = new Regex(
				@"^(?:/((?:[^/~]|~0|~1)*))*$",
				RegexOptions.Compiled );

			private IEnumerable<string> Parse( string value )
			{
				if ( string.IsNullOrEmpty( value ) || value == "/" )
				{
					return Enumerable.Empty<string>();
				}

				Match match = PointerRegex.Match( value );
				if ( !match.Success )
				{
					throw new ArgumentException( $"Invalid JSON Pointer '{value}'", nameof( value ) );
				}

				return value.Split( '/' )
							.Skip( 1 ) // Skip the initial empty string before the first '/'
							.Select( Unescape );
			}

			private static string Unescape( string token )
			{
				return token.Replace( "~1", "/" ).Replace( "~0", "~" );
			}

			private static string Escape( string token )
			{
				return token.Replace( "~", "~0" ).Replace( "/", "~1" );
			}

			public bool Equals( Pointer? other )
			{
				if ( other is null ) return false;
				return ReferenceTokens.SequenceEqual( other.ReferenceTokens );
			}

			public override bool Equals( object? obj )
			{
				return obj is Pointer other && Equals( other );
			}

			public override int GetHashCode()
			{
				unchecked
				{
					return ReferenceTokens.Aggregate( 17, ( hash, token ) => hash * 31 + token.GetHashCode() );
				}
			}

			public static bool operator ==( Pointer? left, Pointer? right )
			{
				if ( left is null ) return right is null;
				return left.Equals( right );
			}

			public static bool operator !=( Pointer? left, Pointer? right )
			{
				return !(left == right);
			}

			public override string ToString()
			{
				if ( ReferenceTokens.IsEmpty )
					return TokenSeparator; // Return "/" for root

				return TokenSeparator + string.Join( TokenSeparator, ReferenceTokens.Select( Escape ) );
			}
		}

		/// <summary>
		/// Custom JSON converter for the Pointer class that serializes a Pointer as a string
		/// and deserializes a string back into a Pointer using the Parse method.
		/// </summary>
		public class PointerJsonConverter : JsonConverter<Pointer>
		{
			public override Pointer Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
			{
				if ( reader.TokenType == JsonTokenType.Null )
					return Pointer.Root;

				if ( reader.TokenType == JsonTokenType.String )
				{
					string? pointerString = reader.GetString();
					return pointerString == null ? Pointer.Root : new Pointer( pointerString );
				}

				throw new JsonException( $"Unable to convert {reader.TokenType} to Pointer" );
			}

			public override void Write( Utf8JsonWriter writer, Pointer value, JsonSerializerOptions options )
			{
				if ( value == null )
				{
					writer.WriteNullValue();
					return;
				}

				writer.WriteStringValue( value.ToString() );
			}
		}
	}
}
