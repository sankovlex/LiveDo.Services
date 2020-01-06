using FluentValidation;
using LiveDo.Auth.WebApp.ViewModels;

namespace LiveDo.Auth.WebApp.Validators
{
	/// <summary>
	/// Валидация данных о входе
	/// </summary>
	public class LoginInputModelValidator : AbstractValidator<LoginInputModel>
	{
		/// <inheritdoc />
		public LoginInputModelValidator()
		{
			RuleFor(p => p.Username)
				.NotEmpty()
				.WithMessage("Имя пользователя обязательно.");

			RuleFor(p => p.Password)
				.NotEmpty()
				.WithMessage("Пароль обязателен.");
		}
	}
}
