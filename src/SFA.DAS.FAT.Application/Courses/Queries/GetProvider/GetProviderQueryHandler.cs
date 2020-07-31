using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAT.Domain.Validation;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetProvider
{
    public class GetProviderQueryHandler : IRequestHandler<GetProviderQuery, GetProviderResult>
    {

        private readonly IValidator<GetProviderQuery> _validator;
        public GetProviderQueryHandler(IValidator<GetProviderQuery> validator)
        {
            _validator = validator;
        }

        public async Task<GetProviderResult> Handle(GetProviderQuery query, CancellationToken cancellationToken)
        {

            var validatationResult = await _validator.ValidateAsync(query);
            throw new NotImplementedException();
        }
    }
}
