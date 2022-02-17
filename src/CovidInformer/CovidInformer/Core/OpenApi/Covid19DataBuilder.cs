using System;
using LibraProgramming.Domain.Entities;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using CovidInformer.Core.OpenApi.Entities;
using CovidInformer.Entities;

namespace LibraProgramming.Data.OpenApi.Core
{
    internal sealed class Covid19DataBuilder
    {
        private readonly Dictionary<RegionInfo, CountryInfo> countries;
        private readonly Uri source;
        private readonly BigInteger latest;
        private readonly DateTime updated;

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
                    var latest = new BigInteger(countryInfo.Latest);

                    // countryInfo.Country
                    // countryInfo.CountryCode
                    // countryInfo.Province
                    // countryInfo.Coordinates
                    // countryInfo.Latest

                    if (false == countries.TryGetValue(region, out var ci))
                    {
                        ci = new CountryInfo(region, latest);
                        countries.Add(region, ci);
                    }
                    else
                    {
                        ci = new CountryInfo(region, ci.Total + latest);
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

            var builder = new Covid19DataBuilder(countries, confirmedCasesInfo.Source, confirmedCasesInfo.Latest, confirmedCasesInfo.Updated);

            return builder;
        }

        private Covid19DataBuilder(
            Dictionary<RegionInfo, CountryInfo> countries,
            Uri source,
            BigInteger latest,
            DateTime updated)
        {
            this.countries = countries;
            this.source = source;
            this.latest = latest;
            this.updated = updated;
        }

        public Covid19Data Build()
        {
            var collection = new List<CountryInfo>();

            foreach (var kvp in countries)
            {
                collection.Add(kvp.Value);
            }

            var data = new Covid19Data(collection.AsReadOnly(), latest, updated);

            return data;
        }
    }
}