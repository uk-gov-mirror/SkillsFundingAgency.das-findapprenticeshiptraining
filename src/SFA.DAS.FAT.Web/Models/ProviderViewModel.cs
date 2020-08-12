using System;
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
                NationalOverallAchievementRatePercentage = source.NationalOverallAchievementRate.HasValue ? $"{Math.Round(source.NationalOverallAchievementRate.Value)/100:0%}" : ""
            };
                
        }
    }
}