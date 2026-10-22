using System.Runtime.CompilerServices;

namespace Sandbox;

public partial class Particle
{
	/// <summary>
	/// Allows creating a class that will exist for as long as a particle.
	/// The methods get called in the particle thread, which removes the need to run through
	/// the particle list again, but it has the danger and restrictions that come with threaded code.
	/// </summary>
	public abstract class BaseListener
	{
		/// <summary>
		/// The component that created this listener. May be null.
		/// </summary>
		public Component Source { get; private set; }

		internal void SetSourceComponent( Component source )
		{
			Source = source;
		}

		/// <summary>
		/// Called in a thread. The particle is in its first position.
		/// </summary>
		public abstract void OnEnabled( Particle p );

		/// <summary>
		/// Called in a thread, guarenteed to be called after OnEnabled
		/// </summary>
		public abstract void OnUpdate( Particle p, float dt );

		/// <summary>
		/// Called in a thread. OnUpdate won't be called again.
		/// </summary>
		public abstract void OnDisabled( Particle p );
	}

	List<BaseListener> _controllers;

	/// <summary>
	/// Add a listener.
	/// </summary>
	public void AddListener( BaseListener i, Component sourceComponent )
	{
		if ( sourceComponent is not null )
		{
			i.SetSourceComponent( sourceComponent );
		}

		_controllers ??= new List<BaseListener>( 1 );
		_controllers.Add( i );

		if ( hasUpdated )
		{
			i.OnEnabled( this );
			i.OnUpdate( this, 0.01f );
		}
	}

	/// <summary>
	/// Remove a listener
	/// </summary>
	public void RemoveListener( BaseListener i )
	{
		_controllers?.Remove( i );
	}

	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	internal void OnEnabled()
	{
		if ( _controllers is null ) return;

		for ( int i = 0; i < _controllers.Count; i++ )
		{
			try
			{
				_controllers[i].OnEnabled( this );
			}
			catch ( Exception e )
			{
				Log.Error( e );
			}
		}
	}

	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	internal void OnUpdate( float dt )
	{
		if ( _controllers is null ) return;

		for ( int i = 0; i < _controllers.Count; i++ )
		{
			try
			{
				_controllers[i].OnUpdate( this, dt );
			}
			catch ( Exception e )
			{
				Log.Error( e );
			}
		}
	}

	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	internal void OnDisabled()
	{
		if ( _controllers is null ) return;

		for ( int i = 0; i < _controllers.Count; i++ )
		{
			try
			{
				_controllers[i].OnDisabled( this );
			}
			catch ( Exception e )
			{
				Log.Error( e );
			}
		}

		_controllers.Clear();
	}

	/// <summary>
	/// Remove all listeners with this component set as the source. This is most commonly called when
	/// the passed component is destroyed or disabled, to remove any effects created.
	/// </summary>
	internal void DisableListenersForComponent( Component c )
	{
		if ( _controllers is null ) return;

		for ( int i = 0; i < _controllers.Count; i++ )
		{
			if ( _controllers[i].Source != c ) continue;

			try
			{
				_controllers[i].OnDisabled( this );
			}
			catch ( Exception e )
			{
				Log.Error( e );
			}

			_controllers.RemoveAt( i );
			i--;
		}
	}
}
