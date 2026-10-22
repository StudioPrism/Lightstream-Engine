namespace NativeMapDoc;

internal enum SelectionConversionMethod_t
{
	SELECTION_CONVERT_NONE,
	SELECTION_CONVERT_STANDARD,
	SELECTION_CONVERT_CONNECTED,
	SELECTION_CONVERT_BOUNDRY
};

// Set of operations that may be used when applying a component to a selection set
internal enum SelectionOperation_t
{
	SELECT_OP_NONE,                 // Do not modify the set
	SELECT_OP_SET,                  // Clear the current set, making the new component the only selected component
	SELECT_OP_ADD,                  // Add the component to the current set
	SELECT_OP_REMOVE,               // Remove the component from the current set
	SELECT_OP_TOGGLE,               // Remove the component if it is in the current set or add it to the set otherwise
	SELECT_OP_SET_UNLESS_SELECTED   // If the component is in the set do nothing, otherwise clear the set and add the component
};
