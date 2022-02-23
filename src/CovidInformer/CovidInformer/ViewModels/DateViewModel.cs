using System;

namespace CovidInformer.ViewModels
{
    public sealed class DateViewModel : BaseViewModel
    {
        private DateTime dateTime;

        public DateTime DateTime
        {
            get => dateTime;
            set => SetProperty(ref dateTime, value);
        }
    }
}