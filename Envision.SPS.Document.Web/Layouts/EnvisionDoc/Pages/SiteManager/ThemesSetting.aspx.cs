using System;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web;
using Microsoft.SharePoint.Utilities;

namespace Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.SiteManager
{
    public partial class ThemesSetting : LayoutsPageBase
    {
        private const string BluethemValue = "此调色板主要是 白色，带有 灰色-80% 和 深蓝色。";
        private const string OrangethemValue = "此调色板主要是 浅黄色，带有 深红色 和 橙色。";//"此调色板主要是 白色，带有 灰色-80% 和 橙色。";
        private const string GreenthemValue = "此调色板主要是 白色，带有 灰色-80% 和 绿色。";
        public string RadioSelectId = "";
        public string ThemResult = string.Empty;
        public string ReturnFalse = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SPContext.Current.Web.DoesUserHavePermissions(SPBasePermissions.ManageWeb))
                {
                    Response.Write("你没有权限");
                    Response.End();
                }
                try
                {
                    using (SPWeb spw = SPContext.Current.Web)
                    {
                        SPThemeInfo thinfo = spw.ThemeInfo;
                        string spwinfo = thinfo.AccessibleDescription;
                        switch (spwinfo)
                        { 
                            case BluethemValue:
                                RadioSelectId = "theme1";
                                break;
                            case GreenthemValue:
                                RadioSelectId = "theme2";
                                break;
                            case OrangethemValue:
                                RadioSelectId = "theme3";
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
 
                }
            }
        }
        protected void BtnSaveTheme_Click(object sender, EventArgs e)
        {
            if (this.ThemeVlues.Value == "" || this.ThemeVlues.Value == string.Empty) return;
            else {
                bool result = this.SetSPWebTheme(this.ThemeVlues.Value);
                if (result) ThemResult = "layer.msg('皮肤更换成功！'); parent.location.reload();"; else ThemResult = "layer.msg('皮肤更换失败!'); parent.location.reload();";
             }
        }


        private bool SetSPWebTheme(string selectedColorPaletteUrl)
        {

            if (selectedColorPaletteUrl == "" && selectedColorPaletteUrl == string.Empty) return false;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                    {
                        spSite.AllowUnsafeUpdates = true;
                        using (SPWeb sPweb = spSite.OpenWeb())
                        {
                           SPThemeInfo thinfo=  sPweb.ThemeInfo;
                            var fontSchemeUrl = "/_catalogs/theme/15/fontscheme001.spfont";
                            string backgroundImageUrl = null;
                            //true 存储在跟网站，false 存储在当前网站
                            const bool shareGenerated = false;
                            sPweb.AllowUnsafeUpdates = true;
                            sPweb.ValidateFormDigest();
                            sPweb.ApplyTheme(selectedColorPaletteUrl, fontSchemeUrl, backgroundImageUrl, shareGenerated);
                            sPweb.Update();
                            sPweb.AllowUnsafeUpdates = false;
                        }
                    }
                });
            }
            catch {
                return false;
            }

            return true;

        }
    }
}
