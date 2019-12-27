using LiveDo.Auth.Domain.Users;
using LiveDo.Auth.Domain.Users.Services;
using LiveDo.Auth.Domain.Users.Services.Password;
using LiveDo.Auth.Repositories.Users;
using LiveDo.Auth.UsersDbContext;
using Microsoft.Extensions.DependencyInjection;

namespace LiveDo.Auth.WebApi.Extensions
{
	internal static class UserServicesExtensions
	{
		public static void AddUserServices(this IServiceCollection services)
		{
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddSingleton<IPasswordHasher, PasswordHasher>();
		}
	}
}
