using System;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Envision.SPS.Utility.Models;

namespace Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages
{
    public partial class DocLibrariesList : LayoutsPageBase
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
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite spSite = SPContext.Current.Site;
                using (SPWeb web = spSite.OpenWeb())
                {
                    SPgvFiles.DataSource = GetSPListConllection(web);
                    SPgvFiles.DataBind();
                }

            });
        }

        private List<SPDocLibraryProperties> GetSPListConllection(SPWeb spweb)
        {
            var dataSource = new List<SPDocLibraryProperties>();
            SPListCollection splistCollection = spweb.GetListsOfType(SPBaseType.DocumentLibrary);
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
            return dataSource;

        }
    }
}
