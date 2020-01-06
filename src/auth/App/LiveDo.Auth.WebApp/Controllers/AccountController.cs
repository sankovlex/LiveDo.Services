using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Services;
using LiveDo.Auth.Domain.Users;
using LiveDo.Auth.WebApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LiveDo.Auth.WebApp.Controllers
{
	/// <summary>
	/// Аккаунт пользователя
	/// </summary>
	public class AccountController : Controller
	{
		private readonly IIdentityServerInteractionService _interactionService;
		private readonly IOptions<IdentityServerOptions> _options;
		private readonly UserManager<User> _userManager;
		private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;

		/// <inheritdoc />
		public AccountController(
			IIdentityServerInteractionService interactionService,
			IOptions<IdentityServerOptions> options,
			UserManager<User> userManager,
			IAuthenticationSchemeProvider authenticationSchemeProvider)
		{
			_interactionService = interactionService 
				?? throw new ArgumentNullException(nameof(interactionService));
			_options = options 
				?? throw new ArgumentNullException(nameof(options));
			_userManager = userManager 
				?? throw new ArgumentNullException(nameof(userManager));
			_authenticationSchemeProvider = authenticationSchemeProvider 
				?? throw new ArgumentNullException(nameof(authenticationSchemeProvider));
		}

		/// <summary>
		/// Возвращает представление начальной страницы
		/// </summary>
		[HttpGet]
		[Authorize]
		public IActionResult Index()
		{
			var user = new UserViewModel()
			{
				Username = User.Identity.Name
			};

			return View(user);
		}

		/// <summary>
		/// Возвращает форму с авторизацией
		/// </summary>
		/// <param name="returnUrl">URL для перенаправления на исходное приложение</param>
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Login(string returnUrl)
		{
			List<ExternalProviderViewModel> providers = (await _authenticationSchemeProvider.GetAllSchemesAsync())
				.Where(schema => schema.DisplayName != null)
				.Select(schema => new ExternalProviderViewModel
				{
					DisplayName = schema.DisplayName,
					SchemeName = schema.Name
				})
				.ToList();
			
			var login = new LoginInputModel
			{
				ReturnUrl = returnUrl,
				ExternalProviders = providers
			};

			return View(login);
		}

		/// <summary>
		/// Авторизует пользователя с указанными данными
		/// </summary>
		/// <param name="model">Данные пользователя</param>
		/// <param name="cancellationToken">Токен отмены</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[AllowAnonymous]
		public async Task<IActionResult> Login(
			[FromForm] LoginInputModel model,
			CancellationToken cancellationToken)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			User user = await _userManager.FindByNameAsync(model.Username);

			if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
			{
				ModelState.AddModelError(string.Empty, "Invalid user credentials.");
				return View(model);
			}

			var properties = new AuthenticationProperties
			{
				IsPersistent = true,
				ExpiresUtc = DateTimeOffset.UtcNow.Add(
					_options.Value.Authentication.CookieLifetime)
			};

			IList<Claim> claims = await _userManager.GetClaimsAsync(user);
			
			await HttpContext.SignInAsync(
				subject: user.Id,
				name: user.Email,
				properties: properties,
				claims: claims.ToArray());

			return this.Redirect(!string.IsNullOrEmpty(model.ReturnUrl) &&
				_interactionService.IsValidReturnUrl(model.ReturnUrl)
					? model.ReturnUrl
					: "~/");
		}

		/// <summary>
		/// Возвращает представление с формой выхода
		/// </summary>
		/// <param name="logoutId">
		/// Идентификатор выхода. Содержит информацию о приложении,
		/// с которого был запрошен выход
		/// </param>
		[HttpGet]
		[Authorize]
		public IActionResult Logout(string logoutId)
		{
			var logout = new LogoutViewModel
			{
				LogoutId = logoutId
			};

			return View(logout);
		}

		/// <summary>
		/// Выполняет выход по указанным данным
		/// </summary>
		/// <param name="logoutInputModel">Информация о выходе</param>
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Logout(
			LogoutInputModel logoutInputModel)
		{
			if (logoutInputModel.LogoutId == null)
			{
				await HttpContext.SignOutAsync();
				return this.RedirectToAction("Login");
			}

			LogoutRequest context = await _interactionService
				.GetLogoutContextAsync(logoutInputModel.LogoutId);

			var loggedOut = new LoggedOutViewModel
			{
				PostLogoutRedirectUri = context.PostLogoutRedirectUri,
				LogoutId = logoutInputModel.LogoutId,
				ClientName = context.ClientName ?? context.ClientId
			};

			await HttpContext.SignOutAsync();

			return View("LoggedOut", loggedOut);
		}
	}
}
