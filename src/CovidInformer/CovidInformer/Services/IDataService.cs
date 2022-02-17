using System.Threading;
using System.Threading.Tasks;

namespace CovidInformer.Services
{
    public interface IDataService : IDataProvider
    {
        Task UpdateDataAsync(CancellationToken cancellationToken);
    }
}