namespace LiveDo.Auth.WebApp.ViewModels
{
	/// <summary>
	/// Модель ошибки
	/// </summary>
	public class ErrorViewModel
	{
		/// <summary>
		/// Режим отображения подходящий под запрос
		/// </summary>
		public string DisplayMode { get; set; }

		/// <summary>
		/// Локаль UI подходящая под запрос
		/// </summary>
		public string UiLocales { get; set; }

		/// <summary>
		/// Код ошибки
		/// </summary>
		public string Error { get; set; }

		/// <summary>
		/// Подробное описание ошибки
		/// </summary>
		public string ErrorDescription { get; set; }

		/// <summary>
		/// Идентификатор запроса для диагностики
		/// </summary>
		public string RequestId { get; set; }

		/// <summary>
		/// Адрес перенаправления, после получения ошибки
		/// </summary>
		public string RedirectUri { get; set; }
		
		/// <summary>
		/// Режим ответа
		/// </summary>
		public string ResponseMode { get; set; }
	}
}
