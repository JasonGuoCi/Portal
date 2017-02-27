using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Navigation;
using Microsoft.SharePoint.WebControls;
using Envision.SPS.Utility.Models;
using Envision.SPS.Utility.Utilities;

namespace Envision.SPS.Portal.Web.ControlTemplates
{
    public partial class TopNavigation : UserControl
    {

        #region Properties

        /// <summary>
        /// 站点Id
        /// </summary>
        protected string NavHtml
        {
            get
            {
                return Convert.ToString(ViewState["navHtml"]);
            }
            set
            {
                ViewState["navHtml"] = value;
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if(!IsPostBack)
            {
                GetNavigation();
            }
        }

        private void GetNavigation()
        {
            List<TopMenuItem> dataSource = new List<TopMenuItem>();
            StringBuilder navMenuHtml = new StringBuilder();
            if (CacheHelper.GetCache("SPNavigationCollectionCache") != null)
            {
                navMenuHtml = (StringBuilder)CacheHelper.GetCache("SPNavigationCollectionCache");
                this.NavHtml = navMenuHtml.ToString();
            }
            else
            {
                try
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                     {
                         using (SPWeb web = SPContext.Current.Site.RootWeb)
                         {
                             SPNavigationNodeCollection topLinkBar =web.Navigation.TopNavigationBar;
                             
                             foreach (SPNavigationNode nodeItem in topLinkBar)
                             {
                                 if (nodeItem.Url.Contains("PageNotFoundError.aspx") || nodeItem.Url.Contains("/InfoManager") || nodeItem.Url.Contains("/Pages/Home.aspx") || nodeItem.Url.Contains("default.aspx") || nodeItem.Url.Contains("/users") || nodeItem.Url.Contains("/searchcenter")) continue;
                                 if (!nodeItem.IsVisible) continue;
                                 var item = new TopMenuItem();
                                 item.ID = nodeItem.Id;
                                 item.Url = nodeItem.Url;
                                 item.Title = nodeItem.Title;
                                 item.ParentId = 0;
                                 dataSource.Add(item);
                                 navMenuHtml.Append(GetLevel1StartTag());
                                 navMenuHtml.Append(GetLevel1MenuHtml(item));
                                 if (nodeItem.TargetParentObjectType==SPObjectType.Web)
                                 {
                                     if(nodeItem.Url!="/"){ 
                                     
                                         using(SPWeb subWeb =SPContext.Current.Site.OpenWeb(nodeItem.Url))
                                         {
                                             GetLevel2ChildenWebNavigation(subWeb.Webs, ref dataSource, ref navMenuHtml);
                                         }

                                     }
                                 }
                                 else
                                 {
                                     GetLevel2ChildenNavigation(nodeItem.Children, ref dataSource, ref navMenuHtml);
                                 }
                                 navMenuHtml.Append(GetLevel1EndTag());
                               
                             }
                         }
                     });

                    CacheHelper.SetCache("SPNavigationCollectionCache", navMenuHtml, DateTime.Now.AddHours(2), TimeSpan.Zero);


                    this.NavHtml = navMenuHtml.ToString();

                }
                catch { }
            }
        }


        private void GetLevel2ChildenWebNavigation(SPWebCollection webs, ref List<TopMenuItem> dataSource, ref StringBuilder navMenuHtml)
        {
            try
            {
                if (webs.Count>0)
                {
                    navMenuHtml.Append("<dd>");
                    foreach (SPWeb web in webs)
                    {
                        var item = new TopMenuItem();
                        item.ID = 0;//web.ID;
                        item.Url = web.Url;
                        item.Title = web.Title;
                        item.ParentId = 0;
                        dataSource.Add(item);
                        navMenuHtml.Append(GetLevel2StartTag());
                        navMenuHtml.Append(GetLevel2MenuHtml(item));
                        GetLevel3ChildenWebNavigation(web.Webs, ref dataSource, ref navMenuHtml);
                        navMenuHtml.Append(GetLevel2EndTag());
                    }
                    navMenuHtml.Append("</dd>");
                }

            }
            catch { }
        }
         private void GetLevel3ChildenWebNavigation(SPWebCollection webs, ref List<TopMenuItem> dataSource, ref StringBuilder navMenuHtml)
        {
            try
            {
                if (webs.Count > 0)
                {
                    navMenuHtml.Append(GetLevel3StartTag());
                    foreach (SPWeb web in webs)
                    {
                        var item = new TopMenuItem();
                        item.ID = 0;//web.ID;
                        item.Url = web.Url;
                        item.Title = web.Title;
                        item.ParentId = 0;
                        dataSource.Add(item);
                      
                        navMenuHtml.Append(GetLevel3MenuHtml(item));
                        GetLevel3ChildenWebNavigation(web.Webs, ref dataSource, ref navMenuHtml);
                       
                    }
                    navMenuHtml.Append(GetLevel3EndTag());

                }

            }
            catch { }

        }

