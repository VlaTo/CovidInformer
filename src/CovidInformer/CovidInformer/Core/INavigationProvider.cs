using System;

namespace CovidInformer.Core
{
    public interface INavigationProvider
    {
        bool CanGoForward
        {
            get;
        }

        bool CanNavigate(Type targetPageType);

        void Navigate(Type targetPageType, object parameter);
    }
}