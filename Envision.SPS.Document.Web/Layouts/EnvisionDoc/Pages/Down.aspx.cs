using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Envision.SPS.Utility.Utilities;
using System.Web;
using System.IO;
using System.Text;

namespace Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages
{
    public partial class Down : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string filename = HttpUtility.UrlDecode(IBRequest.GetQueryString("FileName"));
                GetSPS_EventBus(filename);
            }
        }

        public bool GetSPS_EventBus(string filename)
        {
            try
            {

                if (!string.IsNullOrEmpty(filename))
                {
                    
                    //string WebPath = System.Configuration.ConfigurationManager.AppSettings["UploadReportFileWebPath"];
                    string WebPath = Server.MapPath("../TemplateFiles/Files/" + filename);

                    FileInfo DownloadFile = new FileInfo(WebPath);
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.Buffer = false;
                    Response.ContentType = "application/x-excel";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename, Encoding.UTF8).ToString());
                    Response.Charset = "gb2312";
                    Response.ContentEncoding = System.Text.Encoding.UTF7;
                    Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
                    Response.WriteFile(DownloadFile.FullName);
                    Response.Flush();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
