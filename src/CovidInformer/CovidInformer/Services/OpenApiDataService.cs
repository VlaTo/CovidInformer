using System;
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

        public Task<Covid19Data> GetDataAsync(CancellationToken cancellationToken = default)
        {
            return provider.DownloadDataAsync(cancellationToken);
        }

        public Task UpdateDataAsync(CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }
}