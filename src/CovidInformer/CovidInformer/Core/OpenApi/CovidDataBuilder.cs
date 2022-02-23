using CovidInformer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CovidInformer.Core.OpenApi
{
    internal sealed class CovidDataBuilder
    {
        private IEnumerable<CountryInfo> countries;
        private Uri source;
        private ulong latestTotal;
        private DateTime updateDate;
        private DateTime oldestDate;
        private DateTime latestDate;

        public CovidDataBuilder SetCountries(IEnumerable<CountryInfo> value)
        {
            countries = value ?? Enumerable.Empty<CountryInfo>();
            return this;
        }

        public CovidDataBuilder SetLatestTotal(ulong value)
        {
            latestTotal = value;
            return this;
        }

        public CovidDataBuilder SetUpdateDate(DateTime value)
        {
            updateDate = value;
            return this;
        }

        public CovidDataBuilder SetOldestDate(DateTime value)
        {
            oldestDate = value;
            return this;
        }

        public CovidDataBuilder SetLatestDate(DateTime value)
        {
            latestDate = value;
            return this;
        }

        public CovidDataBuilder SetSource(Uri value)
        {
            source = value;
            return this;
        }

        public CovidData Build()
        {
            var data = new CovidData(countries.ToArray(), latestTotal, updateDate, oldestDate, latestDate);
            return data;
        }
    }
}