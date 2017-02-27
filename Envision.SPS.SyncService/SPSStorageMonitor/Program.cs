using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Envision.SPS.DataAccess;
using System.Data;



namespace SPSStorageMonitor
{
    /// <summary>
    /// 定时插入监控信息处理类
    /// </summary>
    public class Program
    {
        static SPDBBase spdb = new SPDBBase();
        static List<AllDocs> docList = new List<AllDocs>();

        /// <summary>
        /// Main Method
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {

            Console.WriteLine("正在执行~");
            docList = spdb.SP_Envision_GetListSizeResult();
            List<SPS_Storage> storageList = new List<SPS_Storage>();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    var SPSRootSiteUrl = System.Configuration.ConfigurationManager.AppSettings["SPSRootSiteUrl"];
                    using (SPSite spSite = new SPSite(SPSRootSiteUrl))
                    {
                        using (SPWeb web = spSite.OpenWeb())
                        {
                            InsertWebItem(web, ref storageList);
                            if (web.Webs.Count > 0)
                            {
                                BuildSubWeb(web, ref storageList);
                            }
                            foreach (SPList splist in web.Lists)
                            {
                                if (splist.BaseType == SPBaseType.DocumentLibrary && splist.BaseTemplate != SPListTemplateType.ListTemplateCatalog && splist.BaseTemplate != SPListTemplateType.DesignCatalog && splist.BaseTemplate == SPListTemplateType.DocumentLibrary && splist.AllowDeletion &&
                                    !splist.IsSiteAssetsLibrary)
                                {
                                    InsertListItem(web, splist, ref storageList);
                                }
                            }
                            SPDBBase db = new SPDBBase();
                            db.InsertList(storageList);
                        }
                    }
                });
            }
            catch (Exception e)
            {
                Console.WriteLine("执行出错--" + e.Message);
                Console.ReadLine();
            }
            Console.WriteLine("执行成功~");
        }

        private static void GetSubWebId(SPWeb web, ref List<Guid> webIds)
        {
            foreach (SPWeb item in web.Webs)
            {
                webIds.Add(item.ID);
                if (item.Webs.Count > 0)
                {
                    GetSubWebId(item, ref webIds);
                }
            }
        }

        /// <summary>
        /// 构建需要插入的对象列表
        /// </summary>
        /// <param name="spweb"></param>
        /// <param name="storageList"></param>
        private static void BuildSubWeb(SPWeb spweb, ref List<SPS_Storage> storageList)
        {
            foreach (SPWeb web in spweb.Webs)
            {
                InsertWebItem(web, ref storageList);

                if (web.Webs.Count > 0) BuildSubWeb(web, ref storageList);

                DataTable tbl = new DataTable();
                foreach (SPList list in web.Lists)
                {
                    if (list.BaseType == SPBaseType.DocumentLibrary && list.BaseTemplate != SPListTemplateType.ListTemplateCatalog && list.BaseTemplate != SPListTemplateType.DesignCatalog && list.BaseTemplate == SPListTemplateType.DocumentLibrary)
                    {
                        InsertListItem(web, list, ref storageList);
                    }
                }
            }

        }

        /// <summary>
        /// 插入文档库对象
        /// </summary>
        /// <param name="web"></param>
        /// <param name="list"></param>
        /// <param name="storageList"></param>
        /// <param name="tbl"></param>
        private static void InsertListItem(SPWeb web, SPList list, ref List<SPS_Storage> storageList)
        {
            try
            {
                SPS_Storage model = new SPS_Storage();
                model.WebName = web.Title;
                model.WebID = web.ID.ToString("N");
                model.WebUrl = web.Url;
                model.ListName = list.Title;
                model.ListID = list.ID.ToString("N");
                model.ListUrl = list.DefaultViewUrl;
                model.FolderNumber = list.Folders.Count;
                model.FileNumber = list.ItemCount;
                model.Storage = GetSumSize(list.ID.ToString());
                model.Owners = list.Author.Name;
                model.DesitionType = 1;//0:站点 1:列表
                model.Created = DateTime.Now;//list.Created;
                model.CreatorAccount = list.Author.LoginName;
                model.CreatorUserName = list.Author.Name;
                storageList.Add(model);
            }
            catch { }
        }

        /// <summary>
        /// 根据文档库ID获取文档库容量大小
        /// </summary>
        /// <param name="listID"></param>
        /// <returns></returns>
        public static decimal GetSumSize(string listID)
        {
            var sum = (from o in docList where o.ListId.ToString() == listID select o).Sum(t => Convert.ToInt64(t.Size));
            var sumresult = Convert.ToDecimal(sum) / 1024 / 1024;
            return sumresult;
        }

        /// <summary>
        /// 获取Site大小
        /// </summary>
        /// <param name="webIds"></param>
        /// <returns></returns>
        public static decimal GetSumSize(List<Guid> webIds)
        {
            var sum = (from o in docList where webIds.Contains(o.WebId) select o).Sum(t => Convert.ToInt64(t.Size));
            var sumresult = Convert.ToDecimal(sum) / 1024 / 1024;
            return Math.Round(sumresult, 2);
        }



        /// <summary>
        /// 插入WEB对象
        /// </summary>
        /// <param name="web"></param>
        /// <param name="storageList"></param>
        private static void InsertWebItem(SPWeb web, ref List<SPS_Storage> storageList)
        {
            try
            {
                List<Guid> webIds = new List<Guid>();
                GetSubWebId(web, ref  webIds);
                decimal storage = GetSumSize(webIds);

                SPS_Storage webmodel = new SPS_Storage();
                webmodel.WebName = web.Title;
                webmodel.WebID = web.ID.ToString("N");
                webmodel.WebUrl = web.Url;
                webmodel.ListName = "";
                webmodel.ListID = "";
                webmodel.ListUrl = "";
                webmodel.FolderNumber = web.Folders.Count;
                webmodel.FileNumber = web.Files.Count;
                webmodel.Storage = storage;//转换为 M单位 
                webmodel.Owners = web.SiteAdministrators[0].Name;
                webmodel.DesitionType = 0;//0:站点 1:列表
                webmodel.Created = DateTime.Now; //web.Created;
                webmodel.CreatorAccount = web.Author.LoginName;
                webmodel.CreatorUserName = web.Author.Name;
                webmodel.ParentWebID = web.ParentWebId.ToString("N");
                storageList.Add(webmodel);
            }
            catch { }
        }

    }
}
