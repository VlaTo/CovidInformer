using System;
using System.Threading;
using System.Threading.Tasks;
using CovidInformer.Core.Db;
using CovidInformer.Core.Db.Providers;
using CovidInformer.Core.Db.Repositories;
using CovidInformer.Entities;

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

        public async Task<Covid19Data> GetDataAsync(CancellationToken cancellationToken = default)
        {
            Covid19Data data;

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
            var date = data.Updated.Date;

            try
            {
                var update = await updateRepository.FindAsync(date, cancellationToken);

                if (null == update)
                {
                    update = await updateRepository.AddAsync(date, cancellationToken);
                }

                for (var index = 0; index < data.Countries.Count; index++)
                {
                    //var countryName = data.Countries[index].Region.EnglishName;
                    var region = data.Countries[index].Region;
                    var country = await countryRepository.FindAsync(region.EnglishName, cancellationToken);

                    if (null == country)
                    {
                        country = await countryRepository.AddAsync(region.EnglishName, region.NativeName, region.TwoLetterISORegionName, cancellationToken);
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
    }
}