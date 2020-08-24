using System.Threading.Tasks;

namespace SFA.DAS.FAT.Domain.Interfaces
{
    public interface ILocationService
    {
        Task<Domain.Locations.Locations> GetLocations(string searchTerm);
    }
}