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
using Envision.SPS.Utility.Models;
using System.Reflection;
using NPOI.SS.Util;
using System.Web;
using Envision.SPS.DataAccess;

namespace Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.Reports
{
    public partial class PermissionsReport : LayoutsPageBase
    {
        public string Result = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowDocLibraries();
            }
        }

        private void ShowDocLibraries()
        {
            //首次加载文档库列表

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = SPContext.Current.Site)
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
                                        sb.Append("<p class='p_div'>");
                                        sb.Append("<input type='checkbox' name='LIDocumentLibrary' class='checkbox'  style='float:left;' value='" + splist.ID.ToString() + "' title='" + splist.Title + "'/><a href='#' class='itm'>" + splist.Title + "</a>");
                                        sb.Append("</p>");
                                    }
                                }
                            }
                            sb.Append("</li>");
                            this.ULDocumentLibrary.InnerHtml = sb.ToString();
                        }
                    }
                });
            }
            catch
            {

            }

        }

        protected void ButSaveExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //string newGuid = DateTime.Now.ToString("yyyyMMddHHmmss") + "_PermissionsReport";
                string oFilePath = MapPath("../../TemplateFiles/LibraryPermissionsReport.xlsx");
                string createFilePath = MapPath("../../") + "TemplateFiles/Files/";
                DataAccess.EventBusDAL dal = new EventBusDAL();
                if (HidCheckSelectGuid.Value != string.Empty && HidCheckSelectTile.Value != string.Empty)
                {
                    string newGuid = "";
                    for (int i = 0; i < HidCheckSelectTile.Value.Split(',').Length; i++)
                    {
                        newGuid += this.HidCheckSelectTile.Value.Split(',')[i] + "_权限报表_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".xlsx" + ",";
                    }
                    newGuid = newGuid.Substring(0, newGuid.Length - 1);
                    SPS_EventBus spsEventModel = VerifyEventBus(SPContext.Current.Site.ID.ToString(), SPContext.Current.Web.ID.ToString(), this.HidCheckSelectGuid.Value, "PermmisionExport", SPContext.Current.Site.OpenWeb().Users.Web.CurrentUser.Name, SPContext.Current.Site.OpenWeb().Users.Web.CurrentUser.ID.ToString(), SPContext.Current.Site.OpenWeb().Users.Web.CurrentUser.Email.ToString(), Convert.ToByte("0"), false, newGuid, createFilePath);
                    long Id = 0;
                    if (dal.AddSPS_EventBus(spsEventModel, ref Id))
                    {
                        //Result = "hideLoading();layer.msg(\"操作成功,正在为您导出权限信息!\");";
                        Response.Redirect("PermissionsReportLoading.aspx?id=" + Id, false);
                    }
                    else
                    {
                        Result = "hideLoading();layer.msg(\"操作失败!\");";
                    }
                }
                else
                    Result = "hideLoading();layer.msg(\"操作失败!\");";
            }
            catch { }
        }


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

    }
}

