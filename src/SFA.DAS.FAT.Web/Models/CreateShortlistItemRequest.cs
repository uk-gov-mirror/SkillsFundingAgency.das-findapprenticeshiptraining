using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAT.Web.Models
{
    public class CreateShortlistItemRequest
    {
        [FromRoute(Name = "providerId")]
        public int Ukprn { get ; set ; }
        [FromRoute(Name="id")]
        public int TrainingCode { get ; set ; }
        public string SectorSubjectArea { get ; set ; }
        public string RouteName { get; set; }
    }
}