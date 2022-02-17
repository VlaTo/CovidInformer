using CovidInformer.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace CovidInformer.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}