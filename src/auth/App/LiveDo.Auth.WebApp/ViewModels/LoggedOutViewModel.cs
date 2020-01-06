namespace LiveDo.Auth.WebApp.ViewModels
{
	/// <summary>
	/// Модель отображения информации о успешном выходе
	/// </summary>
	public class LoggedOutViewModel : LogoutViewModel
	{
		/// <summary>
		/// Наименование приложения, с которого было выполнено перенаправление
		/// </summary>
		public string ClientName { get; set; }
        
		/// <summary>
		/// Адрес перенаправления на приложение
		/// </summary>
		public string PostLogoutRedirectUri { get; set; }
	}
}