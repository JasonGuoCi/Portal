using Envision.SPS.Utility.Localization.Dictionaries.Xml;
using Envision.SPS.Utility.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Envision.SPS.Utility.IO.Extensions;
using Envision.SPS.Utility.Exceptions;

namespace Envision.SPS.Utility.Localization
{
    public class XmlEmbeddedFileLocalizationDictionaryProvider : LocalizationDictionaryProviderBase
    {
        private readonly Assembly _assembly;
        private readonly string _rootNamespace;

        /// <summary>
        /// Creates a new <see cref="XmlEmbeddedFileLocalizationDictionaryProvider"/> object.
        /// </summary>
        /// <param name="assembly">Assembly that contains embedded xml files</param>
        /// <param name="rootNamespace">Namespace of the embedded xml dictionary files</param>
        public XmlEmbeddedFileLocalizationDictionaryProvider(Assembly assembly, string rootNamespace)
        {
            _assembly = assembly;
            _rootNamespace = rootNamespace;
        }

        public override void Initialize(string sourceName)
        {
            string p1 = System.Web.HttpContext.Current.Server.MapPath("~/_layouts/15/EnvisionPortal/Localization/Source/Envision.xml");
            string p2 = System.Web.HttpContext.Current.Server.MapPath("~/_layouts/15/EnvisionPortal/Localization/Source/Envision-zh-CN.xml");
            var resourceNames = new[] { p1, p2 };
            var resourceNam = _assembly.GetManifestResourceNames();
            foreach (var resourceName in resourceNames)
            {

                using (var stream = FileToStream(resourceName))
                {
                    var bytes = stream.GetAllBytes();
                    var xmlString = Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3); //Skipping byte order mark

                    var dictionary = CreateXmlLocalizationDictionary(xmlString);
                    if (Dictionaries.ContainsKey(dictionary.CultureInfo.Name))
                    {
                        throw new IBInitializationException(sourceName + " source contains more than one dictionary for the culture: " + dictionary.CultureInfo.Name);
                    }

                    Dictionaries[dictionary.CultureInfo.Name] = dictionary;

                    if (resourceName.EndsWith(sourceName + ".xml"))
                    {
                        if (DefaultDictionary != null)
                        {
                            throw new IBInitializationException("Only one default localization dictionary can be for source: " + sourceName);
                        }

                        DefaultDictionary = dictionary;
                    }
                }
            }
            CacheHelper.Insert(IBKeys.LANGUAGEINFO, Dictionaries);
        }

        public Stream FileToStream(string fileName)
        {
            // 打开文件 
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[] 
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream 
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        protected virtual XmlLocalizationDictionary CreateXmlLocalizationDictionary(string xmlString)
        {
            return XmlLocalizationDictionary.BuildFomXmlString(xmlString);
        }
    }
}
