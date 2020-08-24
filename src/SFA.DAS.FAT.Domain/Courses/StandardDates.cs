using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace SFA.DAS.FAT.Domain.Courses
{
    public class StandardDates
    {
        public DateTime? LastDateStarts { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime EffectiveFrom { get; set; }
    }
}
