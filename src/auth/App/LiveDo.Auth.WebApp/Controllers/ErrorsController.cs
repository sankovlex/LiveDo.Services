using System;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using LiveDo.Auth.WebApp.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace LiveDo.Auth.WebApp.Controllers
{
	public class ErrorsController : Controller
	{
		private readonly IIdentityServerInteractionService _interaction;
		private readonly IWebHostEnvironment _environment;

		/// <inheritdoc />
		public ErrorsController(
			IIdentityServerInteractionService interaction,
			IWebHostEnvironment environment)
		{
			_interaction = interaction 
				?? throw new ArgumentNullException(nameof(interaction));
			_environment = environment 
				?? throw new ArgumentNullException(nameof(environment));
		}
		
		/// <summary>
		/// Возвращает ошибку сервера аутентификации
		/// </summary>
		/// <param name="errorId">Идентификатор ошибки</param>
		public async Task<IActionResult> Error(string errorId)
		{
			var errorViewModel = new ErrorViewModel();

			ErrorMessage message = await _interaction.GetErrorContextAsync(errorId);

			if (message != null)
			{
				errorViewModel.Error = message.Error;
				errorViewModel.RequestId = message.RequestId;
				errorViewModel.UiLocales = message.UiLocales;
				errorViewModel.DisplayMode = message.DisplayMode;
				errorViewModel.RedirectUri = message.RedirectUri;
				errorViewModel.ResponseMode = message.ResponseMode;

				if (_environment.IsDevelopment())
				{
					errorViewModel.ErrorDescription = message.ErrorDescription;
				}
			}

			return View("Error", errorViewModel);
		}
	}
}
