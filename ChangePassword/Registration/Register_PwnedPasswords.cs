using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace ChangePassword.Registration
{
    public static class Register_PwnedPasswords
    {
        public const string HttpClientName = "PwnedPasswords";

        public static void AddPwnedPasswords(this IServiceCollection services)
        {
            services.AddHttpClient(HttpClientName, client => ConfigureClient(client));
            services.AddSingleton<PwnedPasswords>();
        }

        internal static HttpClient CreateClient()
        {
            var client = new HttpClient();
            ConfigureClient(client);

            return client;
        }

        private static void ConfigureClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://api.pwnedpasswords.com");
        }
    }
}
