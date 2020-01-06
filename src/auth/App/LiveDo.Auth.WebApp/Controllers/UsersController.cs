using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiveDo.Auth.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LiveDo.Auth.WebApp.Controllers
{
	[ApiController]
	[Route("users")]
	public class UsersController : ControllerBase
	{
		private readonly UserManager<User> _userManager;

		/// <inheritdoc />
		public UsersController(UserManager<User> userManager)
		{
			_userManager = userManager 
				?? throw new ArgumentNullException(nameof(userManager));
		}

		[HttpPost]
		public async Task<IActionResult> Post(
			UserInputModel userInputModel,
			CancellationToken cancellationToken)
		{
			bool isExisted = await _userManager
				.Users
				.AnyAsync(u => u.Email == userInputModel.Email, cancellationToken);

			if (isExisted)
			{
				ModelState.AddModelError(
					"Email", 
					$"Email {userInputModel.Email} already exists.");
				
				return base.BadRequest(new ValidationProblemDetails(ModelState));
			}

			var user = new User
			{
				UserName = userInputModel.Email,
				Email = userInputModel.Email
			};

			await _userManager.AddPasswordAsync(user, userInputModel.Password);

			await _userManager.CreateAsync(user);

			return base.Ok();
		}
	}

	public class UserInputModel
	{
		public string Email { get; set; }

		public string Password { get; set; }
	}
}
