using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Envision.SPS.Utility.Models;
using System.Collections.Generic;
using Envision.SPS.Utility.Utilities;
using System.Linq;
using Microsoft.SharePoint.Utilities;
using System.Web.UI.WebControls;

namespace Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.DocumentManager
{
    public partial class DocManager : LayoutsPageBase
    {
        protected string keywords = string.Empty;
        protected int totalCount;
        protected int page;
        protected int pageSize = 10;
        private string thisurl = string.Empty;
        protected string sort = "name|0";
        protected string listtemplate = string.Empty;
        protected internal EnvisionSiteConfig siteConfig;
        protected void Page_Load(object sender, EventArgs e)
        {
            siteConfig = ConfigManager.loadConfig();
            page = IBRequest.GetQueryInt("page");
            keywords = IBRequest.GetQueryString("keywords");
            this.pageSize = GetPageSize(10); //每页数量
            string weburl = SPContext.Current.Web.Url.Replace("http://", "");
            if (weburl.IndexOf('/') >= 0)
            {
                thisurl =weburl.Substring(weburl.IndexOf('/'));
            }
            hidCurrentWebUrl.Value = SPContext.Current.Web.Url;
            if (!IsPostBack)
            {
                
                txtDoclibName.Text = keywords.Trim();
                if (!SPContext.Current.Web.DoesUserHavePermissions(SPBasePermissions.ManageWeb))
                {
                    createDocLib.Visible = false;
                }
                GetSPListConllection();
                GetDocLibTemplate();
            }
        }

