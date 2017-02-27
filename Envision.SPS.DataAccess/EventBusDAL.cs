using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envision.SPS.DataAccess
{
    public class EventBusDAL
    {
        EFDataContext _db;


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

        public bool InsertList(List<SPS_EventBus> list)
        {
            try
            {

                this.DB.SPS_EventBus.InsertAllOnSubmit(list);
                this.DB.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateSPS_EventBus(SPS_EventBus model)
        {
            try
            {
                SPS_EventBus st = this.DB.SPS_EventBus.SingleOrDefault(c => c.ID == model.ID);
                if (st == null)
                {
                    return false;
                }
                st.CompletedTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                st.Status = 1;
                st.IsEmail = true;
                this.DB.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\log.txt", System.Environment.NewLine + "Error--" + System.DateTime.Now + " --" + ex.Message + "--》" + ex.StackTrace);
                return false;
            }
        }

        public bool UpdateSPS_EventBusStateError(SPS_EventBus model)
        {
            try
            {
                SPS_EventBus st = this.DB.SPS_EventBus.SingleOrDefault(c => c.ID == model.ID);
                if (st == null)
                {
                    return false;
                }
                st.Status = 2;
                this.DB.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool AddSPS_EventBus(SPS_EventBus model, ref long Id)
        {
            try
            {
                this.DB.SPS_EventBus.InsertOnSubmit(model);
                this.DB.SubmitChanges();
                Id = this.DB.SPS_EventBus.Max(p => p.ID);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public List<SPS_EventBus> SP_Envision_GetEventBus()
        {
            var query = from a in DB.SPS_EventBus where a.Status == 0 select a;
            return query.ToList();
        }
        /// <summary>
        /// 取出单条数据
        /// </summary>
        /// <returns></returns>
        public SPS_EventBus SP_Envision_GetEventBusModel()
        {
            SPS_EventBus spsBus = (from a in DB.SPS_EventBus where a.Status == 0 orderby a.CreatedTime ascending select a).FirstOrDefault();
            return spsBus;
        }
        /// <summary>
        /// 根据ID获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SPS_EventBus SPS_EventBusSP_Envision_GetEventBusModelById(int id)
        {
            SPS_EventBus spsBus = (from a in DB.SPS_EventBus where a.ID == id orderby a.CreatedTime ascending select a).FirstOrDefault();
            return spsBus;

        }
    }
}
