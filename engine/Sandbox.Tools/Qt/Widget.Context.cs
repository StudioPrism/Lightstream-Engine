namespace Editor
{
	public partial class Widget : QObject
	{
		private Dictionary<string, object> contexts;

		/// <summary>
		/// Set a context value on this widget. This context will be available to its children via FindContext.
		/// </summary>
		public void SetContext( string key, object value )
		{
			contexts ??= new Dictionary<string, object>();
			contexts[key] = value;
		}

		/// <summary>
		/// Remove a context on this widget. This will NOT remove contexts set from parent objects.
		/// </summary>
		/// <param name="key"></param>
		public void ClearContext( string key )
		{
			contexts?.Remove( key );
		}

		/// <summary>
		/// Find a context on this widget. If not found, look at the parent. If not found, look at the parent.
		/// This is useful for passing information down to child widgets without any effort.
		/// </summary>
		public T GetContext<T>( string key, T defaultIfMissing = default )
		{
			if ( contexts != null && contexts.TryGetValue( key, out object value ) && value is T t )
				return t;

			if ( Parent == null )
				return defaultIfMissing;

			return Parent.GetContext( key, defaultIfMissing );
		}
	}

}
