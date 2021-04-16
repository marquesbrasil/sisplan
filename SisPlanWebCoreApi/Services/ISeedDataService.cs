using SisPlanWebCoreApi.Repositories;
using System.Threading.Tasks;

namespace SisPlanWebCoreApi.Services
{
    public interface ISeedDataService
    {
        Task Initialize(ClienteDbContext context);
    }
}
