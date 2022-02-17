using System;
using System.Collections.Generic;
using System.Numerics;
using LibraProgramming.Domain.Entities;

namespace CovidInformer.Entities
{
    public sealed class Covid19Data
    {
        public IReadOnlyList<CountryInfo> Countries
        {
            get;
        }

        public BigInteger Total
        {
            get;
        }

        public DateTime Updated
        {
            get;
        }

        public Covid19Data(IReadOnlyList<CountryInfo> countries, BigInteger total, DateTime updated)
        {
            Countries = countries;
            Total = total;
            Updated = updated;
        }
    }
}