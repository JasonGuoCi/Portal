using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using System.IO;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Web.UI;
using NPOI.XSSF.UserModel;
using System.Web;

namespace Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.Reports
{
    /// <summary>
    /// 审计报表处理类
    /// </summary>
    public partial class AuditReport : LayoutsPageBase
    {
        /// <summary>
        /// PageLoad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //首次加载文档库列表
            if (!IsPostBack)
            {
                try
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        using (SPWeb web = SPContext.Current.Site.OpenWeb())
                        {
                            SPListCollection splistconllection = web.GetListsOfType(SPBaseType.DocumentLibrary);
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<li class='c_li c_t1'>");
                            foreach (SPList splist in splistconllection)
                            {
                                if (splist.BaseType == SPBaseType.DocumentLibrary && splist.BaseTemplate != SPListTemplateType.ListTemplateCatalog && splist.BaseTemplate != SPListTemplateType.DesignCatalog && splist.BaseTemplate == SPListTemplateType.DocumentLibrary && splist.AllowDeletion && splist.AllowDeletion && !splist.IsSiteAssetsLibrary)
                                {
                                    if ((web.DoesUserHavePermissions(SPBasePermissions.ManageWeb)) || splist.DoesUserHavePermissions(SPBasePermissions.FullMask))
                                    {
                                        sb.Append(" <p class='p_div'>");
                                        sb.Append("<input type='checkbox' name='LIDocumentLibrary' value='" + splist.ID.ToString("N") + "' title='" + splist.Title + "' class='checkbox'/><a href='#' class='itm'>" + splist.Title + "</a>");
                                        sb.Append("</p>");
                                    }
                                }
                            }
                            sb.Append("</li>");
                            this.ULDocumentLibrary.InnerHtml = sb.ToString();
                        }
                    });
                }
                catch (Exception ex)
                {

                }
            }
        }

        /// <summary>
        /// 根据审计操作类型 获取审计列表
        /// </summary>
        /// <param name="listTypeS">文档库名称数组</param>
        /// <param name="eventType">Audit操作方式 View Update Delete</param>
        /// <returns></returns>
        public List<CurrentSPAuditEntry> GetAuditList(string[] listTypeS, SPAuditEventType eventType, string start, string end)
        {
            List<CurrentSPAuditEntry> curAuditLists = new List<CurrentSPAuditEntry>();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                    {
                        using (SPWeb web = SPContext.Current.Site.OpenWeb())
                        {
                            foreach (string listType in listTypeS)
                            {
                                SPAuditQuery query = new SPAuditQuery(site);
                                query.SetRangeStart(Convert.ToDateTime(start));
                                //孙晓龙 
                                //选择的结束日期加1，20160410
                                query.SetRangeEnd(Convert.ToDateTime(end).AddHours(15).AddMinutes(59));
                                query.AddEventRestriction(eventType);
                                query.RestrictToList(web.Lists[new Guid(listType)]);
                                SPAuditEntryCollection collection = site.Audit.GetEntries(query);

                                List<SPAuditEntry> Lists = collection.Cast<SPAuditEntry>().ToList();
                                foreach (var item in Lists)
                                {
                                    CurrentSPAuditEntry model = new CurrentSPAuditEntry();
                                    model.UserId = item.UserId;
                                    model.ItemType = item.ItemType;
                                    model.DocLocation = item.DocLocation;
                                    model.Occurred = item.Occurred;
                                    model.EventName = item.EventName;
                                    string userName = web.AllUsers.GetByID(item.UserId).LoginName;
                                    model.UserName = userName.Substring(userName.IndexOf('|') + 1);
                                    model.ItemId = item.ItemId;
                                    model.DocLibName = listType;
                                    curAuditLists.Add(model);

                                }
                            }
                        }
                    }
                });
            }
            catch
            {
            }
            return curAuditLists;
        }

        /// <summary>
        /// 根据审计操作类型 获取审计列表
        /// </summary>
        /// <param name="listType">文档库名称</param>
        /// <param name="eventType">Audit操作方式 View Update Delete</param>
        /// <returns></returns>
        public List<CurrentSPAuditEntry> GetAuditList(string listType, SPAuditEventType eventType, string start, string end)
        {
            List<CurrentSPAuditEntry> auditLists = new List<CurrentSPAuditEntry>();

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                    {
                        using (SPWeb web = SPContext.Current.Site.OpenWeb())
                        {
                            SPAuditQuery query = new SPAuditQuery(site);
                            query.SetRangeStart(Convert.ToDateTime(start));
                            //孙晓龙 
                            //选择的结束日期加1，20160410
                            query.SetRangeEnd(Convert.ToDateTime(end).AddHours(15).AddMinutes(59));
                            query.AddEventRestriction(eventType);
                            query.RestrictToList(web.Lists[listType]);

                            SPAuditEntryCollection collection = site.Audit.GetEntries(query);
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
                                model.DocLibName = listType;
                                auditLists.Add(model);
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

            public string DocLibName { get; set; }

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
        /// 导出到Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            //string newGuid = Guid.NewGuid().ToString("N");
            //下载文件名称
            string newGuid = "审计报表_" + this.ddlOperType.SelectedItem.Text.Trim() +"_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "";

            //string oFilePath = MapPath("../../") + "TemplateFiles/AuditReport.xls";
            //string createFilePath = MapPath("../../") + "TemplateFiles/Files/" + newGuid + ".xls";

            string oFilePath = MapPath("../../") + "TemplateFiles/AuditReport.xlsx";
            string createFilePath = MapPath("../../") + "TemplateFiles/Files/" + newGuid + ".xlsx";

            //文件下载路径
            //string WebPath = System.Configuration.ConfigurationManager.AppSettings["UploadReportFileWebPath"];
            string WebPath = "/_layouts/15/EnvisionDoc/Pages/Down.aspx?FileName=" + HttpUtility.UrlEncode(newGuid) + ".xlsx";
            //string reportWebPath = WebPath + newGuid + ".xls";
            string reportWebPath = WebPath;
            try
            {
                System.IO.File.Copy(oFilePath, createFilePath, true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.btnExport, this.GetType(), "script", "layer.alert('未找到Excel模板文件！');", true);
                return;
            }

            try
            {
                FileStream file = new FileStream(createFilePath, FileMode.Open, FileAccess.Read);
                //XLS
                //HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
                //xlsx
                XSSFWorkbook hssfworkbook = new XSSFWorkbook(file);
                //删除第一个sheet
                hssfworkbook.RemoveSheetAt(0);
                ICellStyle leftstyle = hssfworkbook.CreateCellStyle();
                leftstyle.VerticalAlignment = VerticalAlignment.CENTER;
                leftstyle.Alignment = HorizontalAlignment.LEFT;
                leftstyle.BorderBottom = BorderStyle.THIN;
                leftstyle.BorderLeft = BorderStyle.THIN;
                leftstyle.BorderRight = BorderStyle.THIN;
                leftstyle.BorderTop = BorderStyle.THIN;
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
                string selectedValue = this.hidSelectedValue.Value;
                var selValueArray = selectedValue.Split(',');

                string selectedText = this.hidSelectedText.Value;
                var selTextArray = selectedText.Split(',');

                SPAuditEventType eventType;
                string operType = this.ddlOperType.SelectedValue;
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

                List<CurrentSPAuditEntry> querylist = GetAuditList(selValueArray, eventType, start, end);
                //if (querylist == null || querylist.Count == 0) return;
                int cnt = querylist.Count;
                if (cnt > 0)
                {
                    for (int j = 0; j < selValueArray.Length; j++)
                    {
                        //HSSFSheet DataSheet = (HSSFSheet)hssfworkbook.CreateSheet(selTextArray[j] + " 操作明细");//sheetName
                        //HSSFSheet DataSheet2 = (HSSFSheet)hssfworkbook.CreateSheet(selTextArray[j] + " 操作次数");
                        XSSFSheet DataSheet = (XSSFSheet)hssfworkbook.CreateSheet(selTextArray[j] + " 操作明细");//sheetName
                        XSSFSheet DataSheet2 = (XSSFSheet)hssfworkbook.CreateSheet(selTextArray[j] + " 操作次数");

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
                        setCell(r1, 1, selTextArray[j], cellStyle, DataSheet);

                        setCellHead(r2, 0, "导出日期：", cellStyleHead, DataSheet);
                        setCell(r2, 1, DateTime.Now.ToString("yyyy-MM-dd"), cellStyle, DataSheet);

                        //循环行Title
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

                        //循环行Title
                        setCellHead(rshnum1, 0, "文档位置", cellStyleHead, DataSheet2);
                        setCellHead(rshnum1, 1, "操作次数", cellStyleHead, DataSheet2);

                        List<CurrentSPAuditEntry> list = querylist.Where(o => o.DocLibName.Equals(selValueArray[j])).ToList(); //GetAuditList(selTextArray[j], eventType);
                        //存储操作次数信息
                        Dictionary<string, int> dic = GetOperNumList(list);

                        //操作明细Sheet组装
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
                                //循环行区域赋值
                                setCell(r, 0, item.ItemType, leftstyle, DataSheet);
                                setCell(r, 1, item.UserName, leftstyle, DataSheet);
                                setCell(r, 2, item.DocLocation, leftstyle, DataSheet);
                                if (item.Occurred != null && !string.IsNullOrEmpty(item.Occurred.ToString()))
                                    setCell(r, 3, DateTime.Parse(item.Occurred.AddHours(8).ToString()).ToString("yyyy-MM-dd HH:mm:ss"), cellStyle, DataSheet);
                                else
                                    setCell(r, 3, string.Empty, cellStyle, DataSheet);
                                setCell(r, 4, operType, cellStyle, DataSheet);
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

                    FileStream file2 = null;
                    try
                    {
                        file2 = new FileStream(createFilePath, FileMode.Create);
                    }
                    catch (Exception)
                    {
                        ScriptManager.RegisterStartupScript(this.btnExport, this.GetType(), "script", "layer.alert('请检查模板文件权限！');", true);
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
                    ScriptManager.RegisterStartupScript(this.btnExport, this.GetType(), "script", "layer.alert('未找到可导出数据！');", true);
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
        /// 包含设置表格背景色
        /// </summary>
        /// <param name="row_cbCenter"></param>
        /// <param name="cellIndex"></param>
        /// <param name="value"></param>
        /// <param name="cellStyle"></param>
        /// <param name="sh"></param>
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

        /// <summary>
        /// 不设置表格背景色
        /// </summary>
        /// <param name="row_cbCenter"></param>
        /// <param name="cellIndex"></param>
        /// <param name="value"></param>
        /// <param name="cellStyle"></param>
        /// <param name="sh"></param>
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


    }
}
