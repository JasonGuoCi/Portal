using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Envision.SPS.Utility.Utilities;
using Envision.SPS.Utility.Exceptions;
using Envision.SPS.Utility.Enums;
using System.Collections.Generic;

namespace Envision.SPS.Portal.Web.Layouts.EnvisionPortal.Pages.Announcement
{
    public partial class AnnouncementDetailed : BasePage
    {
        int listItemId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                listItemId = IBRequest.GetQueryInt("listItemId");
                GetAnnouncementDetailed(listItemId);
            }
        }
        public void GetAnnouncementDetailed(int listItemId)
        {
            try
            {
                string weburl = SPContext.Current.Web.Url;
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite spSite = new SPSite(weburl))
                    {
                        spSite.AllowUnsafeUpdates = true;
                        using (SPWeb web = spSite.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;

                            SPListItem listItem = web.Lists[this.EnvisionPagesConfig.EnvsionAnnouncement].GetItemById(listItemId);
                            ltlTitle.Text = listItem.Title;
                            ltltPublistDate.Text = listItem["PublishedDate"] == null ? "" : IBUtils.ObjectToDateTime(listItem["PublishedDate"]).ToString("yyyy-MM-dd");
                            ltlContetnBox.Text = IBUtils.ObjectToStr(listItem["ContentBox"]);
                            SPAttachmentCollection attachmentCollection = listItem.Attachments;
                            string urlPrefix = attachmentCollection.UrlPrefix;
                            if (attachmentCollection.Count > 0)
                            {
                                attachmentDiv.Visible = true;
                                List<object> attachment = new List<object>();
                                foreach (string attName in attachmentCollection)
                                {
                                    SPFile file = web.GetFile(urlPrefix + attName);
                                    attachment.Add(new
                                    {
                                        name = file.Name,
                                        url = web.Url + "/" + file.Url
                                    });
                                }

                                RepAttachment.DataSource = attachment;
                                RepAttachment.DataBind();
                            }

                            web.AllowUnsafeUpdates = false;
                        }
                        spSite.AllowUnsafeUpdates = false;
                    }
                });
            }
            catch (Exception ex)
            {

            }
        }

    }
}
