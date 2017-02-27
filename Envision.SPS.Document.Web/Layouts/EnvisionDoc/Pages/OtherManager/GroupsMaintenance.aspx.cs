using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Envision.SPS.Utility.Models;
using Envision.SPS.Utility.Utilities;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Envision.SPS.Utility.Handlers;
using System.Linq;

namespace Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.OtherManager
{
    public partial class GroupsMaintenance : LayoutsPageBase
    {
        protected string keywords = string.Empty;
        protected int totalCount;
        protected int page;
        protected int pageSize = 10;
        protected internal EnvisionSiteConfig siteConfig;
        //protected string currentUrl = string.Empty;
        private string thisurl = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            page = IBRequest.GetQueryInt("page");
            keywords = IBRequest.GetQueryString("keywords");
            this.pageSize = GetPageSize(10); //每页数量
            //currentUrl = SPContext.Current.Web.Url;
            string weburl = SPContext.Current.Web.Url.Replace("http://", "");
            if (weburl.IndexOf('/') >= 0)
            {
                thisurl = weburl.Substring(weburl.IndexOf('/'));
            }
            hidCurrentWebUrl.Value = SPContext.Current.Site.RootWeb.Url;
            if (!IsPostBack)
            {
                txtGroupName.Text = keywords.Trim();

                bindGroups();
            }
        }

        protected void btnSearchGroups_Click(object sender, EventArgs e)
        {
            this.keywords = txtGroupName.Text.Trim();
            this.page = 0;
            bindGroups();
        }

        private void bindGroups()
        {
            var dataSource = new List<SPGroupModel>();
            dataSource = ListHandler.GetGroupsInfoSiteForIsSiteAdmin();
            RepGroupsList.DataSource = GetPagedList(dataSource, page, this.pageSize, this.keywords); ;
            RepGroupsList.DataBind();
            string pageUrl = IBUtils.CombUrlTxt(this.thisurl + "/_layouts/15/EnvisionDoc/pages/OtherManager/GroupsMaintenance.aspx", "keywords={0}&page={1}",
               this.keywords, "__id__");
            PageContent.InnerHtml = IBUtils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);

            total.InnerText = "共" + totalCount + "记录";

            txtPageNum.Text = this.pageSize.ToString();
        }

        private List<SPGroupModel> GetPagedList(List<SPGroupModel> dataSource,
           int pageindex, int pageSize, string keywords)
        {
            List<SPGroupModel> docList;
            List<SPGroupModel> data = null;
            if (!string.IsNullOrEmpty(keywords))
            {
                data = dataSource.Where(p => p.Id != null && p.Name.Contains(keywords)).ToList();
                docList = data.Where(p => p.Id != null && p.Name.Contains(keywords)).Skip((pageindex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                data = dataSource.Where(p => p.Id != null).ToList();
                docList = dataSource.Where(p => p.Id != null).Skip((pageindex - 1) * pageSize).Take(pageSize).ToList();
            }
            this.totalCount = data.Count();
            return docList;
        }

        //protected void RepGroupsList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        //    {
        //        HyperLink linkedit = (HyperLink)e.Item.FindControl("linkedit");
        //        HyperLink linkAddUser = (HyperLink)e.Item.FindControl("linkAddUser");
        //        if (linkedit.ToolTip.ToUpper() == "FALSE")
        //        {
        //            linkedit.Visible = false;
        //            linkAddUser.Visible = false;
        //        }

        //    }
        //}

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    IBUtils.WriteCookie("groupsMaintenance_page_size", _pagesize.ToString(), 14400);
                    this.pageSize = _pagesize;
                }
            }
            Response.Redirect(IBUtils.CombUrlTxt(this.thisurl + "/_layouts/15/EnvisionDoc/pages/OtherManager/GroupsMaintenance.aspx", "keywords={0}&page={1}",
               this.keywords, "__id__"));
        }

        #region 返回DocLibrary每页数量=========================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(IBUtils.GetCookie("groupsMaintenance_page_size"), out _pagesize))
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
