using Envision.SPS.Utility.Enums;
using Envision.SPS.Utility.Models;
using Envision.SPS.Utility.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envision.SPS.Utility.Handlers
{
    public static class LeftNavHandler
    {
        public static string GetLeftMeuns()
        {
            string xmlString = XMLHelper.GetXmlString(ConfigManager.leftMenuName);
            //LeftMenu menuList = XMLHelper.Deserialize(typeof(LeftMenu), xmlString) as LeftMenu;
            LeftMenu menuList = XMLHelper.DeserializeFromXml<LeftMenu>(xmlString) as LeftMenu;
            return Util.WriteJsonpToResponse(ResponseStatus.Success,menuList);
        }
    }
}
