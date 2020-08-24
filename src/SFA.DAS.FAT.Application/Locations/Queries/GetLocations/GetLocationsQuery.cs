using MediatR;

namespace SFA.DAS.FAT.Application.Locations.Queries.GetLocations
{
    public class GetLocationsQuery : IRequest<GetLocationsQueryResponse>
    {
        public string SearchTerm { get; set; }
        
    }
}