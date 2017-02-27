using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.DocumentManager
{
    public partial class DocRoles : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hidCurrentWebUrl.Value = SPContext.Current.Web.Url;
            }
        }
    }
}
