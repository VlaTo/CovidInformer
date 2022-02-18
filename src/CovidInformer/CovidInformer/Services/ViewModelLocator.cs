using CovidInformer.Core;
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
                var dataService = DependencyService.Resolve<IDataService>();
                var taskQueue = DependencyService.Resolve<TaskQueue>();
                return new FeedPageViewModel(dataService, taskQueue);
            }
        } 
    }
}