using System;
using System.Security.Cryptography.X509Certificates;

namespace LiveDo.Auth.WebApp.Services
{
	/// <summary>
	/// Предоставляет доступ к хранилищу X509 сертификатов
	/// </summary>
	internal static class X509CertificateProvider
	{
		/// <summary>
		/// Возвращает X509 сертификат по имени из персонального хранилища на локальной машине
		/// </summary>
		/// <param name="name">Наименование субъекта кому выдан сертификат (Issued To)</param>
		/// <exception cref="InvalidOperationException">
		/// Возникает при нарушении однозначности поиска по имени
		/// </exception>
		public static X509Certificate2 GetX509CertificateFromPersonalStoreByName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(
					message: "Value cannot be null or empty.", 
					paramName: nameof(name));
			}

			using (var x509Store = new X509Store(
				StoreName.My,
				StoreLocation.LocalMachine,
				OpenFlags.ReadOnly))
			{
				X509Certificate2Collection certificates = x509Store.Certificates;

				X509Certificate2Collection target = certificates.Find(
					X509FindType.FindBySubjectName, 
					name,
					true);
				
			
				if (target.Count > 1)
				{
					throw new InvalidOperationException(
						$"Certificates store contains multiple certificates with this name: {name}");
				}

				if (target.Count == 0)
				{
					return null;
				}

				return target[0];
			}
		}
	}
}