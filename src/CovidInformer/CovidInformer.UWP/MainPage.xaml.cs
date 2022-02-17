namespace CovidInformer.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new CovidInformer.App());
        }
    }
}
