using System.Threading;
using System.Threading.Tasks;
using CovidInformer.Entities;

namespace CovidInformer.Services
{
    public interface IDataProvider
    {
        Task<Covid19Data> GetDataAsync(CancellationToken cancellationToken = default);
    }
}
