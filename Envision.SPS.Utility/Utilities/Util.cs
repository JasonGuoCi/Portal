using System;
using System.Collections.Generic;
using System.Xml;
using Envision.SPS.Utility.Enums;
using Envision.SPS.Utility.Extensions;

namespace Envision.SPS.Utility.Utilities
{
    public class Util
    {
        public static string GetString(object obj)
        {
            return obj == null ? string.Empty : obj.ToString();
        }

        public static DateTime? GetDateTime(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            DateTime result;
            if (DateTime.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            return null;
        }

        public static bool? GetBoolean(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            bool result;
            if (bool.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            return null;
        }

        public static int? GetInt(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            int result;
            if (int.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            return null;
        }

        public static decimal? GetDecimal(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            decimal result;
            if (decimal.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            return null;
        }

        public static List<string> GetWords(string keyword)
        {
            return string.IsNullOrEmpty(keyword) ? new List<string>() : new List<string> { keyword };
        }

        public static string GetString(XmlNode xmlNode, string attributeName)
        {
            if (xmlNode.Attributes == null)
            {
                return string.Empty;
            }
            XmlAttribute xmlAttribute = xmlNode.Attributes[attributeName];
            return xmlAttribute == null ? string.Empty : xmlAttribute.Value;
        }

        public static string Encrypt(string contents)
        {
            return contents;
        }

        public static string Decrypt(string contents)
        {
            return contents;
        }

        public static string GetWebPath(string url)
        {
            Uri uri = new Uri(url);
            return (uri.Host + uri.PathAndQuery.TrimEnd('/')).Replace("/", ".");
        }

        public static string GetWebUrl(string url)
        {
            Uri uri = new Uri(url);
            return string.Format(@"{0}://{1}/", uri.Scheme, uri.Host);
        }

        public static string WriteJsonpToResponse(string contents)
        {
            return Encrypt(contents);
        }

        public static string WriteJsonpToResponse(ResponseStatus responseStatus, object data = null)
        {
            object result = new
            {
                Status = responseStatus,
                Result = data,
            };
            return WriteJsonpToResponse(result.ToJsonString());
        }

        public static string WriteJsonpToResponse(ResponseStatus responseStatus, string pageListHtml, object data = null)
        {
            object result = new
            {
                Status = responseStatus,
                Result = data,
                PageContent = pageListHtml
            };
            return WriteJsonpToResponse(result.ToJsonString());
        }


    }
}
