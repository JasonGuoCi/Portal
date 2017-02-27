using System;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Envision.SPS.Utility.Extensions
{
    public static class BaseTypeExtension
    {
        public static string ToJsonString(this object obj)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(obj);
            jsonString = Regex.Replace(jsonString, @"\\/Date\((\d+)\)\\/", match =>
            {
                DateTime dateTime = new DateTime(1970, 1, 1);
                dateTime = dateTime.AddMilliseconds(long.Parse(match.Groups[1].Value));
                dateTime = dateTime.ToLocalTime();
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            });
            return jsonString;
        }
    }
}
