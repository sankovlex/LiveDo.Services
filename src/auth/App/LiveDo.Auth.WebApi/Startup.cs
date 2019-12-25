using System;
using System.IO;
using System.Reflection;
using LiveDo.Auth.UsersDbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace LiveDo.Auth.WebApi
{
	public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration 
				?? throw new ArgumentNullException(nameof(configuration));
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
					Title = "Waybit Shipment",
					Version = "v1"
				});

				// Set the comments path for the Swagger JSON and UI.
				string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				configuration.IncludeXmlComments(xmlPath);
			});
			
			string connectionString = _configuration.GetConnectionString("LiveDo.Auth.Users");
			services.AddDbContext<AuthDbContext>(options =>
			{
				options.UseNpgsql(connectionString);
			});
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
