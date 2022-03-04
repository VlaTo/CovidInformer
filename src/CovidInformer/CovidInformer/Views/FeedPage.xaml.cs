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

        private void DoSelectDateDialogDismiss(object sender, EventArgs e)
        {
            DialogDismiss();
        }

        private void DoSelectDateDialogApply(object sender, EventArgs e)
        {
            ;
            DialogDismiss();
        }

        private void DoSelectDateDialogEmpty(object sender, EventArgs e)
        {
            ;
        }

        private void DoSelectDateDialogOpen(object sender, EventArgs e)
        {
            DatePickerDialog.IsVisible = true;
        }

        private void DialogDismiss()
        {
            DatePickerDialog.IsVisible = false;
        }
    }
}