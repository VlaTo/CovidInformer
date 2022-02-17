using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CovidInformer.Entities;
using CovidInformer.Services;
using LibraProgramming.Data.OpenApi.Core;
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
            
            var builder = new Covid19DataBuilder();

            foreach (var counter in latests.Counters)
            {
                // counter.Country
                // counter.Value
                // latests.Updated
            }


        }

        public void Dispose()
        {
            ;
        }
    }
}