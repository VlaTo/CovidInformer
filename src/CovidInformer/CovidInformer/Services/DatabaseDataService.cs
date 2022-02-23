using CovidInformer.Core.Db;
using CovidInformer.Core.Db.Providers;
using CovidInformer.Core.Db.Repositories;
using CovidInformer.Entities;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace CovidInformer.Services
{
    internal sealed class DatabaseDataService : IDataService
    {
        private readonly DatabaseContext context;
        private readonly IDataService dataService;

        public DatabaseDataService(DatabaseContext context, IDataService dataService)
        {
            this.context = context;
            this.dataService = dataService;
        }

        public async Task<CovidData> GetDataAsync(CancellationToken cancellationToken = default)
        {
            CovidData data;

            using (var db = new GetDataProvider(context))
            {
                data = await db.GetDataAsync(CancellationToken.None);
            }

            if (null == data)
            {
                data = await dataService.GetDataAsync(cancellationToken);
            }

            return data;
        }

        public async Task UpdateDataAsync(CancellationToken cancellationToken)
        {
            var data = await dataService.GetDataAsync(cancellationToken);

            if (null == data)
            {
                return;
            }

            var updateRepository = new UpdateRepository(context);
            var countryRepository = new CountryRepository(context);
            var counterRepository = new CounterRepository(context);
            var date = data.UpdateDate.Date;

            try
            {
                var update = await updateRepository.FindAsync(date, cancellationToken);

                if (null == update)
                {
                    update = await updateRepository.AddAsync(date, cancellationToken);
                }

                for (var index = 0; index < data.Countries.Count; index++)
                {
                    var ci = data.Countries[index];
                    var country = await countryRepository.FindAsync(ci.CountryName, cancellationToken);

                    if (null == country)
                    {
                        if (false == TryGetRegion(ci.CountryCode, out var nativeName))
                        {
                            nativeName = ci.CountryName;
                        }

                        country = await countryRepository.AddAsync(ci.CountryName, nativeName, ci.CountryCode, cancellationToken);
                    }

                    var counter = await counterRepository.FindAsync(country, update, cancellationToken);

                    if (null == counter)
                    {
                        counter = await counterRepository.AddAsync(country, update, cancellationToken);
                        country.Counters.Add(counter);
                        update.Counters.Add(counter);
                    }

                    counter.Value = data.Countries[index].Total;
                }

                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            counterRepository.Dispose();
            countryRepository.Dispose();
            updateRepository.Dispose();
        }

        private static bool TryGetRegion(string regionName, out string nativeName)
        {
            try
            {
                var regionInfo = new RegionInfo(regionName);

                nativeName = regionInfo.NativeName;

                return true;
            }
            catch (ArgumentException)
            {
                nativeName = null;
                return false;
            }
        }
    }
}