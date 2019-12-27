using System.Reflection;
using IdentityServer4.Configuration;
using LiveDo.Auth.WebApi.Services;
using LiveDo.Auth.WebApi.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LiveDo.Auth.WebApi.Extensions
{
	internal static class IdentityServerExtensions
	{
		public static IIdentityServerBuilder AddIdentityServerConfigurationDbContext(
			this IIdentityServerBuilder builder,
			string connectionString)
		{
			string migrationsAssembly = typeof(Startup).GetTypeInfo()
				.Assembly
				.GetName()
				.Name;

			builder.AddConfigurationStore(configuration =>
			{
				configuration.ConfigureDbContext = context =>
				{
					context.UseNpgsql(
						connectionString,
						sql => sql.MigrationsAssembly(migrationsAssembly));
				};
			});

			return builder;
		}

		public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services, IdentityServerEndpointSettings endpointSettings)
		{
			IIdentityServerBuilder builder = services.AddIdentityServer(options =>
			{
				options.Endpoints = new EndpointsOptions
				{
					EnableAuthorizeEndpoint = endpointSettings.EnableAuthorizeEndpoint,
					EnableCheckSessionEndpoint = endpointSettings.EnableCheckSessionEndpoint,
					EnableEndSessionEndpoint = endpointSettings.EnableEndSessionEndpoint,
					EnableUserInfoEndpoint = endpointSettings.EnableUserInfoEndpoint,
					EnableDiscoveryEndpoint = endpointSettings.EnableDiscoveryEndpoint,
					EnableIntrospectionEndpoint = endpointSettings.EnableIntrospectionEndpoint,
					EnableTokenEndpoint = endpointSettings.EnableTokenEndpoint,
					EnableTokenRevocationEndpoint = endpointSettings.EnableTokenRevocationEndpoint
				};
			});

			return builder;
		}

		public static IIdentityServerBuilder AddProfileServices(this IIdentityServerBuilder services)
		{
			services.AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();
			services.AddProfileService<ProfileService>();

			return services;
		}
	}
}
