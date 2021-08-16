using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebApiHolidays.Data.Models
{
    public class Holiday
    {
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("localName")]
        public string LocalName { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }
        [JsonProperty("fixed")]
        public string Fixed { get; set; }
        [JsonProperty("global")]
        public string Global { get; set; }
        [JsonProperty("countries")]
        public string Countries { get; set; }
        [JsonProperty("launchYear")]
        public string LaunchYear { get; set; }
        [JsonProperty("types")]
        public List<string> Types { get; set; }
    }
}