        private void GetLevel2ChildenNavigation(SPNavigationNodeCollection navigationNodeCollection, ref List<TopMenuItem> dataSource, ref StringBuilder navMenuHtml)
        {

            try
            {
                if (navigationNodeCollection.Count>0)
                {
                    navMenuHtml.Append("<dd>");
                    foreach (SPNavigationNode nodeItem in navigationNodeCollection)
                    {
                        var item = new TopMenuItem();
                        item.ID = nodeItem.Id;
                        item.Url = nodeItem.Url;
                        item.Title = nodeItem.Title;
                        item.ParentId = nodeItem.ParentId;
                        dataSource.Add(item);
                        navMenuHtml.Append(GetLevel2StartTag());
                        navMenuHtml.Append(GetLevel2MenuHtml(item));
                        GetLevel3ChildenNavigation(nodeItem.Children, ref dataSource, ref navMenuHtml);
                        navMenuHtml.Append(GetLevel2EndTag());
                    }
                    navMenuHtml.Append("</dd>");
                }

            }
            catch { }

        }
        private void GetLevel3ChildenNavigation(SPNavigationNodeCollection navigationNodeCollection, ref List<TopMenuItem> dataSource, ref StringBuilder navMenuHtml)
        {

            try
            {
                if (navigationNodeCollection.Count > 0)
                {
                    navMenuHtml.Append(GetLevel3StartTag());
                    foreach (SPNavigationNode nodeItem in navigationNodeCollection)
                    {
                        var item = new TopMenuItem();
                        item.ID = nodeItem.Id;
                        item.Url = nodeItem.Url;
                        item.Title = nodeItem.Title;
                        item.ParentId = nodeItem.ParentId;
                        dataSource.Add(item);
                        navMenuHtml.Append(GetLevel3MenuHtml(item));
                       
                    }
                    navMenuHtml.Append(GetLevel3EndTag());
                }

            }
            catch { }

        }


        #region Level1 Menu
        private string GetLevel1StartTag()
        {
            StringBuilder sbMenu = new StringBuilder();
            sbMenu.Append("<dl onmouseover=\"addNavtigationCss(this)\"><dt >");
            return sbMenu.ToString();

        }
        private string GetLevel1MenuHtml(TopMenuItem menutem)
        {
            StringBuilder sbMenu = new StringBuilder();
            sbMenu.Append("<a href='" + menutem.Url + "'>" + menutem.Title + "</a></dt>");
            return sbMenu.ToString();
        }
        private string GetLevel1EndTag()
        {
            StringBuilder sbMenu = new StringBuilder();
            sbMenu.Append("</dl>");
            return sbMenu.ToString();

        }

        #endregion

        #region Level2 Menu
        private string GetLevel2StartTag()
        {
            StringBuilder sbMenu = new StringBuilder();
           // sbMenu.Append("<dd>");
            sbMenu.Append("<ul>");
            return sbMenu.ToString();

        }
        private string GetLevel2MenuHtml(TopMenuItem menutem)
        {
            StringBuilder sbMenu = new StringBuilder();
            sbMenu.Append("<li class='nav_l2'><a href='" + menutem.Url + "'>" + menutem.Title + "</a></li>");
            return sbMenu.ToString();
        }
        private string GetLevel2EndTag()
        {

            StringBuilder sbMenu = new StringBuilder();
            sbMenu.Append("</ul>");
            //sbMenu.Append("</dd>");
            return sbMenu.ToString();
        }

        #endregion

        #region Level3 Menu
        private string GetLevel3StartTag()
         {
             StringBuilder sbMenu = new StringBuilder();
             sbMenu.Append("<li class='nav_l3'>");
             return sbMenu.ToString();

         }
        private string GetLevel3MenuHtml(TopMenuItem menutem)
        {
            StringBuilder sbMenu = new StringBuilder();
            sbMenu.Append("<a href='" + menutem.Url + "'>" + menutem.Title + "</a>");
            return sbMenu.ToString();

        }
        private string GetLevel3EndTag()
        {
            StringBuilder sbMenu = new StringBuilder();
            sbMenu.Append("</li>");
            return sbMenu.ToString();

        }

        #endregion

    }
}