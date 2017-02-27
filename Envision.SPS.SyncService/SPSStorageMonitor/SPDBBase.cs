using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSStorageMonitor
{
    public class SPDBBase
    {
        DBDataContext _db;
        SPSDBDataContext _spsdb;

        public DBDataContext DB
        {
            get
            {
                if (this._db != null)
                {
                    return this._db;
                }
                else
                {
                    this._db = new DBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["SPConnStr"].ConnectionString);
                    return this._db;
                }
            }
        }


        public SPSDBDataContext SPSDB
        {
            get
            {
                if (this._spsdb != null)
                {
                    return this._spsdb;
                }
                else
                {
                    this._spsdb = new SPSDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
                    return this._spsdb;
                }
            }
        }


        public bool InsertList(List<SPS_Storage> list)
        {
            try
            {
                this.SPSDB.SPS_Storage.InsertAllOnSubmit(list);
                this.SPSDB.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }



        public List<SP_Envision_GetListSizeResult> SP_Envision_GetListSizeResult(string listID)
        {
            return
                DB.SP_Envision_GetListSize(listID).ToList();
        }


        public List<AllDocs> SP_Envision_GetListSizeResult()
        {
            var query = from a in DB.AllDocs select a;
            return query.ToList();

        }
    }
}
