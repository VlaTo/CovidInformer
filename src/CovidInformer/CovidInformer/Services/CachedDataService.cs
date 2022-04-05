using System.Diagnostics;
using CovidInformer.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace CovidInformer.Services
{
    internal sealed class CachedDataService : IDataService
    {
        private readonly IDataService dataService;
        private CovidData data;

        public CachedDataService(IDataService dataService)
        {
            this.dataService = dataService;
            data = null;
        }

        public async Task<CovidData> GetDataAsync(CancellationToken cancellationToken = default)
        {
            if (null == data)
            {
                Debug.WriteLine("No cached data");
                data = await dataService.GetDataAsync(cancellationToken);
            }
            else
            {
                Debug.WriteLine("Fetching data from cache");
            }

            return data;
        }

        public Task UpdateDataAsync(CancellationToken cancellationToken)
        {
            data = null;
            return dataService.UpdateDataAsync(cancellationToken);
        }
    }
}