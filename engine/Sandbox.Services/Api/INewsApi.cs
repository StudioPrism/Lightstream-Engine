using Refit;

namespace Sandbox.Services;

public partial class ServiceApi
{
	public interface INewsApi
	{
		[Get( "/news/platform" )]
		Task<NewsPostDto[]> GetPlatformNews( int take, int skip );

		[Get( "/news/package/{package}" )]
		Task<NewsPostDto[]> GetPackageNews( string package, int take, int skip );

		[Get( "/news/organization/{org}" )]
		Task<NewsPostDto[]> GetOrganizationNews( string org, int take, int skip );

		[Get( "/news" )]
		Task<NewsPostDto[]> GetNews( int take, int skip );
	}
}
