using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Envision.SPS.Document.Web.Layouts
{
    public partial class DocSettings : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hidCurrentWebUrl.Value = SPContext.Current.Web.Url;
                hidIsWebManager.Value = SPContext.Current.Web.DoesUserHavePermissions(SPBasePermissions.ManageWeb).ToString().ToUpper();
            }
        }
    }
}
