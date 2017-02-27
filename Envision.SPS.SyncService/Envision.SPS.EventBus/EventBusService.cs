using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration.Install;

namespace Envision.SPS.EventBus
{

    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;
        public ProjectInstaller()
        {
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();
            service.StartType = ServiceStartMode.Automatic;
            service.ServiceName = "Envision EventBus";
            Installers.Add(process);
            Installers.Add(service);
        }
    }

    public partial class EventBusService : ServiceBase
    {
        private System.Timers.Timer _Timer = new System.Timers.Timer();
        public EventBusService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogService.WriteLog("Service Start!");
            _Timer.AutoReset = false;
            _Timer.Elapsed += new System.Timers.ElapsedEventHandler(_Timer_Elapsed);
            if (args != null && args.Length > 0)
            {
                try
                {
                    //LogService.WriteLog("Service Start try" + args[0]);
                    //int inttemp;
                    //int second = int.TryParse(args[0], out inttemp) ? inttemp : 0;
                    //if (second > 0)
                    //{
                    //    _Timer.Interval = second * 1000;
                    //    _Timer.Start();
                    //}
                    //else
                    StartByConfig();
                }
                catch (Exception ex)
                {
                    LogService.WriteLog("Error：" + ex.Message + "\r\n" + ex.StackTrace);
                    StartByConfig();
                }
            }
            else
            {
                LogService.WriteLog("Service Start else" + args.ToString());
                StartByConfig();
                LogService.WriteLog("Service Start else" + _Timer.Interval);
            }
        }

        /// <summary>Timer EventHandler
        /// Timer EventHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //写入业务代码入口
            try
            {
                SPPermissionsHandle handler = new SPPermissionsHandle();
                bool result = true;
                while (result)
                {
                    DataAccess.SPS_EventBus item = handler.GetEnventBusItem();
                    if (item == null)
                        result = false;
                    else
                        handler.StartExportExcel(item);
                }

                StartByConfig();
            }
            catch (Exception ex)
            {
                LogService.WriteLog("运行失败!错误原因：" + ex.Message);
                _Timer.AutoReset = false;
                StartByConfig();
            }
        }
        /// <summary>间隔时间
        /// Settings
        /// </summary>
        private void StartByConfig()
        {
            int INTERVAL_SECOND;
            if (int.TryParse(System.Configuration.ConfigurationManager.AppSettings["INTERVAL_SECOND"], out INTERVAL_SECOND))
            { _Timer.Interval = INTERVAL_SECOND * 1000; }
            else
            { _Timer.Interval = 10 * 1000; }
            _Timer.Start();
        }

        protected override void OnStop()
        {
            LogService.WriteLog("Service Stop! ");
        }
    }
}
