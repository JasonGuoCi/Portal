using Envision.SPS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envision.SPS.API
{
    public class ReportBO
    {
        private ReportDA Dao { get; set; }

        public ReportBO()
        {
            Dao = new ReportDA();
        }

        public List<SP_SPS_Storage_QueryListResult> GetSPS_Storage_QueryListResult(string listName)
        {
            return this.Dao.GetSPS_Storage_QueryListResult(listName);
        }




        public List<SPS_Storage> GetSPS_Storage_QueryList(string listID)
        {
            return this.Dao.GetSPS_Storage_QueryList(listID);
        }

        public List<SPS_Storage> GetSPS_Storage_QueryListByWebID(List<string> webIDs)
        {
            return this.Dao.GetSPS_Storage_QueryListByWebID(webIDs);
        }


    }
}
