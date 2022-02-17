using System;
using System.Text.Json.Serialization;

namespace CovidInformer.Core.OpenApi.Entities
{
    [Serializable]
    public sealed class ConfirmedCasesInfo
    {
        [JsonPropertyName("locations")]
        public LocationInfo[] Locations
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

        [JsonPropertyName("last_updated")]
        public DateTime Updated
        {
            get;
            set;
        }

        [JsonPropertyName("source")]
        public Uri Source
        {
            get;
            set;
        }
    }
}