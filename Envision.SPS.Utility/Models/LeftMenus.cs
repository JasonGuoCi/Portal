using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Envision.SPS.Utility.Models
{
    
    [XmlType(TypeName = "LeftMenu")]
    public class LeftMenu
    {
        [XmlArray("Menus")]
        public List<MenuTab> menutab { get; set; }
    }

    [XmlType(TypeName = "MenuTab")]
    public class MenuTab
    {
        [XmlAttribute]
        public string name { get; set; }

        [XmlArray("Tabs")]
        public List<TabsMenu> menu { get; set; }
    }

    [XmlType(TypeName = "Menu")]
    public class TabsMenu
    {
        [XmlAttribute]
        public string name { get; set; }
        [XmlAttribute]
        public string url { get; set; }
    }
}
