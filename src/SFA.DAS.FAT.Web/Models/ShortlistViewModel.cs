using System;
using System.Collections.Generic;
using SFA.DAS.FAT.Domain.Shortlist;

namespace SFA.DAS.FAT.Web.Models
{
    public class ShortlistViewModel
    {
        public IEnumerable<ShortlistItemViewModel> Shortlist { get; set; }
    }

    public class ShortlistItemViewModel
    {
        public Guid Id { get; set; }
        public ProviderViewModel Provider { get; set; }
        public CourseViewModel Course { get; set; }
        public string LocationDescription { get; set; }
        public DateTime CreatedDate { get; set; }

        public static implicit operator ShortlistItemViewModel(ShortlistItem source)
        {
            return new ShortlistItemViewModel
            {
                Id = source.Id,
                LocationDescription = source.LocationDescription,
                CreatedDate = source.CreatedDate,
                Course = source.Course,
                Provider = source.Provider
            };
        }
    }
}
