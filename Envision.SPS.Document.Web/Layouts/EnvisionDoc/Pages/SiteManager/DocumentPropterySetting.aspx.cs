using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Envision.SPS.Utility.Utilities;
using Envision.SPS.Utility.Models;
using Envision.SPS.Utility.Handlers;

namespace Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.SiteManager
{
    public partial class DocumentPropterySetting : LayoutsPageBase
    {
        protected internal EnvisionSiteConfig siteConfig;
        public string Result = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            siteConfig = ConfigManager.loadConfig();
            if (!IsPostBack)
            {
                if (!HasDocLibrarySettings()) { CreateDocLibrarySettings(); }

                if(!SPContext.Current.Web.DoesUserHavePermissions(SPBasePermissions.ManageWeb))
                {
                    Response.Write("你没有权限");
                    Response.End();
                }
                GetDocProptery();
                
            }
        }

        private void GetDocProptery()
        {
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb sPweb = spSite.OpenWeb())
                    {
                        sPweb.AllowUnsafeUpdates = true;
                        SPList spList = sPweb.Lists[siteConfig.DocLibrarySettings];
                        SPListItemCollection ListItemCollection = spList.Items;
                        if (ListItemCollection.Count > 0)
                        {
                            SPListItem ListItem = ListItemCollection[0];
                            rblCollations.SelectedValue = IBUtils.ObjectToStr(ListItem["SortName"]);
                            rblAscDesc.SelectedValue = IBUtils.ObjectToStr(ListItem["SortType"]);
                            rblOpen.SelectedValue = IBUtils.ObjectToStr(ListItem["ExtendType"]);
                            sPweb.AllowUnsafeUpdates = false;
                        }
                    }
                }
            });
        }

        protected void btnSavaSet_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb sweb = site.OpenWeb())
                    {
                        sweb.AllowUnsafeUpdates = true;
                        SPList list=sweb.Lists[siteConfig.DocLibrarySettings];
                        SPListItemCollection items = list.Items;

                        if (list.ItemCount <= 0)
                        {
                            SPListItem item = items.Add();
                            item["SortName"] = rblCollations.SelectedValue;
                            item["SortType"] = rblAscDesc.SelectedValue;
                            item["ExtendType"] = rblOpen.SelectedValue;
                            item.Update();
                            Result = "layer.msg('添加成功!');";
                            sweb.AllowUnsafeUpdates = false;
                        }
                        else
                        {
                            SPListItem item = list.GetItemById(items[0].ID);
                            item["SortName"] = rblCollations.SelectedValue;
                            item["SortType"] = rblAscDesc.SelectedValue;
                            item["ExtendType"] = rblOpen.SelectedValue;
                            item.Update();
                            Result = "layer.msg('修改成功!');";
                            sweb.AllowUnsafeUpdates = false;
                        }
                    }
                }
            });
        }



        /// <summary>
        /// 创建DocLibrarySettings
        /// </summary>
        /// <returns></returns>
        public void CreateDocLibrarySettings()
        {
            try
            {
                DocumentLibraryAttr documentLibraryAttr = new DocumentLibraryAttr();
                documentLibraryAttr.DocumentLibraryName = "DocLibrarySettings";
                documentLibraryAttr.DocumentLibraryTemplateId = "DocLibrarySettingsTemplate.stp";
                ListHandler.CreateDocumentlabrary(documentLibraryAttr);
            }
            catch
            {
                try
                {
                    DocumentLibraryAttr documentLibraryAttr = new DocumentLibraryAttr();
                    documentLibraryAttr.DocumentLibraryName = "DocLibrarySettings";
                    documentLibraryAttr.DocumentLibraryTemplateId = "DocLibrarySettingsTemplate2.stp";
                    ListHandler.CreateDocumentlabrary(documentLibraryAttr);

                }
                catch
                {
                }

            }
        }

        /// <summary>
        /// 验证是否有DocLibrarySettings
        /// </summary>
        /// <returns></returns>
        public bool HasDocLibrarySettings()
        {
            bool result = true;
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb sPweb = spSite.OpenWeb())
                    {
                        SPList list = null;
                        try
                        {
                            list = sPweb.Lists["DocLibrarySettings"];
                        }
                        catch
                        {
                            result=false;
                        }
                    }
                }
            });

            return result;
        }
    }
}
