using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;
using System.Data;
using System.Collections.Generic;
using Envision.SPS.Utility.Utilities;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

namespace Envision.SPS.Portal.Web.Layouts.EnvisionPortal.Pages.DepartmentSupport
{
    public partial class List : BasePage
    {
        protected string DepartmentTitle = "ALL";
        private string keywords = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDepartment();
                BindsFAQ();
                ltlUser.Text = SPContext.Current.Site.OpenWeb().CurrentUser.Name;
            }
        }

        private void BindDepartment()
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb spWeb = spSite.OpenWeb())
                        {
                            BindDepartmentData(spWeb);
                        }
                    }
                });
            }
            catch
            { }
        }

        private void BindDepartmentData(SPWeb spweb)
        {
            string listName = this.EnvisionPagesConfig.EnvisionDepartments;
            string orderby = "<FieldRef Name=\"SeqNo\" Ascending=\"True\"/><FieldRef Name=\"PublishedDate\" Ascending=\"True\"/>";
            SPListItemCollection itemCollection = GetListItems(spweb, listName, null, null, null, orderby, null);
            List<object> departmentList = new List<object>();
            foreach (SPListItem item in itemCollection)
            {
                departmentList.Add(new
                {
                    id = item.ID,
                    departName = IBUtils.ObjectToStr(item["DepartName"]),
                    departNameEn = IBUtils.ObjectToStr(item["DepartName_EN"]),
                    description = IBUtils.ObjectToStr(item["Description"]),
                    SeqNo = IBUtils.ObjToInt(item["SeqNo"], 0)
                });
            }
            RepDepartment.DataSource = departmentList;
            RepDepartment.DataBind();
        }
        private void BindsFAQ()
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb spWeb = spSite.OpenWeb())
                        {
                            BindFAQData(spWeb);
                        }
                    }
                });
            }
            catch
            { }

        }
        private void BindFAQData(SPWeb spweb)
        {

            string listName = this.EnvisionPagesConfig.EnvisionKnowledgeInfo;
            string orderString = string.Empty;
            string whereString = string.Empty;
            if (DepartmentTitle != "ALL")
            {
                if (string.IsNullOrEmpty(this.keywords))
                {
                    whereString = "<Eq><FieldRef Name=\"DepartName\" /><Value Type=\"Lookup\">" + DepartmentTitle + "</Value></Eq>";
                }
                else
                {
                    whereString = string.Format(@"<And>
                                                    <Contains>
                                                        <FieldRef Name='Title' />
                                                        <Value Type='Text'>{0}</Value>
                                                    </Contains>
                                                    <Eq>
                                                        <FieldRef Name='DepartName' />
                                                        <Value Type='Lookup'>{1}</Value>
                                                    </Eq>
                                                 </And>", keywords, DepartmentTitle);
                }
                orderString = "<FieldRef Name=\"SeqNo\" Ascending=\"True\"/><FieldRef Name=\"PublishedDate\" Ascending=\"False\"/>";
            }
            else
            {
                if (!string.IsNullOrEmpty(this.keywords))
                {
                    whereString = "<Contains><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + keywords + "</Value></Contains>";
                }
                orderString = "<FieldRef Name=\"SeqNo\" Ascending=\"True\"/><FieldRef Name=\"PublishedDate\" Ascending=\"False\"/>";
            }
            SPListItemCollection itemCollection = GetListItems(spweb, listName, null, null, whereString, orderString, null);
            List<object> supportList = new List<object>();
            foreach (SPListItem item in itemCollection)
            {
                supportList.Add(new
                {
                    id = item.ID,
                    title = IBUtils.ObjectToStr(item["Title"]),
                    titleEn = IBUtils.ObjectToStr(item["Title_EN"]),
                    contentBox = IBUtils.ObjectToStr(item["ContentBox"]) == "" ? "&nbsp;" : IBUtils.ObjectToStr(item["ContentBox"]),
                    departName = IBUtils.ObjectToStr(item["DepartName"]),
                    attachemtns = GetAttachments(item.Attachments, spweb)
                });
            }
            this.RepSupportTitle.DataSource = supportList;
            this.RepSupportTitle.DataBind();

            this.RepSupportAnswer.DataSource = supportList;
            this.RepSupportAnswer.DataBind();
        }

        private string GetAttachments(SPAttachmentCollection spAttavchmentCollection, SPWeb web)
        {
            StringBuilder strhtml = new StringBuilder();
            string urlPrefix = spAttavchmentCollection.UrlPrefix;
            if (spAttavchmentCollection.Count > 0)
            {
                List<object> attachment = new List<object>();
                strhtml.Append("<dd class='FAQ_attachment'>");
                strhtml.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                strhtml.Append("<tr><td class='type'>附件：</td></tr>");
                strhtml.Append("<tr><td>");
                foreach (string attName in spAttavchmentCollection)
                {
                    SPFile file = web.GetFile(urlPrefix + attName);
                    strhtml.Append("<div style='z-index:4;position: relative;'><a href=\"" + web.Url + "/" + file.Url + "\">" + file.Name + "</a></div>");
                }
                strhtml.Append("</td></tr></table></dd>");
            }
            return strhtml.ToString();
        }

        protected void hidBtnSeach_Click(object sender, EventArgs e)
        {
            DepartmentTitle = string.IsNullOrEmpty(hidType.Value) ? "ALL" : hidType.Value.ToString();
            this.keywords = "";
            //BindDepartment();
            BindsFAQ();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString("N"), " init();", true);
        }

        protected void btnSearchTitle_Click(object sender, EventArgs e)
        {
            DepartmentTitle = string.IsNullOrEmpty(hidType.Value) ? "ALL" : hidType.Value.ToString();
            this.keywords = txtkeyword.Text.Trim();
            //BindDepartment();
            BindsFAQ();
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString("N"), " $(\"#sidebar-nav\").niceScroll({ touchbehavior: false, cursorcolor: \"#7C7C7C\", cursoropacitymax: 0, cursorwidth: 0 });", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString("N"), "init();", true);
        }

        protected void RepSupportAnswer_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            //{
            //    Literal ltlTitle = (Literal)e.Item.FindControl("categoryTitle");
            //    Repeater RepSystemLinks = (Repeater)e.Item.FindControl("RepAttachments");
            //    RepSystemLinks.DataSource = GetSystemLinksByCategoryName(ltlTitle.Text);
            //    RepSystemLinks.DataBind();
            //    i = 1;
            //}
        }

    }
}
