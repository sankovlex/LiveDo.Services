using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveDo.Auth.UsersDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace LiveDo.Auth.DbContext.Migrations2
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSerilog()
				.ConfigureServices((hostContext, services) =>
				{
					IConfiguration configuration = hostContext.Configuration;
					string connectionString = configuration.GetConnectionString("LiveDo.Auth.Users");
					
					var loggerFactory = new LoggerFactory();
					loggerFactory.CreateLogger(typeof(Program));
					
					services.AddDbContext<AuthDbContext>(options =>
					{
						options.UseNpgsql(connectionString);
						options.UseLoggerFactory(loggerFactory);
					});
					services.AddHostedService<Worker>();
				})
				.ConfigureLogging(logging => { 
					logging
						.AddConsole()
						.AddDebug();
				})
				.ConfigureAppConfiguration(config =>
				{
					config.AddUserSecrets("dotnet-LiveDo.Auth.DbContext.Migrations2-B3AC3F1B-0B25-42BA-AEE7-64EC2EC73DAD");
				});
	}
}
