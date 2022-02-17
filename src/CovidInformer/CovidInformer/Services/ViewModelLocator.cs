using CovidInformer.Core;
using CovidInformer.Core.Db;
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
                var dataProvider = DependencyService.Resolve<IDataProvider>();
                var database = DependencyService.Resolve<DatabaseContext>();
                var taskQueue = DependencyService.Resolve<TaskQueue>();
                return new FeedPageViewModel(database, dataProvider, taskQueue);
            }
        } 
    }
}