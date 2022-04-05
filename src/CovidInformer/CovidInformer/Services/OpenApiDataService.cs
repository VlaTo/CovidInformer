using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CovidInformer.Core.OpenApi.Providers;
using CovidInformer.Entities;

namespace CovidInformer.Services
{
    internal sealed class OpenApiDataService : IDataService
    {
        private readonly OpenApiDataProvider provider;

        public OpenApiDataService(OpenApiDataProvider provider)
        {
            this.provider = provider;
        }

        public Task<CovidData> GetDataAsync(CancellationToken cancellationToken = default)
        {
            Debug.WriteLine($"Start fetching data from {nameof(OpenApiDataService)}...");
            return provider.DownloadDataAsync(cancellationToken);
        }

        public Task UpdateDataAsync(CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }
}