using Envision.SPS.Utility.Handlers;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envision.SPS.Portal.Web.Layouts.EnvisionPortal
{
    public class BasePage : LayoutsPageBase
    {
        protected internal Envision.SPS.Utility.Models.EnvisionPortalConfig EnvisionPagesConfig;
        public BasePage()
        {
            this.Load += new EventHandler(BasePage_Load);
            EnvisionPagesConfig = EnvisionPortalConfig.loadConfig();
            EnvisionPortalConfig.LanguageInfo();
        }

        private void BasePage_Load(object sender, EventArgs e)
        {
            //判断管理员是否登录
        }

        public static string L(string name)
        {
            return EnvisionBaseLanguage.L(name);
        }

        public static SPListItemCollection GetListItems(SPWeb web, string title, string scope, string viewFields, string where, string orderBy, int? rowLimit)
        {
            SPList list = web.Lists[title];
            var query = new SPQuery
            {
                ViewXml = string.Format(@"<View{0}>", string.IsNullOrEmpty(scope) ? "" : string.Format(@" Scope=""{0}""", scope)) +
                              (string.IsNullOrEmpty(viewFields) ? "" : ("<ViewFields>" + viewFields + "</ViewFields>")) +
                              (!string.IsNullOrEmpty(where) || !string.IsNullOrEmpty(orderBy) ? "<Query>" : "") +
                              (!string.IsNullOrEmpty(where) ? "<Where>" + where + "</Where>" : "") +
                              (!string.IsNullOrEmpty(orderBy) ? "<OrderBy>" + orderBy + "</OrderBy>" : "") +
                              (!string.IsNullOrEmpty(where) || !string.IsNullOrEmpty(orderBy) ? "</Query>" : "") +
                              (rowLimit.HasValue ? string.Format("<RowLimit>{0}</RowLimit>", rowLimit.Value) : "") +
                          "</View>"
            };
            SPListItemCollection listItemCollection = list.GetItems(query);
            return listItemCollection;
        }
    }
}
