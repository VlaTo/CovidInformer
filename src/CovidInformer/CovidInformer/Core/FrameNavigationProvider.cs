using System;
using Xamarin.Forms;

namespace CovidInformer.Core
{
    public sealed class FrameNavigationProvider : INavigationProvider
    {
        private readonly Frame frame;
        private INavigation navigation;

        public bool CanGoForward
        {
            get;
        }

        public FrameNavigationProvider(Frame frame)
        {
            this.frame = frame;
        }

        public bool CanNavigate(Type targetPageType)
        {
            //frame.Navigation
            return true;
        }

        public void Navigate(Type targetPageType, object parameter)
        {
            //navigation.PushAsync(targetPageType, true);
        }
    }
}