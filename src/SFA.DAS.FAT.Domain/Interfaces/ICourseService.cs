using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Domain.Interfaces
{
    public interface ICourseService
    {
        Task<TrainingCourse> GetCourse(int courseId);
        Task<TrainingCourses> GetCourses(string keyword, List<Guid> requestRouteIds, List<int> requestLevels, OrderBy orderBy);
        Task<TrainingCourseProviders> GetCourseProviders(int courseId, string queryLocation, IEnumerable<DeliveryModeType> queryDeliveryModes, ProviderSortBy sortBy);
        Task<TrainingCourseProviderDetails> GetCourseProviderDetails(int providerId, int standardId, string location);
    }
}
