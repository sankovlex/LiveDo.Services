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
	[ApiController]
	[Route("clients")]
	public class ClientsController : ControllerBase
	{
		private readonly IConfigurationDbContext _configurationDbContext;

		/// <inheritdoc />
		public ClientsController(IConfigurationDbContext configurationDbContext)
		{
			_configurationDbContext = configurationDbContext 
				?? throw new ArgumentNullException(nameof(configurationDbContext));
		}

		/// <summary>
		/// Returns all clients.
		/// </summary>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <response code="200">Success.</response>
		[HttpGet]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClientModel[]))]
		public async Task<IActionResult> Get(CancellationToken cancellationToken)
		{
			List<ClientEntity> clients = await _configurationDbContext
				.Clients
				.ToListAsync(cancellationToken);

			return base.Ok(clients.Select(c => c.ToModel()));
		}

		/// <summary>
		/// Returns client by id.
		/// </summary>
		/// <param name="id">Client id.</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <response code="200">Success.</response>
		/// <response code="204">Client not found.</response>
		[HttpGet("{id}")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClientModel))]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
		{
			ClientEntity client = await _configurationDbContext
				.Clients
				.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

			return base.Ok(client.ToModel());
		}

		/// <summary>
		/// Create client.
		/// </summary>
		/// <param name="clientModel">Client info.</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <response code="201">Created.</response>
		/// <response code="409">Client cannot was created.</response>
		[HttpPost]
		[Produces("application/json")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<IActionResult> Post(ClientModel clientModel, CancellationToken cancellationToken)
		{
			ClientEntity entity = clientModel.ToEntity();

			await _configurationDbContext.Clients.AddAsync(entity, cancellationToken);

			int isSaved = await _configurationDbContext.SaveChangesAsync();

			if (isSaved == 0)
			{
				return base.Conflict();
			}

			return base.CreatedAtAction(nameof(Get), entity.ToModel());
		}

		/// <summary>
		/// Update client.
		/// </summary>
		/// <param name="id">Client id.</param>
		/// <param name="clientModel">Client info.</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <response code="202">Accepted.</response>
		/// <response code="404">Client resource not found.</response>
		/// <response code="409">Client cannot was created.</response>
		[HttpPut("{id}")]
		[Produces("application/json")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<IActionResult> Put(
			int id,
			ClientModel clientModel,
			CancellationToken cancellationToken)
		{
			ClientEntity entity = await _configurationDbContext
				.Clients
				.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

			if (entity == null)
			{
				return base.NotFound();
			}
			
			_configurationDbContext.Clients
				.Attach(entity)
				.CurrentValues
				.SetValues(clientModel.ToEntity());

			int isSaved = await _configurationDbContext.SaveChangesAsync();
			
			if (isSaved == 0)
			{
				return base.Conflict();
			}

			return base.Accepted(Url.Action(nameof(Get), id), entity.ToModel());
		}
	}
}
