using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using System.Web.UI;
using System.Collections.Generic;
using Envision.SPS.API;
using Envision.SPS.DataAccess;
using System.Linq;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using NPOI.XSSF.UserModel;
using System.Web;



namespace Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.Reports
{
    /// <summary>
    /// 文档库监控报表处理类
    /// </summary>
    public partial class DocumentLibraryMonitoringReport : LayoutsPageBase
    {
        static List<AllDocs> docList = new List<AllDocs>();
        static DBBase spdb = new DBBase();

        protected void Page_Load(object sender, EventArgs e)
        {

            //首次加载文档库列表
            if (!this.IsPostBack)
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

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            //string newGuid = DateTime.Now.ToString("yyyyMMddHHmmss") + Guid.NewGuid().ToString("N");
            //下载文件名称
            string newGuid = "统计报表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "";

            //xls
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
                ScriptManager.RegisterStartupScript(this.btnExport, this.GetType(), "script", "alert('未找到Excel模板文件！');", true);
                return;
            }

            try
            {
                FileStream file = new FileStream(createFilePath, FileMode.Open, FileAccess.Read);
                //XLS
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
                string selectedValue = this.hidSelectedValue.Value;
                var selValueArray = selectedValue.Split(',');

                string selectedText = this.hidSelectedText.Value;
                var selTextArray = selectedText.Split(',');

                ReportBO bo = new ReportBO();
                //获取最新数据
                List<SPS_Storage> storagelist = GetStorageItem(selValueArray);
                //查询快照表
                List<SPS_Storage> querylist = bo.GetSPS_Storage_QueryList(selectedValue);
                //将最新数据添加到查询出来的快照表数据中
                querylist.AddRange(storagelist);
                querylist = querylist.OrderByDescending(o => o.Created).ToList();

                if (querylist == null || querylist.Count == 0) return;
                int cnt = querylist.Count;

                if (cnt > 0)
                {
                    for (int j = 0; j < selValueArray.Length; j++)
                    {
                        //XLS
                        //HSSFSheet DataSheet = (HSSFSheet)hssfworkbook.CreateSheet(selTextArray[j] + " 监控列表");//sheetName
                        //XLSX
                        XSSFSheet DataSheet = (XSSFSheet)hssfworkbook.CreateSheet(selTextArray[j] + " 监控列表");//sheetName
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

                        List<SPS_Storage> list = querylist.Where(o => o.ListID.Equals(selValueArray[j])).ToList();
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
                    }

                    FileStream file2 = null;
                    try
                    {
                        file2 = new FileStream(createFilePath, FileMode.Create);
                    }
                    catch (Exception)
                    {
                        ScriptManager.RegisterStartupScript(this.btnExport, this.GetType(), "script", "alert('请检查模板文件权限！');", true);
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
                    ScriptManager.RegisterStartupScript(this.btnExport, this.GetType(), "script", "alert('未找到可导出数据！');", true);
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
        /// 获取最新的文档库监控信息
        /// </summary>
        /// <param name="selectorLibraries"></param>
        /// <returns></returns>
        private List<SPS_Storage> GetStorageItem(string[] selectorLibraries)
        {
            List<SPS_Storage> storageList = new List<SPS_Storage>();
            //var SPSRootSiteUrl = System.Configuration.ConfigurationManager.AppSettings["SPSRootSiteUrl"];
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = SPContext.Current.Site)
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            DataTable tbl = new DataTable();
                            if (selectorLibraries.Length < 1) return;

                            foreach (string libItem in selectorLibraries)
                            {
                                SPList splist = web.Lists[new Guid(libItem)];
                                if (splist == null) continue;
                                InsertListItem(web, splist, ref storageList);
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
                SPQuery spq = new SPQuery();
                spq.ViewAttributes = "Scope=\"Recursive\"";
                model.FileNumber = list.GetItems(spq).Count;

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
            //获取DOClist
            //docList = spdb.SP_Envision_GetListSizeResult();
            //var sum = (from o in docList where o.ListId.ToString() == listID select o).Sum(t => Convert.ToInt64(t.Size));
            var sum = spdb.GetEnvisionListSizeResult(listID);
            var sumresult = Convert.ToDecimal(sum) / 1024 / 1024;
            return Math.Round(sumresult, 2);
        }





    }
}
