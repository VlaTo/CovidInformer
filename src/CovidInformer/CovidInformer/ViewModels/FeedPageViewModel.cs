using CovidInformer.Core;
using CovidInformer.Entities;
using CovidInformer.Models;
using CovidInformer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CovidInformer.ViewModels
{
    public class FeedPageViewModel : BaseViewModel
    {
        private readonly IDataService dataService;
        private readonly TaskExecutor executor;
        private IReadOnlyList<CountryInfo> items;
        private string filter;
        private DateTime oldestDate;
        private DateTime updateDate;
        private ulong total;

        public DateTime OldestDate
        {
            get => oldestDate;
            set => SetProperty(ref oldestDate, value);
        }

        public DateTime UpdateDate
        {
            get => updateDate;
            set => SetProperty(ref updateDate, value);
        }

        public ulong Total
        {
            get => total;
            set => SetProperty(ref total, value);
        }

        public bool IsInitializing
        {
            get;
            private set;
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

        public ICommand SelectDate
        {
            get;
        }

        public FeedPageViewModel(IDataService dataService, TaskExecutor executor)
        {
            this.dataService = dataService;
            this.executor = executor;

            items = null;
            // Title = "Browse";

            Items = new ObservableCollection<Item>();
            Refresh = new Command<string>(PerformRefresh);
            Search = new Command<string>(PerformSearch);
            SelectDate = new Command(DoSelectDate);
        }

        private async Task ExecuteRefreshItems(string arg)
        {
            IsBusy = true;

            try
            {
                Debug.WriteLine($"[ExecuteRefreshItems] mode: {arg}");

                if (String.Equals(arg, "update"))
                {
                    await dataService.UpdateDataAsync(CancellationToken.None);
                }

                var data = await dataService.GetDataAsync(CancellationToken.None);

                if (null != data)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //OldestDate = data.OldestDate;
                        UpdateDate = data.UpdateDate;
                        Total = data.LatestTotal;

                        AssignItems(data.Countries);

                        IsInitializing = false;
                        IsBusy = false;
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
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
                var name = countryInfo.CountryName;

                if (false == func.Invoke(name))
                {
                    continue;
                }

                var index = FindIndex(name);

                Items.Insert(
                    index,
                    new Item
                    {
                        Id = -1,
                        Text = name,
                        Description = countryInfo.NativeName,
                        Total = countryInfo.Total
                    }
                );
            }
        }

        private void PerformRefresh(string arg)
        {
            if (executor.TryRun(() => ExecuteRefreshItems(arg)))
            {
                ;
            }
        }

        private void PerformSearch(string pattern)
        {
            filter = pattern;
            PerformFiltering();
        }

        private void DoSelectDate()
        {
            Debug.WriteLine("[DoSelectDate]");
        }

        /*private void PerformLoad()
        {
            IsBusy = true;
            taskQueue.EnqueueTask(ExecuteLoadItemsCommand);
        }*/

        /*private async Task ExecuteLoadItemsCommand()
        {
            try
            {
                var data = await dataService.GetDataAsync(CancellationToken.None);

                if (null != data)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //OldestDate = data.OldestDate.Date - TimeSpan.FromDays(1.0d);
                        UpdateDate = data.UpdateDate.Date;
                        Total = data.LatestTotal;

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
        }*/

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
    }
}