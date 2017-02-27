using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envision.SPS.DataAccess
{
    public class DBBase
    {
        EFDataContext _db;
        SPDBDataContext _spdb;

        public EFDataContext DB
        {
            get
            {
                if (this._db != null)
                {
                    return this._db;
                }
                else
                {
                    this._db = new EFDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
                    return this._db;
                }
            }
        }

        public SPDBDataContext SPDB
        {
            get
            {
                if (this._spdb != null)
                {
                    return this._spdb;
                }
                else
                {
                    this._spdb = new SPDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["SPConnStr"].ConnectionString);
                    return this._spdb;
                }
            }
        }

        public bool InsertList(List<SPS_Storage> list)
        {
            try
            {
                this.DB.SPS_Storage.InsertAllOnSubmit(list);
                this.DB.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddSPS_Storage(SPS_Storage model)
        {
            try
            {
                SPS_Storage st = this.DB.SPS_Storage.SingleOrDefault(c => c.CreatorAccount == model.CreatorAccount);
                if (st != null)
                {
                    return false;
                }
                this.DB.SPS_Storage.InsertOnSubmit(model);
                this.DB.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public SPS_Storage GetSPS_StorageByAccount(string strADAcount)
        {
            return (from e in this.DB.SPS_Storage where e.CreatorAccount.ToLower() == strADAcount.ToLower() select e).FirstOrDefault();
        }

        public List<AllDocs> SP_Envision_GetListSizeResult()
        {
            var query = from a in SPDB.AllDocs select a;
            return query.ToList();

        }


        public decimal GetEnvisionListSizeResult(string listId)
        {
            decimal result = (from a in SPDB.AllDocs where a.ListId.ToString() == listId select a).Sum(t => Convert.ToInt64(t.Size));
            return result;
        }

    }
}
