using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Envision.SPS.Utility.Localization
{
    public static class Language
    {
        public static void BuilLanguage()
        {

            XmlEmbeddedFileLocalizationDictionaryProvider dd =
                new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "Envision.SPS.Portal.Web.Layouts.EnvisionPortal.Localization.Source.Envision.xml"
                        );
            dd.Initialize("EnvisionPortal");
        }
    }
}
