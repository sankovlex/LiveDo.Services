using System;
using Microsoft.Extensions.DependencyInjection;

namespace LiveDo.Auth.WebApp.Extensions
{
	public class ApplicationServicesBuilder : IApplicationServicesBuilder
	{
		public ApplicationServicesBuilder(IServiceCollection services)
		{
			Services = services 
				?? throw new ArgumentNullException(nameof(services));
		}

		/// <inheritdoc />
		public IServiceCollection Services { get; }
	}
}
