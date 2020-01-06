using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using LiveDo.Auth.WebApp.Services;
using LiveDo.Auth.WebApp.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LiveDo.Auth.WebApp.Extensions
{
	internal static class IdentityServerExtensions
	{
		public static IIdentityServerBuilder AddIdentityServerConfigurationStore(
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

		public static IIdentityServerBuilder AddProfileServices(this IIdentityServerBuilder services)
		{
			services.AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();
			services.AddProfileService<ProfileService>();

			return services;
		}

		public static IIdentityServerBuilder AddX509Certificate(
			this IIdentityServerBuilder services,
			X509CertificateOptions options)
		{
			X509Certificate2 certificate = X509CertificateProvider
				.GetX509CertificateFromPersonalStoreByName(options.IssuedTo);

			services.AddSigningCredential(certificate);

			return services;
		}
	}
}
