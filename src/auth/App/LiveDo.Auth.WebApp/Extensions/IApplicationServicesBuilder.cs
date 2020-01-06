using Microsoft.Extensions.DependencyInjection;

namespace LiveDo.Auth.WebApp.Extensions
{
	public interface IApplicationServicesBuilder
	{
		IServiceCollection Services { get; }
	}
}
