using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using Envision.SPS.Utility.Models;
using System.Text;
using Envision.SPS.DataAccess;
using Envision.SPS.Utility.Utilities;
using System.IO;
using System.Web;

namespace Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages
{
    public partial class DownLoadFile : LayoutsPageBase
    {
        EventBusDAL dal=new EventBusDAL();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id = IBRequest.GetQueryInt("id",0);
                string filename =HttpUtility.UrlDecode(IBRequest.GetQueryString("FileName"));
                GetSPS_EventBus(id, filename);
            }

        }
        public bool GetSPS_EventBus(int id,string filename)
        {
            try
            {
                SPS_EventBus model = dal.SPS_EventBusSP_Envision_GetEventBusModelById(id);
                if (model != null)
                {
                    
                        FileInfo DownloadFile = new FileInfo(model.FilePath+ filename);
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.Buffer = false;
                        Response.ContentType = "application/x-excel";
                        Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename,Encoding.UTF8).ToString());
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
