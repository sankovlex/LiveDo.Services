using System;
using System.IO;
using System.Reflection;
using LiveDo.Auth.Domain.Users;
using LiveDo.Auth.UsersDbContext;
using LiveDo.Auth.WebApp.Extensions;
using LiveDo.Auth.WebApp.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace LiveDo.Auth.WebApp
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
			services.AddMvc(opt =>
				{
					opt.EnableEndpointRouting = false;
				})
				.AddControllersAsServices()
				.AddRazorRuntimeCompilation()
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

			string connectionString = _configuration.GetConnectionString("ConfigurationStore");
			
			services.AddApplication(_configuration);

			services.AddIdentity<User, IdentityRole>()
				.AddEntityFrameworkStores<AuthDbContext>()
				.AddDefaultTokenProviders();
			
			IIdentityServerBuilder identityServerBuilder = services
				.AddIdentityServer(_configuration)
				.AddIdentityServerConfigurationStore(connectionString)
				.AddProfileServices();
			
			if (_environment.IsDevelopment())
			{
				identityServerBuilder.AddDeveloperSigningCredential();
			}
			else
			{
				var options = _configuration.Get<X509CertificateOptions>();
				identityServerBuilder.AddX509Certificate(options);
			}

			services.AddAuthentication()
				.AddFacebook(options =>
				{
					options.AppId = _configuration["authentication:facebook:app-id"];
					options.AppSecret = _configuration["authentication:facebook:app-secret"];
				});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles();
			
			app.UseSwagger();
			app.UseSwaggerUI(configuration =>
			{
				configuration.SwaggerEndpoint(
					url: "/swagger/v1/swagger.json",
					name: "LiveDo Auth");
				configuration.RoutePrefix = "docs";
			});

			app.UseIdentityServer();
			app.UseAuthorization();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					"default",
					"{controller=Account}/{action=Index}");
			});
		}
	}
}
