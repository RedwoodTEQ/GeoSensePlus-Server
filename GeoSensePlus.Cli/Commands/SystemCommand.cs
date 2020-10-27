using CoreCmd.Attributes;
using GeoSensePlus.Cli.Commands.Shared;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GeoSensePlus.Cli.Commands
{
    [Alias("sys")]
    public class SystemCommand : CommandBase
    {
        const string urlRoot = "https://localhost:5001/system";
        HttpClient _httpClient;

        public SystemCommand()
        {
            // bypass the certificate
            _httpClient = new HttpClient(
                new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true }
            );

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "GeoSense+ CLI");
        }

        public async Task FirebaseKey()
        {
            Console.WriteLine(await _httpClient.GetStringAsync($"{urlRoot}/firebase-key"));
        }

        public async Task Version()
        {
            string url = $"{urlRoot}/version";
            Console.Write(await _httpClient.GetStringAsync(url));
        }
    }
}
