using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envision.SPS.Utility
{
    [Serializable]
    public class WeatherModel
    {
        /// <summary>
        /// 地区
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 当前当地日期
        /// </summary>
        public string CurrentDate { get; set; }
        /// <summary>
        /// 当前当地时间
        /// </summary>
        public string CurrentTime { get; set; }
        /// <summary>
        /// 温度
        /// </summary>
        public string Temperature { get; set; }
        /// <summary>
        /// 天气情况
        /// </summary>
        public string Conditions { get; set; }
        /// <summary>
        /// 天气图标
        /// </summary>
        public string Img { get; set; }
    }
}
