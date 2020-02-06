using ChangePassword.Registration;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChangePassword
{
    public class PwnedPasswords
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PwnedPasswords(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> IsPasswordBanned(string password)
        {
            var client = _httpClientFactory.CreateClient(Register_PwnedPasswords.HttpClientName);

            // Detailed here https://haveibeenpwned.com/API/v2
            // But in summary...  this uses a k-Anonymity model which means we only have to supply the API with a partial part of
            // our hashed password.  The first 5 characters in this case.  The API then returns the suffixs of any matching hashed passwords.
            // If our hashed password is not in the results then we are good.
            using (var sha1 = SHA1.Create())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
                var asHex = ByteArrayToHexString(hash);

                var prefix = asHex.Substring(0, 5);
                var suffix = asHex.Substring(5);

                var results = await client.GetStringAsync("/range/" + prefix);

                return results.Contains(suffix);
            }
        }

        private string ByteArrayToHexString(byte[] bytes)
        {
            return string.Join(string.Empty, Array.ConvertAll(bytes, b => b.ToString("X2")));
        }

    }
}
