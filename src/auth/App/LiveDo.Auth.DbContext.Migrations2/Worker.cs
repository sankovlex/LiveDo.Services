using System;
using System.Threading;
using System.Threading.Tasks;
using LiveDo.Auth.UsersDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LiveDo.Auth.DbContext.Migrations2
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly AuthDbContext _authDbContext;

		public Worker(ILogger<Worker> logger, AuthDbContext authDbContext)
		{
			_logger = logger;
			_authDbContext = authDbContext 
				?? throw new ArgumentNullException(nameof(authDbContext));
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_authDbContext.Database.SetCommandTimeout(200);
			_authDbContext.Database.EnsureCreated();
			
			await base.StopAsync(stoppingToken);
		}
	}
}
