using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Envision.SPS.Utility.Utilities;

namespace Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.Reports
{
    public partial class PermissionsReportLoading : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hidId.Value = IBRequest.GetQueryString("id");
            }
        }
    }
}
