using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CovidInformer.Core.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace CovidInformer.Core.Db.Repositories
{
    internal class CountryRepository : IDisposable
    {
        private DatabaseContext context;
        private bool disposed;

        public CountryRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public Task<Country> FindAsync(string countryName, CancellationToken cancellationToken)
        {
            return context.Countries
                .Where(entity => entity.DisplayName == countryName)
                .Include(entity => entity.Counters)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Country> AddAsync(string countryName, string nativeName, string regionName, CancellationToken cancellationToken)
        {
            var entry = await context.Countries.AddAsync(
                new Country
                {
                    DisplayName = countryName,
                    NativeName = nativeName,
                    TwoLetterISORegionName = regionName
                },
                cancellationToken
            );

            if (entry.IsKeySet)
            {
                ;
            }

            return entry.Entity;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool dispose)
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