namespace CovidInformer.Entities
{
    public readonly struct CountryInfo
    {
        public string CountryCode
        {
            get;
        }

        public string CountryName
        {
            get;
        }

        public string NativeName
        {
            get;
        }

        public ulong Total
        {
            get;
        }

        public CountryInfo(string countryCode, string countryName, string nativeName, ulong total)
        {
            CountryCode = countryCode;
            CountryName = countryName;
            NativeName = nativeName;
            Total = total;
        }
    }
}