using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClientModel = IdentityServer4.Models.Client;
using ClientEntity = IdentityServer4.EntityFramework.Entities.Client;

namespace LiveDo.Auth.WebApp.Controllers
{
	/// <summary>
	/// Clients.
	/// </summary>
	[Route("clients")]
	public class ClientsController : Controller
	{
		private readonly IConfigurationDbContext _configurationDbContext;

		/// <inheritdoc />
		public ClientsController(IConfigurationDbContext configurationDbContext)
		{
			_configurationDbContext = configurationDbContext 
				?? throw new ArgumentNullException(nameof(configurationDbContext));
		}

		[HttpGet]
		public async Task<IActionResult> Index(CancellationToken cancellationToken)
		{
			List<ClientEntity> clients = await _configurationDbContext
				.Clients
				.ToListAsync(cancellationToken);

			List<ClientModel> viewModel = clients
				.Select(c => c.ToModel())
				.ToList();

			return View(viewModel);
		}
		
		[HttpGet("edit/{id?}")]
		public async Task<IActionResult> Client(int? id, CancellationToken cancellationToken)
		{
			if (id == null)
			{
				return View(new ClientModel());
			}
			
			ClientEntity client = await _configurationDbContext
				.Clients
				.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

			ClientModel viewModel = client.ToModel();

			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Client(ClientModel clientModel)
		{
			ClientEntity entity = clientModel.ToEntity();

			_configurationDbContext.Clients.Update(entity);
			await _configurationDbContext.SaveChangesAsync();

			return View("Index");
		}
	}
}
