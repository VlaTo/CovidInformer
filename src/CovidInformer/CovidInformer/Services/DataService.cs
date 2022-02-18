using System;
using System.Threading;
using System.Threading.Tasks;
using CovidInformer.Core;
using CovidInformer.Core.Db;
using CovidInformer.Core.Db.Models;
using CovidInformer.Core.Db.Providers;
using CovidInformer.Core.Db.Repositories;
using CovidInformer.Core.OpenApi.Providers;
using CovidInformer.Entities;

namespace CovidInformer.Services
{
    public sealed class DataService : IDataService
    {
        private readonly DatabaseContext context;
        private readonly OpenApiDataProvider provider;
        private readonly CachedValue<Covid19Data> cache;

        public DataService()
        {
            context = new DatabaseContext();
            provider = new OpenApiDataProvider();
            cache = new CachedValue<Covid19Data>();
        }

        public async Task<Covid19Data> GetDataAsync(CancellationToken cancellationToken = default)
        {
            if (false == cache.TryGetValue(out var data))
            {
                data = await GetDataFromDatabaseAsync();
            }

            return data;
        }

        public async Task UpdateDataAsync(CancellationToken cancellationToken)
        {
            var data = await provider.DownloadDataAsync(cancellationToken);

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

        private async Task<Covid19Data> GetDataFromDatabaseAsync()
        {
            Covid19Data data;


            using (var db = new GetDataProvider(context))
            {
                data = await db.GetDataAsync(CancellationToken.None);
            }

            cache.Set(data);

            return data;
        }
    }
}