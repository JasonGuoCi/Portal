using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;
using Envision.SPS.Utility.Handlers;
using Envision.SPS.Utility.Utilities;
namespace Envision.SPS.Portal.Web.ControlTemplates.EnvisionPortal
{
    public partial class CompanyInfo : UserControl
    {
        public string companylnfoHtml = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                GetCompanyInfo();
            }
        }


        private void GetCompanyInfo()
        {
            if (CacheHelper.GetCache("EnvisionCompanyInfoCache") != null)
            {
                companylnfoHtml = (string)CacheHelper.GetCache("EnvisionCompanyInfoCache");
            }
            else
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPWeb web = SPContext.Current.Site.RootWeb)
                    {
                        try
                        {
                            new PortalBase();
                            SPList spList = web.Lists[PortalBase.EnvisionPagesConfig.EnvisionCompanyInfo];
                            SPListItem listItem = spList.Items[0];
                            var backgroundUrl = IBUtils.ObjectToStr(listItem["ImgUrl"]).Split(',')[0];
                            string background=string.Empty;
                            if (!string.IsNullOrEmpty(backgroundUrl))
                            {
                                background = "style='background:url(\"" + backgroundUrl + "\") top center no-repeat;background-size:400px 210px;'";
                            }

                            companylnfoHtml = "<dl class=\"mbox_1\"><dt " + background + "></dt><dd><p>" + IBUtils.ObjectToStr(listItem["Title"]) + "</p><div style='height:190px;' id='companyProfile'>" + IBUtils.DropHTML(IBUtils.ObjectToStr(listItem["ContentBox"].ToString())) + "</div></dd></dl>";
                            CacheHelper.SetCache("EnvisionCompanyInfoCache", companylnfoHtml, DateTime.Now.AddHours(23), TimeSpan.Zero);
                        }
                        catch
                        {
                            companylnfoHtml = "<dl class=\"mbox_1\"><dt></dt><dd><p></p><div></div></dd></dl>";
                        }

                    }
                });

            }
        }

    }
}
