using System.Collections.Generic;

namespace LiveDo.Auth.WebApp.ViewModels
{
	/// <summary>
	/// Модель учетных данных пользователя
	/// </summary>
	public class LoginInputModel
	{
		/// <summary>
		/// Имя пользователя
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// Пароль
		/// </summary>
		public string Password { get; set; }

		public List<ExternalProviderViewModel> ExternalProviders { get; set; }

		/// <summary>
		/// Адрес перенаправления
		/// </summary>
		public string ReturnUrl { get; set; }
	}
}
