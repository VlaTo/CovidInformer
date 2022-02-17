using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using CovidInformer.Entities;
using CovidInformer.Services;
using LibraProgramming.Data.OpenApi.Core;
using LibraProgramming.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CovidInformer.Core.Db.Providers
{
    internal sealed class DatabaseProvider : IDisposable, IDataProvider
    {
        private readonly DatabaseContext context;

        public DatabaseProvider(DatabaseContext context)
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


            var data = new Covid19Data(Array.Empty<CountryInfo>(), new BigInteger(0UL), latests.Updated);

            foreach (var counter in latests.Counters)
            {
                // counter.Country
                // counter.Value
                // latests.Updated
            }

            return data;
        }

        public void Dispose()
        {
            ;
        }
    }
}