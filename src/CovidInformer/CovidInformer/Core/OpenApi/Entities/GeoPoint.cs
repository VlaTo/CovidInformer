using System;
using System.Text.Json.Serialization;
using LibraProgramming.Data.OpenApi.Core;

namespace CovidInformer.Core.OpenApi.Entities
{
    [Serializable]
    public sealed class GeoPoint
    {
        [JsonPropertyName("lat")]
        [JsonConverter(typeof(GeoCoordinateConverter))]
        public float Latitude
        {
            get;
            set;
        }

        [JsonPropertyName("long")]
        [JsonConverter(typeof(GeoCoordinateConverter))]
        public float Longitude
        {
            get;
            set;
        }
    }
}