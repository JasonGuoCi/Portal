using System;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Envision.SPS.Utility.Models;
using Envision.SPS.Utility.Utilities;
using System.Linq;
using Wuqi.Webdiyer;

namespace Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages
{
    public partial class DocLibrariesListV2 : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                IntiData();
            }
        }
        private void IntiData()
        {
           
         GetSPListConllection(1);
          
           
        }

        private void GetSPListConllection(int pageindex)
        {
            var dataSource = new List<SPDocLibraryProperties>();
            if (CacheHelper.GetCache("SPDocLibrariesCache") != null)
            {
                dataSource = (List<SPDocLibraryProperties>) CacheHelper.GetCache("SPDocLibrariesCache");
            }
            else
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPSite spSite = SPContext.Current.Site;
                    using (SPWeb web = spSite.OpenWeb())
                    {
                        SPListCollection splistCollection = web.GetListsOfType(SPBaseType.DocumentLibrary);
                        foreach (SPList list in splistCollection)
                        {
                            //if (list.BaseType == SPBaseType.DocumentLibrary && list.BaseTemplate != SPListTemplateType.ListTemplateCatalog && list.BaseTemplate == SPListTemplateType.DocumentLibrary && list.BaseTemplate != SPListTemplateType.ThemeCatalog)
                            {
                                var item = new SPDocLibraryProperties();
                                item.Id = list.ID.ToString();
                                item.Title = list.Title;
                                item.BaseType = list.BaseType.ToString();
                                item.Description = list.Description.ToString();
                                item.EnableVersioning = list.EnableVersioning.ToString();
                                item.ForceCheckout = list.ForceCheckout.ToString();
                                item.HasUniqueRoleAssignments = list.HasUniqueRoleAssignments.ToString();
                                item.Created = list.Created;
                                dataSource.Add(item);
                            }

                        }
                    }

                });

                CacheHelper.SetCache("SPDocLibrariesCache",dataSource,DateTime.Now.AddMinutes(20),TimeSpan.Zero);
            }

            AspNetPager1.PageSize = 10;

            var q = (from n in dataSource
                     where n.Id != null
                     select new { n.Id, n.Title, n.BaseType, n.Description, n.EnableVersioning, n.ForceCheckout, n.HasUniqueRoleAssignments, n.Created})
                    .Skip((pageindex - 1) * 10).Take(10);

            AspNetPager1.RecordCount = dataSource.Count;
            SPDocLibraryRepeater.DataSource = q;
            SPDocLibraryRepeater.DataBind();
          

        }
        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            GetSPListConllection(AspNetPager1.CurrentPageIndex);
        }
    }
}
