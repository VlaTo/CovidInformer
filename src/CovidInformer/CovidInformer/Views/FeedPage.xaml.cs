using System;
using System.Diagnostics;
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

            var command = ((FeedPageViewModel)BindingContext).Refresh;

            command.Execute("load");
        }

        private void TapGestureRecognizer_OnTapped1(object sender, EventArgs e)
        {
            DatePickerDialog.IsVisible = false;
        }

        private void TapGestureRecognizer_OnTapped2(object sender, EventArgs e)
        {
            ;
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            DatePickerDialog.IsVisible = true;
        }
    }
}