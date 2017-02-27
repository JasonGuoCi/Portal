using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Envision.SPS.Portal.Web.ControlTemplates.EnvisionPortal
{
    public partial class WecomeLoginName : UserControl
    {
        public string wecomeLoginText = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                wecomeLoginText = SPContext.Current.Web.CurrentUser.Name;
            }
        }
    }
}
