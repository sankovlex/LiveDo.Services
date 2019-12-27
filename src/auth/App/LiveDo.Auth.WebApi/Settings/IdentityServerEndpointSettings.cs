namespace LiveDo.Auth.WebApi.Settings
{
	public class IdentityServerEndpointSettings
	{
		private const string SettingPath = "Casebook.API/Auth/Configuration";

		/// <summary>
		/// Доступно получение токенов, например, в Implicit Flow
		/// </summary>
		public bool EnableAuthorizeEndpoint { get; set; }

		/// <summary>
		/// Доступно получение статуса сессии
		/// </summary>
		public bool EnableCheckSessionEndpoint { get; set; }

		/// <summary>
		/// Доступен логаут по инициативе пользователя
		/// </summary>
		public bool EnableEndSessionEndpoint { get; set; }

		/// <summary>
		/// Доступна возможность получения claims аутентифицированного пользователя
		/// </summary>
		public bool EnableUserInfoEndpoint { get; set; }

		/// <summary>
		/// Доступно получение метаданных в OpenId Connect
		/// </summary>
		public bool EnableDiscoveryEndpoint { get; set; }

		/// <summary>
		/// Доступно получение информации о токенах
		/// </summary>
		public bool EnableIntrospectionEndpoint { get; set; }

		/// <summary>
		/// Доступно получение access_token (можно получить через через authorization_endpoint)
		/// </summary>
		public bool EnableTokenEndpoint { get; set; }

		/// <summary>
		/// Доступно использование refresh и reference tokens
		/// </summary>
		public bool EnableTokenRevocationEndpoint { get; set; }

		/// <summary>
		/// Значение получателя X509 сертификата (только для PROD)
		/// </summary>
		public string X509CertificateIssuedTo { get; set; }
	}
}
