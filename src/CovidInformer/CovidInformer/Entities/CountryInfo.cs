using System.Globalization;

namespace CovidInformer.Entities
{
    public readonly struct CountryInfo
    {
        public RegionInfo Region
        {
            get;
        }

        public ulong Total
        {
            get;
        }

        public CountryInfo(RegionInfo region, ulong total)
        {
            Region = region;
            Total = total;
        }
    }
}