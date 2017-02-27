using Envision.SPS.Utility.Localization.Dictionaries;
using Envision.SPS.Utility.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envision.SPS.Utility.Handlers
{
    public class EnvisionPortalConfig
    {
        /// <summary>
        ///  读取配置文件
        /// </summary>
        public static Models.EnvisionPortalConfig loadConfig()
        {
            Models.EnvisionPortalConfig model = CacheHelper.Get<Models.EnvisionPortalConfig>(IBKeys.CACHE_PORTAL_CONFIG);
            if (model == null)
            {
                CacheHelper.Insert(IBKeys.CACHE_PORTAL_CONFIG, loadConfig(IBUtils.GetXmlMapPathConfig(IBKeys.FILE_PORTAL_XML_CONFIG)),
                    IBUtils.GetXmlMapPathConfig(IBKeys.FILE_PORTAL_XML_CONFIG));
                model = CacheHelper.Get<Models.EnvisionPortalConfig>(IBKeys.CACHE_PORTAL_CONFIG);
            }
            return model;
        }

        private static Models.EnvisionPortalConfig loadConfig(string configFilePath)
        {
            Models.EnvisionPortalConfig siteconfig = (Models.EnvisionPortalConfig)SerializationHelper.Load(typeof(Models.EnvisionPortalConfig), configFilePath);
            return siteconfig;
        }

        private static IDictionary<string, ILocalizationDictionary> Dictionaries = new Dictionary<string, ILocalizationDictionary>();
        public static void LanguageInfo()
        {
            Dictionaries = CacheHelper.Get<IDictionary<string, ILocalizationDictionary>>(IBKeys.LANGUAGEINFO);
            if (Dictionaries == null || Dictionaries.Count <= 0)
            {
                Localization.Language.BuilLanguage();
            }
        }
    }
}
