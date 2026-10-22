
internal enum KeyValues3Type_t : byte
{
	KEYVALUES3_TYPE_INVALID,

	KEYVALUES3_TYPE_NULL,           // no value

	KEYVALUES3_TYPE_BOOL,           // boolean - true or false
	KEYVALUES3_TYPE_INT64,          // signed integer (default int)
	KEYVALUES3_TYPE_UINT64,         // uinteger that needs 64 bits of storage
	KEYVALUES3_TYPE_DOUBLE,         // float or double

	KEYVALUES3_TYPE_STRING,         // utf8 string
	KEYVALUES3_TYPE_BINARY_BLOB,    // raw bytes

	KEYVALUES3_TYPE_ARRAY,          // Ordered list of KV3 values
	KEYVALUES3_TYPE_TABLE,          // Ordered list of (string,KV3) members

	KEYVALUES3_TYPE_MAX,
}
