using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Envision.SPS.Utility.Enums;
using Envision.SPS.Utility.Utilities;
using System.Web;
using Envision.SPS.Utility.Exceptions;
using Microsoft.SharePoint;
using Envision.SPS.Utility.Models;
using System.IO;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Administration;
using Envision.SPS.DataAccess;
using Envision.SPS.Utility.Extensions;

namespace Envision.SPS.Utility.Handlers
{
    public class ListHandler
    {
        public static string AddDocumentLibrary(DocumentLibraryAttr documentLibraryAttr)
        {
            try
            {
                CreateDocumentlabrary(documentLibraryAttr);
                SPGroupAddToDocumentLabrary(documentLibraryAttr);

                return Util.WriteJsonpToResponse(ResponseStatus.Success);
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

        private static void SPGroupAddToDocumentLabrary(DocumentLibraryAttr documentLibraryAttr)
        {
            try
            {
                string webUrl = SPContext.Current.Web.Url;
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite spSite = new SPSite(webUrl))
                    {
                        spSite.AllowUnsafeUpdates = true;
                        using (SPWeb web = spSite.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;

                            SPList list = web.Lists[documentLibraryAttr.DocumentLibraryName];

                            foreach (var item in documentLibraryAttr.spuserGroups)
                            {
                                SPServiceUtil.AddGroupToRolesForList(web, list, item.Name, (SPGroupRolesCategory)IBUtils.StrToInt(item.GroupRole, 0));
                            }

                            var DocumentVersionControlCategory = (SPDocumentVersionControlCategory)IBUtils.StrToInt(documentLibraryAttr.DocumentVersionControlId, 0);
                            switch (DocumentVersionControlCategory)
                            {
                                case SPDocumentVersionControlCategory.None:
                                    list.EnableVersioning = false;
                                    list.EnableMinorVersions = false;
                                    break;
                                case SPDocumentVersionControlCategory.Main:
                                    list.EnableVersioning = true;
                                    break;
                                case SPDocumentVersionControlCategory.Minor:
                                    list.EnableVersioning = true;
                                    list.EnableMinorVersions = true;
                                    break;
                            }

                            var DocForceChekout = (SPForceCheckout)IBUtils.StrToInt(documentLibraryAttr.DocumentDefaultCheckOutId, 0);
                            switch (DocForceChekout)
                            {
                                case SPForceCheckout.是:
                                    list.ForceCheckout = true;
                                    break;
                                case SPForceCheckout.否:
                                    list.ForceCheckout = false;
                                    break;
                                default:
                                    list.ForceCheckout = false;
                                    break;
                            }

                            list.Update();

                            web.AllowUnsafeUpdates = false;
                        }
                        spSite.AllowUnsafeUpdates = false;
                    }
                });
            }
            catch (Exception)
            {

            }
        }

