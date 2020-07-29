using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetProvider
{
    public class GetProviderQuery : IRequest<GetProviderResult>
    {
        public int ProviderId { get; set; }
    }
}
