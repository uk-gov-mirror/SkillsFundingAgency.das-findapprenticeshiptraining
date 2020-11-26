using System.Collections.Generic;

namespace SFA.DAS.FAT.Web.Models
{
    public class GaData
    {
        public string DataLoaded { get; set; } = "dataLoaded";
        public IDictionary<string, string> Extras { get; set; } = new Dictionary<string, string>();

        public string Location { get; set; }
        public string Vpv { get; set; }

    }
}
