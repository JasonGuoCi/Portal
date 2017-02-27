using Envision.SPS.Utility.Localization.Dictionaries;
using Envision.SPS.Utility.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Envision.SPS.Utility.Extensions;

namespace Envision.SPS.Utility.Handlers
{
    public class EnvisionBaseLanguage
    {
        public static string L(string name)
        {
            return GetString(name);
        }

        private static IDictionary<string, ILocalizationDictionary> Dictionaries;
        public static void LanguageInfo()
        {
            Dictionaries = CacheHelper.Get<IDictionary<string, ILocalizationDictionary>>(IBKeys.LANGUAGEINFO);
            if (Dictionaries.Count <= 0)
            {
                Localization.Language.BuilLanguage();
            }
        }

        public static string GetString(string name)
        {
            LanguageInfo();
            return GetString(name, Thread.CurrentThread.CurrentUICulture);
        }

        public static string GetString(string name, CultureInfo culture)
        {

            var value = GetStringOrNull(name, culture);

            if (value == null)
            {
                return "";
            }

            return value;
        }

        public static string GetStringOrNull(string name, CultureInfo culture)
        {
            var cultureName = string.IsNullOrEmpty(IBUtils.GetCookie(IBKeys.CURRENTLANGUAGE)) ? (culture.Name.ToUpper() != "ZH-CN" ? "EN" : "CN") : IBUtils.GetCookie(IBKeys.CURRENTLANGUAGE).ToUpper() == "CN" ? "CN" : "EN";
            var dictionaries = Dictionaries;

            //Try to get from original dictionary (with country code)
            ILocalizationDictionary originalDictionary;
            if (dictionaries.TryGetValue(cultureName, out originalDictionary))
            {
                var strOriginal = originalDictionary.GetOrNull(name);
                if (strOriginal != null)
                {
                    return strOriginal.Value;
                }
            }


            //Try to get from same language dictionary (without country code)
            if (cultureName.Contains("-")) //Example: "tr-TR" (length=5)
            {
                ILocalizationDictionary langDictionary;
                if (dictionaries.TryGetValue(GetBaseCultureName(cultureName), out langDictionary))
                {
                    var strLang = langDictionary.GetOrNull(name);
                    if (strLang != null)
                    {
                        return strLang.Value;
                    }
                }
            }

            return "";
        }

        private static string GetBaseCultureName(string cultureName)
        {
            return cultureName.Contains("-")
                ? cultureName.Left(cultureName.IndexOf("-", StringComparison.InvariantCulture))
                : cultureName;
        }
    }
}
