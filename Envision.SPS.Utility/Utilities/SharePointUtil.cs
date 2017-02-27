using System;
using System.Net;
using System.Collections.Generic;
using Microsoft.SharePoint;
using SPClient = Microsoft.SharePoint.Client;

namespace Envision.SPS.Utility.Utilities
{
    public class SharePointUtil
    {

        public static List<SPClient.PermissionKind> GetWebPermissionKinds()
        {
            return new List<SPClient.PermissionKind>
            {
                SPClient.PermissionKind.ManageLists
            };
        }

        public static List<SPClient.PermissionKind> GetListPermissionKinds()
        {
            return new List<SPClient.PermissionKind>
            {
                SPClient.PermissionKind.ManageLists,
                SPClient.PermissionKind.AddListItems,
                SPClient.PermissionKind.DeleteListItems,
                SPClient.PermissionKind.EditListItems,
                SPClient.PermissionKind.ViewListItems,
                SPClient.PermissionKind.Open,
                SPClient.PermissionKind.OpenItems,
                SPClient.PermissionKind.DeleteVersions,
                SPClient.PermissionKind.ManagePermissions
            };
        }

        public static List<SPClient.PermissionKind> GetListItemPermissionKinds()
        {
            return new List<SPClient.PermissionKind>
            {
                SPClient.PermissionKind.AddListItems,
                SPClient.PermissionKind.DeleteListItems,
                SPClient.PermissionKind.EditListItems,
                SPClient.PermissionKind.ViewListItems,
                SPClient.PermissionKind.ViewVersions,
                SPClient.PermissionKind.Open,
                SPClient.PermissionKind.OpenItems,
                SPClient.PermissionKind.DeleteVersions,
                SPClient.PermissionKind.ManagePermissions
            };
        }

        public static List<SPBasePermissions> GetListItemBasePermissions()
        {
            return new List<SPBasePermissions>
            {
                SPBasePermissions.AddListItems,
                SPBasePermissions.DeleteListItems,
                SPBasePermissions.EditListItems,
                SPBasePermissions.ViewListItems,
                SPBasePermissions.ViewVersions,
                SPBasePermissions.Open,
                SPBasePermissions.OpenItems,
                SPBasePermissions.DeleteVersions,
                SPBasePermissions.ManagePermissions
            };
        }

        public static SPClient.ListItemCollection GetListItems(SPClient.ClientContext clientContext, Guid? listId, string title, string scope, string viewFields, string where, string orderBy, int? rowLimit, string serverRelativeUrl = null)
        {
            SPClient.List list = listId.HasValue ? clientContext.Web.Lists.GetById(listId.Value) : clientContext.Web.Lists.GetByTitle(title);
            var query = new SPClient.CamlQuery
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
            if (!string.IsNullOrWhiteSpace(serverRelativeUrl))
            {
                query.FolderServerRelativeUrl = serverRelativeUrl;
            }
            SPClient.ListItemCollection listItemCollection = list.GetItems(query);
            clientContext.Load(listItemCollection);
            clientContext.ExecuteQuery();
            return listItemCollection;
        }

        public static SPListItemCollection GetSPListItems(Guid? listId, string title, string scope, string viewFields, string where, string orderBy, int? rowLimit, string serverRelativeUrl = null)
        {
            SPList list = listId.HasValue ? SPContext.Current.Web.Lists[listId.Value] : SPContext.Current.Web.Lists[title];
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
            if (!string.IsNullOrWhiteSpace(serverRelativeUrl))
            {
                //query.FolderServerRelativeUrl = serverRelativeUrl;
                //query.Folder = serverRelativeUrl;
            }
            query.ViewAttributes = "Scope='Recursive'";
            SPListItemCollection listItemCollection = list.GetItems(query);
            return listItemCollection;
        }
        public static SPClient.ClientContext GetClientContext()
        {
            string webUrl = SPContext.Current.Web.Url;

            SPClient.ClientContext clientContext = new SPClient.ClientContext(webUrl);
            return clientContext;
        }
        public static SPClient.User GetCurrentUser(SPClient.ClientContext clientContext)
        {
            SPClient.User user = clientContext.Web.CurrentUser;
            clientContext.Load(user, field => field.Id, field => field.Email, field => field.Title, field => field.LoginName);
            clientContext.ExecuteQuery();
            return user;
        }

        public static string GetCurrentUserName(SPClient.ClientContext clientContext)
        {
            SPClient.User user = clientContext.Web.CurrentUser;
            clientContext.Load(user);
            clientContext.ExecuteQuery();
            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                return user.Email;
            }
            string[] arrary = user.LoginName.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            return arrary[arrary.Length - 1];
        }

        public static string GetServerRelativeUrl(SPClient.ClientContext clientContext, Guid? listId, int? listItemId, string title)
        {
            SPClient.List list = listId.HasValue ? clientContext.Web.Lists.GetById(listId.Value) : clientContext.Web.Lists.GetByTitle(title);
            if (!listItemId.HasValue)
            {
                clientContext.Load(list.RootFolder, field => field.ServerRelativeUrl);
                clientContext.ExecuteQuery();
                return list.RootFolder.ServerRelativeUrl;
            }
            SPClient.ListItem listItem = list.GetItemById(listItemId.Value);
            clientContext.Load(listItem);
            clientContext.ExecuteQuery();
            return Util.GetString(listItem["FileRef"]);
        }

        public static string GetServerRelativeUrl(Guid? listId, int? listItemId, string title)
        {
            SPList list = listId.HasValue ? SPContext.Current.Web.Lists[listId.Value] : SPContext.Current.Web.Lists[title];
            if (!listItemId.HasValue)
            {
                return list.RootFolder.ServerRelativeUrl;
            }
            SPListItem listItem = list.GetItemById(listItemId.Value);
            return IBUtils.ObjectToStr(listItem["FileRef"]);
        }
    }
}
