using System;
using System.IO;
using System.Linq;
using System.Web;
//using InfoBase.OA.SharePoint2013.Document.Handler.Enums;
//using InfoBase.OA.SharePoint2013.Document.Handler.Extensions;
//using InfoBase.OA.SharePoint2013.Document.Handler.Handlers;
//using InfoBase.OA.SharePoint2013.Document.Handler.Utilities;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using SPClient = Microsoft.SharePoint.Client;

namespace Envision.SPS.Document.Web.Layouts.EnvisionDoc.Handlers
{
    /// <summary>
    /// UploadHandler 的摘要说明
    /// </summary>
    public class ExportLogToExcelHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            //Guid listId = new Guid(context.Request["ListId"]);
            //LogGroup logGroup = (LogGroup)Convert.ToInt32(context.Request["LogGroup"]);
            //LogType logType = (LogType)Convert.ToInt32(context.Request["LogType"]);
            //string userName = context.Request["UserName"];
            //string name = context.Request["Name"];
            //DateTime? startDate = Util.GetDateTime(context.Request["StartDate"]);
            //if (!startDate.HasValue)
            //{
            //    startDate = DateTime.MinValue;
            //}
            //DateTime? endDate = Util.GetDateTime(context.Request["EndDate"]);
            //if (!endDate.HasValue)
            //{
            //    endDate = DateTime.MaxValue;
            //}
            //string sortName = context.Request["SortName"];
            //string sortType = context.Request["SortType"];
            //ExportLogToExcel(listId, logGroup, logType, userName, name, startDate.Value, endDate.Value, sortName, sortType);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        //public static void ExportLogToExcel(Guid listId, LogGroup logGroup, LogType logType, string userName, string name, DateTime startDate, DateTime endDate, string sortName, string sortType)
        //{
        //    string title = ConfigManager.GetXmlAppSetting(ConfigDefine.Log);
        //    using (SPClient.ClientContext clientContext = SharePointUtil.GetClientContext())
        //    {
        //        SPClient.ListItemCollection listItemCollection = ListItemHandler.GetListItemCollection(clientContext, title, listId, logGroup, logType, userName, name, startDate, endDate, sortName, sortType, int.MaxValue, 1);
        //        string path = HttpContext.Current.Server.MapPath("~/_layouts/15/UDoc/TemplateFiles/AuditLog.xls");
        //        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        //        HSSFWorkbook hssfWorkbook = new HSSFWorkbook(fileStream);
        //        ISheet sheet = (HSSFSheet)hssfWorkbook.GetSheetAt(0);
        //        for (int i = 0; i < listItemCollection.Count; i++)
        //        {
        //            SPClient.ListItem listItem = listItemCollection[i];
        //            IRow row = sheet.CreateRow(i + 1);
        //            const int startIndex = 0;
        //            row.CreateCell(startIndex).SetCellValue(string.Format(@"{0}/{1}{2}", Util.GetString(listItem["WebUrl"]), Util.GetString(listItem["ListTitle"]), Util.GetString(listItem["LocalPath"])));
        //            row.CreateCell(startIndex + 1).SetCellValue(Util.GetString(listItem["ListItemName"]));
        //            row.CreateCell(startIndex + 2).SetCellValue(Util.GetString(listItem["VersionLabel"]));
        //            row.CreateCell(startIndex + 3).SetCellValue(Util.GetString(listItem["UserName"]));
        //            row.CreateCell(startIndex + 4).SetCellValue(Util.GetString(listItem["CreatedDate"]));
        //            row.CreateCell(startIndex + 5).SetCellValue(GetLogType(Util.GetString(listItem["LogType"])).ToDisplayName());
        //            row.CreateCell(startIndex + 6).SetCellValue(string.IsNullOrWhiteSpace(Util.GetString(listItem["DestLocalPath"])) ? "" : string.Format(@"{0}/{1}{2}", Util.GetString(listItem["WebUrl"]), Util.GetString(listItem["DestListTitle"]), Util.GetString(listItem["DestLocalPath"])));
        //        }
        //        using (MemoryStream memoryStream = new MemoryStream())
        //        {
        //            hssfWorkbook.Write(memoryStream);
        //            HttpResponse response = HttpContext.Current.Response;
        //            response.Clear();
        //            response.ClearHeaders();
        //            response.Buffer = false;
        //            response.ContentType = "application/x-excel";
        //            response.AddHeader("Content-Disposition", "attachment;filename=" + string.Format(@"AuditLog_{0}.xls", DateTime.Now.ToString("yyyyMMddHHmm")));
        //            response.AppendHeader("Content-Length", memoryStream.Length.ToString());
        //            response.BinaryWrite(memoryStream.ToArray());
        //            response.Flush();
        //            response.End();
        //        }
        //    }
        //}

        //private static LogType GetLogType(string name)
        //{
        //    Type type = typeof(LogType);
        //    string[] names = Enum.GetNames(type);
        //    foreach (string item in names.Where(item => string.Equals(item, name)))
        //    {
        //        return (LogType)Enum.Parse(type, item);
        //    }
        //    return LogType.None;
        //}

    }
}