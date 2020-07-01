using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAT.Web.Models
{
    public class CoursesRouteModel
    {
        [FromQuery]
        public string Keyword { get; set; }
    }
}