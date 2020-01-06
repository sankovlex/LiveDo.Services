using System.Threading.Tasks;
using IdentityServer4.Stores;

namespace LiveDo.Auth.WebApp.Extensions
{
	internal static class ClientStoreExtensions
	{
		public static async Task<bool> IsPkceClientAsync(this IClientStore store, string client_id)
		{
			if (!string.IsNullOrWhiteSpace(client_id))
			{
				var client = await store.FindEnabledClientByIdAsync(client_id);
				return client?.RequirePkce == true;
			}

			return false;
		}
	}
}
