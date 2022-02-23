using CovidInformer.Core;
using CovidInformer.Core.Db;
using CovidInformer.Core.OpenApi.Providers;
using CovidInformer.ViewModels;
using Xamarin.Forms;

namespace CovidInformer.Services
{
    public sealed class ViewModelLocator
    {
        public FeedPageViewModel FeedPageViewModel
        {
            get
            {
                var openApiProvider = DependencyService.Resolve<OpenApiDataProvider>();
                var databaseContext = DependencyService.Resolve<DatabaseContext>();

                var openApiService = new OpenApiDataService(openApiProvider);
                var databaseService = new DatabaseDataService(databaseContext, openApiService);
                var cachedService = new CachedDataService(databaseService);

                var taskExecutor = new TaskExecutor();
                
                return new FeedPageViewModel(cachedService, taskExecutor);
            }
        } 
    }
}