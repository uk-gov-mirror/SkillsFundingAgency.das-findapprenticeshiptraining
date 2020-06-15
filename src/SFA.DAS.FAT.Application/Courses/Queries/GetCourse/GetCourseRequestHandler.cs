using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAT.Domain.Validation;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetCourse
{
    public class GetCourseRequestHandler : IRequestHandler<GetCourseRequest, GetCourseResult>
    {
        private readonly IValidator<GetCourseRequest> _validator;

        public GetCourseRequestHandler (IValidator<GetCourseRequest> validator)
        {
            _validator = validator;
        }
        public async Task<GetCourseResult> Handle(GetCourseRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid())
            {
                throw new ValidationException(validationResult.DataAnnotationResult,null, null);
            }
            
            return new GetCourseResult();
        }
    }
}