using Envision.SPS.Utility.Models;
using System.Web;
using System.Xml;

namespace Envision.SPS.Utility.Utilities
{
    public static class ConfigManager
    {
        private static readonly string FileName = HttpContext.Current.Server.MapPath(@"~/_layouts/15/UDoc/Configs/Settings.config");
        private static readonly string SiteMenuXmlFileme = HttpContext.Current.Server.MapPath(@"~/_layouts/15/UDoc/Configs/SiteMenus.xml");

        public static string GetXmlAppSetting(string key)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(FileName);
            XmlNode xmlNode = xmlDocument.SelectSingleNode(string.Format(@"/configuration/appSettings/add[@key='{0}']", key));
            if (xmlNode == null || xmlNode.Attributes == null)
            {
                return null;
            }
            return xmlNode.Attributes["value"].Value;
        }

        public static XmlNodeList GetSiteMenuXml()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(SiteMenuXmlFileme);
            return xmlDocument.SelectNodes(@"/Nav");
        }

        public const string leftMenuName = "SiteMenus";
        public const string xmlConfigPath = "~/_layouts/15/EnvisionDoc/Configs/SiteConfig.config";
        //此文档模板类型不需要
        public static int[] notTypeDocTemplate =new []{100,1000};
        public const int Type_Client = 101;
        /// <summary>
        ///  读取配置文件
        /// </summary>
        public static EnvisionSiteConfig loadConfig()
        {
            EnvisionSiteConfig model = CacheHelper.Get<EnvisionSiteConfig>(IBKeys.CACHE_SITE_CONFIG);
            if (model == null)
            {
                CacheHelper.Insert(IBKeys.CACHE_SITE_CONFIG, loadConfig(IBUtils.GetXmlMapPath(xmlConfigPath)),
                    IBUtils.GetXmlMapPath(IBKeys.FILE_SITE_XML_CONFING));
                model = CacheHelper.Get<EnvisionSiteConfig>(IBKeys.CACHE_SITE_CONFIG);
            }
            return model;
        }

        public static EnvisionSiteConfig loadConfig(string configFilePath)
        {
            return (EnvisionSiteConfig)SerializationHelper.Load(typeof(EnvisionSiteConfig), configFilePath);
        }
    }
}
