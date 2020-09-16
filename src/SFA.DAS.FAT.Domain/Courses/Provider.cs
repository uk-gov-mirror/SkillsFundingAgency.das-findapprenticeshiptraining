using System.Collections.Generic;

namespace SFA.DAS.FAT.Domain.Courses
{
    public class Provider
    {
        public uint ProviderId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public int? OverallCohort { get; set; }
        public decimal? OverallAchievementRate { get; set; }
        public int? NationalOverallCohort { get; set; }
        public decimal? NationalOverallAchievementRate { get; set; }
        public IEnumerable<DeliveryMode> DeliveryModes { get; set; }
    }

    public class DeliveryMode
    {
        public DeliveryModeType DeliveryModeType { get; set; }
        public decimal DistanceInMiles { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public string County { get; set; }
    }

    public enum DeliveryModeType
    {
        Workplace = 0,
        DayRelease = 1,
        BlockRelease = 2,
        NotFound = 3
    }
}
