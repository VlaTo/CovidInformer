using System.Globalization;

namespace CovidInformer.Services
{
    internal sealed class DefaultPluralService : PluralService
    {
        public DefaultPluralService(CultureInfo cultureInfo)
            : base(cultureInfo)
        {
        }

        protected override Noun GetNoun(ulong quantity)
        {
            return Noun.Other;
        }
    }
}