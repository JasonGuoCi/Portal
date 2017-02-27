using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using Envision.SPS.API;
using Envision.SPS.DataAccess;
using System.Collections.Generic;
using System.Web.UI;
using Envision.SPS.Utility.Utilities;
using System.Linq;
using NPOI.XSSF.UserModel;
using System.Web;

namespace Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.ITMonitor
{
    public partial class ITSiteTypeView : LayoutsPageBase
    {
        private Guid siteId;
        private Guid webId;
        static List<AllDocs> docList = new List<AllDocs>();
        DBBase spdb = new DBBase();
        protected void Page_Load(object sender, EventArgs e)
        {
            docList = spdb.SP_Envision_GetListSizeResult();
            siteId = new Guid(IBRequest.GetQueryString("siteId"));
            webId = new Guid(IBRequest.GetQueryString("webId"));

            if (!IsPostBack)
            {
                BindPage();
            }
        }

        #region sunxiaolong
        private void BindPage()
        {
            decimal weStorage;
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite spSite = new SPSite(siteId))
                {
                    using (SPWeb web = spSite.OpenWeb(webId))
                    {
                        List<Guid> webIds = new List<Guid>();
                        int webSubCount = 0;
                        int documentLibraryTotalCount = 0;
                        string weburl = web.Url.Replace("http://", "");
                        if (weburl.IndexOf('/') >= 0)
                        {
                            currentWebUrl.Value = weburl.Substring(weburl.IndexOf('/'));
                        }

                        webIds.Add(web.ID);
                        //documentLibraryTotalCount = web.GetListsOfType(SPBaseType.DocumentLibrary).Count;
                        documentLibraryTotalCount = documentLibraryCount(web);

                        GetSubWebId(web, ref webIds, ref webSubCount, ref documentLibraryTotalCount);
                        weStorage = GetSumSize(webIds);

                        ltlName.Text = web.Title;
                        ltlDocumentLibraryCount.Text = documentLibraryTotalCount.ToString();
                        ltlChildSiteCount.Text = webSubCount.ToString();
                        ltlCurrentStorage.Text = weStorage.ToString() + "M";

                    }
                }
            });
        }

        private void GetSubWebId(SPWeb web, ref List<Guid> webIds, ref int webSubCount, ref int documentLibraryTotalCount)
        {
            foreach (SPWeb item in web.Webs)
            {
                webSubCount++;
                //documentLibraryTotalCount += item.GetListsOfType(SPBaseType.DocumentLibrary).Count;
                documentLibraryTotalCount += documentLibraryCount(item);
                webIds.Add(item.ID);
                if (item.Webs.Count > 0)
                {
                    GetSubWebId(item, ref webIds, ref webSubCount, ref documentLibraryTotalCount);
                }
            }
        }
        private int documentLibraryCount(SPWeb web)
        {
            int count = 0;
            foreach (SPList list in web.Lists)
            {
                if (!list.Hidden&&list.BaseType == SPBaseType.DocumentLibrary &&
                            list.BaseTemplate != SPListTemplateType.ListTemplateCatalog &&
                            list.BaseTemplate != SPListTemplateType.DesignCatalog &&
                            list.BaseTemplate == SPListTemplateType.DocumentLibrary &&
                            list.AllowDeletion &&
                            !list.IsSiteAssetsLibrary)
                    count++;
            }
            return count;
        }

        private void GetSubWebId(SPWeb web, ref List<string> webIds)
        {
            foreach (SPWeb item in web.Webs)
            {
                webIds.Add(item.ID.ToString("N"));
                if (item.Webs.Count > 0)
                {
                    GetSubWebId(item, ref webIds);
                }
            }
        }


        private List<string> GetAllWebIdsByWebID()
        {
            List<string> webIds = new List<string>();

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite spSite = new SPSite(siteId))
                {
                    using (SPWeb web = spSite.OpenWeb(webId))
                    {
                        webIds.Add(web.ID.ToString("N"));
                        GetSubWebId(web, ref webIds);
                    }
                }
            });
            return webIds;
        }





        /// <summary>
        /// 获取Site大小
        /// </summary>
        /// <param name="listID"></param>
        /// <returns></returns>
        public static decimal GetSumSize(List<Guid> webIds)
        {
            var sum = (from o in docList where webIds.Contains(o.WebId) select o).Sum(t => Convert.ToInt64(t.Size));
            var sumresult = Convert.ToDecimal(sum) / 1024 / 1024;
            return Math.Round(sumresult, 2);
        }
        #endregion

        #region SetCell
        /// <summary>
        /// Set Cell Value
        /// </summary>
        /// <param name="row_cbCenter"></param>
        /// <param name="cellIndex"></param>
        /// <param name="value"></param>
        private void setCell(NPOI.SS.UserModel.IRow row_cbCenter, int cellIndex, object value)
        {
            NPOI.SS.UserModel.ICell cell_cbCenter = row_cbCenter.GetCell(cellIndex);

            if (cell_cbCenter == null)
            {

                cell_cbCenter = row_cbCenter.CreateCell(cellIndex);
            }

            cell_cbCenter.SetCellValue(value == null ? "" : value.ToString());

        }

        /// <summary>
        /// Set Cell Value
        /// </summary>
        /// <param name="row_cbCenter"></param>
        /// <param name="cellIndex"></param>
        /// <param name="value"></param>
        private void setCell(NPOI.SS.UserModel.IRow row_cbCenter, int cellIndex, object value, NPOI.SS.UserModel.CellType ctype)
        {
            NPOI.SS.UserModel.ICell cell_cbCenter = row_cbCenter.GetCell(cellIndex);

            if (cell_cbCenter == null)
            {
                cell_cbCenter = row_cbCenter.CreateCell(cellIndex);
            }
            cell_cbCenter.SetCellType(ctype);
            if (value == null) return;
            if (cell_cbCenter.CellType == NPOI.SS.UserModel.CellType.NUMERIC)
            {
                double d = 0;
                Double.TryParse(value.ToString(), out d);
                cell_cbCenter.SetCellValue(d);
            }

        }

        private void setCellHead(NPOI.SS.UserModel.IRow row_cbCenter, int cellIndex, object value, ICellStyle cellStyle, ISheet sh)
        {
            NPOI.SS.UserModel.ICell cell_cbCenter = row_cbCenter.GetCell(cellIndex);

            if (cell_cbCenter == null)
            {
                cell_cbCenter = row_cbCenter.CreateCell(cellIndex);
            }
            //sh.SetColumnWidth(0, 15 * 256);
            //sh.SetColumnWidth(1, 35 * 256);
            //sh.SetColumnWidth(2, 15 * 256);
            //sh.SetColumnWidth(3, 25 * 256);

            //cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LIGHT_CORNFLOWER_BLUE.index;
            //cellStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;

            ////设置单元格上下左右边框线  
            //cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
            //cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
            //cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
            //cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            ////边框颜色  
            //cellStyle.BottomBorderColor = HSSFColor.BLACK.index;
            //cellStyle.TopBorderColor = HSSFColor.BLACK.index;
            //cellStyle.LeftBorderColor = HSSFColor.BLACK.index;
            //cellStyle.RightBorderColor = HSSFColor.BLACK.index;



            ////文字水平和垂直对齐方式  
            //cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            //cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.CENTER;
            //是否换行  
            //cellStyle.WrapText = true;
            //缩小字体填充  
            //cellStyle.ShrinkToFit = true;

            cell_cbCenter.CellStyle = cellStyle;

            cell_cbCenter.SetCellValue(value == null ? "" : value.ToString());
        }

        private void setCell(NPOI.SS.UserModel.IRow row_cbCenter, int cellIndex, object value, ICellStyle cellStyle, ISheet sh)
        {
            NPOI.SS.UserModel.ICell cell_cbCenter = row_cbCenter.GetCell(cellIndex);

            if (cell_cbCenter == null)
            {

                cell_cbCenter = row_cbCenter.CreateCell(cellIndex);
            }
            //sh.SetColumnWidth(0, 15 * 256);
            //sh.SetColumnWidth(1, 35 * 256);
            //sh.SetColumnWidth(2, 15 * 256);
            //sh.SetColumnWidth(3, 25 * 256);

            //cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LIGHT_CORNFLOWER_BLUE.index;
            //cellStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;

            ////设置单元格上下左右边框线  
            //cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
            //cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
            //cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
            //cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            ////边框颜色  
            //cellStyle.BottomBorderColor = HSSFColor.BLACK.index;
            //cellStyle.TopBorderColor = HSSFColor.BLACK.index;
            //cellStyle.LeftBorderColor = HSSFColor.BLACK.index;
            //cellStyle.RightBorderColor = HSSFColor.BLACK.index;


            ////文字水平和垂直对齐方式  
            //cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            //cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.CENTER;
            //是否换行  
            //cellStyle.WrapText = true;
            //缩小字体填充  
            //cellStyle.ShrinkToFit = true;

            cell_cbCenter.CellStyle = cellStyle;

            cell_cbCenter.SetCellValue(value == null ? "" : value.ToString());
        }
        #endregion

        /// <summary>
        /// 统计报表导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMonitorExport_Click(object sender, EventArgs e)
        {
            SPSite site = new SPSite(siteId);
            SPWeb web = site.OpenWeb(webId);
            //string newGuid = Guid.NewGuid().ToString("N");
            //下载文件名称
            string newGuid = web.Title+ "_统计报表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "";
            //XLS
            //string oFilePath = MapPath("../../") + "TemplateFiles/IT_MonitoringReport.xls";
            //string createFilePath = MapPath("../../") + "TemplateFiles/Files/" + newGuid + ".xls";
            //XLSX
            string oFilePath = MapPath("../../") + "TemplateFiles/IT_MonitoringReport.xlsx";
            string createFilePath = MapPath("../../") + "TemplateFiles/Files/" + newGuid + ".xlsx";

            //文件下载路径
            //string WebPath = System.Configuration.ConfigurationManager.AppSettings["UploadReportFileWebPath"];
            string WebPath = "/_layouts/15/EnvisionDoc/Pages/Down.aspx?FileName=" + HttpUtility.UrlEncode(newGuid) + ".xlsx";
            //xls
            //string reportWebPath = WebPath + newGuid + ".xls";
            //XLSX
            string reportWebPath = WebPath;
            try
            {
                System.IO.File.Copy(oFilePath, createFilePath, true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.btnMonitorExport, this.GetType(), "script", "alert('未找到Excel模板文件！');", true);
                return;
            }

            try
            {
                FileStream file = new FileStream(createFilePath, FileMode.Open, FileAccess.Read);
                //XLS
                //HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
                //HSSFSheet DataSheet = (HSSFSheet)hssfworkbook.GetSheetAt(0);
                //XLSX
                XSSFWorkbook hssfworkbook = new XSSFWorkbook(file);
                XSSFSheet DataSheet = (XSSFSheet)hssfworkbook.GetSheetAt(0);
                DataSheet.SetColumnWidth(0, 15 * 256);
                DataSheet.SetColumnWidth(1, 35 * 256);
                DataSheet.SetColumnWidth(2, 15 * 256);
                DataSheet.SetColumnWidth(3, 25 * 256);

                ICellStyle cellStyle = hssfworkbook.CreateCellStyle();
                //设置单元格上下左右边框线  
                cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
                cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
                cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
                cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
                //边框颜色  
                cellStyle.BottomBorderColor = HSSFColor.BLACK.index;
                cellStyle.TopBorderColor = HSSFColor.BLACK.index;
                cellStyle.LeftBorderColor = HSSFColor.BLACK.index;
                cellStyle.RightBorderColor = HSSFColor.BLACK.index;
                //文字水平和垂直对齐方式  
                cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.CENTER;

                ReportBO bo = new ReportBO();
                //获取最新数据
                List<SPS_Storage> latestStorageList = GetWebStorageItem();
                //查询数据库信息
                List<string> webIdStrings = GetAllWebIdsByWebID();
                List<SPS_Storage> querylist = bo.GetSPS_Storage_QueryListByWebID(webIdStrings);
                querylist.AddRange(latestStorageList);
                querylist = querylist.OrderBy(o => o.WebName).ToList();

                //if (querylist == null || querylist.Count == 0) return;
                int cnt = querylist.Count;

                if (cnt > 0)
                {
                    for (int i = 0; i < cnt; i++)
                    {
                        try
                        {
                            NPOI.SS.UserModel.IRow r = DataSheet.GetRow(i + 1);
                            var item = querylist[i];

                            //循环行第一行
                            if (r == null)
                            {
                                r = DataSheet.CreateRow(i + 1);
                            }

                            //循环行区域赋值
                            if (item.WebName != null && !string.IsNullOrEmpty(item.WebName.ToString()))
                                setCell(r, 0, item.WebName, cellStyle, DataSheet);
                            else
                                setCell(r, 0, string.Empty, cellStyle, DataSheet);

                            setCell(r, 1, item.WebUrl, cellStyle, DataSheet);

                            setCell(r, 2, Convert.ToDecimal(item.Storage.ToString()), cellStyle, DataSheet);

                            setCell(r, 3, item.Owners, cellStyle, DataSheet);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }


                    FileStream file2 = null;
                    try
                    {
                        file2 = new FileStream(createFilePath, FileMode.Create);
                    }
                    catch (Exception)
                    {
                        ScriptManager.RegisterStartupScript(this.btnMonitorExport, this.GetType(), "script", "alert('请检查模板文件权限！');", true);
                        return;
                    }

                    hssfworkbook.Write(file2);
                    file2.Close();
                    this.hidDownLoadURL.Value = reportWebPath;


                    //FileInfo DownloadFile = new FileInfo(createFilePath);
                    //Response.Clear();
                    //Response.ClearHeaders();
                    //Response.Buffer = false;
                    //Response.ContentType = "application/x-excel";
                    //Response.AddHeader("Content-Disposition", "attachment;filename=" + string.Format("{0}_{1}.{2}", "IT_MonitoringReport", DateTime.Now.ToString("yyyyMMddhhmmss"), "xls"));
                    //Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
                    //Response.WriteFile(DownloadFile.FullName);
                    //Response.Flush();
                    //Response.End();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.btnMonitorExport, this.GetType(), "script", "alert('未找到可导出数据！');", true);
                    return;
                }
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript1", "<script>hideLoading();</script>");
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\log.txt", System.Environment.NewLine + "Error--" + System.DateTime.Now + " --" + ex.Message + "--》" + ex.StackTrace);
            }
        }



        /// <summary>
        /// 获取最新的站点监控信息
        /// </summary>
        /// <param name="selectorLibraries"></param>
        /// <returns></returns>
        private List<SPS_Storage> GetWebStorageItem()
        {
            List<SPS_Storage> storageList = new List<SPS_Storage>();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite spSite = new SPSite(siteId))
                    {
                        using (SPWeb web = spSite.OpenWeb(webId))
                        {
                            InsertWebItem(web, ref storageList);
                            if (web.Webs.Count > 0)
                            {
                                BuildSubWeb(web, ref storageList);
                            }

                        }
                    }
                });
            }
            catch
            {
            }
            return storageList;
        }

        //插入所有站点及子站点信息
        private List<SPS_Storage> BuildSubWeb(SPWeb spweb, ref List<SPS_Storage> storageList)
        {

            foreach (SPWeb web in spweb.Webs)
            {
                InsertWebItem(web, ref storageList);
                if (web.Webs.Count > 0) BuildSubWeb(web, ref storageList);
            }
            return storageList;
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
        /// 获取站点信息
        /// </summary>
        /// <param name="web"></param>
        /// <param name="list"></param>
        /// <param name="storageList"></param>
        private static void InsertWebItem(SPWeb web, ref List<SPS_Storage> storageList)
        {
            try
            {
                List<Guid> webIds = new List<Guid>();
                webIds.Add(web.ID);
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

        /// <summary>
        /// 站点大小
        /// </summary>
        /// <param name="listID"></param>
        /// <returns></returns>
        public static decimal GetSumSize(string webID)
        {
            var sum = (from o in docList where o.WebId.ToString() == webID select o).Sum(t => Convert.ToInt64(t.Size));
            var sumresult = Convert.ToDecimal(sum) / 1024 / 1024;
            return Math.Round(sumresult, 2);
        }




    }
}
