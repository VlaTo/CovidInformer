using CovidInformer.Core.OpenApi.Entities;
using CovidInformer.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public async Task<CovidData> DownloadDataAsync(CancellationToken cancellationToken = default)
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

                            return Map(locationsInfo);
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

        public CovidData Map(ConfirmedCasesInfo confirmedCasesInfo)
        {
            // locationsInfo.Latest
            // locationsInfo.Source
            // locationsInfo.Updated

            var formatProvider = CultureInfo.InvariantCulture;
            var countries = new Dictionary<string, CountryInfo>();

            foreach (var countryInfo in confirmedCasesInfo.Locations)
            {
                var countryCode = countryInfo.CountryCode;
                var countryName = countryInfo.Country;

                try
                {
                    // countryInfo.Country
                    // countryInfo.CountryCode
                    // countryInfo.Province
                    // countryInfo.Coordinates
                    // countryInfo.Latest

                    if (false == countries.TryGetValue(countryCode, out var ci))
                    {
                        if (false == TryGetNativeName(countryCode, out var nativeName))
                        {
                            nativeName = null;
                        }

                        countries.Add(countryCode, new CountryInfo(countryCode, countryName, nativeName, countryInfo.Latest));
                    }
                    else
                    {
                        countries[countryCode] = new CountryInfo(countryCode, countryName, ci.NativeName, ci.Total + countryInfo.Latest);
                    }

                    foreach (var kvp in countryInfo.History.Data)
                    {
                        if (false == DateTime.TryParse(kvp.Key, formatProvider, DateTimeStyles.AdjustToUniversal, out var date))
                        {
                            ;
                        }

                        // kvp.Key - Date
                        // kvp.Value.ValueKind == JsonValueKind.Number
                        // (ulong)kvp.Value.GetInt64()
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }

            return new CovidDataBuilder()
                .SetCountries(countries.Values)
                .SetSource(confirmedCasesInfo.Source)
                .SetLatestTotal(confirmedCasesInfo.Latest)
                .SetUpdateDate(confirmedCasesInfo.Updated)
                .SetOldestDate(confirmedCasesInfo.Updated)
                .SetLatestDate(confirmedCasesInfo.Updated)
                .Build();
        }

        private static bool TryGetNativeName(string regionName, out string nativeName)
        {
            try
            {
                var regionInfo = new RegionInfo(regionName);

                nativeName = regionInfo.NativeName;

                return true;
            }
            catch (ArgumentException)
            {
                nativeName = null;
                return false;
            }
        }
    }
}