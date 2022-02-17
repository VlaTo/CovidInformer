using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CovidInformer.Core.OpenApi.Entities
{
    [Serializable]
    public class HistoryInfo
    {
        [JsonExtensionData]
        public IDictionary<string, JsonElement> Data
        {
            get; 
            set;
        }
    }
}