using CovidInformer.Core;
using CovidInformer.Models;
using CovidInformer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CovidInformer.Entities;
using Xamarin.Forms;

namespace CovidInformer.ViewModels
{
    public class FeedPageViewModel : BaseViewModel
    {
        private readonly IDataService dataService;
        private readonly TaskQueue taskQueue;
        private IReadOnlyList<CountryInfo> items;
        private string filter;
        private DateTime updated;
        private BigInteger total;

        public DateTime Updated
        {
            get => updated;
            set => SetProperty(ref updated, value);
        }

        public BigInteger Total
        {
            get => total;
            set => SetProperty(ref total, value);
        }

        public ObservableCollection<Item> Items
        {
            get;
        }

        public ICommand Refresh
        {
            get;
        }

        public ICommand Search
        {
            get;
        }

        public FeedPageViewModel(
            IDataService dataService,
            TaskQueue taskQueue)
        {
            this.dataService = dataService;
            this.taskQueue = taskQueue;

            items = null;
            // Title = "Browse";

            Items = new ObservableCollection<Item>();
            Refresh = new Command(PerformRefresh);
            Search = new Command<string>(PerformSearch);
            
            IsBusy = true;

            taskQueue.EnqueueTask(ExecuteLoadItemsCommand);
        }

        private async Task ExecuteLoadItemsCommand()
        {
            try
            {
                var data = await dataService.GetDataAsync(CancellationToken.None);

                if (null != data)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Updated = data.Updated;
                        Total = data.Total;

                        AssignItems(data.Countries);
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ExecuteRefreshItemsCommand()
        {
            try
            {
                await dataService.UpdateDataAsync(CancellationToken.None);
                
                var data = await dataService.GetDataAsync(CancellationToken.None);

                if (null != data)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Updated = data.Updated;
                        Total = data.Total;

                        AssignItems(data.Countries);
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /*async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }*/

        private void AssignItems(IReadOnlyList<CountryInfo> countries)
        {
            items = countries;
            PerformFiltering();
        }

        private void PerformFiltering()
        {
            bool NoFilter(string str) => true;

            bool Filter(string str) => str.StartsWith(filter);

            Items.Clear();

            var func = String.IsNullOrEmpty(filter)
                ? new Func<string, bool>(NoFilter)
                : new Func<string, bool>(Filter);

            foreach (var countryInfo in items)
            {
                var name = countryInfo.Region.EnglishName;

                if (false == func.Invoke(name))
                {
                    continue;
                }

                var index = FindIndex(name);

                Items.Insert(
                    index,
                    new Item
                    {
                        Id = countryInfo.Region.GeoId,
                        Text = countryInfo.Region.EnglishName,
                        Description = countryInfo.Region.DisplayName,
                        Total = countryInfo.Total
                    }
                );
            }
        }

        private int FindIndex(string str)
        {
            var comparer = CultureInfo.CurrentUICulture.CompareInfo;

            for (var index = 0; index < Items.Count; index++)
            {
                if (0 > comparer.Compare(Items[index].Text, str))
                {
                    continue;
                }

                return index;
            }

            return Items.Count;
        }

        private void PerformRefresh()
        {
            IsBusy = true;

            taskQueue.EnqueueTask(ExecuteRefreshItemsCommand);
        }

        private void PerformSearch(string pattern)
        {
            filter = pattern;
            PerformFiltering();
        }
    }
}