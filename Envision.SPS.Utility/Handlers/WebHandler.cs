using System;
using System.Collections.Generic;
using Envision.SPS.Utility.Enums;
using Envision.SPS.Utility.Exceptions;
using Envision.SPS.Utility.Utilities;
using SPClient = Microsoft.SharePoint.Client;

namespace Envision.SPS.Utility.Handlers
{
    public class WebHandler
    {
        public static string CheckPermission()
        {
            try
            {
                using (SPClient.ClientContext clientContext = SharePointUtil.GetClientContext())
                {
                    SPClient.Web web = clientContext.Web;
                    clientContext.Load(web, field => field.EffectiveBasePermissions);
                    clientContext.ExecuteQuery();
                    SPClient.BasePermissions basePermissions = web.EffectiveBasePermissions;
                    List<object> results = new List<object>();
                    List<SPClient.PermissionKind> permissionKinds = SharePointUtil.GetWebPermissionKinds();
                    foreach (var permissionKind in permissionKinds)
                    {
                        results.Add(new
                        {
                            PermissionKind = permissionKind,
                            HasPermission = basePermissions.Has(permissionKind)
                        });
                    }
                    return Util.WriteJsonpToResponse(ResponseStatus.Success, results);
                }
            }
            catch (OptionException)
            {
                return Util.WriteJsonpToResponse(ResponseStatus.Noneffect);
            }
            catch (Exception exception)
            {
                return Util.WriteJsonpToResponse(ResponseStatus.Exception, exception.Message);
            }
        }
    }
}