        private void GetDocLibTemplate()
        {
            List<ListTemplateCatalog> listTemplateCatalog = new List<ListTemplateCatalog>();
            string defaultTemplateName = string.Empty;

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite spSite = SPContext.Current.Site)
                {
                    spSite.AllowUnsafeUpdates = true;
                    using (SPWeb web = spSite.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        SPListTemplateCollection listTemplateCollection = web.ListTemplates;

                        foreach (SPListTemplate item in listTemplateCollection)
                        {
                            if (item.Type_Client == ConfigManager.Type_Client)
                            {
                                listTemplateCatalog.Add(new ListTemplateCatalog() { Name = item.Name, Type_Client = item.Type_Client, InternalName = "" });
                                defaultTemplateName = item.Name;
                                break;
                            }
                        }

                        SPListTemplateCollection CustomeListTemplateCollection = spSite.GetCustomListTemplates(web);
                        foreach (SPListTemplate item in CustomeListTemplateCollection)
                        {
                            if (item.Type_Client == ConfigManager.Type_Client)
                            {
                                listTemplateCatalog.Add(new ListTemplateCatalog() { Name = item.Name, Type_Client = item.Type_Client, InternalName = item.InternalName.ToString() });
                            }
                        }
                        web.AllowUnsafeUpdates = false;
                    }
                    spSite.AllowUnsafeUpdates = false;
                }
            });
            ddlDocTemplate.DataSource = listTemplateCatalog;
            ddlDocTemplate.DataTextField = "Name";
            ddlDocTemplate.DataValueField = "InternalName";
            ddlDocTemplate.DataBind();
            ddlDocTemplate.SelectedValue = defaultTemplateName;

        }

        private void GetSPListConllection()
        {
            var dataSource = new List<SPDocLibraryProperties>();

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite spSite = SPContext.Current.Site)
                {
                    spSite.AllowUnsafeUpdates = true;
                    using (SPWeb web = spSite.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        SPListCollection splistCollection = web.GetListsOfType(SPBaseType.DocumentLibrary);
                        foreach (SPList list in splistCollection)
                        {
                            if (!list.Hidden && list.BaseType == SPBaseType.DocumentLibrary &&
                             list.BaseTemplate != SPListTemplateType.ListTemplateCatalog &&
                             list.BaseTemplate != SPListTemplateType.DesignCatalog &&
                             list.BaseTemplate == SPListTemplateType.DocumentLibrary &&
                             list.AllowDeletion &&
                             !list.IsSiteAssetsLibrary)
                            {

                                if (!list.DoesUserHavePermissions(SPBasePermissions.FullMask) && !SPContext.Current.Web.DoesUserHavePermissions(SPBasePermissions.ManageWeb))
                                {
                                    continue;
                                }

                                var item = new SPDocLibraryProperties();
                                item.Id = list.ID.ToString();
                                item.Title = list.Title;
                                item.BaseType = list.BaseType.ToString();
                                item.Description = list.Description.ToString();
                                item.EnableVersioning = GetVersionControl(list);
                                item.ForceCheckout = list.ForceCheckout.ToString();
                                item.HasUniqueRoleAssignments = list.HasUniqueRoleAssignments.ToString();
                                item.Created = list.Created;
                                dataSource.Add(item);

                            }
                        }
                        web.AllowUnsafeUpdates = false;
                    }
                    spSite.AllowUnsafeUpdates = false;
                }
            });
            string[] sortRule = sort.Split('|');
            if (sortRule[0] == "name")
            {
                if (sortRule[1] == "0")
                {
                    //名称升序
                    dataSource = dataSource.OrderBy(p => p.Title).ToList();
                }
                else
                {
                    dataSource = dataSource.OrderByDescending(p => p.Title).ToList();
                }
            }
            else
            {
                if (sortRule[1] == "0")
                {
                    //时间升序
                    dataSource = dataSource.OrderBy(p => p.Created).ToList();
                }
                else
                {
                    //时间降序
                    dataSource = dataSource.OrderByDescending(p => p.Created).ToList();
                }
            }

            RepDocumentList.DataSource = GetPagedList(dataSource, page, this.pageSize, this.keywords);
            RepDocumentList.DataBind();


            string pageUrl = IBUtils.CombUrlTxt(this.thisurl + "/_layouts/15/EnvisionDoc/pages/DocumentManager/DocManager.aspx", "keywords={0}&page={1}",
               this.keywords, "__id__");
            PageContent.InnerHtml = IBUtils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);

            total.InnerText = "共" + totalCount + "记录";

            txtPageNum.Text = this.pageSize.ToString();
        }

        private List<SPDocLibraryProperties> GetPagedList(List<SPDocLibraryProperties> dataSource,
            int pageindex, int pageSize, string keywords)
        {
            List<SPDocLibraryProperties> docList;
            List<SPDocLibraryProperties> data;
            if (!string.IsNullOrEmpty(keywords))
            {
                data = dataSource.Where(p => p.Id != null && p.Title.Contains(keywords)).ToList();
                docList = data.Where(p => p.Id != null && p.Title.Contains(keywords)).Skip((pageindex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                data = dataSource.Where(p => p.Id != null).ToList();
                docList = dataSource.Where(p => p.Id != null).Skip((pageindex - 1) * pageSize).Take(pageSize).ToList();
            }
            this.totalCount = data.Count();
            return docList;
        }


        protected void btnSearchDoclib_Click(object sender, EventArgs e)
        {
            this.keywords = txtDoclibName.Text.Trim();
            this.page = 0;
            GetSPListConllection();
        }

        private string GetVersionControl(SPList spList)
        {
            string VersionControl = string.Empty;
            if (spList.EnableVersioning && spList.EnableMinorVersions)
            {
                //次版本
                VersionControl = "次版本";
            }
            else if (spList.EnableVersioning && !spList.EnableMinorVersions)
            {
                //主版本
                VersionControl = "主版本";
            }
            else if (!spList.EnableVersioning && !spList.EnableMinorVersions)
            {
                //无
                VersionControl = "无";
            }
            return VersionControl;
        }

        public string GetHasUniqueRoleAssignments(string value)
        {
            string result = string.Empty;
            if (value.ToUpper() == "TRUE")
            {
                result = "否";
            }
            else
            {
                result = "是";
            }
            return result;
        }
        public string GetForceCheckout(string value)
        {
            string result = string.Empty;
            if (value.ToUpper() == "TRUE")
            {
                result = "是";
            }
            else
            {
                result = "否";
            }
            return result;
        }

        protected void DocNameSort_Click(object sender, EventArgs e)
        {
            this.keywords = txtDoclibName.Text.Trim();
            if (DocNameSort.Text.Trim() == "0")
            {
                DocNameSort.Text = "1";
                sort = "name|1";
            }
            else
            {
                DocNameSort.Text = "0";
                sort = "name|0";
            }
            GetSPListConllection();
        }

        protected void DocCreateTimeSort_Click(object sender, EventArgs e)
        {
            this.keywords = txtDoclibName.Text.Trim();
            if (DocCreateTimeSort.Text.Trim() == "0")
            {
                DocCreateTimeSort.Text = "1";
                sort = "time|1";
            }
            else
            {
                DocCreateTimeSort.Text = "0";
                sort = "time|0";
            }
            GetSPListConllection();
        }
        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    IBUtils.WriteCookie("docmanager_page_size", _pagesize.ToString(), 14400);
                    this.pageSize = _pagesize;
                    this.page = IBRequest.GetQueryInt("page");
                    this.keywords = txtDoclibName.Text.Trim();
                }
            }
            //GetSPListConllection();
            Response.Redirect(IBUtils.CombUrlTxt(this.thisurl + "/_layouts/15/EnvisionDoc/pages/DocumentManager/DocManager.aspx", "keywords={0}&page={1}", this.keywords, "__id__"));
        }

        #region 返回DocLibrary每页数量=========================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(IBUtils.GetCookie("docmanager_page_size"), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    return _pagesize;
                }
            }
            return _default_size;
        }
        #endregion
    }
}
