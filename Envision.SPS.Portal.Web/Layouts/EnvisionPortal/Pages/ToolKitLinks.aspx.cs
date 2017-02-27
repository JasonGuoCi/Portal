using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using Envision.SPS.Utility.Utilities;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data;

namespace Envision.SPS.Portal.Web.Layouts.EnvisionPortal.Pages
{
    public partial class ToolKitLinks : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSystemLinksCategory();
                ltlUser.Text = SPContext.Current.Site.OpenWeb().CurrentUser.Name;
            }
        }

        private void BindSystemLinksCategory()
        {
            try
            {
                List<object> linkCategoryList = new List<object>();
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        site.AllowUnsafeUpdates = true;
                        using (SPWeb web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;

                            SPListItemCollection linkCategoryItems = web.Lists[this.EnvisionPagesConfig.EnvisionLinksCategory].GetItems();

                            foreach (SPItem item in linkCategoryItems)
                            {
                                linkCategoryList.Add(new
                                {
                                    id = item.ID.ToString(),
                                    category = IBUtils.ObjectToStr(item["Category"]),
                                    categoryEn = IBUtils.ObjectToStr(item["Category_EN"])
                                });
                            }


                            web.AllowUnsafeUpdates = false;
                        }
                        site.AllowUnsafeUpdates = false;
                    }
                });
                RepCategory.DataSource = linkCategoryList;
                RepCategory.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        private List<object> GetSystemLinksByCategoryName(string CategoryName)
        {
            List<object> SystemLinks = new List<object>();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        site.AllowUnsafeUpdates = true;
                        using (SPWeb web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            string where = "<Eq><FieldRef Name=\"Category\" /><Value Type=\"Lookup\">" + CategoryName + "</Value></Eq>";
                            string orderby = "<FieldRef Name=\"SeqNo\" Ascending=\"True\"/>";
                            SPListItemCollection linkItems = GetListItems(web, this.EnvisionPagesConfig.EnvisionSystemLink, null, null, where, orderby, null);

                            foreach (SPItem item in linkItems)
                            {
                                SystemLinks.Add(new
                                {
                                    id = item.ID.ToString(),
                                    title = IBUtils.ObjectToStr(item["Title"]),
                                    linkUrl = IBUtils.ObjectToStr(item["LinkUrl"]).Split(',')[0].ToString(),
                                    imageUrl = IBUtils.ObjectToStr(item["ImageUrl"]).Split(',')[0].ToString(),
                                    imageUrl2 = IBUtils.ObjectToStr(item["ImageUrl2"]).Split(',')[0].ToString()
                                });
                            }


                            web.AllowUnsafeUpdates = false;
                        }
                        site.AllowUnsafeUpdates = false;
                    }
                });
                return SystemLinks;
            }
            catch (Exception ex)
            {
                return SystemLinks;
            }
        }
        protected void RepCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Literal ltlTitle = (Literal)e.Item.FindControl("categoryTitle");
                Repeater RepSystemLinks = (Repeater)e.Item.FindControl("RepSystemLinks");
                RepSystemLinks.DataSource = GetSystemLinksByCategoryName(ltlTitle.Text);
                RepSystemLinks.DataBind();
                i = 1;
            }
        }

        protected int i = 1;
        protected void RepSystemLinks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                //repter换行
                if (i % 7 == 0 && i > 0)
                {
                    e.Item.Controls.Add(new LiteralControl("</tr><tr>"));
                }
                i++;
            }
        }
    }
}
