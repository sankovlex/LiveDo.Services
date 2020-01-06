using LiveDo.Auth.WebApp.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveDo.Auth.WebApp.Extensions
{
	public static class ApplicationServicesExtensions
	{
		public static IApplicationServicesBuilder AddApplication(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<ApplicationOptions>(configuration);

			string connectionString = configuration.GetConnectionString("LiveDo.Auth.Users");
			
			return services.AddApplicationBuilder()
				.AddAuthDbContext(connectionString);
		}
		
		private static IApplicationServicesBuilder AddApplicationBuilder(this IServiceCollection services)
		{
			return new ApplicationServicesBuilder(services);
		}
	}
}
