using System;
using System.Collections.Generic;

namespace CovidInformer.Entities
{
    public sealed class CovidData
    {
        public IReadOnlyList<CountryInfo> Countries
        {
            get;
        }

        public ulong LatestTotal
        {
            get;
        }

        public DateTime UpdateDate
        {
            get;
        }

        public DateTime OldestDate
        {
            get;
        }

        public DateTime LatestDate
        {
            get;
        }

        public CovidData(
            IReadOnlyList<CountryInfo> countries,
            ulong latestTotal,
            DateTime updateDate,
            DateTime oldestDate,
            DateTime latestDate)
        {
            Countries = countries;
            LatestTotal = latestTotal;
            UpdateDate = updateDate;
            OldestDate = oldestDate;
            LatestDate = latestDate;
        }
    }
}