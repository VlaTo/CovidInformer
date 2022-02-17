using System.Globalization;
using System.Numerics;

namespace LibraProgramming.Domain.Entities
{
    public readonly struct CountryInfo
    {
        public RegionInfo Region
        {
            get;
        }

        public BigInteger Total
        {
            get;
        }

        public CountryInfo(RegionInfo region, BigInteger total)
        {
            Region = region;
            Total = total;
        }
    }
}