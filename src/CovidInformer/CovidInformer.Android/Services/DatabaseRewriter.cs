using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using CovidInformer.Services;

namespace CovidInformer.Droid.Services
{
    public sealed class DatabaseRewriter : IDatabaseRewriter
    {
        private const string Filename = "coviddata.db";

        public async Task RewriteDatabaseAsync()
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dataSource = Path.Combine(basePath, Filename);
            await using var input = Application.Context.Assets.Open(Filename);
            await using var output = File.OpenWrite(dataSource);
            await input.CopyToAsync(output);
        }
    }
}