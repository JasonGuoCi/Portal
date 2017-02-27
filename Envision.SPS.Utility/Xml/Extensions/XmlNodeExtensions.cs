using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Envision.SPS.Utility.Xml.Extensions
{
    public static class XmlNodeExtensions
    {
        /// <summary>
        /// 从XML节点获取一个属性的值。
        /// </summary>
        /// <param name="node">The Xml node</param>
        /// <param name="attributeName">Attribute name</param>
        /// <returns>Value of the attribute</returns>
        public static string GetAttributeValueOrNull(this XmlNode node, string attributeName)
        {
            if (node.Attributes == null || node.Attributes.Count <= 0)
            {
                throw new ApplicationException(node.Name + " node has not " + attributeName + " attribute");
            }

            return node.Attributes
                .Cast<XmlAttribute>()
                .Where(attr => attr.Name == attributeName)
                .Select(attr => attr.Value)
                .FirstOrDefault();
        }
    }
}
