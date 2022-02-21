﻿using CovidInformer.Core;
using CovidInformer.Core.Db;
using System.Threading;
using CovidInformer.Core.OpenApi.Providers;
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
        }

        protected override void OnStart()
        {
            var db = DependencyService.Resolve<DatabaseContext>();
            
            db.Database.EnsureCreated();
            //db.Database.Migrate();

            var taskQueue = DependencyService.Resolve<TaskQueue>();
            
            taskQueue.Initialize(CancellationToken.None);
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
