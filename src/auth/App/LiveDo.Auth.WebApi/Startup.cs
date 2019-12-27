using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4.EntityFramework.DbContexts;
using LiveDo.Auth.UsersDbContext;
using LiveDo.Auth.WebApi.Extensions;
using LiveDo.Auth.WebApi.Services;
using LiveDo.Auth.WebApi.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace LiveDo.Auth.WebApi
{
	public class Startup
	{
		private readonly IConfiguration _configuration;
		private readonly IWebHostEnvironment _environment;

		public Startup(IConfiguration configuration, IWebHostEnvironment environment)
		{
			_configuration = configuration 
				?? throw new ArgumentNullException(nameof(configuration));
			_environment = environment 
				?? throw new ArgumentNullException(nameof(environment));
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc(opt => opt.EnableEndpointRouting = false)
				.AddNewtonsoftJson()
				.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

			services.AddSwaggerGen(configuration =>
			{
				configuration.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "LiveDo Auth",
					Version = "v1"
				});

				// Set the comments path for the Swagger JSON and UI.
				string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				configuration.IncludeXmlComments(xmlPath);
			});
			
			string liveDoUsersConnectionString = _configuration.GetConnectionString("LiveDo.Auth.Users");
			services.AddDbContext<AuthDbContext>(options =>
			{
				options.UseNpgsql(liveDoUsersConnectionString);
			});

			string identityServerConfigurationConnectionString 
				= _configuration.GetConnectionString("IdentityServerConfiguration");
			
			services.AddUserServices();
			
			var identityServerSettings = _configuration
				.GetSection(nameof(IdentityServerEndpointSettings))
				.Get<IdentityServerEndpointSettings>();
			
			IIdentityServerBuilder identityServerBuilder = services
				.AddIdentityServer(identityServerSettings)
				.AddIdentityServerConfigurationDbContext(identityServerConfigurationConnectionString)
				.AddProfileServices();
			
			if (_environment.IsDevelopment())
			{
				identityServerBuilder.AddDeveloperSigningCredential();
			}
			else
			{
				X509Certificate2 certificate = 
					X509CertificateProvider.GetX509CertificateFromPersonalStoreByName(
						identityServerSettings.X509CertificateIssuedTo);

				identityServerBuilder.AddSigningCredential(certificate);
			}
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();
			app.UseSwaggerUI(configuration =>
			{
				configuration.SwaggerEndpoint(
					url: "/swagger/v1/swagger.json",
					name: "Waybit Shipment");
				configuration.RoutePrefix = "docs";
			});

			app.UseMvc();
		}
	}
}
