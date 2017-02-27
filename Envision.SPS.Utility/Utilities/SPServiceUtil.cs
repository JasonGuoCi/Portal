using System;
using System.Net;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Envision.SPS.Utility.Enums;

namespace Envision.SPS.Utility.Utilities
{
    public static class SPServiceUtil
    {

        #region GroupOptions
        /// <summary>
        /// 判断组是否存在
        /// </summary>
        /// <param name="web"></param>
        /// <param name="groupname"></param>
        /// <returns></returns>
        public static SPGroup GetGroupsByName(SPWeb web, string groupname)
        {
            SPGroup spg = null;
            try
            {
                foreach (SPGroup grouplist in web.SiteGroups)
                {
                    if (grouplist.ToString().ToLower() == groupname.ToLower())
                    {
                        spg = grouplist;
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Util.WriteJsonpToResponse(ResponseStatus.Exception, exception.Message);
            }
            return spg;
        }

        /// <summary>
        /// 判断组是否存在
        /// </summary>
        /// <param name="web"></param>
        /// <param name="groupname"></param>
        /// <returns></returns>
        public static bool IsExistGroup(SPWeb web, string groupname)
        {
            try
            {
                foreach (SPGroup grouplist in web.SiteGroups)
                {
                    if (grouplist.ToString().ToLower() == groupname.ToLower())
                        return true;
                }
                return false;
            }
            catch (Exception exception)
            {
                Util.WriteJsonpToResponse(ResponseStatus.Exception, exception.Message);
            }
            return false;
        }

        public static bool IsExistDocumentLibrary(SPWeb web, string DocumentLibraryName)
        {
            SPListCollection list = web.Lists;
            foreach (SPList item in list)
            {
                if (item.Title.ToUpper() == DocumentLibraryName.ToUpper())
                    return false;
            }
            return true;
        }

        public static bool IsExistEditDocumentLibrary(SPWeb web, string DocumentLibraryName, string newDocumentLibraryName)
        {
            SPListCollection list = web.Lists;
            foreach (SPList item in list)
            {
                if (item.Title.ToUpper() == DocumentLibraryName.ToUpper())
                {
                    continue;
                }
                if (item.Title.ToUpper() == newDocumentLibraryName.ToUpper())
                    return false;
            }
            return true;
        }


        /// <summary>
        /// 判断组是否存在
        /// </summary>
        /// <param name="web"></param>
        /// <param name="groupname"></param>
        /// <returns></returns>
        public static bool IsExistEditGroup(SPWeb web, string groupname,string newGroupsName)
        {
            try
            {
                foreach (SPGroup grouplist in web.SiteGroups)
                {
                    if (grouplist.Name.ToLower() == groupname.ToLower())
                    {
                        continue;
                    }

                    if(grouplist.Name.ToUpper()==newGroupsName.ToUpper())
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                Util.WriteJsonpToResponse(ResponseStatus.Exception, exception.Message);
            }
            return false;
        }

        /// <summary>
        /// 新建组
        /// </summary>
        /// <param name="web"></param>
        /// <param name="groupname"></param>
        /// <param name="member"></param>
        /// <param name="spuser"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static bool AddGroup(SPWeb web, string groupname, SPMember member, SPUser spuser, string description)
        {
            try
            {
                if (!IsExistGroup(web, groupname))
                {
                    web.SiteGroups.Add(groupname, member, spuser, description);
                    AddGroupToRoles(web, groupname, new string[] { "read" });
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static bool AddUserToGroup(SPWeb web, string groupname, SPUser spuser)
        {
            try
            {
                if (!IsExistGroup(web, groupname))
                {
                    web.SiteGroups[groupname].AddUser(spuser);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 判断指定组是否存在用户
        /// </summary>
        /// <param name="web"></param>
        /// <param name="username"></param>
        /// <param name="groupname"></param>
        /// <returns></returns>
        public static bool IsExistUser(SPWeb web, string username, string groupname)
        {
            try
            {
                foreach (SPUser userlist in web.SiteGroups[groupname].Users)
                {
                    if (userlist.ToString().ToLower() == username.ToLower())
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 根据指定的组新建用户
        /// </summary>
        /// <param name="web"></param>
        /// <param name="loginname">登录名:Domin\\Name形式</param>
        /// <param name="groupname">组名称</param>
        /// <param name="email">Email</param>
        /// <param name="cnname">中文名</param>
        /// <param name="notes">用户说明</param>
        /// <returns>bool</returns>
        public static bool AddUserToGroup(SPWeb web, string loginname, string groupname, string email, string cnname, string notes)
        {
            try
            {
                if (!IsExistUser(web, loginname, groupname))
                {
                    web.SiteGroups[groupname].AddUser(loginname, email, cnname, notes);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static bool AddGroupToRolesForList(SPWeb web, SPList spList, string groupname, SPGroupRolesCategory roles)
        {
            try
            {


                if (IsExistGroup(web, groupname))
                {
                    //改变站点继承权
                    if (!spList.HasUniqueRoleAssignments)
                    {
                        spList.BreakRoleInheritance(false);
                        //.RoleDefinitions.BreakInheritance(true, true);//复制父站点角色定义并且保持权限
                    }

                    //站点继承权改变后重新设置状态
                    web.AllowUnsafeUpdates = true;

                    //组权限分配与定义(New)
                    SPRoleDefinitionCollection roleDefinitions = web.RoleDefinitions;
                    SPRoleAssignmentCollection roleAssignments = spList.RoleAssignments;

                    SPMember memCrossSiteGroup = web.SiteGroups[groupname];
                    SPPrincipal myssp = (SPPrincipal)memCrossSiteGroup;
                    SPRoleAssignment myroles = new SPRoleAssignment(myssp);

                    SPRoleDefinition definition = null;
                    switch (roles)
                    {
                        case SPGroupRolesCategory.FullControl:
                            definition = web.RoleDefinitions.GetByType(SPRoleType.Administrator);
                            break;
                        case SPGroupRolesCategory.Edit:
                            definition = web.RoleDefinitions.GetByType(SPRoleType.Editor);
                            break;
                        case SPGroupRolesCategory.Read:
                            definition = web.RoleDefinitions.GetByType(SPRoleType.Reader);
                            break;
                        case SPGroupRolesCategory.View:
                            try
                            {
                                definition = web.RoleDefinitions["仅查看"];
                            }
                            catch
                            {
                                definition = web.RoleDefinitions["View Only"];
                            }
                            break;
                    }
                    if (definition != null) myroles.RoleDefinitionBindings.Add(definition);

                    roleAssignments.Add(myroles);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 组权限分配与定义(New),把权限加到对应的组下
        /// </summary>
        /// <param name="web"></param>
        /// <param name="groupname"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static bool AddGroupToRoles(SPWeb web, string groupname, string[] roles)
        {
            try
            {
                string[] _roles = roles;
                int rolemun = _roles.Length;

                if (IsExistGroup(web, groupname))
                {
                    //改变站点继承权
                    if (!web.HasUniqueRoleDefinitions)
                    {
                        web.RoleDefinitions.BreakInheritance(true, true);//复制父站点角色定义并且保持权限
                    }

                    //站点继承权改变后重新设置状态
                    web.AllowUnsafeUpdates = true;

                    //组权限分配与定义(New)
                    SPRoleDefinitionCollection roleDefinitions = web.RoleDefinitions;
                    SPRoleAssignmentCollection roleAssignments = web.RoleAssignments;

                    SPGroup currentGroup = web.SiteGroups[groupname];
                    SPMember memCrossSiteGroup = web.SiteGroups[groupname];
                    currentGroup.OnlyAllowMembersViewMembership = false;
                    currentGroup.Update();

                    SPPrincipal myssp = (SPPrincipal)memCrossSiteGroup;
                    SPRoleAssignment myroles = new SPRoleAssignment(myssp);
                    SPRoleDefinitionBindingCollection roleDefBindings = myroles.RoleDefinitionBindings;
                    if (rolemun > 0)
                    {
                        for (int i = 0; i < rolemun; i++)
                        {
                            SPRoleDefinition definition = null;

                            if (_roles[i] == "admin")
                            {
                                definition = web.RoleDefinitions.GetByType(SPRoleType.Administrator);
                            }
                            else if (_roles[i] == "edit")
                            {
                                definition = web.RoleDefinitions.GetByType(SPRoleType.Editor);
                            }
                            else if (_roles[i] == "read")
                            {
                                definition = web.RoleDefinitions.GetByType(SPRoleType.Reader);
                            }
                            if (definition != null) roleDefBindings.Add(definition);
                        }
                    }

                    roleAssignments.Add(myroles);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static bool SetRolesToList(SPWeb web, string listName, string groupName, string[] roles)
        {
            try
            {
                SPList spList = web.Lists[listName];
                if (!spList.HasUniqueRoleAssignments)
                {
                    spList.BreakRoleInheritance(true, true);//复制父站点角色定义并且保持权限
                    SPGroup spGroup = web.SiteGroups[groupName];
                    SPRoleAssignment spRoleAss = new SPRoleAssignment(spGroup); // 权限分配者,需要和用户或组关联
                    for (int i = 0; i < roles.Length - 1; i++)
                    {
                        spRoleAss.RoleDefinitionBindings.Add(web.RoleDefinitions[roles[i]]); //绑定权限
                    }
                    spList.RoleAssignments.Add(spRoleAss); //列表绑定组或用户权限   
                }

                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
    }
}
