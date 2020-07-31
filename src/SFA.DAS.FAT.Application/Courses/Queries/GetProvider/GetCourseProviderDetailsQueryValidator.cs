﻿using System.Threading.Tasks;
using SFA.DAS.FAT.Domain.Validation;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetProvider
{
    public class GetCourseProviderDetailsQueryValidator : IValidator<GetProviderQuery>
    {
        public Task<ValidationResult> ValidateAsync(GetProviderQuery item)
        {
            var validationResult = new ValidationResult();

            if (item.ProviderId < 1)
            {
                validationResult.AddError(nameof(item.ProviderId));
            }

            return Task.FromResult(validationResult);
        }
    }
}
