using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using CovidInformer.Core.OpenApi.Entities;
using CovidInformer.Entities;

namespace CovidInformer.Core.OpenApi
{
    internal sealed class Covid19DataBuilder
    {
        private IEnumerable<CountryInfo> countries;
        private Uri source;
        private ulong latest;
        private DateTime updated;

        public static Covid19DataBuilder From(ConfirmedCasesInfo confirmedCasesInfo)
        {
            // locationsInfo.Latest
            // locationsInfo.Source
            // locationsInfo.Updated

            var formatProvider = CultureInfo.InvariantCulture;
            var countries = new Dictionary<RegionInfo, CountryInfo>();

            foreach (var countryInfo in confirmedCasesInfo.Locations)
            {
                try
                {
                    var region = new RegionInfo(countryInfo.CountryCode);
                    // var latest = new BigInteger(countryInfo.Latest);

                    // countryInfo.Country
                    // countryInfo.CountryCode
                    // countryInfo.Province
                    // countryInfo.Coordinates
                    // countryInfo.Latest

                    if (false == countries.TryGetValue(region, out var ci))
                    {
                        ci = new CountryInfo(region, countryInfo.Latest);
                        countries.Add(region, ci);
                    }
                    else
                    {
                        ci = new CountryInfo(region, ci.Total + countryInfo.Latest);
                        countries[region] = ci;
                    }
                    
                    var history = new List<KeyValuePair<DateTime, BigInteger>>();

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
            
            var builder = new Covid19DataBuilder()
                .SetCountries(countries.Values)
                .SetSource(confirmedCasesInfo.Source)
                .SetLatest(confirmedCasesInfo.Latest)
                .SetUpdated(confirmedCasesInfo.Updated);

            return builder;
        }

        public Covid19DataBuilder SetCountries(IEnumerable<CountryInfo> value)
        {
            countries = value ?? Enumerable.Empty<CountryInfo>();
            return this;
        }

        public Covid19DataBuilder SetLatest(ulong value)
        {
            latest = value;
            return this;
        }

        public Covid19DataBuilder SetUpdated(DateTime value)
        {
            updated = value;
            return this;
        }

        public Covid19DataBuilder SetSource(Uri value)
        {
            source = value;
            return this;
        }

        public Covid19Data Build()
        {
            var data = new Covid19Data(countries.ToArray(), latest, updated);
            return data;
        }
    }
}