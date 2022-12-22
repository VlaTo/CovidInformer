using System.Globalization;

namespace CovidInformer.Services
{
    internal sealed class EnglishPluralService : PluralService
    {
        public EnglishPluralService(CultureInfo cultureInfo)
            : base(cultureInfo)
        {
        }

        protected override Noun GetNoun(ulong quantity)
        {
            if (0 == quantity)
            {
                return Noun.Zero;
            }

            if (1 == (quantity % 10))
            {
                return Noun.One;
            }

            /*if ((quantity % 10) >= 2 && (quantity % 10) <= 4 && ((quantity % 100) < 10 || (quantity % 100) >= 20))
            {
                return Noun.Two;
            }

            if (10 > quantity)
            {
                return Noun.Few;
            }*/

            return Noun.Many;
        }
    }
}