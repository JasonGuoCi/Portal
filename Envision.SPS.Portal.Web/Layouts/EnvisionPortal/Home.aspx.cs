using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;

namespace Envision.SPS.Portal.Web.Layouts.EnvisionPortal
{
    public partial class Home : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hidCurrentWebUrl.Value = SPContext.Current.Web.Url;
                ltlUser.Text = SPContext.Current.Web.CurrentUser.Name;
            }
        }
    }
}
