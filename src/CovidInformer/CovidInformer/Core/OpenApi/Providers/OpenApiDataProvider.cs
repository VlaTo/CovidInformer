using CovidInformer.Core.OpenApi.Entities;
using CovidInformer.Entities;
using LibraProgramming.Data.OpenApi.Core;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CovidInformer.Core.OpenApi.Providers
{
    public class OpenApiDataProvider
    {
        private readonly HttpClient httpClient;

        public OpenApiDataProvider()
        {
            httpClient = new HttpClient();
        }

        public async Task<Covid19Data> DownloadDataAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var uri = new Uri("https://coronavirus-tracker-api.herokuapp.com/confirmed");
                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                request.Headers.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true
                };

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                request.Headers.Connection.Add("keep-alive");

                using (var response = await httpClient.SendAsync(request, cancellationToken))
                {
                    using (var message = response.EnsureSuccessStatusCode())
                    {
                        using (var stream = await message.Content.ReadAsStreamAsync())
                        {
                            var locationsInfo = await JsonSerializer.DeserializeAsync<ConfirmedCasesInfo>(stream, cancellationToken: cancellationToken);

                            if (null == locationsInfo)
                            {

                                return null;
                            }

                            var builder = Covid19DataBuilder.From(locationsInfo);

                            return builder.Build();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}