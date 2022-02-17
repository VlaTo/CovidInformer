using CovidInformer.Core;
using CovidInformer.Models;
using LibraProgramming.Domain.Entities;
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
using CovidInformer.Core.Db;
using CovidInformer.Services;
using Xamarin.Forms;

namespace CovidInformer.ViewModels
{
    public class FeedPageViewModel : BaseViewModel
    {
        private readonly DatabaseContext databaseContext;
        private readonly IDataProvider dataProvider;
        private readonly TaskQueue taskQueue;
        private IReadOnlyList<CountryInfo> data;
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
            DatabaseContext databaseContext,
            IDataProvider dataProvider,
            TaskQueue taskQueue)
        {
            this.databaseContext = databaseContext;
            this.dataProvider = dataProvider;
            this.taskQueue = taskQueue;

            Title = "Browse";
            Items = new ObservableCollection<Item>();
            Refresh = new Command(PerformRefresh);
            Search = new Command<string>(PerformSearch);
        }

        async Task ExecuteLoadItemsCommand()
        {
            try
            {
                var countries = databaseContext.Countries.OrderBy(entity => entity.DisplayName);
                    

                var covid19Data = await dataProvider.GetDataAsync(CancellationToken.None);

                Device.BeginInvokeOnMainThread(() =>
                {
                    Updated = covid19Data.Updated;
                    Total = covid19Data.Total;

                    AssignItems(covid19Data.Countries);

                    foreach (var ci in covid19Data.Countries)
                    {
                        Items.Add(new Item
                        {
                            Id = ci.Region.GeoId,
                            Text = $"{ci.Region.EnglishName} ({ci.Region.DisplayName})",
                            Description = ci.Total.ToString()
                        });
                    }
                });
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

        public void OnAppearing()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            taskQueue.EnqueueTask(ExecuteLoadItemsCommand);
        }


        /*private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }*/

        /*async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }*/
        private void AssignItems(IReadOnlyList<CountryInfo> countries)
        {
            data = countries;
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

            foreach (var countryInfo in data)
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
            ;
        }

        private void PerformSearch(string pattern)
        {
            filter = pattern;
            PerformFiltering();
        }
    }
}