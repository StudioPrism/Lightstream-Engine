namespace Editor;

public static partial class EditorUtility
{
	public static partial class Account
	{
		public static async ValueTask Assure()
		{
			if ( AccountInformation.Session is not null )
				return;

			await AccountInformation.Update();
		}

		public static Task Refresh() => AccountInformation.Update();

		public static bool IsMember( string org ) => AccountInformation.HasOrganization( org );

		public static IReadOnlyList<Package.Organization> Memberships => AccountInformation.Memberships;
		public static IReadOnlyList<Package> Favourites => AccountInformation.Favourites;
		public static IReadOnlyList<StreamService> ServiceLinks => AccountInformation.Links;

	}
}