        public static void CreateDocumentlabrary(DocumentLibraryAttr documentLibraryAttr)
        {
            string webUrl = SPContext.Current.Web.Url;
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite spSite = new SPSite(webUrl))
                {
                    spSite.AllowUnsafeUpdates = true;
                    using (SPWeb web = spSite.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;

                        SPListTemplate ListTemplate = null;
                        if (!string.IsNullOrEmpty(documentLibraryAttr.DocumentLibraryTemplateId))
                        {
                            ListTemplate = GetCustomListTemplate(documentLibraryAttr.DocumentLibraryTemplateId);
                        }
                        else
                        {
                            ListTemplate = GetDefaultListTemplate();
                        }

                        web.Lists.Add(documentLibraryAttr.DocumentLibraryName, null, ListTemplate);

                        web.Update();

                        web.AllowUnsafeUpdates = false;
                    }
                    spSite.AllowUnsafeUpdates = false;
                }
            });
        }

        public static string IsExistDocumentLibrary(string DocumentLabraryName)
        {
            if (string.IsNullOrEmpty(DocumentLabraryName))
            {
                return "{ \"info\":\"名称不可为空\", \"status\":\"n\" }";
            }
            bool isExist = true;
            try
            {
                string webUrl = SPContext.Current.Web.Url;
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite spSite = new SPSite(webUrl))
                    {
                        spSite.AllowUnsafeUpdates = true;
                        using (SPWeb web = spSite.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;

                            isExist = SPServiceUtil.IsExistDocumentLibrary(web, DocumentLabraryName);

                            web.AllowUnsafeUpdates = false;
                        }
                        spSite.AllowUnsafeUpdates = false;
                    }
                });
                if (isExist)
                {
                    return "{ \"info\":\"该名称可使用\", \"status\":\"y\" }";
                }
                else
                {
                    return "{ \"info\":\"该名称已被占用！\", \"status\":\"n\" }";
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

        public static string IsExistSPGroups(string spGroupsName)
        {
            if (string.IsNullOrEmpty(spGroupsName))
            {
                return "{ \"info\":\"名称不可为空\", \"status\":\"n\" }";
            }
            bool isExist = true;
            try
            {
                string webUrl = SPContext.Current.Web.Url;
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite spSite = new SPSite(webUrl))
                    {
                        spSite.AllowUnsafeUpdates = true;
                        using (SPWeb web = spSite.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;

                            isExist = SPServiceUtil.IsExistGroup(web, spGroupsName);

                            web.AllowUnsafeUpdates = false;
                        }
                        spSite.AllowUnsafeUpdates = false;
                    }
                });
                if (!isExist)
                {
                    return "{ \"info\":\"该名称可使用\", \"status\":\"y\" }";
                }
                else
                {
                    return "{ \"info\":\"该名称已被占用！\", \"status\":\"n\" }";
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

        public static string isExistEditDocumentLibrary(string DocumentLabraryName, string newDocumentLibraryName)
        {
            if (string.IsNullOrEmpty(newDocumentLibraryName))
            {
                return "{ \"info\":\"名称不可为空\", \"status\":\"n\" }";
            }
            bool isExist = true;
            try
            {
                string webUrl = SPContext.Current.Web.Url;
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite spSite = new SPSite(webUrl))
                    {
                        spSite.AllowUnsafeUpdates = true;
                        using (SPWeb web = spSite.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;

                            isExist = SPServiceUtil.IsExistEditDocumentLibrary(web, DocumentLabraryName, newDocumentLibraryName);

                            web.AllowUnsafeUpdates = false;
                        }
                        spSite.AllowUnsafeUpdates = false;
                    }
                });
                if (isExist)
                {
                    return "{ \"info\":\"该名称可使用\", \"status\":\"y\" }";
                }
                else
                {
                    return "{ \"info\":\"该名称已被占用！\", \"status\":\"n\" }";
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

        public static string isExistEditGroupsName(string GroupsName, string newGroupsName)
        {
            if (string.IsNullOrEmpty(newGroupsName))
            {
                return "{ \"info\":\"名称不可为空\", \"status\":\"n\" }";
            }
            bool isExist = true;
            try
            {
                string webUrl = SPContext.Current.Web.Url;
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite spSite = new SPSite(webUrl))
                    {
                        spSite.AllowUnsafeUpdates = true;
                        using (SPWeb web = spSite.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;

                            isExist = SPServiceUtil.IsExistEditGroup(web, GroupsName, newGroupsName);

                            web.AllowUnsafeUpdates = false;
                        }
                        spSite.AllowUnsafeUpdates = false;
                    }
                });
                if (isExist)
                {
                    return "{ \"info\":\"该名称可使用\", \"status\":\"y\" }";
                }
                else
                {
                    return "{ \"info\":\"该名称已被占用！\", \"status\":\"n\" }";
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

        public static string AddSPGroups(SPGroups spGroups)
        {
            try
            {
                bool addResult = false;
                string webUrl = SPContext.Current.Web.Url;
                SPGroup spg = null;
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite spSite = new SPSite(webUrl))
                    {
                        spSite.AllowUnsafeUpdates = true;
                        using (SPWeb web = spSite.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;

                            if (!string.IsNullOrEmpty(spGroups.GroupsOwnerName))
                            {
                                SPGroup ownergroup = web.SiteGroups.GetByID(spGroups.GroupsOwnerId);
                                addResult = SPServiceUtil.AddGroup(web, spGroups.GroupsName, ownergroup, SPContext.Current.Web.CurrentUser, spGroups.GroupsDescription);

                            }
                            else
                            {
                                SPUser userGroup = SPContext.Current.Web.CurrentUser;
                                addResult = SPServiceUtil.AddGroup(web, spGroups.GroupsName, userGroup, SPContext.Current.Web.CurrentUser, spGroups.GroupsDescription);
                            }
                            spg = SPServiceUtil.GetGroupsByName(web, spGroups.GroupsName);
                            if (spg != null)
                            {
                                spGroups.GroupsId = spg.ID;
                            }

                            web.AllowUnsafeUpdates = false;
                        }
                        spSite.AllowUnsafeUpdates = false;
                    }
                });

                if (addResult)
                {
                    return Util.WriteJsonpToResponse(ResponseStatus.Success, spGroups);
                }
                else
                {
                    return Util.WriteJsonpToResponse(ResponseStatus.Noneffect);
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

        /// <summary>
        /// 编辑SPGroups
        /// </summary>
        /// <param name="spGroups"></param>
        /// <returns></returns>
        public static string EditSPGroups(SPGroups spGroups)
        {

            try
            {
                string webUrl = SPContext.Current.Web.Url;
                var item = new SPGroupModel();
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite spSite = new SPSite(webUrl))
                    {
                        spSite.AllowUnsafeUpdates = true;
                        using (SPWeb web = spSite.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            SPGroup group = web.SiteGroups.GetByID(spGroups.GroupsId);

                            item.Id = group.ID.ToString();
                            SPGroup ownerGroups = null;
                            try
                            {
                                ownerGroups = web.SiteGroups.GetByID(spGroups.GroupsOwnerId);//[spGroups.GroupsOwnerName];
                            }
                            catch (Exception)
                            {

                            }
                            if (ownerGroups != null)
                            {
                                group.Owner = ownerGroups;
                            }
                            group.Description = spGroups.GroupsDescription;
                            group.Name = spGroups.GroupsName;

                            group.Update();

                            web.AllowUnsafeUpdates = false;
                        }
                        spSite.AllowUnsafeUpdates = false;
                    }
                });
                return Util.WriteJsonpToResponse(ResponseStatus.Success, item);
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


        /// <summary>
        /// 编辑群组信息
        /// </summary>
        /// <param name="groupsName">群组名称</param>
        /// <returns></returns>
        public static string GetSPGroupsByName(string groupsName)
        {
            try
            {

                SPGroups spGroups = new SPGroups();
                string webUrl = SPContext.Current.Web.Url;
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite spSite = new SPSite(webUrl))
                    {
                        spSite.AllowUnsafeUpdates = true;
                        using (SPWeb web = spSite.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            SPGroup groups = web.SiteGroups[groupsName];
                            spGroups.GroupsDescription = groups.Description;
                            if (groups.Owner.ToString().IndexOf('|') > 0)
                            {
                                spGroups.GroupsOwnerName = groups.Owner.ToString().Split('|')[1];
                            }
                            else
                            {
                                spGroups.GroupsOwnerName = groups.Owner.ToString();
                            }

                            spGroups.GroupsOwnerId = groups.Owner.ID;

                            SPUserCollection spUsers = groups.Users;

                            web.AllowUnsafeUpdates = false;
                        }
                        spSite.AllowUnsafeUpdates = false;
                    }
                });


                return Util.WriteJsonpToResponse(ResponseStatus.Success, spGroups);

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

        /// <summary>
        /// 获取文档库的基本信息
        /// </summary>
        /// <returns></returns>
        public static string GetDocumentLibraryInfo(string docLibName)
        {
            if (string.IsNullOrEmpty(docLibName))
            {
                return "";
            }
            try
            {
                DocumentLibraryAttr doclib = new DocumentLibraryAttr();
                string webUrl = SPContext.Current.Web.Url;
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite spSite = new SPSite(webUrl))
                    {
                        spSite.AllowUnsafeUpdates = true;
                        using (SPWeb web = spSite.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            Guid gdocLibId = new Guid();
                            Guid.TryParse(docLibName, out gdocLibId);
                            SPList spList = web.Lists[gdocLibId];

                            doclib.DocumentDefaultCheckOutId = spList.ForceCheckout ? "1" : "0";
                            if (spList.EnableMinorVersions && spList.EnableVersioning)
                            {
                                doclib.DocumentVersionControlId = "2";
                            }
                            else if (spList.EnableVersioning && !spList.EnableMinorVersions)
                            {
                                doclib.DocumentVersionControlId = "1";
                            }
                            else
                            {
                                doclib.DocumentVersionControlId = "0";
                            }
                            doclib.BeforeDocumentLibraryName = spList.Title;

                            web.AllowUnsafeUpdates = false;
                        }
                        spSite.AllowUnsafeUpdates = false;
                    }
                });


                return Util.WriteJsonpToResponse(ResponseStatus.Success, doclib);

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

        public static string EditDocumentLibrary(DocumentLibraryAttr docLibAttr)
        {
            try
            {
                string webUrl = SPContext.Current.Web.Url;
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite spSite = new SPSite(webUrl))
                    {
                        spSite.AllowUnsafeUpdates = true;
                        using (SPWeb web = spSite.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            Guid listId = new Guid();
                            Guid.TryParse(docLibAttr.listId, out listId);
                            SPList DocumentLibraryList = web.Lists[listId];

                            DocumentLibraryList.Title = docLibAttr.DocumentLibraryName;

                            var DocumentVersionControlCategory = (SPDocumentVersionControlCategory)IBUtils.StrToInt(docLibAttr.DocumentVersionControlId, 0);
                            switch (DocumentVersionControlCategory)
                            {
                                case SPDocumentVersionControlCategory.None:
                                    DocumentLibraryList.EnableVersioning = false;
                                    DocumentLibraryList.EnableMinorVersions = false;
                                    break;
                                case SPDocumentVersionControlCategory.Main:
                                    DocumentLibraryList.EnableVersioning = true;
                                    DocumentLibraryList.EnableMinorVersions = false;
                                    break;
                                case SPDocumentVersionControlCategory.Minor:
                                    DocumentLibraryList.EnableVersioning = true;
                                    DocumentLibraryList.EnableMinorVersions = true;
                                    break;
                            }

                            var DocForceChekout = (SPForceCheckout)IBUtils.StrToInt(docLibAttr.DocumentDefaultCheckOutId, 0);
                            switch (DocForceChekout)
                            {
                                case SPForceCheckout.是:
                                    DocumentLibraryList.ForceCheckout = true;
                                    break;
                                case SPForceCheckout.否:
                                    DocumentLibraryList.ForceCheckout = false;
                                    break;
                                default:
                                    DocumentLibraryList.ForceCheckout = false;
                                    break;
                            }

                            DocumentLibraryList.Update();

                            web.AllowUnsafeUpdates = false;
                        }
                        spSite.AllowUnsafeUpdates = false;
                    }
                });
                return Util.WriteJsonpToResponse(ResponseStatus.Success);
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

        public static string GetUserGroups(string keyword)
        {
            var dataSource = new List<SPGroupModel>();
            try
            {
                //获取站点集群组
                dataSource = GetGroupsInfoAll();

                if (!string.IsNullOrEmpty(keyword))
                {
                    dataSource = dataSource.Where(p => p.Name.Contains(keyword)).ToList();
                }
                return Util.WriteJsonpToResponse(ResponseStatus.Success, dataSource);
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

        public static string GetUserGroups(int pageindex, string keyword, int category)
        {
            var dataSource = new List<SPGroupModel>();
            int pageSize = 10;
            try
            {
                //获取站点集群组
                dataSource = GetGroupsInfoAll();
                int pageTotal = 0;
                dataSource = GetPagedList(dataSource, pageindex, pageSize, keyword, ref pageTotal);
                string PageContent = IBUtils.OutPageListAjax(pageSize, pageindex, pageTotal, keyword, category, 4);
                return Util.WriteJsonpToResponse(ResponseStatus.Success, PageContent, dataSource);
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

        private static List<SPGroupModel> GetPagedList(List<SPGroupModel> dataSource,
          int pageindex, int pageSize, string keywords, ref int total)
        {
            List<SPGroupModel> docList;
            List<SPGroupModel> data = null;
            if (!string.IsNullOrEmpty(keywords))
            {
                data = dataSource.Where(p => p.Id != null && p.Name.Contains(keywords)).ToList();
                docList = data.Where(p => p.Id != null && p.Name.Contains(keywords)).Skip((pageindex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                data = dataSource.Where(p => p.Id != null).ToList();
                docList = dataSource.Where(p => p.Id != null).Skip((pageindex - 1) * pageSize).Take(pageSize).ToList();
            }
            total = data.Count;
            return docList;
        }

        public static List<SPGroupModel> GetGroupsInfo()
        {
            var dataSource = new List<SPGroupModel>();
            string webUrl = SPContext.Current.Web.Url;
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite spSite = new SPSite(webUrl))
                {
                    spSite.AllowUnsafeUpdates = true;
                    using (SPWeb web = spSite.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;

                        SPRoleAssignmentCollection sprCollection = web.RoleAssignments;


                        SPGroupCollection spGroupCollection = web.Groups;
                        SPGroupCollection spuserowner = SPContext.Current.Web.CurrentUser.OwnedGroups;
                        foreach (SPGroup spGroup in spGroupCollection)
                        {
                            //SPMember memCrossSiteGroup = spGroup;
                            //SPPrincipal myssp = (SPPrincipal)memCrossSiteGroup;
                            //SPRoleAssignment myroles = new SPRoleAssignment(myssp);
                            //SPRoleDefinitionBindingCollection roleDefBindings = myroles.RoleDefinitionBindings;

                            if (spGroup.Roles.Count == 1)
                            {
                                //if(spGroup.Roles[0]=="")
                                string roleName = spGroup.Roles[0].Name;
                                if (roleName == "受限访问" || roleName.ToUpper() == "LIMITED ACCESS")
                                    continue;
                            }

                            var item = new SPGroupModel();
                            item.Id = spGroup.ID.ToString();
                            item.Name = spGroup.Name;
                            item.Description = spGroup.Description;
                            //如果没有权限continue
                            if (!IsOwnerPermmion(spuserowner, spGroup))
                            {
                                continue;
                            }

                            if (spGroup.Owner is SPUser)
                            {

                                if (spGroup.Owner.ToString().IndexOf('|') > 0)
                                {
                                    item.Owner = spGroup.Owner.ToString().Split('|')[1];
                                }
                                else
                                {
                                    item.Owner = spGroup.Owner.ToString();
                                }
                                item.OwnerType = "SPUser";
                                item.IsPermmsion = IsOwnerPermmion(spuserowner, spGroup);

                            }
                            else if (spGroup.Owner is SPGroup)
                            {
                                item.Owner = spGroup.Owner.ToString();
                                item.OwnerType = "SPGroup";
                                item.IsPermmsion = IsOwnerPermmion(spuserowner, spGroup);
                            }

                            dataSource.Add(item);
                        }
                        web.AllowUnsafeUpdates = false;
                    }
                    spSite.AllowUnsafeUpdates = false;

                }
            });
            return dataSource;
        }

        /// <summary>
        /// 获取站点集群组
        /// </summary>
        /// <returns></returns>
        public static List<SPGroupModel> GetGroupsInfoAll()
        {
            var dataSource = new List<SPGroupModel>();
            string webUrl = SPContext.Current.Web.Url;
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite spSite = new SPSite(webUrl))
                {
                    spSite.AllowUnsafeUpdates = true;
                    using (SPWeb web = spSite.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;

                        SPGroupCollection spGroupCollection = web.SiteGroups;
                        SPGroupCollection spuserowner = SPContext.Current.Web.CurrentUser.OwnedGroups;
                        foreach (SPGroup spGroup in spGroupCollection)
                        {
                            var item = new SPGroupModel();
                            item.Id = spGroup.ID.ToString();
                            item.Name = spGroup.Name;
                            item.Description = spGroup.Description;

                            if (spGroup.Owner is SPUser)
                            {
                                if (spGroup.Owner.ToString().IndexOf('|') > 0)
                                {
                                    item.Owner = spGroup.Owner.ToString().Split('|')[1];
                                }
                                else
                                {
                                    item.Owner = spGroup.Owner.ToString();
                                }
                                item.OwnerType = "SPUser";
                                item.IsPermmsion = IsOwnerPermmion(spuserowner, spGroup);
                            }
                            else if (spGroup.Owner is SPGroup)
                            {
                                item.Owner = spGroup.Owner.ToString();
                                item.OwnerType = "SPGroup";
                                item.IsPermmsion = IsOwnerPermmion(spuserowner, spGroup);
                            }
                            dataSource.Add(item);
                        }
                        web.AllowUnsafeUpdates = false;
                    }
                    spSite.AllowUnsafeUpdates = false;

                }
            });
            return dataSource;
        }

        /// <summary>
        /// 获取站点集群组并判断是否为站点集管理员
        /// </summary>
        /// <returns></returns>
        public static List<SPGroupModel> GetGroupsInfoSiteForIsSiteAdmin()
        {
            var dataSource = new List<SPGroupModel>();
            string webUrl = SPContext.Current.Web.Url;
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite spSite = new SPSite(webUrl))
                {
                    spSite.AllowUnsafeUpdates = true;
                    using (SPWeb web = spSite.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;

                        SPGroupCollection spGroupCollection = web.SiteGroups;
                        SPGroupCollection spuserowner = SPContext.Current.Web.CurrentUser.OwnedGroups;
                        foreach (SPGroup spGroup in spGroupCollection)
                        {
                            var item = new SPGroupModel();
                            item.Id = spGroup.ID.ToString();
                            item.Name = spGroup.Name;
                            item.Description = spGroup.Description;

                            if (spGroup.Owner is SPUser)
                            {
                                if (spGroup.Owner.ToString().IndexOf('|') > 0)
                                {
                                    item.Owner = spGroup.Owner.ToString().Split('|')[1];
                                }
                                else
                                {
                                    item.Owner = spGroup.Owner.ToString();
                                }
                                item.OwnerType = "SPUser";
                                item.IsPermmsion = IsOwnerOrSiteAdminPermmion(spuserowner, spGroup);
                            }
                            else if (spGroup.Owner is SPGroup)
                            {
                                item.Owner = spGroup.Owner.ToString();
                                item.OwnerType = "SPGroup";
                                item.IsPermmsion = IsOwnerOrSiteAdminPermmion(spuserowner, spGroup);
                            }
                            if (item.IsPermmsion)
                            {
                                dataSource.Add(item);
                            }
                        }
                        web.AllowUnsafeUpdates = false;
                    }
                    spSite.AllowUnsafeUpdates = false;

                }
            });
            return dataSource;
        }
        private static bool IsOwnerOrSiteAdminPermmion(SPGroupCollection groups, SPGroup group)
        {
            bool result = false;
            if (SPContext.Current.Web.CurrentUser.IsSiteAdmin) return true;

            foreach (SPGroup item in groups)
            {
                if (item.Name == group.Name)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        private static bool IsOwnerPermmion(SPGroupCollection groups, SPGroup group)
        {
            bool result = false;
            if (SPContext.Current.Web.DoesUserHavePermissions(SPBasePermissions.ManageWeb)) return true;

            foreach (SPGroup item in groups)
            {
                if (item.Name == group.Name)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private static SPListTemplate GetDefaultListTemplate()
        {
            SPListTemplate defaultListTemplate = null;
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite spSite = SPContext.Current.Site)
                {
                    spSite.AllowUnsafeUpdates = true;
                    using (SPWeb web = spSite.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        SPListTemplateCollection listTemplateCollection = web.ListTemplates;
                        foreach (SPListTemplate item in listTemplateCollection)
                        {
                            if (item.Type_Client == ConfigManager.Type_Client)
                            {
                                defaultListTemplate = item;
                                break;
                            }
                        }
                        web.AllowUnsafeUpdates = false;
                    }
                    spSite.AllowUnsafeUpdates = false;
                }
            });
            return defaultListTemplate;
        }

        private static SPListTemplate GetCustomListTemplate(string InternalName)
        {
            SPListTemplate CustomListTemplate = null;
            SPSecurity.RunWithElevatedPrivileges(() =>
               {
                   using (SPSite spSite = SPContext.Current.Site)
                   {
                       spSite.AllowUnsafeUpdates = true;
                       using (SPWeb web = spSite.OpenWeb())
                       {
                           web.AllowUnsafeUpdates = true;
                           SPListTemplateCollection CustomeListTemplateCollection = spSite.GetCustomListTemplates(web);
                           foreach (SPListTemplate item in CustomeListTemplateCollection)
                           {
                               if (item.InternalName == InternalName)
                               {
                                   CustomListTemplate = item;
                                   break;
                               }
                           }

                           web.AllowUnsafeUpdates = false;
                       }
                       spSite.AllowUnsafeUpdates = false;
                   }
               });
            return CustomListTemplate;
        }

        public static string GetTreeNodes()
        {
            try
            {
                string webUrl = SPContext.Current.Web.Url;
                List<object> treeNodes = new List<object>();
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite spSite = SPContext.Current.Site)
                    {
                        using (SPWeb web = spSite.OpenWeb())
                        {
                            List<Guid> listIds;

                            List<object> listTreeNodes = ListHandler.GetListTreeNodes(out listIds);
                            List<SPBasePermissions> basePermissions = SharePointUtil.GetListItemBasePermissions();
                            string listFileRef;
                            //SPListItemCollection listItemCollection;
                            for (int i = 0; i < listIds.Count; i++)
                            {
                                treeNodes.Add(listTreeNodes[i]);
                                listFileRef = SharePointUtil.GetServerRelativeUrl(listIds[i], null, null);
                            }
                        }
                    }

                });
                return Util.WriteJsonpToResponse(ResponseStatus.Success, treeNodes);
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

        public static string GetTreeChildNodes(Guid listItemId, Guid listId)
        {
            try
            {
                string webUrl = SPContext.Current.Web.Url;
                List<object> treeNodes = new List<object>();
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite spSite = new SPSite(webUrl))
                    {
                        using (SPWeb web = spSite.OpenWeb())
                        {
                            //string folderServerRelativeUrl = SharePointUtil.GetServerRelativeUrl(listId, listItemId, null);
                            List<SPBasePermissions> basePermissions = SharePointUtil.GetListItemBasePermissions();
                            SPListItemCollection listItemCollection;
                            SPList list;
                            list = web.Lists[listId];
                            SPQuery spQuery = new SPQuery();
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<OrderBy><FieldRef Name='BaseName'></FieldRef></OrderBy>");
                            spQuery.Query = sb.ToString();
                            SPFolder spfolder = null;

                            string listFileRef = SharePointUtil.GetServerRelativeUrl(listId, null, null);
                            if (listItemId != listId)
                            {
                                spfolder = web.GetFolder(listItemId);
                                spQuery.Folder = spfolder;
                                listItemCollection = list.GetItems(spQuery);
                                foreach (SPListItem item in listItemCollection)
                                {
                                    if (item.FileSystemObjectType == SPFileSystemObjectType.Folder)
                                    {
                                        SPFolder childFolder = item.Folder;
                                        SPPermissionInfo permissionInfo = item.GetUserEffectivePermissionInfo(SPContext.Current.Site.OpenWeb().CurrentUser.ToString());
                                        string permissions = string.Empty;

                                        //判断用户权限
                                        permissions = GetAssignments(permissionInfo);

                                        if (string.Equals(childFolder.Name, "Forms", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            continue;
                                        }

                                        treeNodes.Add(BuildTreeNode(listId, childFolder, basePermissions));
                                    }
                                }
                            }
                            else
                            {


                                spfolder = list.RootFolder;
                                spQuery.Folder = spfolder;
                                listItemCollection = list.GetItems(spQuery);

                                foreach (SPListItem item in listItemCollection)
                                {
                                    if (item.FileSystemObjectType == SPFileSystemObjectType.Folder)
                                    {
                                        SPFolder childFolder = item.Folder;
                                        SPPermissionInfo permissionInfo = item.GetUserEffectivePermissionInfo(SPContext.Current.Site.OpenWeb().CurrentUser.ToString());
                                        string permissions = string.Empty;

                                        //判断用户权限
                                        permissions = GetAssignments(permissionInfo);

                                        if (string.Equals(childFolder.Name, "Forms", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            continue;
                                        }

                                        treeNodes.Add(BuildTreeNode(listId, childFolder, basePermissions));
                                    }
                                }
                            }
                        }
                    }
                });
                return Util.WriteJsonpToResponse(ResponseStatus.Success, treeNodes);
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

        private static string GetAssignments(SPPermissionInfo permissionInfo)
        {
            string permissions = string.Empty;
            for (int i = 0; i < permissionInfo.RoleAssignments.Count; i++)
            {
                var permission = permissionInfo.RoleAssignments[i].RoleDefinitionBindings;

                if (permission == null)
                {
                    continue;
                }

                foreach (SPRoleDefinition item in permission)
                {
                    //完全控制
                    if (item.BasePermissions.ToString() == "FullMask")
                    {
                        permissions += string.IsNullOrEmpty(permissions) ? permission[0].BasePermissions.ToString() : "," + permission[0].BasePermissions.ToString();
                        break;
                    }
                    //添加文件夹权限
                    if (item.BasePermissions.ToString().Contains(SPBasePermissions.AddListItems.ToString()))
                    {
                        permissions += string.IsNullOrEmpty(permissions) ? SPBasePermissions.AddListItems.ToString() : "," + SPBasePermissions.AddListItems.ToString();
                    }
                    //签入权限
                    if (item.BasePermissions.ToString().Contains(SPBasePermissions.CancelCheckout.ToString()))
                    {
                        permissions += string.IsNullOrEmpty(permissions) ? SPBasePermissions.CancelCheckout.ToString() : "," + SPBasePermissions.CancelCheckout.ToString();
                    }
                }

            }
            return permissions;
        }
        internal static object BuildTreeNode(Guid listId, SPFolder folder, List<SPBasePermissions> basePermissions)
        {

            return new
            {
                id = folder.UniqueId.ToString(),
                name = folder.Name,
                listItemId = folder.Item.ID.ToString(),
                currentWebUrl = SPContext.Current.Web.Url,
                ParentId = folder.ParentListId.ToString(),
                pid = listId,
                listId = listId,
                Title = folder.ParentFolder.Name,
                ImageUrl = "",
                ListItemType = ListItemType.Folder,
                isParent = true,
                icon = "/_layouts/15/images/folder.gif?rev=23"
            };
        }
        internal static object BuildTreeNode(string listFileRef, SPListItem listItem, Guid listId, List<SPBasePermissions> basePermissions)
        {
            string fileRef = Util.GetString(listItem["FileRef"]);
            bool isRoot = fileRef.Substring(listFileRef.Length + 1).IndexOf("/", StringComparison.Ordinal) == -1;
            string title = Util.GetString(listItem["Title"]);
            if (string.IsNullOrWhiteSpace(title))
            {
                title = Path.GetFileNameWithoutExtension(fileRef);
            }
            bool isFolder = IsFolder(listItem);
            SPListItem parentListItem = isRoot ? null : GetListItemByUrl(listId, fileRef.Substring(0, fileRef.LastIndexOf("/", StringComparison.Ordinal)));
            return new
            {
                Id = Util.GetString(listItem["UniqueId"]).ToLower(),
                ListItemId = listItem.ID.ToString(),
                ParentId = parentListItem == null ? listId.ToString() : Util.GetString(parentListItem["UniqueId"]).ToLower(),
                ListId = listId,
                Title = title,
                ImageUrl = "",
                ListItemType = isFolder ? ListItemType.Folder : ListItemType.DocumentSet,
                BasePermissions = GetBasePermissions(listItem, basePermissions)
            };
        }

        internal static List<object> GetBasePermissions(SPListItem listItem, List<SPBasePermissions> basePermissions)
        {
            List<object> basePermission = new List<object>();
            foreach (var permissionKind in basePermissions)
            {
                basePermission.Add(new
                {
                    PermissionKind = permissionKind,
                    HasPermission = listItem.EffectiveBasePermissions.HasFlag(permissionKind)
                });
            }
            return basePermission;
        }

        internal static List<object> GetFolderBasePermissions(SPFolder listItem, List<SPBasePermissions> basePermissions)
        {
            List<object> basePermission = new List<object>();
            foreach (var permissionKind in basePermissions)
            {
                basePermission.Add(new
                {
                    PermissionKind = permissionKind,
                    HasPermission = listItem.Item.EffectiveBasePermissions.HasFlag(permissionKind)
                });
            }
            return basePermission;
        }
        internal static SPListItem GetListItemByUrl(Guid listId, string url)
        {
            string where = string.Format(@"<Eq>
                                        <FieldRef Name='FileRef' />
                                        <Value Type='Text'>{0}</Value>
                                    </Eq>", url);
            SPListItemCollection listItemCollection = SharePointUtil.GetSPListItems(listId, null, "RecursiveAll", null, where, null, 1);
            return listItemCollection.Count == 0 ? null : listItemCollection[0];
        }

        internal static bool IsFolder(SPListItem listItem)
        {
            SPContentType contentType = listItem.ContentType;
            return new[] { "文件夹" }.Contains(contentType.Name);
        }

        internal static List<object> GetListTreeNodes(out List<Guid> listIds)
        {
            listIds = new List<Guid>();
            List<SPList> lists = GetDocumentLists();
            List<object> treeNodes = new List<object>();

            List<SPBasePermissions> basePermissions = SharePointUtil.GetListItemBasePermissions();

            foreach (SPList list in lists)
            {
                if (!list.Hidden && list.BaseType == SPBaseType.DocumentLibrary &&
                          list.BaseTemplate != SPListTemplateType.ListTemplateCatalog &&
                          list.BaseTemplate != SPListTemplateType.DesignCatalog &&
                          list.BaseTemplate == SPListTemplateType.DocumentLibrary &&
                          list.AllowDeletion &&
                          !list.IsSiteAssetsLibrary)
                {
                    listIds.Add(list.ID);
                    treeNodes.Add(BuildTreeNode(list, basePermissions));
                }
            }
            return treeNodes;
        }

        internal static List<SPList> GetDocumentLists()
        {
            SPWeb web = SPContext.Current.Web;
            List<SPList> documentLists = new List<SPList>();

            foreach (SPList list in web.Lists)
            {
                bool usersPermissson = list.DoesUserHavePermissions(SPContext.Current.Site.OpenWeb().CurrentUser, SPBasePermissions.FullMask);

                if (usersPermissson && list.AllowDeletion && list.BaseType.ToString().Equals("DocumentLibrary", StringComparison.InvariantCultureIgnoreCase))
                    documentLists.Add(list);
            }
            return documentLists;
        }

        internal static object BuildTreeNode(SPList list, IEnumerable<SPBasePermissions> permissionKinds)
        {
            List<object> basePermissions = new List<object>();
            foreach (SPBasePermissions permissionKind in permissionKinds)
            {
                basePermissions.Add(new
                {
                    PermissionKind = permissionKind.ToString(),
                    HasPermission = list.EffectiveBasePermissions.HasFlag(permissionKind)
                });
            }
            return new
            {
                id = list.ID.ToString(),
                listId = list.ID.ToString(),
                listItemId = "",
                currentWebUrl = SPContext.Current.Web.Url,
                ListItemType = ListItemType.DocumentLibrary,
                name = list.Title,
                list.Description,
                BasePermissions = basePermissions,
                isParent = true,
                icon = "/_layouts/15/images/itdl.png?rev=23"
            };
        }

        public static string GetItTreeNodes()
        {
            try
            {
                string webUrl = SPContext.Current.Web.Url;
                List<object> ItTreeNodes = new List<object>();
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    SPWeb web = SPContext.Current.Site.RootWeb;
                    SPWebCollection spwebCollection = web.Webs;
                    ItTreeNodes.Add(new { id = web.Site.ID.ToString(), siteId = web.Site.ID.ToString(), webId = web.ID.ToString(), IdType = 5, pId = 0, name = web.Title, isParent = true, open = true, iconOpen = "/_layouts/15/images/SharePointFoundation16.png?rev=23", iconClose = "/_layouts/15/images/SharePointFoundation16.png?rev=23" });
                    foreach (SPList list in web.Lists)
                    {
                        if (!list.Hidden && list.BaseType == SPBaseType.DocumentLibrary &&
                            list.BaseTemplate != SPListTemplateType.ListTemplateCatalog &&
                            list.BaseTemplate != SPListTemplateType.DesignCatalog &&
                            list.BaseTemplate == SPListTemplateType.DocumentLibrary &&
                            list.AllowDeletion &&
                            !list.IsSiteAssetsLibrary)
                            ItTreeNodes.Add(new { id = list.ID.ToString(), pId = web.Site.ID.ToString(), siteId = web.Site.ID.ToString(), webId = web.ID.ToString(), listId = list.ID.ToString(), IdType = 0, name = list.Title, isParent = true, icon = "/_layouts/15/images/itdl.png?rev=23" });
                    }

                    foreach (SPWeb item in spwebCollection)
                    {
                        ItTreeNodes.Add(new { id = item.ID.ToString(), pId = web.Site.ID.ToString(), siteId = web.Site.ID.ToString(), webId = item.ID.ToString(), IdType = 5, name = item.Title, isParent = true, iconOpen = "/_layouts/15/images/SharePointFoundation16.png?rev=23", iconClose = "/_layouts/15/images/SharePointFoundation16.png?rev=23" });
                    }

                    //包含站点集集合

                    //SPWebApplication webApp = SPContext.Current.Site.WebApplication;
                    //SPSiteCollection siteCollections = webApp.Sites;

                    //foreach (SPSite site in web.Webs)
                    //{
                    //    ItTreeNodes.Add(new { id = site.ID.ToString(), pId = 0, name = site.RootWeb.Title, isParent = true, open = true });
                    //    SPWebCollection spwebCollection = site.RootWeb.Webs;
                    //    foreach (SPWeb item in spwebCollection)
                    //    {
                    //        ItTreeNodes.Add(new { id = item.ID.ToString(), pId = site.ID.ToString(), siteId = site.ID.ToString(), webId = item.ID.ToString(), IdType = 5, name = item.Name, isParent = true });
                    //    }
                    //}

                });

                return Util.WriteJsonpToResponse(ResponseStatus.Success, ItTreeNodes);
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

        private static List<SPList> GetDocumentLibraryByWeb(SPWeb web)
        {
            List<SPList> documentLists = new List<SPList>();
            foreach (SPList list in web.Lists)
            {
                if (!list.Hidden && list.BaseType == SPBaseType.DocumentLibrary &&
                            list.BaseTemplate != SPListTemplateType.ListTemplateCatalog &&
                            list.BaseTemplate != SPListTemplateType.DesignCatalog &&
                            list.BaseTemplate == SPListTemplateType.DocumentLibrary &&
                            list.AllowDeletion &&
                            !list.IsSiteAssetsLibrary)
                    documentLists.Add(list);
            }
            return documentLists;
        }

        private static List<object> GetDocumentFolderList(SPWeb web, Guid siteId, Guid webId, Guid? ListId)
        {
            List<object> treeNodes = new List<object>();
            SPList list;
            list = web.Lists[ListId.Value];
            SPQuery spQuery = new SPQuery();
            StringBuilder sb = new StringBuilder();
            sb.Append("<OrderBy><FieldRef Name='BaseName'></FieldRef></OrderBy>");
            spQuery.Query = sb.ToString();
            SPFolder spfolder = null;

            spfolder = list.RootFolder;
            spQuery.Folder = spfolder;
            SPListItemCollection listItemCollection = list.GetItems(spQuery);

            foreach (SPListItem item in listItemCollection)
            {
                if (item.FileSystemObjectType == SPFileSystemObjectType.Folder)
                {
                    SPFolder childFolder = item.Folder;
                    if (string.Equals(childFolder.Name, "Forms", StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }
                    treeNodes.Add(new
                    {
                        id = childFolder.UniqueId.ToString(),
                        name = childFolder.Name,
                        siteId = siteId,
                        webId = webId,
                        listItemId = childFolder.Item.ID.ToString(),
                        pid = ListId,
                        IdType = 1,
                        listId = ListId,
                        isParent = true
                    });
                }
            }
            return treeNodes;
        }

        private static List<object> GetFolderList(SPWeb web, Guid siteId, Guid webId, Guid? ListId, Guid? listItemId)
        {
            List<object> treeNodes = new List<object>();
            SPList list;
            list = web.Lists[ListId.Value];
            SPQuery spQuery = new SPQuery();
            StringBuilder sb = new StringBuilder();
            sb.Append("<OrderBy><FieldRef Name='BaseName'></FieldRef></OrderBy>");
            spQuery.Query = sb.ToString();
            SPFolder spfolder = null;
            spfolder = web.GetFolder(listItemId.Value);
            spQuery.Folder = spfolder;
            SPListItemCollection listItemCollection = list.GetItems(spQuery);
            foreach (SPListItem item in listItemCollection)
            {
                if (item.FileSystemObjectType == SPFileSystemObjectType.Folder)
                {
                    SPFolder childFolder = item.Folder;
                    if (string.Equals(childFolder.Name, "Forms", StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }
                    treeNodes.Add(new
                    {
                        id = childFolder.UniqueId.ToString(),
                        name = childFolder.Name,
                        siteId = siteId,
                        webId = webId,
                        listItemId = childFolder.Item.ID.ToString(),
                        pid = ListId,
                        IdType = 1,
                        listId = ListId,
                        isParent = true
                    });
                    //treeNodes.Add(BuildTreeNode(listId, childFolder, basePermissions));
                }
            }
            return treeNodes;
        }

        internal static List<object> GetItListTreeNodes(Guid siteId, SPWeb web, out List<Guid> listIds)
        {
            listIds = new List<Guid>();
            List<SPList> lists = GetDocumentLibraryByWeb(web);
            List<object> treeNodes = new List<object>();

            foreach (SPList list in lists)
            {
                listIds.Add(list.ID);

                treeNodes.Add(new
                {
                    id = list.ID.ToString(),
                    pId = web.ID.ToString(),
                    listId = list.ID.ToString(),
                    siteId = siteId,
                    webId = web.ID.ToString(),
                    IdType = 0,
                    listItemId = "",
                    name = list.Title,
                    isParent = true,
                    icon = "/_layouts/15/images/itdl.png?rev=23"
                });
            }

            foreach (SPWeb item in web.Webs)
            {
                listIds.Add(item.ID);
                treeNodes.Add(new
                {
                    id = item.ID.ToString(),
                    pId = web.ID.ToString(),
                    listId = item.ID.ToString(),
                    siteId = siteId,
                    webId = item.ID.ToString(),
                    IdType = 5,
                    listItemId = "",
                    name = item.Title,
                    isParent = true,
                    iconOpen = "/_layouts/15/images/SharePointFoundation16.png?rev=23",
                    iconClose = "/_layouts/15/images/SharePointFoundation16.png?rev=23"
                });
            }
            return treeNodes;
        }
        public static string GetItTreeChildNodes(Guid siteId, Guid webId, Guid? ListId, Guid? folderId, int IdType)
        {
            try
            {
                ListItemType listTiemType = (ListItemType)IdType;
                List<object> treeNodes = new List<object>();
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite spSite = new SPSite(siteId))
                    {
                        using (SPWeb web = spSite.OpenWeb(webId))
                        {
                            switch (listTiemType)
                            {
                                case ListItemType.DocumentLibrary:
                                    //treeNodes = GetDocumentFolderList(web, siteId, webId, ListId);
                                    break;
                                case ListItemType.Folder:
                                    //treeNodes = GetFolderList(web, siteId, webId, ListId, folderId);
                                    break;
                                case ListItemType.SPWeb:
                                    List<Guid> listIds;
                                    List<object> listTreeNodes = ListHandler.GetItListTreeNodes(siteId, web, out listIds);
                                    for (int i = 0; i < listIds.Count; i++)
                                    {
                                        treeNodes.Add(listTreeNodes[i]);
                                    }
                                    break;
                                default:
                                    break;
                            }

                        }
                    }
                });
                return Util.WriteJsonpToResponse(ResponseStatus.Success, treeNodes);
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

        public static string GetPermissionsReportResult(int id)
        {
            EventBusDAL _da = new EventBusDAL();
            string result = string.Empty;
            var model = _da.SPS_EventBusSP_Envision_GetEventBusModelById(id);
            if (model != null)
            {
                if (model.Status == 1)
                {
                    object modelResult = new
                    {
                        status = "y",
                        result = model
                    };
                    result = modelResult.ToJsonString();
                }
                else if (model.Status == 2)
                {
                    result = "{\"status\":\"error\" }";
                }
                else
                {
                    result = "{\"status\":\"n\" }";
                }
            }
            else
            {
                result = "{\"status\":\"null\" }";
            }
            return result;
        }
    }
}
