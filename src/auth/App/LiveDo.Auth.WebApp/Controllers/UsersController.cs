using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using LiveDo.Auth.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LiveDo.Auth.WebApp.Controllers
{
	[Route("users")]
	public class UsersController : Controller
	{
		private readonly UserManager<User> _userManager;

		/// <inheritdoc />
		public UsersController(UserManager<User> userManager)
		{
			_userManager = userManager 
				?? throw new ArgumentNullException(nameof(userManager));
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var input = new UserInputModel();

			return View(input);
		}

		[HttpPost]
		public async Task<IActionResult> Register(
			UserInputModel userInputModel,
			CancellationToken cancellationToken)
		{
			if (userInputModel.Password != userInputModel.ConfirmPassword)
			{
				ModelState.AddModelError(
					"ConfirmPassword", 
					$"Password and confirm password is incorrect.");

				return View("Index", userInputModel);
			}

			bool isExisted = await _userManager
				.Users
				.AnyAsync(u => u.Email == userInputModel.Email, cancellationToken);
			
			if (isExisted)
			{
				ModelState.AddModelError(
					"Email", 
					$"Email {userInputModel.Email} already exists.");

				return View("Index", userInputModel);
			}

			var user = new User
			{
				UserName = userInputModel.Email,
				Email = userInputModel.Email
			};

			await _userManager.AddPasswordAsync(user, userInputModel.Password);
			await _userManager.CreateAsync(user);

			return Redirect(userInputModel.RedirectUrl ?? Url.Action("Index", "Account"));
		}
	}

	public class UserInputModel
	{
		[EmailAddress]
		public string Email { get; set; }

		[DataType(DataType.Password)]
		public string Password { get; set; }
		
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		[HiddenInput]
		[DisplayName("")]
		public string RedirectUrl { get; set; }
	}
}
