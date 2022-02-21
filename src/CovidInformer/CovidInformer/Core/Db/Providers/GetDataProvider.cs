using CovidInformer.Core.OpenApi;
using CovidInformer.Entities;
using CovidInformer.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public async Task<Covid19Data> GetDataAsync(CancellationToken cancellationToken = default)
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
                var region = new RegionInfo(counter.Country.TwoLetterISORegionName);
                countries.Add(new CountryInfo(region, counter.Value));
                // counter.Country
                // counter.Value
                // latests.Updated
            }

            var builder = new Covid19DataBuilder()
                .SetLatest(0UL)
                .SetUpdated(latests.Updated)
                .SetCountries(countries);

            return builder.Build();
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