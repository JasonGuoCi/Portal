using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Envision.SPS.Document.Web.Layouts
{
    public partial class ITSettings : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SPContext.Current.Web.CurrentUser.IsSiteAdmin)
                {
                    Response.Write("没有权限");
                    Response.End();
                }
            }
        }
    }
}
