using System;
using System.Collections.Generic;

namespace Editor.ActionGraphs;

public class UndoStack
{
	private record struct Frame( string Name, string State );

	private readonly List<Frame> _stack = new();
	private int _index;

	public bool CanUndo => _index > 0;
	public bool CanRedo => _index < _stack.Count - 1;

	public string UndoName => CanUndo ? $"Undo {_stack[_index - 1].Name}" : null;
	public string RedoName => CanRedo ? $"Redo {_stack[_index].Name}" : null;

	private readonly Func<string> _captureStateFunc;

	public UndoStack( Func<string> captureStateFunc )
	{
		_captureStateFunc = captureStateFunc;
	}

	public void Push( string name )
	{
		var state = _captureStateFunc();

		if ( _index > 0 && _stack[_index - 1].State.Equals( state, StringComparison.Ordinal ) )
		{
			_stack[_index - 1] = new Frame( name, state );
			return;
		}

		if ( _index != _stack.Count )
		{
			_stack.RemoveRange( _index, _stack.Count - _index );
		}

		_stack.Add( new Frame( name, state ) );
		_index = _stack.Count;
	}

	public string Undo()
	{
		if ( !CanUndo )
		{
			throw new InvalidOperationException();
		}

		if ( _index == _stack.Count )
		{
			// Add a restore point if we're at the top of the undo stack

			_stack.Add( new Frame( null, _captureStateFunc() ) );
		}

		--_index;
		return _stack[_index].State;
	}

	public string Redo()
	{
		if ( !CanRedo )
		{
			throw new InvalidOperationException();
		}

		++_index;

		var frame = _stack[_index];

		if ( frame.Name == null && _index == _stack.Count - 1 )
		{
			// We've reached the top of the stack, get rid of the restore point

			_stack.RemoveAt( _index );
		}

		return frame.State;
	}

	public void Clear()
	{
		_stack.Clear();
		_index = 0;
	}
}
