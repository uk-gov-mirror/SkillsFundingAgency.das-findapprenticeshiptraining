using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Web.Models
{
    public class ProviderViewModel
    {
        public decimal? OverallAchievementRate { get ; set ; }

        public int? OverallCohort { get ; set ; }

        public uint ProviderId { get ; set ; }

        public string Website { get ; set ; }

        public string Phone { get ; set ; }

        public string Email { get ; set ; }

        public string Name { get ; set ; }
        public string OverallAchievementRatePercentage { get ; set ; }
        public string NationalOverallAchievementRatePercentage { get ; set ; }
        public IEnumerable<DeliveryModeViewModel> DeliveryModes { get; set; }

        public static implicit operator ProviderViewModel(Provider source)
        {
            return new ProviderViewModel
            {
                Name = source.Name,
                Email = source.Email,
                Phone = source.Phone,
                Website = source.Website,    
                ProviderId = source.ProviderId,
                OverallCohort = source.OverallCohort,
                OverallAchievementRate = source.OverallAchievementRate,
                OverallAchievementRatePercentage = source.OverallAchievementRate.HasValue ? $"{Math.Round(source.OverallAchievementRate.Value)/100:0%}" : "",
                NationalOverallAchievementRatePercentage = source.NationalOverallAchievementRate.HasValue ? $"{Math.Round(source.NationalOverallAchievementRate.Value)/100:0%}" : "",
                DeliveryModes = BuildDeliveryModes(source.DeliveryModes.ToList())
            };
        }

        private static IEnumerable<DeliveryModeViewModel> BuildDeliveryModes(List<DeliveryMode> source)
        {
            var dayRelease = source.SingleOrDefault(mode =>
                mode.DeliveryModeType == Domain.Courses.DeliveryModeType.DayRelease);
            var blockRelease = source.SingleOrDefault(mode => 
                mode.DeliveryModeType == Domain.Courses.DeliveryModeType.BlockRelease);

            return new List<DeliveryModeViewModel>
            {
                new DeliveryModeViewModel
                {
                    DeliveryModeType = DeliveryModeType.Workplace,
                    IsAvailable = source.Any(mode => mode.DeliveryModeType == Domain.Courses.DeliveryModeType.Workplace)
                },
                new DeliveryModeViewModel
                {
                    DeliveryModeType = DeliveryModeType.DayRelease,
                    IsAvailable = dayRelease != default,
                    FormattedDistanceInMiles = dayRelease != default ? $"({dayRelease.DistanceInMiles:##.#} miles away)" : null
                },
                new DeliveryModeViewModel
                {
                    DeliveryModeType = DeliveryModeType.BlockRelease,
                    IsAvailable = blockRelease != default,
                    FormattedDistanceInMiles = blockRelease != default ? $"({blockRelease.DistanceInMiles:##.#} miles away)" : null
                },
            };
        }
    }

    public class DeliveryModeViewModel
    {
        public DeliveryModeType DeliveryModeType { get; set; }
        public string FormattedDistanceInMiles { get; set; }
        public bool IsAvailable { get; set; }
    }

    public enum DeliveryModeType
    {
        [Description("At apprentice’s workplace")]
        Workplace = 0,
        [Description("Day release")]
        DayRelease = 1,
        [Description("Block release")]
        BlockRelease = 2
    }
}
