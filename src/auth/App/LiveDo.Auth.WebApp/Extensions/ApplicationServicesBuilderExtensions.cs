using LiveDo.Auth.UsersDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LiveDo.Auth.WebApp.Extensions
{
	public static class ApplicationServicesBuilderExtensions
	{
		public static IApplicationServicesBuilder AddAuthDbContext(
			this IApplicationServicesBuilder builder,
			string connectionString)
		{
			builder.Services.AddDbContext<AuthDbContext>(options =>
			{
				options.UseNpgsql(connectionString);
			});

			return builder;
		}
	}
}
