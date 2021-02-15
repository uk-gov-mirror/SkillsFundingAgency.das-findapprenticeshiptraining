using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAT.Web.Models
{
    public class CreateShortListItemRequest
    {
        [FromRoute(Name = "providerId")]
        public int Ukprn { get ; set ; }
        [FromRoute(Name="id")]
        public int TrainingCode { get ; set ; }
        [FromBody]
        public string SectorSubjectArea { get ; set ; }
        [FromBody]
        public string RouteName { get; set; }
    }
}