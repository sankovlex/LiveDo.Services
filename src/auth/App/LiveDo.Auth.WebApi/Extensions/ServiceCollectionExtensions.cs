using System.Reflection;
using IdentityServer4.Configuration;
using LiveDo.Auth.WebApi.Services;
using LiveDo.Auth.WebApi.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LiveDo.Auth.WebApi.Extensions
{
	/// <summary>
	/// Методы расширения для контейнера Identity Server 4
	/// </summary>
	internal static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Добавляет Identity Server 4 с указанной Consul конфигурацией
		/// </summary>
		/// <param name="services">Контейнер</param>
		/// <param name="settings">Consul конфигурация</param>
		public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services, IdentityServerSettings settings)
		{
			IIdentityServerBuilder builder = services.AddIdentityServer(options =>
			{
				options.Endpoints = new EndpointsOptions
				{
					EnableAuthorizeEndpoint = settings.EnableAuthorizeEndpoint,
					EnableCheckSessionEndpoint = settings.EnableCheckSessionEndpoint,
					EnableEndSessionEndpoint = settings.EnableEndSessionEndpoint,
					EnableUserInfoEndpoint = settings.EnableUserInfoEndpoint,
					EnableDiscoveryEndpoint = settings.EnableDiscoveryEndpoint,
					EnableIntrospectionEndpoint = settings.EnableIntrospectionEndpoint,
					EnableTokenEndpoint = settings.EnableTokenEndpoint,
					EnableTokenRevocationEndpoint = settings.EnableTokenRevocationEndpoint
				};
			});

			// Resource owner password dependencies
			builder.AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();
			builder.AddProfileService<ProfileService>();

			return builder;
		}

		/// <summary>
		/// Добавляет конфигурацию с использованием хранилища EF
		/// </summary>
		/// <param name="builder">Контейнер</param>
		/// <param name="connectionString">Строка подключения к хранилищу</param>
		/// <returns></returns>
		public static IIdentityServerBuilder AddEfConfigurationStore(this IIdentityServerBuilder builder, string connectionString)
		{
			string migrationsAssembly = typeof(Startup).GetTypeInfo()
				.Assembly
				.GetName()
				.Name;
			
			builder.AddConfigurationStore(options =>
			{
				options.ConfigureDbContext = context =>
				{
					context.UseNpgsql(
						connectionString,
						sql => sql.MigrationsAssembly(migrationsAssembly));
				};
			});

			return builder;
		}
	}
}
