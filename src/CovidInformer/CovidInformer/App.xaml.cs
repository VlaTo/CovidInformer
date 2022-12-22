using System;
using System.Threading.Tasks;
using CovidInformer.Core;
using CovidInformer.Core.Db;
using CovidInformer.Core.OpenApi.Providers;
using CovidInformer.Views;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace CovidInformer
{
    public partial class App
    {

        public App()
        {
            InitializeComponent();

            DependencyService.RegisterSingleton(TaskQueue.GetInstance());
            DependencyService.RegisterSingleton(new DatabaseContext());
            DependencyService.RegisterSingleton(new OpenApiDataProvider());

            MainPage = new AppShell();
            //MainPage = new CreateDatabasePage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
