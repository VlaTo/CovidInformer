using CovidInformer.ViewModels;

namespace CovidInformer.Views
{
    public partial class FeedPage
    {
        public FeedPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var command = ((FeedPageViewModel)BindingContext).Load;

            command.Execute(null);
        }
    }
}