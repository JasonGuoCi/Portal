using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envision.SPS.DataAccess
{
    public class ReportDA
    {
        private EFDataContext DataContext { get; set; }

        public ReportDA()
        {
            var connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            DataContext = new EFDataContext(connString);
        }

        public List<SP_SPS_Storage_QueryListResult> GetSPS_Storage_QueryListResult(string listName)
        {
            return
                DataContext.SP_SPS_Storage_QueryList(listName).ToList();
        }



        public List<SPS_Storage> GetSPS_Storage_QueryList(string listID)
        {
            return
                DataContext.SPS_Storage.Where(o => listID.Split(',').Contains(o.ListID)).ToList();
        }


        public List<SPS_Storage> GetSPS_Storage_QueryListByWebID(List<string> webIDs)
        {
            return
                DataContext.SPS_Storage.Where(o => webIDs.Contains(o.WebID) && o.DesitionType == 0).ToList();
        }
    }
}
