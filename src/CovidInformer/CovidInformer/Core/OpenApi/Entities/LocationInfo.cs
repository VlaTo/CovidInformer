using System;
using System.Text.Json.Serialization;

namespace CovidInformer.Core.OpenApi.Entities
{
    [Serializable]
    public sealed class LocationInfo
    {
        [JsonPropertyName("country")]
        public string Country
        {
            get;
            set;
        }

        [JsonPropertyName("country_code")]
        public string CountryCode
        {
            get;
            set;
        }

        [JsonPropertyName("province")]
        public string Province
        {
            get;
            set;
        }

        [JsonPropertyName("coordinates")]
        public GeoPoint Coordinates
        {
            get;
            set;
        }

        [JsonPropertyName("history")]
        public HistoryInfo History
        {
            get;
            set;
        }

        [JsonPropertyName("latest")]
        public ulong Latest
        {
            get;
            set;
        }
    }
}