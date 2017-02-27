using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Envision.SPS.EventBus
{
    public class LogService
    {
        /// <summary>
        /// 写日志文件信息
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteLog(string msg)
        {
            try
            {
                //Console.WriteLine("\r\n" + DateTime.Now + "  " + msg);
                string dicPath = System.AppDomain.CurrentDomain.BaseDirectory + "Log";
                if (!Directory.Exists(dicPath))
                    Directory.CreateDirectory(dicPath);
                string file = dicPath + "\\" + DateTime.Now.ToShortDateString().Replace("/", "") + "ErrorLog.txt";
                if (!File.Exists(file))
                    using (File.Create(file))
                    {

                    };

                StreamWriter sw = new StreamWriter(file, true, Encoding.GetEncoding("GB2312"));
                sw.WriteLine("\r\n" + DateTime.Now + ":  " + msg);
                sw.Dispose();
            }
            catch { }
        }
    }
}
