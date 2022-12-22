using System;
using System.Threading.Tasks;
using CovidInformer.Core.Db;
using CovidInformer.Services;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CovidInformer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateDatabasePage
    {
        public CreateDatabasePage()
        {
            InitializeComponent();
        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            //var db = DependencyService.Resolve<DatabaseContext>();
            //await db.Database.EnsureCreatedAsync();
            //await db.Database.MigrateAsync();

            var rewriter = DependencyService.Resolve<IDatabaseRewriter>();
            await rewriter.RewriteDatabaseAsync();
        }
    }
}