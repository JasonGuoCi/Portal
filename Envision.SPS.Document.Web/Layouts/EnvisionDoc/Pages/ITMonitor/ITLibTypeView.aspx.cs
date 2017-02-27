using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using System.IO;
using Envision.SPS.API;
using Envision.SPS.DataAccess;
using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using System.Linq;
using System.Web.UI;
using Envision.SPS.Utility.Utilities;
using Envision.SPS.Utility.Models;
using System.Web;
using NPOI.SS.Util;
using System.Text;
using NPOI.XSSF.UserModel;

namespace Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.ITMonitor
{
    public partial class ITLibTypeView : LayoutsPageBase
    {
        private Guid siteId;
        private Guid webId;
        private Guid listId;
        private string listName;
        static List<AllDocs> docList = new List<AllDocs>();
        DBBase spdb = new DBBase();
        protected void Page_Load(object sender, EventArgs e)
        {
            docList = spdb.SP_Envision_GetListSizeResult();
            siteId = new Guid(IBRequest.GetQueryString("siteId"));
            webId = new Guid(IBRequest.GetQueryString("webId"));
            listId = new Guid(IBRequest.GetQueryString("listId"));
            listName = HttpUtility.UrlDecode(IBRequest.GetQueryString("listName"));
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
                        SPList list = web.Lists[listId];
                        string weburl = web.Url.Replace("http://", "");
                        if (weburl.IndexOf('/') >= 0)
                        {
                            currentWebUrl.Value = weburl.Substring(weburl.IndexOf('/'));
                        }

                        int folderCount = 0;
                        folderCount = list.Folders.Count;
                        weStorage = GetSumSize(list.ID);

                        ltlName.Text = list.Title;
                        ltlFoldeCount.Text = folderCount.ToString();
                        ltlFilesCount.Text = (list.ItemCount - folderCount).ToString();
                        ltlCurrentStorage.Text = weStorage.ToString() + "M";
                    }
                }
            });


        }

        /// <summary>
        /// 获取文档库大小
        /// </summary>
        /// <param name="listID"></param>
        /// <returns></returns>
        public static decimal GetSumSize(Guid listId)
        {
            var sum = (from o in docList where o.ListId == listId select o).Sum(t => Convert.ToInt64(t.Size));
            var sumresult = Convert.ToDecimal(sum) / 1024 / 1024;
            return Math.Round(sumresult, 2);
        }
        #endregion

        #region SetCell
        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowstart"></param>
        /// <param name="rowend"></param>
        /// <param name="colstart"></param>
        /// <param name="colend"></param>
        public static void SetCellRangeAddress(ISheet sheet, int rowstart, int rowend, int colstart, int colend)
        {
            CellRangeAddress cellRangeAddress = new CellRangeAddress(rowstart, rowend, colstart, colend);
            sheet.AddMergedRegion(cellRangeAddress);
        }
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

        #region 权限报表
        #region 数据操作

        /// <summary>
        /// 验证数据是否完整
        /// </summary>
        /// <param name="SiteId">站点集ID</param>
        /// <param name="WebId">站点ID</param>
        /// <param name="ListId">文件库ID</param>
        /// <param name="EventName">事件名称</param>
        /// <param name="UserName">用户名称</param>
        /// <param name="UserId">用户ID</param>
        /// <param name="UsersEmail">用户邮箱地址</param>
        /// <param name="Status">状态</param>
        /// <param name="IsEmail">是否发送邮件</param>
        /// <param name="FileName">文件名</param>
        /// <param name="FilePath">文件路径</param>
        /// <returns></returns>
        public SPS_EventBus VerifyEventBus(string SiteId, string WebId, string ListId, string EventName, string UserName, string UserId, string UsersEmail, byte Status, bool IsEmail, string FileName, string FilePath)
        {
            SPS_EventBus spsModel = new SPS_EventBus();
            if (SiteId == string.Empty || SiteId == "") return null; else spsModel.SiteID = SiteId;
            if (WebId == string.Empty || WebId == "") return null; else spsModel.WebID = WebId;
            if (ListId == string.Empty || ListId == "") return null; else spsModel.ListID = ListId;
            if (EventName == string.Empty || EventName == "") return null; else spsModel.EventName = EventName;
            spsModel.EventTime = DateTime.Now;
            if (UserName == string.Empty || UserName == "") return null; else spsModel.UserName = UserName;
            if (UserId == string.Empty || UserId == "") return null; else spsModel.UserID = UserId;
            if (UsersEmail == null) spsModel.UserEmail = ""; else spsModel.UserEmail = UsersEmail;
            //spsModel.UserEmail = "791558255@qq.com";
            spsModel.Status = 0;
            spsModel.IsEmail = false;
            if (FileName == string.Empty || FileName == "") return null; else spsModel.FileName = FileName;
            if (FilePath == string.Empty || FilePath == "") return null; else spsModel.FilePath = FilePath;
            spsModel.CreatedTime = DateTime.Now;
            return spsModel;

        }
        #endregion
        /// <summary>
        /// 导出权限事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPermissionExport_Click(object sender, EventArgs e)
        {
            string Result = string.Empty;
            try
            {
                //string newGuid = DateTime.Now.ToString("yyyyMMddHHmmss") + "_PermissionsReport";
                string oFilePath = MapPath("../../TemplateFiles/LibraryPermissionsReport.xlsx");
                string createFilePath = MapPath("../../") + "TemplateFiles/Files/";
                DataAccess.EventBusDAL dal = new EventBusDAL();
                long Id = 0;
                
                using (SPSite sit = new SPSite(siteId))
                {
                    using (SPWeb web = sit.OpenWeb(webId))
                    {
                        string newGuid = web.Lists[listId].Title + "_权限报表_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".xlsx" + "";
                        SPS_EventBus spsEventModel = VerifyEventBus(siteId.ToString(), webId.ToString(), listId.ToString(), "PermmisionExport", web.Users.Web.CurrentUser.Name, web.Users.Web.CurrentUser.ID.ToString(), web.Users.Web.CurrentUser.Email.ToString(), Convert.ToByte("0"), false, newGuid, createFilePath);
                        if (dal.AddSPS_EventBus(spsEventModel, ref Id))
                        {
                            // Result = "hideLoading();layer.msg(\"操作成功,正在为您导出权限信息!\");";
                            Response.Redirect("../Reports/PermissionsReportLoading.aspx?id=" + Id, false);
                        }
                        else
                        {
                            Result = "hideLoading();layer.msg(\"操作失败!\");";
                        }
                    }
                }


            }
            catch
            {
                Result = "hideLoading();layer.msg(\"操作失败!\");";
            }
        }
        #endregion

        #region 审计报表
        /// <summary>
        /// 根据审计操作类型 获取审计列表
        /// </summary>
        /// <param name="listTypeS">文档库名称数组</param>
        /// <param name="eventType">Audit操作方式 View Update Delete</param>
        /// <returns></returns>
        public List<SPAuditEntry> GetAuditList(string[] listTypeS, SPAuditEventType eventType)
        {
            List<SPAuditEntry> auditLists = new List<SPAuditEntry>();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPWeb web = SPContext.Current.Site.OpenWeb(this.webId))
                    {
                        foreach (string listType in listTypeS)
                        {
                            SPAuditQuery query = new SPAuditQuery(web.Site);
                            query.AddEventRestriction(eventType);
                            query.RestrictToList(web.Lists[listType]);
                            SPAuditEntryCollection collection = web.Site.Audit.GetEntries(query);

                            List<SPAuditEntry> Lists = collection.Cast<SPAuditEntry>().ToList();
                            foreach (var item in Lists)
                            {
                                //项类型        item.ItemType
                                //用户标识项    item.UserId
                                //文档路径      item.DocLocation
                                //发生时间      item.Occurred
                                //事件          item.EventName
                                auditLists.Add(item);

                            }
                        }
                    }
                });
            }
            catch
            {
            }
            return auditLists;
        }


        public List<SPAuditEntry> GetAuditList(Guid listType, SPAuditEventType eventType)
        {
            List<SPAuditEntry> auditLists = new List<SPAuditEntry>();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPWeb web = SPContext.Current.Site.OpenWeb(this.webId))
                    {
                        SPAuditQuery query = new SPAuditQuery(web.Site);
                        query.AddEventRestriction(eventType);

                        query.RestrictToList(web.Lists[listType]);
                        SPAuditEntryCollection collection = web.Site.Audit.GetEntries(query);

                        List<SPAuditEntry> Lists = collection.Cast<SPAuditEntry>().ToList();
                        foreach (var item in Lists)
                        {
                            //项类型        item.ItemType
                            //用户标识项    item.UserId
                            //文档路径      item.DocLocation
                            //发生时间      item.Occurred
                            //事件          item.EventName
                            auditLists.Add(item);
                        }
                    }
                });
            }
            catch
            {
            }
            return auditLists;
        }

        /// <summary>
        /// 根据审计操作类型 获取审计列表
        /// </summary>
        /// <param name="listType">文档库名称</param>
        /// <param name="eventType">Audit操作方式 View Update Delete</param>
        /// <returns></returns>
        public List<CurrentSPAuditEntry> GetAuditList(string listType, SPAuditEventType eventType)
        {
            List<CurrentSPAuditEntry> auditLists = new List<CurrentSPAuditEntry>();

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPWeb web = SPContext.Current.Site.OpenWeb(this.webId))
                    {
                        SPAuditQuery query = new SPAuditQuery(web.Site);
                        query.AddEventRestriction(eventType);
                        query.RestrictToList(web.Lists[listType]);

                        SPAuditEntryCollection collection = web.Site.Audit.GetEntries(query);
                        List<SPAuditEntry> Lists = collection.Cast<SPAuditEntry>().ToList();

                        foreach (var item in Lists)
                        {
                            CurrentSPAuditEntry model = new CurrentSPAuditEntry();
                            model.UserId = item.UserId;
                            model.ItemType = item.ItemType;
                            model.DocLocation = item.DocLocation;
                            model.Occurred = item.Occurred;
                            model.EventName = item.EventName;
                            model.UserName = web.AllUsers.GetByID(item.UserId).LoginName;
                            model.ItemId = item.ItemId;
                            auditLists.Add(model);
                        }
                    }
                });
            }
            catch
            {
            }
            return auditLists;
        }

        public List<CurrentSPAuditEntry> GetCurrentSPAuditList(Guid listType, SPAuditEventType eventType, string start, string end)
        {
            List<CurrentSPAuditEntry> auditLists = new List<CurrentSPAuditEntry>();

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPWeb web = SPContext.Current.Site.OpenWeb(this.webId))
                    {
                        SPAuditQuery query = new SPAuditQuery(web.Site);
                        query.SetRangeStart(Convert.ToDateTime(start));
                        //孙晓龙 
                        //选择的结束日期加1，20160410
                        query.SetRangeEnd(Convert.ToDateTime(end).AddHours(15).AddMinutes(59));
                        query.AddEventRestriction(eventType);
                        query.RestrictToList(web.Lists[listType]);

                        SPAuditEntryCollection collection = web.Site.Audit.GetEntries(query);
                        List<SPAuditEntry> Lists = collection.Cast<SPAuditEntry>().ToList();

                        foreach (var item in Lists)
                        {
                            CurrentSPAuditEntry model = new CurrentSPAuditEntry();
                            model.UserId = item.UserId;
                            model.ItemType = item.ItemType;
                            model.DocLocation = item.DocLocation;
                            model.Occurred = item.Occurred;
                            model.EventName = item.Event.ToString();
                            string userName = web.AllUsers.GetByID(item.UserId).LoginName;
                            model.UserName = userName.Substring(userName.IndexOf('|') + 1);
                            model.ItemId = item.ItemId;
                            auditLists.Add(model);
                        }
                    }
                });
            }
            catch
            {
            }
            return auditLists;
        }

        public int GetCurrentSPAuditListCount(Guid listType, SPAuditEventType eventType, string start, string end)
        {
            List<SPAuditEntry> Lists = new List<SPAuditEntry>();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPWeb web = SPContext.Current.Site.OpenWeb(this.webId))
                    {
                        SPAuditQuery query = new SPAuditQuery(web.Site);
                        query.SetRangeStart(Convert.ToDateTime(start));
                        //孙晓龙 
                        //选择的结束日期加1，20160410
                        query.SetRangeEnd(Convert.ToDateTime(end).AddHours(15).AddMinutes(59));
                        query.AddEventRestriction(eventType);
                        query.RestrictToList(web.Lists[listType]);

                        SPAuditEntryCollection collection = web.Site.Audit.GetEntries(query);
                        Lists = collection.Cast<SPAuditEntry>().ToList();
                    }
                });
            }
            catch
            {
            }
            return Lists.Count;
        }




        /// <summary>
        /// 自定义SPAuditEntry
        /// </summary>
        public class CurrentSPAuditEntry
        {
            public int? AppPrincipalId { get; set; }
            public string DocLocation { get; set; }
            public SPAuditEventType Event { get; set; }
            public string EventData { get; set; }
            public string EventName { get; set; }
            public SPAuditEventSource EventSource { get; set; }
            public Guid ItemId { get; set; }
            public SPAuditItemType ItemType { get; set; }
            public SPAuditLocationType LocationType { get; set; }
            public string MachineIP { get; set; }
            public string MachineName { get; set; }
            public DateTime Occurred { get; set; }
            public Guid SiteId { get; set; }
            public string SourceName { get; set; }
            public int UserId { get; set; }

            public string UserName { get; set; }

        }

        /// <summary>
        /// 判断文件操作次数信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public Dictionary<string, int> GetOperNumList(List<CurrentSPAuditEntry> list)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            foreach (var item in list)
            {
                if (!dic.ContainsKey(item.DocLocation))
                {
                    dic.Add(item.DocLocation, 1);
                }
                else
                {
                    dic[item.DocLocation] = dic[item.DocLocation] + 1;
                }
            }
            return dic;

        }

        /// <summary>
        /// 审计报表导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAduitExport_Click(object sender, EventArgs e)
        {
            //string newGuid = Guid.NewGuid().ToString("N");
            //下载文件名称
            string newGuid = listName + "_审计报表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "";
            //XLS
            //string oFilePath = MapPath("../../") + "TemplateFiles/AuditReport.xls";
            //string createFilePath = MapPath("../../") + "TemplateFiles/Files/" + newGuid + ".xls";
            //XLSX
            string oFilePath = MapPath("../../") + "TemplateFiles/AuditReport.xlsx";
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
                ScriptManager.RegisterStartupScript(this.btnAduitExport, this.GetType(), "script", "layer.alert('未找到Excel模板文件！');", true);
                return;
            }

            try
            {
                FileStream file = new FileStream(createFilePath, FileMode.Open, FileAccess.Read);
                //XLS
                //HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
                //XLSX
                XSSFWorkbook hssfworkbook = new XSSFWorkbook(file);
                //删除第一个sheet
                hssfworkbook.RemoveSheetAt(0);

                ICellStyle cellStyleHead = hssfworkbook.CreateCellStyle();
                cellStyleHead.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LIGHT_CORNFLOWER_BLUE.index;
                cellStyleHead.FillPattern = FillPatternType.SOLID_FOREGROUND;

                //设置单元格上下左右边框线  
                cellStyleHead.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
                cellStyleHead.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
                cellStyleHead.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
                cellStyleHead.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
                //边框颜色  
                cellStyleHead.BottomBorderColor = HSSFColor.BLACK.index;
                cellStyleHead.TopBorderColor = HSSFColor.BLACK.index;
                cellStyleHead.LeftBorderColor = HSSFColor.BLACK.index;
                cellStyleHead.RightBorderColor = HSSFColor.BLACK.index;
                //文字水平和垂直对齐方式  
                cellStyleHead.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                cellStyleHead.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.CENTER;
                ICellStyle leftstyle = hssfworkbook.CreateCellStyle();
                leftstyle.VerticalAlignment = VerticalAlignment.CENTER;
                leftstyle.Alignment = HorizontalAlignment.LEFT;
                leftstyle.BorderBottom = BorderStyle.THIN;
                leftstyle.BorderLeft = BorderStyle.THIN;
                leftstyle.BorderRight = BorderStyle.THIN;
                leftstyle.BorderTop = BorderStyle.THIN;
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


                SPAuditEventType eventType;
                string operType = "查看"; //this.ddlOperType.SelectedValue;
                switch (operType)
                {
                    case "查看":
                        eventType = SPAuditEventType.View;
                        break;
                    case "删除":
                        eventType = SPAuditEventType.Delete;
                        break;
                    case "修改":
                        eventType = SPAuditEventType.Update;
                        break;

                    default:
                        eventType = SPAuditEventType.View;
                        break;
                }

                var start = Convert.ToDateTime(this.calStart.Text).ToString("yyyy-MM-dd");
                var end = Convert.ToDateTime(this.calEnd.Text).ToString("yyyy-MM-dd");




                //XLS
                //HSSFSheet DataSheet = (HSSFSheet)hssfworkbook.CreateSheet("操作明细");//sheetName
                //HSSFSheet DataSheet2 = (HSSFSheet)hssfworkbook.CreateSheet("操作次数");
                //XLSX
                XSSFSheet DataSheet = (XSSFSheet)hssfworkbook.CreateSheet("操作明细");//sheetName
                XSSFSheet DataSheet2 = (XSSFSheet)hssfworkbook.CreateSheet("操作次数");
                DataSheet.SetColumnWidth(0, 15 * 256);
                DataSheet.SetColumnWidth(1, 35 * 256);
                DataSheet.SetColumnWidth(2, 85 * 256);
                DataSheet.SetColumnWidth(3, 25 * 256);

                DataSheet2.SetColumnWidth(0, 85 * 256);
                DataSheet2.SetColumnWidth(1, 15 * 256);

                //操作操作明细Sheet
                NPOI.SS.UserModel.IRow r1 = DataSheet.GetRow(1);
                NPOI.SS.UserModel.IRow r2 = DataSheet.GetRow(2);
                NPOI.SS.UserModel.IRow r4 = DataSheet.GetRow(4);

                //固定前两行 title 行
                if (r1 == null)
                {
                    r1 = DataSheet.CreateRow(1);
                }
                if (r2 == null)
                {
                    r2 = DataSheet.CreateRow(2);
                }

                if (r4 == null)
                {
                    r4 = DataSheet.CreateRow(4);
                }

                //固定前两行 Title行赋值
                setCellHead(r1, 0, "文档库名称：", cellStyleHead, DataSheet);
                setCell(r1, 1, listName, cellStyle, DataSheet);

                setCellHead(r2, 0, "导出日期：", cellStyleHead, DataSheet);
                setCell(r2, 1, DateTime.Now.ToString("yyyy-MM-dd"), cellStyle, DataSheet);


                setCellHead(r4, 0, "项类型", cellStyleHead, DataSheet);
                setCellHead(r4, 1, "用户标识号", cellStyleHead, DataSheet);
                setCellHead(r4, 2, "文档位置", cellStyleHead, DataSheet);
                setCellHead(r4, 3, "发生时间", cellStyleHead, DataSheet);
                setCellHead(r4, 4, "事件", cellStyleHead, DataSheet);

                //操作次数sheet 行title
                NPOI.SS.UserModel.IRow rshnum1 = DataSheet2.GetRow(1);

                //固定行 title 行
                if (rshnum1 == null)
                {
                    rshnum1 = DataSheet2.CreateRow(1);
                }

                setCellHead(rshnum1, 0, "文档位置", cellStyleHead, DataSheet2);
                setCellHead(rshnum1, 1, "操作次数", cellStyleHead, DataSheet2);

                int viewCount = GetCurrentSPAuditListCount(listId, SPAuditEventType.View, start, end);
                int updateCount = GetCurrentSPAuditListCount(listId, SPAuditEventType.Update, start, end);
                int deleteCount = GetCurrentSPAuditListCount(listId, SPAuditEventType.Delete, start, end);

                int cnt = viewCount + updateCount + deleteCount;
                if (cnt > 0)
                {
                    List<CurrentSPAuditEntry> list = GetCurrentSPAuditList(listId, SPAuditEventType.View, start, end);
                    List<CurrentSPAuditEntry> updatelist = GetCurrentSPAuditList(listId, SPAuditEventType.Update, start, end);
                    List<CurrentSPAuditEntry> deletelist = GetCurrentSPAuditList(listId, SPAuditEventType.Delete, start, end);
                    list.AddRange(updatelist);
                    list.AddRange(deletelist);

                    //存储操作次数信息
                    Dictionary<string, int> dic = GetOperNumList(list);

                    //操作明细Sheet组装
                    if (cnt < 600000)
                    {
                        for (int i = 0; i < cnt; i++)
                        {
                            try
                            {
                                NPOI.SS.UserModel.IRow r = DataSheet.GetRow(i + 5);
                                var item = list[i];
                                //循环行第一行
                                if (r == null)
                                {
                                    r = DataSheet.CreateRow(i + 5);
                                }
                                //循环行区域赋值
                                setCell(r, 0, item.ItemType, leftstyle, DataSheet);
                                setCell(r, 1, item.UserName, leftstyle, DataSheet);
                                setCell(r, 2, item.DocLocation, leftstyle, DataSheet);
                                if (item.Occurred != null && !string.IsNullOrEmpty(item.Occurred.ToString()))
                                    setCell(r, 3, DateTime.Parse(item.Occurred.AddHours(8).ToString()).ToString("yyyy-MM-dd HH:mm:ss"), cellStyle, DataSheet);
                                else
                                    setCell(r, 3, string.Empty, cellStyle, DataSheet);
                                setCell(r, 4, item.EventName, cellStyle, DataSheet);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }
                        //操作次数Sheet组装
                        int m = 0;
                        foreach (KeyValuePair<string, int> kv in dic)
                        {
                            try
                            {
                                NPOI.SS.UserModel.IRow r = DataSheet2.GetRow(m + 2);
                                //循环行第一行
                                if (r == null)
                                {
                                    r = DataSheet2.CreateRow(m + 2);
                                }
                                //循环行区域赋值
                                setCell(r, 0, kv.Key, leftstyle, DataSheet2);
                                setCell(r, 1, kv.Value.ToString(), cellStyle, DataSheet2);
                                m++;
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.btnAduitExport, this.GetType(), "script", "layer.alert('当前将会导出" + cnt + "条数据.由于数据量过大,请缩小审计导出日期区间再进行导出!');", true);
                        return;
                    }




                    FileStream file2 = null;
                    try
                    {
                        file2 = new FileStream(createFilePath, FileMode.Create);
                    }
                    catch (Exception)
                    {
                        ScriptManager.RegisterStartupScript(this.btnAduitExport, this.GetType(), "script", "layer.alert('请检查模板文件权限！');", true);
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
                    //Response.AddHeader("Content-Disposition", "attachment;filename=" + string.Format("{0}_{1}.{2}", "AuditReport", DateTime.Now.ToString("yyyyMMddhhmmss"), "xls"));
                    //Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
                    //Response.WriteFile(DownloadFile.FullName);
                    //Response.Flush();
                    //Response.End();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.btnAduitExport, this.GetType(), "script", "layer.alert('未找到可导出数据！');", true);
                    return;
                }
                //关闭Loading
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript1", "<script>hideLoading();</script>");
            }
            catch (Exception ex)
            {
                //System.IO.File.AppendAllText(@"C:\log.txt", System.Environment.NewLine + "Error--" + System.DateTime.Now + " --" + ex.Message + "--》" + ex.StackTrace);
                this.hidDownLoadURL.Value = reportWebPath;
            }

        }
        #endregion

        #region 统计报表
        /// <summary>
        /// 统计报表导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMonitorExport_Click(object sender, EventArgs e)
        {
            //string newGuid = Guid.NewGuid().ToString("N");
            //下载文件名称
            string newGuid = listName + "_统计报表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "";
            //XLS
            //string oFilePath = MapPath("../../") + "TemplateFiles/DocumentLibraryMonitoringReport.xls";
            //string createFilePath = MapPath("../../") + "TemplateFiles/Files/" + newGuid + ".xls";
            //XLSX
            string oFilePath = MapPath("../../") + "TemplateFiles/DocumentLibraryMonitoringReport.xlsx";
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
                ScriptManager.RegisterStartupScript(this.btnMonitorExport, this.GetType(), "script", "layer.alert('未找到Excel模板文件！');", true);
                return;
            }

            try
            {
                FileStream file = new FileStream(createFilePath, FileMode.Open, FileAccess.Read);
                //xls
                //HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
                //XLSX
                XSSFWorkbook hssfworkbook = new XSSFWorkbook(file);


                //删除第一sheet
                hssfworkbook.RemoveSheetAt(0);

                ICellStyle cellStyleHead = hssfworkbook.CreateCellStyle();
                cellStyleHead.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LIGHT_CORNFLOWER_BLUE.index;
                cellStyleHead.FillPattern = FillPatternType.SOLID_FOREGROUND;

                //设置单元格上下左右边框线  
                cellStyleHead.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
                cellStyleHead.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
                cellStyleHead.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
                cellStyleHead.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
                //边框颜色  
                cellStyleHead.BottomBorderColor = HSSFColor.BLACK.index;
                cellStyleHead.TopBorderColor = HSSFColor.BLACK.index;
                cellStyleHead.LeftBorderColor = HSSFColor.BLACK.index;
                cellStyleHead.RightBorderColor = HSSFColor.BLACK.index;

                //文字水平和垂直对齐方式  
                cellStyleHead.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                cellStyleHead.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.CENTER;

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

                //获取前端选中值             

                ReportBO bo = new ReportBO();
                //获取最新数据
                List<SPS_Storage> storagelist = GetStorageItem(listId);
                //查询快照表
                List<SPS_Storage> querylist = bo.GetSPS_Storage_QueryList(listId.ToString("N"));
                //将最新数据添加到查询出来的快照表数据中
                querylist.AddRange(storagelist);
                querylist = querylist.OrderByDescending(o => o.Created).ToList();

                //if (querylist == null || querylist.Count == 0) return;
                int cnt = querylist.Count;

                if (cnt > 0)
                {
                    // for (int j = 0; j < listId.ToString("N").Length; j++)
                    // {
                    //XLS
                    //HSSFSheet DataSheet = (HSSFSheet)hssfworkbook.CreateSheet("监控列表");//sheetName
                    //XLSX
                    XSSFSheet DataSheet = (XSSFSheet)hssfworkbook.CreateSheet("监控列表");//sheetName
                    DataSheet.SetColumnWidth(0, 15 * 256);
                    DataSheet.SetColumnWidth(1, 35 * 256);
                    DataSheet.SetColumnWidth(2, 15 * 256);
                    DataSheet.SetColumnWidth(3, 25 * 256);


                    NPOI.SS.UserModel.IRow r1 = DataSheet.GetRow(1);
                    NPOI.SS.UserModel.IRow r2 = DataSheet.GetRow(2);
                    NPOI.SS.UserModel.IRow r4 = DataSheet.GetRow(4);

                    //固定前两行 title 行
                    if (r1 == null)
                    {
                        r1 = DataSheet.CreateRow(1);
                    }
                    if (r2 == null)
                    {
                        r2 = DataSheet.CreateRow(2);
                    }

                    if (r4 == null)
                    {
                        r4 = DataSheet.CreateRow(4);
                    }

                    setCellHead(r4, 0, "日期", cellStyleHead, DataSheet);
                    setCellHead(r4, 1, "文件夹数", cellStyleHead, DataSheet);
                    setCellHead(r4, 2, "文件数", cellStyleHead, DataSheet);
                    setCellHead(r4, 3, "容量(MB)", cellStyleHead, DataSheet);

                    List<SPS_Storage> list = querylist;//.Where(o => o.ListID.Equals(selValueArray[j])).ToList();
                    int curcnt = list.Count;
                    for (int i = 0; i < curcnt; i++)
                    {
                        try
                        {
                            NPOI.SS.UserModel.IRow r = DataSheet.GetRow(i + 5);
                            var item = list[i];

                            //循环行第一行
                            if (r == null)
                            {
                                r = DataSheet.CreateRow(i + 5);
                            }

                            //固定前两行 Title行赋值
                            setCellHead(r1, 0, "站点名称：", cellStyleHead, DataSheet);
                            setCell(r1, 1, item.WebName.ToString(), cellStyle, DataSheet);
                            setCellHead(r1, 2, "文档库名称：", cellStyleHead, DataSheet);
                            setCell(r1, 3, item.ListName.ToString(), cellStyle, DataSheet);

                            setCellHead(r2, 0, "文档库路径：", cellStyleHead, DataSheet);
                            setCell(r2, 1, item.ListUrl.ToString(), cellStyle, DataSheet);
                            setCellHead(r2, 2, "导出日期：", cellStyleHead, DataSheet);
                            setCell(r2, 3, DateTime.Now.ToString("yyyy-MM-dd"), cellStyle, DataSheet);

                            //循环行区域赋值
                            if (item.Created != null && !string.IsNullOrEmpty(item.Created.ToString()))
                                setCell(r, 0, DateTime.Parse(item.Created.ToString()).ToString("yyyy-MM-dd"), cellStyle, DataSheet);
                            else
                                setCell(r, 0, string.Empty, cellStyle, DataSheet);

                            setCell(r, 1, Convert.ToInt32(item.FolderNumber.ToString()), cellStyle, DataSheet);

                            setCell(r, 2, Convert.ToInt32(item.FileNumber.ToString()), cellStyle, DataSheet);

                            setCell(r, 3, Convert.ToDecimal(item.Storage.ToString() == "" ? "0" : item.Storage.ToString()), cellStyle, DataSheet);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    // }

                    FileStream file2 = null;
                    try
                    {
                        file2 = new FileStream(createFilePath, FileMode.Create);
                    }
                    catch (Exception)
                    {
                        ScriptManager.RegisterStartupScript(this.btnMonitorExport, this.GetType(), "script", "layer.alert('请检查模板文件权限！');", true);
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
                    //Response.AddHeader("Content-Disposition", "attachment;filename=" + string.Format("{0}_{1}.{2}", "DocumentLibraryMonitoringReport", DateTime.Now.ToString("yyyyMMddhhmmss"), "xls"));
                    //Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
                    //Response.WriteFile(DownloadFile.FullName);
                    //Response.Flush();
                    //Response.End();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.btnMonitorExport, this.GetType(), "script", "layer.alert('未找到可导出数据！');", true);
                    return;
                }
                //关闭Loading
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript1", "<script>hideLoading();</script>");
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\log.txt", System.Environment.NewLine + "Error--" + System.DateTime.Now + " --" + ex.Message + "--》" + ex.StackTrace);
            }
        }



        /// <summary>
        /// 获取最新的文档库监控信息
        /// </summary>
        /// <param name="selectorLibraries"></param>
        /// <returns></returns>
        private List<SPS_Storage> GetStorageItem(Guid seletedGuid)
        {
            List<SPS_Storage> storageList = new List<SPS_Storage>();
            //var SPSRootSiteUrl = System.Configuration.ConfigurationManager.AppSettings["SPSRootSiteUrl"];
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite spSite = SPContext.Current.Site)
                    {
                        using (SPWeb web = spSite.OpenWeb(this.webId))
                        {
                            SPList splist = web.Lists[seletedGuid];
                            InsertListItem(web, splist, ref storageList);
                        }
                    }
                });
            }
            catch
            {
            }
            return storageList;
        }

        /// <summary>
        /// 获取文档库信息
        /// </summary>
        /// <param name="web"></param>
        /// <param name="list"></param>
        /// <param name="storageList"></param>
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
                model.FileNumber = list.ItemCount - list.Folders.Count;
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
        /// 获取文档库大小
        /// </summary>
        /// <param name="listID"></param>
        /// <returns></returns>
        public static decimal GetSumSize(string listID)
        {
            var sum = (from o in docList where o.ListId.ToString() == listID select o).Sum(t => Convert.ToInt64(t.Size));
            var sumresult = Convert.ToDecimal(sum) / 1024 / 1024;
            return Math.Round(sumresult, 2);
        }


        #endregion
    }
}
