using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CovidInformer.Core.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace CovidInformer.Core.Db.Repositories
{
    internal class UpdateRepository : IDisposable
    {
        private DatabaseContext context;
        private bool disposed;

        public UpdateRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public Task<Update> FindAsync(DateTime dateTime, CancellationToken cancellationToken)
        {
            return context.Updates
                .Where(entity => entity.Updated == dateTime)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Update> AddAsync(DateTime dateTime, CancellationToken cancellationToken)
        {
            var entry = await context.Updates.AddAsync(
                new Update
                {
                    Updated = dateTime
                }, cancellationToken
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