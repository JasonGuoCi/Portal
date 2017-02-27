using System;
using System.Web;
using Envision.SPS.Utility.Enums;
using Envision.SPS.Utility.Extensions;
using Envision.SPS.Utility.Utilities;
using Envision.SPS.Utility.Handlers;
using System.Collections.Generic;
using Envision.SPS.Utility.Models;

namespace Envision.SPS.Document.Web.Layouts.EnvisionDoc.Handlers
{
    public partial class AjaxHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/Json";
            string contents;
            try
            {
                MethodName methodName = (MethodName)Convert.ToInt32(context.Request["MethodName"]);
                string CreateDocumentJSON;
                string editDocumentjson;
                string keyword;
                string doclibName;
                string editBeforeDocLibName;
                string createSPGroupsattr;
                string spgroupsname;
                string editBeforeGroupsName;
                string editspGroupsStr;
                Guid? listId;
                Guid? foldeItemId;
                Guid? siteId;
                Guid? webId;
                Guid? folderId;
                int IdType;
                int id;
                int pageindex;
                int category;
                switch (methodName)
                {
                    case MethodName.GetLeftMenus:
                        contents = LeftNavHandler.GetLeftMeuns();
                        break;
                    case MethodName.AddDocumentLibrary:
                        CreateDocumentJSON = HttpUtility.UrlDecode(IBRequest.GetQueryString("createDocumentattr"));
                        var DocumentLibraryAttr = IBUtils.ConvertByteDataToObject<DocumentLibraryAttr>(CreateDocumentJSON);
                        contents = ListHandler.AddDocumentLibrary(DocumentLibraryAttr);
                        break;
                    case MethodName.getUserGroups:
                        keyword = HttpUtility.UrlDecode(IBRequest.GetQueryString("keyword"));
                        pageindex = IBRequest.GetQueryInt("pageindex");
                        category = IBRequest.GetQueryInt("category");
                        //contents = ListHandler.GetUserGroups(keyword);
                        contents = ListHandler.GetUserGroups(pageindex, keyword, category);
                        break;
                    case MethodName.IsExistDocumentLibrary:
                        doclibName = HttpUtility.UrlDecode(IBRequest.GetString("param"));
                        contents = ListHandler.IsExistDocumentLibrary(doclibName);
                        break;
                    case MethodName.addSPGroups:
                        createSPGroupsattr = HttpUtility.UrlDecode(IBRequest.GetQueryString("createSPGroupsattr"));
                        var SPGroupsAttr = IBUtils.ConvertByteDataToObject<SPGroups>(createSPGroupsattr);
                        contents = ListHandler.AddSPGroups(SPGroupsAttr);
                        break;
                    case MethodName.getSPGroupsInfoByName:
                        spgroupsname = HttpUtility.UrlDecode(IBRequest.GetQueryString("spgroupsname"));
                        contents = ListHandler.GetSPGroupsByName(spgroupsname);
                        break;
                    case MethodName.editSPGroups:
                        editspGroupsStr = HttpUtility.UrlDecode(IBRequest.GetQueryString("editspGroupsStr"));
                        var EditGroups = IBUtils.ConvertByteDataToObject<SPGroups>(editspGroupsStr);
                        contents = ListHandler.EditSPGroups(EditGroups);
                        break;
                    case MethodName.getDocumentLibrary:
                        doclibName = HttpUtility.UrlDecode(IBRequest.GetQueryString("doclibName"));
                        contents = ListHandler.GetDocumentLibraryInfo(doclibName);
                        break;
                    case MethodName.editDocumentLibrary:
                        editDocumentjson = HttpUtility.UrlDecode(IBRequest.GetQueryString("editDocumentattr"));
                        var EditDocumentLibraryAttr = IBUtils.ConvertByteDataToObject<DocumentLibraryAttr>(editDocumentjson);
                        contents = ListHandler.EditDocumentLibrary(EditDocumentLibraryAttr);
                        break;
                    case MethodName.IsExistSPGroups:
                        spgroupsname = HttpUtility.UrlDecode(IBRequest.GetString("param"));
                        contents = ListHandler.IsExistSPGroups(spgroupsname);
                        break;
                    case MethodName.IsExistEditSPGroups:
                        spgroupsname = HttpUtility.UrlDecode(IBRequest.GetString("param"));
                        editBeforeGroupsName = HttpUtility.UrlDecode(IBRequest.GetQueryString("beforeGroupsName"));
                        contents = ListHandler.isExistEditGroupsName(editBeforeGroupsName, spgroupsname);
                        break;
                    case MethodName.isExistEditDocumentLibrary:
                        doclibName = HttpUtility.UrlDecode(IBRequest.GetString("param"));
                        editBeforeDocLibName = HttpUtility.UrlDecode(IBRequest.GetQueryString("beforeDocLibName"));
                        contents = ListHandler.isExistEditDocumentLibrary(editBeforeDocLibName, doclibName);
                        break;
                    case MethodName.GetTreeNodes:
                        contents = ListHandler.GetTreeNodes();
                        break;
                    case MethodName.GetTreeChildNodes:
                        listId = new Guid(context.Request["listId"]);
                        foldeItemId = new Guid(context.Request["id"]);
                        contents = ListHandler.GetTreeChildNodes(foldeItemId.Value, listId.Value);
                        break;
                    case MethodName.GetItTreeNodes:
                        contents = ListHandler.GetItTreeNodes();
                        break;
                    case MethodName.GetItTreeChildNodes:
                        siteId = null;
                        webId = null;
                        listId = null;
                        folderId = null;
                        if (!string.IsNullOrEmpty(IBRequest.GetFormString("siteId")))
                        {
                            siteId = new Guid(IBRequest.GetFormString("siteId"));
                        }
                        if (!string.IsNullOrEmpty(IBRequest.GetFormString("webId")))
                        {
                            webId = new Guid(IBRequest.GetFormString("webId"));
                        }
                        if (!string.IsNullOrEmpty(IBRequest.GetFormString("listId")))
                        {
                            listId = new Guid(IBRequest.GetFormString("listId"));
                        }
                        if (!string.IsNullOrEmpty(IBRequest.GetFormString("id")))
                        {
                            folderId = new Guid(IBRequest.GetFormString("id"));
                        }
                        IdType = IBRequest.GetFormQueryInt("IdType");
                        contents = ListHandler.GetItTreeChildNodes(siteId.Value, webId.Value, listId, folderId, IdType);
                        break;
                    case MethodName.permissionsReportResult:
                        id=IBRequest.GetQueryInt("id");
                        contents = ListHandler.GetPermissionsReportResult(id);
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
