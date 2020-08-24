using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.FAT.Domain.Locations
{
    public class Locations
    {
        [JsonProperty("locations")]
        public List<LocationItem> LocationItems { get; set; }

        public class LocationItem
        {
            [JsonProperty("location")]
            public LocationPoint LocationPoint { get; set; }

            [JsonProperty("localAuthorityName")]
            public string LocalAuthorityName { get; set; }

            [JsonProperty("locationName")]
            public string LocationName { get; set; }

            [JsonProperty("countyName")]
            public string CountyName { get; set; }
        }
        
        public class LocationPoint
        {
            [JsonProperty("geoPoint")]
            public List<double> GeoPoint { get; set; }
        }
    }
}