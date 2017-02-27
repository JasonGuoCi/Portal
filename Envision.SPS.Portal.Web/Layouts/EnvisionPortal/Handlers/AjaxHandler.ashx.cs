using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web;
using Envision.SPS.Utility.Utilities;
using Envision.SPS.Utility.Enums;
using Envision.SPS.Utility.Handlers;

namespace Envision.SPS.Portal.Web.Layouts.EnvisionPortal.Handlers
{
    public partial class AjaxHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/Json";
            string contents;
            int listItemId;
            try
            {
                PortalMethodName methodName = (PortalMethodName)Convert.ToInt32(context.Request["MethodName"]);

                switch (methodName)
                {
                    case PortalMethodName.GetWeather:
                        contents = PortalHandler.GetWeather();
                        break;
                    case PortalMethodName.GetAnnouncement:
                        contents = PortalHandler.GetAnnouncement();
                        break;
                    case PortalMethodName.GetAnnouncementDetailed:
                        listItemId = IBRequest.GetQueryInt("listItemId");
                        contents = PortalHandler.GetAnnouncementDetailed(listItemId);
                        break;
                    default:
                        contents = Util.WriteJsonpToResponse(ResponseStatus.Failure, "Url Is Error!");
                        break;
                }
            }
            catch (Exception exception)
            {
                contents = Util.WriteJsonpToResponse(ResponseStatus.Exception, exception.Message);
            }
            context.Response.Write(contents);
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
