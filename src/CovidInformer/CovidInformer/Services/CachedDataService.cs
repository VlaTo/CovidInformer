using CovidInformer.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace CovidInformer.Services
{
    internal sealed class CachedDataService : IDataService
    {
        private readonly IDataService dataService;
        private Covid19Data data;

        public CachedDataService(IDataService dataService)
        {
            this.dataService = dataService;
            data = null;
        }

        public async Task<Covid19Data> GetDataAsync(CancellationToken cancellationToken = default)
        {
            if (null == data)
            {
                data = await dataService.GetDataAsync(cancellationToken);
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