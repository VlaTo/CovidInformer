using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CovidInformer.Core.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace CovidInformer.Core.Db.Repositories
{
    internal class CounterRepository : IDisposable
    {
        private DatabaseContext context;
        private bool disposed;

        public CounterRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public Task<Counter> FindAsync(Country country, Update update, CancellationToken cancellationToken)
        {
            return context.Counters
                .Where(entity => entity.CountryId == country.Id && entity.UpdateId == update.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Counter> AddAsync(Country country, Update update, CancellationToken cancellationToken)
        {
            var entry = await context.Counters.AddAsync(
                new Counter
                {
                    Country = country,
                    Update = update
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