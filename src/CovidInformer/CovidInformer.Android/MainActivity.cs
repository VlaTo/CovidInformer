using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using CovidInformer.Droid.Services;
using CovidInformer.Services;
using Xamarin.Forms;
using Platform = Xamarin.Essentials.Platform;

namespace CovidInformer.Droid
{
    [Activity(Label = "@string/app_name", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            
            DependencyService.RegisterSingleton<IDatabaseRewriter>(new DatabaseRewriter());

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}