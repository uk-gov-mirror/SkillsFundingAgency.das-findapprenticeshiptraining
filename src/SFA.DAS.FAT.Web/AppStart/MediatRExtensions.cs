using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourse;
using SFA.DAS.FAT.Domain.Validation;

namespace SFA.DAS.FAT.Web.AppStart
{
    public static class MediatRExtensions
    {
        public static void AddMediatRValidation(this IServiceCollection services)
        {
            services.AddScoped(typeof(IValidator<GetCourseRequest>), typeof(GetCourseRequestValidator));
        }
    }
}