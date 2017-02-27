using Envision.SPS.Utility.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Envision.SPS.Utility
{
    public class Weather
    {
        private static int ftoc(double f)
        {
            double ss = Convert.ToDouble(f);
            double c = (ss - 32) / 1.8;
            return Convert.ToInt32(c);
        }
        public static void SearchWeatherList(string strPath, List<WeatherModel> ListWT)
        {

            XmlDocument xmlDoc = new XmlDocument();
           
            xmlDoc.Load(strPath);
            //查找<users>   
            XmlNode root = xmlDoc.SelectSingleNode("query")["results"];
            //XmlNode root = xmlDoc.SelectSingleNode("rss");//["channel"];
            //获取到所有<users>的子节点    
            XmlNodeList nodeList = root.ChildNodes;

            DateTime dt = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now, TimeZoneInfo.Local);
            //遍历所有子节点    
            foreach (XmlNode xn in nodeList)
            {
                XmlElement xe = (XmlElement)xn;
                XmlNodeList subList = xe.ChildNodes;
                WeatherModel wt = new WeatherModel();
                foreach (XmlNode xmlNode in subList)
                {

                    if ("lastBuildDate".Equals(xmlNode.Name))
                    {
                        wt.CurrentDate = xmlNode.InnerText;
                    }
                    if ("yweather:location".Equals(xmlNode.Name))
                    {
                        //上海天气实际显示的是南翔的天气
                        if (xmlNode.Attributes["city"].Value.ToUpper() == "NANXIANG")
                        {
                            wt.Location = "Shanghai";
                        }
                        else
                        {
                            wt.Location = xmlNode.Attributes["city"].Value;
                            //if (wt.Location.ToUpper() == "HONG KONG")
                            //{
                            //    wt.Location = "HongKong";
                            //}
                        }
                    }
                    else if ("item".Equals(xmlNode.Name))
                    {
                        wt.Conditions = xmlNode.ChildNodes[5].Attributes["text"].Value;
                        //if (wt.Location.ToUpper() == "SAN JOSE")
                        //{
                        //    wt.Temperature = xmlNode.ChildNodes[5].Attributes["temp"].Value + "℉";
                        //}
                        //else
                        //{
                            wt.Temperature = ftoc(Convert.ToDouble(xmlNode.ChildNodes[5].Attributes["temp"].Value)).ToString() + "℃";
                        //}
                        wt.Img = xmlNode.ChildNodes[5].Attributes["code"].Value + ".png";
                    }
                }
                //if (wt.Location.ToUpper() == "BEIJING" || wt.Location.ToUpper() == "SHANGHAI" || wt.Location.ToUpper() == "HONGKONG")
                //{
                //    wt.CurrentDate = Common.IBUtils.GetbeijingDateMonth(dt);
                //    wt.CurrentTime = Common.IBUtils.GetbeijingTime(dt);
                //}
                //else if (wt.Location.ToUpper() == "SAN JOSE")
                //{
                //    wt.CurrentDate = Common.IBUtils.GetjiujinshanDateMonth(dt);
                //    wt.CurrentTime = Common.IBUtils.GetjiujinshanTime(dt);
                //}
                //else if (wt.Location.ToUpper() == "MUNICH")
                //{
                //    wt.CurrentDate = Common.IBUtils.GetmoniheiDateMonth(dt);
                //    wt.CurrentTime = Common.IBUtils.GetmoniheiTime(dt);
                //}
                //else if (wt.Location.ToUpper() == "LONDON")
                //{
                //    wt.CurrentDate = Common.IBUtils.GetlundunDateMonth(dt);
                //    wt.CurrentTime = Common.IBUtils.GetlundunTime(dt);
                //}

                if (wt.Location.ToUpper() == "SANTA CLARA")
                {
                    //硅谷(聖塔克拉拉)
                    wt.CurrentDate = IBUtils.GetGuiGuDate(dt);
                    wt.CurrentTime = IBUtils.GetGuiGuTime(dt);
                }
                else if (wt.Location.ToUpper() == "SILKEBORG")
                {
                    //锡尔克堡（丹麦）
                    wt.CurrentDate = IBUtils.GetXiErKeBaoDate(dt);
                    wt.CurrentTime = IBUtils.GetXiErKeBaoTime(dt);
                }
                else if (wt.Location.ToUpper() == "HOUSTON")
                {
                    //休斯顿
                    wt.CurrentDate = IBUtils.GetXiuSiDunDate(dt);
                    wt.CurrentTime = IBUtils.GetXiuSiDunTime(dt);
                }
                ListWT.Add(wt);
            }
        }
        public static List<WeatherModel> ReadParseXml()
        {
            List<WeatherModel> ListWT = new List<WeatherModel>();
            //shanghai
            //SearchWeatherList("http://weather.yahooapis.com/forecastrss?w=2151860&u=f", ListWT);
            //beijing
            //SearchWeatherList("http://weather.yahooapis.com/forecastrss?w=2151330&u=f", ListWT);
            //London
            //SearchWeatherList("http://weather.yahooapis.com/forecastrss?w=44418&u=f", ListWT);
            //Munich
            //SearchWeatherList("http://weather.yahooapis.com/forecastrss?w=676757&u=f", ListWT);
            //San jose
            //SearchWeatherList("http://weather.yahooapis.com/forecastrss?w=2488042&u=f", ListWT);
            //hong kong,hk
            //SearchWeatherList("http://weather.yahooapis.com/forecastrss?w=2165352&u=f", ListWT);

            ////硅谷
            //SearchWeatherList("http://weather.yahooapis.com/forecastrss?w=2488836&u=f", ListWT);
            ////锡尔克堡
            //SearchWeatherList("http://weather.yahooapis.com/forecastrss?w=556914&u=f", ListWT);
            ////休斯顿
            //SearchWeatherList("http://weather.yahooapis.com/forecastrss?w=2424766&u=f", ListWT);

            SearchWeatherList("https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20weather.forecast%20where%20woeid%20in%20(2488836,556914,2424766)&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys", ListWT);
            return ListWT;// jss.Serialize(ListWT);
        }
    }
}
