using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using CovidInformer.Resources;

namespace CovidInformer.Services
{
    internal abstract class PluralService : IPluralService
    {
        private readonly ResourceManager resourceManager;

        private static IPluralService instance;

        public static IPluralService Current
        {
            get
            {
                if (null == instance)
                {
                    var cultureInfo = CultureInfo.CurrentUICulture;

                    switch (cultureInfo.TwoLetterISOLanguageName)
                    {
                        case "ru":
                        {
                            instance = new RussianPluralService(new CultureInfo(cultureInfo.TwoLetterISOLanguageName));
                            break;
                        }

                        default:
                        {
                            instance = new DefaultPluralService(cultureInfo);
                            break;
                        }
                    }
                }

                return instance;
            }
        }

        public CultureInfo CultureInfo
        {
            get;
        }

        protected PluralService(CultureInfo cultureInfo)
        {
            CultureInfo = cultureInfo;
            resourceManager = new ResourceManager("CovidInformer.Resources.Resource", typeof(Resource).Assembly);
        }

        public string GetQuantityString(string groupId, ulong quantity, params object[] args)
        {
            var noun = GetNoun(quantity);
            var resourceKey = $"{groupId}_{noun}";
            var format = resourceManager.GetString(resourceKey, CultureInfo);
            return String.Format(format, args);
        }
        
        protected abstract Noun GetNoun(ulong quantity);

        internal enum Noun
        {
            Zero,
            One,
            Two,
            Few,
            Many,
            Other
        }
    }
}