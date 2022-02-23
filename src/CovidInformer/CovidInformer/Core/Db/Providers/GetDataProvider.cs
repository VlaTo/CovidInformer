using CovidInformer.Core.OpenApi;
using CovidInformer.Entities;
using CovidInformer.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CovidInformer.Core.Db.Providers
{
    internal sealed class GetDataProvider : IDisposable, IDataProvider
    {
        private DatabaseContext context;
        private bool disposed;

        public GetDataProvider(DatabaseContext context)
        {
            this.context = context;
        }

        public async Task<CovidData> GetDataAsync(CancellationToken cancellationToken = default)
        {
            var latests = await context.Updates
                .Include(entity => entity.Counters)
                .ThenInclude(entity => entity.Country)
                .OrderByDescending(entity => entity.Updated)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (null == latests)
            {
                return null;
            }

            var countries = new List<CountryInfo>();
            
            foreach (var counter in latests.Counters)
            {
                var countryInfo = new CountryInfo(
                    counter.Country.TwoLetterISORegionName,
                    counter.Country.DisplayName,
                    counter.Country.NativeName,
                    counter.Value
                );

                countries.Add(countryInfo);
                // counter.Country
                // counter.Value
                // latests.Updated
            }

            var oldestDate = await GetOldestDate(cancellationToken);
            var builder = new CovidDataBuilder()
                .SetLatestTotal(0UL)
                .SetCountries(countries)
                .SetUpdateDate(latests.Updated)
                //.SetOldestDate(oldestDate)
                //.SetLatestDate(latests.Updated)
                ;

            return builder.Build();
        }

        private async Task<DateTime> GetOldestDate(CancellationToken cancellationToken = default)
        {
            var update = await context.Updates.OrderByDescending(entity => entity.Updated).FirstOrDefaultAsync(cancellationToken);
            return update?.Updated ?? DateTime.UtcNow;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool dispose)
        {
            if (disposed)
            {
                return;
            }

            try
            {
                if (dispose)
                {
                    context = null;
                }
            }
            finally
            {
                disposed = true;
            }
        }
    }
}