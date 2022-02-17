using System.Threading;
using System.Threading.Tasks;
using CovidInformer.Core;
using CovidInformer.Core.Db;
using CovidInformer.Core.Db.Providers;
using CovidInformer.Core.OpenApi.Providers;
using CovidInformer.Entities;

namespace CovidInformer.Services
{
    public sealed class DataService : IDataService
    {
        private readonly DatabaseContext context;
        private readonly OpenApiDataProvider provider;
        private readonly CachedValue<Covid19Data> cache;

        public DataService(DatabaseContext context, OpenApiDataProvider provider)
        {
            this.context = context;
            this.provider = provider;

            cache = new CachedValue<Covid19Data>();
        }

        public async Task<Covid19Data> GetDataAsync(CancellationToken cancellationToken = default)
        {
            if (false == cache.TryGetValue(out var data))
            {
                data = await GetDataFromDatabaseAsync();
            }

            return data;
        }

        public Task UpdateDataAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        private async Task<Covid19Data> GetDataFromDatabaseAsync()
        {
            Covid19Data data;

            using (var temp = new DatabaseProvider(context))
            {
                data = await temp.GetDataAsync(CancellationToken.None);
            }

            cache.Set(data);

            return data;
        }
    }
}