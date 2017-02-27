using Envision.SPS.Utility.Utilities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Telerik.Web.UI;
using Envision.SPS.Utility;
using Envision.SPS.Utility.Handlers;
using Envision.SPS.Utility.Models;
using System.Collections.Generic;

namespace Envision.SPS.Document.Web.ControlTemplates.EnvisionDoc
{
    public partial class DocCurrentSiteLeftTree : BaseWebPart
    {

        #region Properties

        /// <summary>
        /// 站点Id
        /// </summary>
        protected string SiteId
        {
            get
            {
                return Convert.ToString(ViewState["SiteId"]);
            }
            set
            {
                ViewState["SiteId"] = value;
            }
        }

        /// <summary>
        ///FolderURL
        /// </summary>
        protected string FolderUrl
        {
            get
            {
                return Convert.ToString(ViewState["FolderUrl"]);
            }
            set
            {
                ViewState["FolderUrl"] = value;
            }
        }
        ///FolderURL
        /// </summary>
        protected string SPListId
        {
            get
            {
                return Convert.ToString(ViewState["SPListId"]);
            }
            set
            {
                ViewState["SPListId"] = value;
            }
        }

        protected string SortName
        {
            get
            {
                return Convert.ToString(ViewState["SortName"]);
            }
            set
            {
                ViewState["SortName"] = value;
            }
        }

        protected string SortType
        {
            get
            {
                return Convert.ToString(ViewState["SortType"]);
            }
            set
            {
                ViewState["SortType"] = value;
            }
        }


        protected bool ExtendType
        {
            get
            {
                return Convert.ToBoolean(ViewState["ExtendType"]);
            }
            set
            {
                ViewState["ExtendType"] = value;
            }
        }



        #endregion


        #region Events

        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.Url.ToString().Contains("ManageFeatures.aspx") || HttpContext.Current.Request.Url.ToString().Contains("webdeleted.aspx") || HttpContext.Current.Request.Url.ToString().Contains("deleteweb.aspx")) return;
            if (!IsPostBack)
            {
                string site = System.Web.HttpContext.Current.Request.QueryString["Site"];
                this.SiteId = System.Web.HttpContext.Current.Request.QueryString["SiteId"];
                this.SPListId = System.Web.HttpContext.Current.Request.QueryString["spListId"];
                this.FolderUrl = System.Web.HttpContext.Current.Request.QueryString["RootFolder"];
                //bool hasContainSettingsDoc = true;


                // by howard 2013.10.15
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPWeb web = SPContext.Current.Site.OpenWeb())
                    {
                        this.SortName ="FileLeafRef";
                        this.SortType = "TRUE";
                        this.ExtendType = false;

                        GetDocLibrarySettings(web);

                        var docTreeNode = new RadTreeNode();
                        docTreeNode.Text = "文档";
                        docTreeNode.Value = "文档";
                        SPListCollection splistconllection = web.GetListsOfType(SPBaseType.DocumentLibrary);

                        //存放英文和数字
                        List<SPListModel> spListsa = new List<SPListModel>();
                        //存放中文
                        List<SPListModel> spListsb = new List<SPListModel>();
                        List<SPListModel> spListsBySort = new List<SPListModel>();
                        ASCIIEncoding ascii = new ASCIIEncoding();
                        foreach (SPList splist in splistconllection)
                        {
                            if (!splist.Hidden && splist.BaseType == SPBaseType.DocumentLibrary && splist.BaseTemplate != SPListTemplateType.ListTemplateCatalog && splist.BaseTemplate != SPListTemplateType.DesignCatalog && splist.BaseTemplate == SPListTemplateType.DocumentLibrary && splist.AllowDeletion && !splist.IsSiteAssetsLibrary)
                            {
                                if (!splist.DoesUserHavePermissions(SPBasePermissions.ViewListItems)) continue;

                                byte[] s = ascii.GetBytes(splist.Title.Substring(0, 1));
                                if ((int)s[0] != 63)
                                {
                                    spListsa.Add(new SPListModel { ID = splist.ID, CreatedTime = splist.Created, Title = splist.Title, TitlePin = splist.Title });
                                }
                                else
                                {
                                    spListsb.Add(new SPListModel { ID = splist.ID, CreatedTime = splist.Created, Title = splist.Title, TitlePin = PinYinConverter.GetFirst(splist.Title) });
                                }
                            }
                        }

                        if (this.SortType.ToUpper() == "TRUE")
                        {
                            if (this.SortName.ToUpper() == "FILELEAFREF")
                            {
                                spListsBySort = spListsa.OrderBy(p => p.TitlePin).ToList();
                                spListsBySort.AddRange(spListsb.OrderBy(p => p.TitlePin).ToList());
                            }
                            else
                            {
                                spListsBySort = spListsa.OrderBy(p => p.CreatedTime).ToList();
                                spListsBySort.AddRange(spListsb.OrderBy(p => p.CreatedTime).ToList());
                            }
                        }
                        else
                        {
                            if (this.SortName.ToUpper() == "FILELEAFREF")
                            {
                                spListsBySort = spListsa.OrderByDescending(p => p.TitlePin).ToList();
                                spListsBySort.AddRange(spListsb.OrderByDescending(p => p.TitlePin).ToList());
                            }
                            else
                            {
                                spListsBySort = spListsa.OrderByDescending(p => p.CreatedTime).ToList();
                                spListsBySort.AddRange(spListsb.OrderByDescending(p => p.CreatedTime).ToList());
                            }
                        }

                        foreach (SPListModel item in spListsBySort)
                        {

                            RadTreeNode doc = this.InitRootNodes(web, splistconllection.GetList(item.ID, true));
                            doc.ExpandMode = TreeNodeExpandMode.ClientSide;

                            if (this.ExtendType)
                            {
                                doc.Expanded = true;
                            }
                            else
                            {
                                if (item.ID.ToString() == this.SPListId)
                                {
                                    doc.Expanded = true;
                                }
                                else
                                {
                                    doc.Expanded = false;
                                }
                            }



                            docTreeNode.Nodes.Add(doc);

                        }


                        //foreach (SPList splist in splistconllection)
                        //{
                        //    if (splist.BaseType == SPBaseType.DocumentLibrary && splist.BaseTemplate != SPListTemplateType.ListTemplateCatalog && splist.BaseTemplate != SPListTemplateType.DesignCatalog && splist.BaseTemplate == SPListTemplateType.DocumentLibrary && splist.AllowDeletion && !splist.IsSiteAssetsLibrary)
                        //    {
                        //        if (!splist.DoesUserHavePermissions(SPBasePermissions.ViewListItems)) continue;
                        //        RadTreeNode doc = this.InitRootNodes(web, splist);
                        //        doc.ExpandMode = TreeNodeExpandMode.ClientSide;

                        //        if (this.ExtendType)
                        //        {
                        //            doc.Expanded = true;
                        //        }
                        //        else
                        //        {
                        //            if (splist.ID.ToString() == this.SPListId)
                        //            {
                        //                doc.Expanded = true;
                        //            }
                        //            else
                        //            {
                        //                doc.Expanded = false;
                        //            }
                        //        }



                        //        docTreeNode.Nodes.Add(doc);
                        //    }
                        //}
                        this.rtvDocumentTreeView.Nodes.Add(docTreeNode);

                        docTreeNode.Expanded = true;

                    }
                });
               // if (!hasContainSettingsDoc) { CreateDocLibrarySettings(); Response.Redirect(HttpContext.Current.Request.Url.ToString()); }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void CreateDocLibrarySettings()
        {
            try
            {
                DocumentLibraryAttr documentLibraryAttr = new DocumentLibraryAttr();
                documentLibraryAttr.DocumentLibraryName = "DocLibrarySettings";
                documentLibraryAttr.DocumentLibraryTemplateId = "DocLibrarySettingsTemplate.stp";
                ListHandler.CreateDocumentlabrary(documentLibraryAttr);
            }
            catch
            {
                try
                {
                    DocumentLibraryAttr documentLibraryAttr = new DocumentLibraryAttr();
                    documentLibraryAttr.DocumentLibraryName = "DocLibrarySettings";
                    documentLibraryAttr.DocumentLibraryTemplateId = "DocLibrarySettingsTemplate2.stp";
                    ListHandler.CreateDocumentlabrary(documentLibraryAttr);

                }
                catch
                {
                }

            }
        }
        public bool GetDocLibrarySettings(SPWeb web)
        {
            SPList list = null;
            try
            {
                list = web.Lists["DocLibrarySettings"];
            }
            catch
            {
                return false;
            }
            SPQuery query = new SPQuery();
            StringBuilder sb = new StringBuilder();
            sb.Append("<OrderBy><FieldRef Name='SortName' ></FieldRef></OrderBy>");
            query.Query = sb.ToString();
            query.RowLimit = 10;

            //查询
            SPListItemCollection items = list.GetItems(query);
            if (items.Count != 0)
            {
                foreach (SPListItem splist in items)
                {
                    if (splist != null)
                    {
                        this.SortName = splist["SortName"].ToString() == "按名称" ? "FileLeafRef" : "Created";
                        this.SortType = splist["SortType"].ToString() == "升序" ? "TRUE" : "FALSE";
                        this.ExtendType = Convert.ToBoolean(splist["ExtendType"]);
                    }

                }
            }
            return true;

        }

        #region 暂时不用
        private RadTreeNode CreateNodeByList(SPList spList)
        {
            RadTreeNode node = new RadTreeNode();

            node.Text = spList.Title;
            node.Value = spList.RootFolder.Url;
            node.NavigateUrl = spList.DefaultViewUrl;
            node.ImageUrl = SPControl.GetContextWeb(Context).Url + "/_layouts/images/itdl.png";
            //node.ExpandMode = TreeNodeExpandMode.ClientSide;
            //node.Expanded = true;


            foreach (SPFolder subFolder in spList.RootFolder.SubFolders)
            {
                if (subFolder.Name != "Forms")
                {
                    node.Nodes.Add(CreateNodeByFolder(subFolder, spList));
                }
            }
            return node;
        }

        private RadTreeNode CreateNodeByFolder(SPFolder spFolder, SPList splist)
        {
            RadTreeNode node = new RadTreeNode();
            node.Text = spFolder.Name;
            node.NavigateUrl = splist.DefaultViewUrl + "?RootFolder=" + spFolder.Url; //SPContext.Current.Web.Lists["文档"].DefaultViewUrl + "?RootFolder=" + spFolder.Url;
            node.ImageUrl = SPControl.GetContextWeb(Context).Url + "/_layouts/images/folder.gif";
            //node.ExpandMode = TreeNodeExpandMode.ClientSide;
            //node.Expanded = true;
            foreach (SPFolder subFolder in spFolder.SubFolders)
            {
                node.Nodes.Add(CreateNodeByFolder(subFolder, splist));
            }
            return node;
        }
        #endregion


        /// <summary>
        /// TreeView展开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rtvDocumentTreeView_NodeExpand(object sender, RadTreeNodeEventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPWeb web = SPContext.Current.Site.OpenWeb())
                {
                    SPList spList = web.Lists[new Guid(e.Node.Attributes["OwnerRoot"])];
                    this.InitChildTreeNodes(e.Node, web, spList);
                }

            });
        }


        #endregion

        #region Methods

        /// <summary>
        /// 初始化根节点数据
        /// </summary>

        private RadTreeNode InitRootNodes(SPWeb web, SPList list)
        {
            this.rtvDocumentTreeView.Nodes.Clear();

            var rootTreeNode = new RadTreeNode();
            rootTreeNode.Text = list.Title;
            rootTreeNode.ToolTip = list.Title;

            rootTreeNode.Value = Convert.ToString(list.ID);//"Document";
            rootTreeNode.ImageUrl = SPControl.GetContextWeb(Context).Url + "/_layouts/15/images/itdl.png?rev=23"; //@"/_layouts/EnvisionComm/Icons/Root.gif";
            //new add
            rootTreeNode.NavigateUrl = list.DefaultViewUrl;


            rootTreeNode.Category = "Document";
            rootTreeNode.ExpandMode = TreeNodeExpandMode.ClientSide;



            this.rtvDocumentTreeView.Nodes.Add(rootTreeNode);
            var rootFolder = list.RootFolder;
            //创建查询条件
            SPQuery spQuery = new SPQuery();
            StringBuilder sb = new StringBuilder();
            sb.Append("<Eq><FieldRef Name='ContentType'/><Value Type='Text'>Folder</Value></Eq><OrderBy><FieldRef Name='" + this.SortName + "'  Ascending='" + this.SortType + "'></FieldRef></OrderBy>");
            spQuery.Query = sb.ToString();
            spQuery.Folder = rootFolder;
            SPListItemCollection itemCollection = list.GetItems(spQuery);

            //if (string.IsNullOrEmpty(this.SiteId))
            //{
            //    this.InitChildTreeNodes(rootTreeNode, web, list);
            //    return null;
            //}

            this.InitTargetChildTreeNodes(rootTreeNode, itemCollection, web, list);
            return rootTreeNode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void InitChildTreeNodes(RadTreeNode currentTreeNode, SPFolderCollection folderCollection, SPWeb web, SPList list)
        {
            var result = this.Contains(folderCollection);
            foreach (SPFolder childFolder in folderCollection)
            {
                if (string.Equals(childFolder.Name, "Forms", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                var treeNodeExpandMode = childFolder.SubFolders.Count != 0 ? TreeNodeExpandMode.ServerSideCallBack : TreeNodeExpandMode.ClientSide;
                var expanded = childFolder.SubFolders.Count != 0;

                if ((result && string.Equals(Convert.ToString(childFolder.UniqueId), this.SiteId, StringComparison.CurrentCultureIgnoreCase)))
                {
                    treeNodeExpandMode = TreeNodeExpandMode.ClientSide;
                    expanded = true;
                }
                else
                {
                    if (this.Contains(childFolder.SubFolders))
                    {
                        treeNodeExpandMode = TreeNodeExpandMode.ClientSide;
                        expanded = true;
                    }
                }

                var treeNode = this.CreateChildTreeNode(Convert.ToString(childFolder.UniqueId), childFolder.Name, treeNodeExpandMode, expanded, "FullMask", list, childFolder);

                if (result)
                {
                    if (string.Equals(Convert.ToString(childFolder.UniqueId), this.SiteId, StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.InitChildTreeNodes(treeNode, childFolder.SubFolders, web, list);
                    }
                }
                else
                {
                    if (!string.Equals(Convert.ToString(childFolder.ParentFolder.UniqueId), this.SiteId, StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.InitChildTreeNodes(treeNode, childFolder.SubFolders, web, list);
                    }
                }

                currentTreeNode.Nodes.Add(treeNode);
            }
        }

        /// <summary>
        /// 重载
        /// </summary>
        /// <returns></returns>
        private void InitChildTreeNodes(RadTreeNode currentTreeNode, SPListItemCollection folderCollection, SPWeb web, SPList list)
        {
            var result = this.Contains(folderCollection);
            foreach (SPListItem item in folderCollection)
            {
                if (item.FileSystemObjectType == SPFileSystemObjectType.Folder)
                {
                    //string userAccount = this.Context.User.Identity.Name.Substring(5, this.Context.User.Identity.Name.Length - 5);
                    //SPPermissionInfo permissionInfo = item.GetUserEffectivePermissionInfo(userAccount);

                    //2013.10.11 Xiong Wei, Change show different with quick link Start
                    //if (permissionInfo.RoleAssignments.Count == 0)
                    //{
                    //    continue;
                    //}
                    //2013.10.11 Xiong Wei, Change show different with quick link End
                    if (!item.DoesUserHavePermissions(SPBasePermissions.ViewListItems)) continue;
                    string permissions = string.Empty;

                    //判断用户权限
                    //permissions = GetAssignments(permissionInfo);

                    SPFolder childFolder = item.Folder;
                    if (string.Equals(childFolder.Name, "Forms", StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }

                    var treeNodeExpandMode = childFolder.SubFolders.Count != 0 ? TreeNodeExpandMode.ServerSideCallBack : TreeNodeExpandMode.ClientSide;
                    var expanded = childFolder.SubFolders.Count != 0;

                    if ((result && string.Equals(Convert.ToString(childFolder.UniqueId), this.SiteId, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        treeNodeExpandMode = TreeNodeExpandMode.ClientSide;
                        expanded = true;
                    }
                    else
                    {
                        expanded = false;

                        if (childFolder.SubFolders.Count > 0)
                        {
                            expanded = GetExpanded(childFolder, expanded);
                        }
                    }

                    var treeNode = this.CreateChildTreeNode(Convert.ToString(childFolder.UniqueId), childFolder.Name, treeNodeExpandMode, expanded, permissions, list, childFolder);

                    if (result)
                    {
                        if (string.Equals(Convert.ToString(childFolder.UniqueId), this.SiteId, StringComparison.CurrentCultureIgnoreCase))
                        {
                            //创建查询条件
                            SPQuery spQuery = new SPQuery();
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<OrderBy><FieldRef Name='BaseName' ></FieldRef></OrderBy>");
                            spQuery.Query = sb.ToString();
                            spQuery.Folder = childFolder;
                            SPListItemCollection itemCollection = list.GetItems(spQuery);
                            this.InitChildTreeNodes(treeNode, itemCollection, web, list);
                        }
                    }
                    else
                    {
                        if (!string.Equals(Convert.ToString(childFolder.ParentFolder.UniqueId), this.SiteId, StringComparison.CurrentCultureIgnoreCase))
                        {

                            //创建查询条件
                            SPQuery spQuery = new SPQuery();
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<OrderBy><FieldRef Name='BaseName' ></FieldRef></OrderBy>");
                            spQuery.Query = sb.ToString();
                            spQuery.Folder = childFolder;
                            SPListItemCollection itemCollection = list.GetItems(spQuery);
                            this.InitChildTreeNodes(treeNode, itemCollection, web, list);
                        }
                    }

                    currentTreeNode.Nodes.Add(treeNode);
                }
            }
        }

        private bool GetExpanded(SPFolder childFolder, bool expanded)
        {
            foreach (SPFolder itemfolder in childFolder.SubFolders)
            {
                if (string.Equals(Convert.ToString(itemfolder.UniqueId), this.SiteId, StringComparison.CurrentCultureIgnoreCase))
                {
                    expanded = true;
                }
                else
                {
                    if (childFolder.SubFolders.Count > 0)
                    {
                        expanded = GetExpanded(itemfolder, expanded);
                    }
                }
            }

            return expanded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderCollection"></param>
        /// <returns></returns>
        private bool Contains(SPFolderCollection folderCollection)
        {
            foreach (SPFolder childFolder in folderCollection)
            {
                if (string.Equals(Convert.ToString(childFolder.UniqueId), this.SiteId, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 重载
        /// </summary>
        /// <param name="folderCollection"></param>
        /// <returns></returns>
        private bool Contains(SPListItemCollection folderCollection)
        {
            foreach (SPListItem item in folderCollection)
            {
                if (item.FileSystemObjectType == SPFileSystemObjectType.Folder)
                {
                    SPFolder childFolder = item.Folder;
                    if (string.Equals(Convert.ToString(childFolder.UniqueId), this.SiteId, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 初始化子节点数据
        /// </summary>
        /// <param name="currentTreeNode"></param>
        private void InitChildTreeNodes(RadTreeNode currentTreeNode, SPWeb web, SPList list)
        {
            currentTreeNode.Nodes.Clear();

            //创建查询条件
            SPQuery spQuery = new SPQuery();
            StringBuilder sb = new StringBuilder();
            sb.Append("<OrderBy><FieldRef Name='BaseName'></FieldRef></OrderBy>");
            spQuery.Query = sb.ToString();
            SPFolder spfolder = null;

            if (string.Equals(currentTreeNode.Value, "Document", StringComparison.CurrentCultureIgnoreCase))
            {
                spfolder = list.RootFolder;
            }
            else
            {
                spfolder = web.GetFolder(new Guid(currentTreeNode.Value));
            }

            spQuery.Folder = spfolder;

            SPListItemCollection itemCollection = list.GetItems(spQuery);

            foreach (SPListItem item in itemCollection)
            {
                //SPPermissionInfo permissionInfo = item.GetUserEffectivePermissionInfo(this.Context.User.Identity.Name.Substring(5, this.Context.User.Identity.Name.Length - 5));

                //if (permissionInfo.RoleAssignments.Count == 0)
                //{
                //    continue;
                //}
                if (!item.DoesUserHavePermissions(SPBasePermissions.ViewListItems)) continue;
                string permissions = string.Empty;

                // permissions = GetAssignments(permissionInfo);

                if (item.FileSystemObjectType == SPFileSystemObjectType.Folder)
                {
                    SPFolder childFolder = item.Folder;

                    if (string.Equals(childFolder.Name, "Forms", StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }

                    var childTreeNode = this.CreateChildTreeNode(Convert.ToString(childFolder.UniqueId), childFolder.Name, childFolder.SubFolders.Count != 0 ? TreeNodeExpandMode.ServerSideCallBack : TreeNodeExpandMode.ClientSide, childFolder.SubFolders.Count != 0, permissions, list, spfolder);
                    currentTreeNode.Nodes.Add(childTreeNode);
                }
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

        /// <summary>
        /// 构建子节点
        /// </summary>
        /// <param name="value"></param>
        /// <param name="text"></param>
        /// <param name="expandMode"></param>
        /// <param name="expanded"></param>
        private RadTreeNode CreateChildTreeNode(string value, string text, TreeNodeExpandMode expandMode, bool expanded, string strPermissions, SPList splist, SPFolder spFolder)
        {
            var childTreeNode = new RadTreeNode();
            childTreeNode.Text = text;
            childTreeNode.Value = value;
            childTreeNode.ToolTip = text;
            childTreeNode.Category = "Folder";
            childTreeNode.ImageUrl = SPControl.GetContextWeb(Context).Url + "/_layouts/15/images/folder.gif";//@"/_layouts/EnvisionComm/Icons/Folder.gif";
            //new add
            //childTreeNode.NavigateUrl = splist.DefaultViewUrl + "?RootFolder=" + HttpUtility.UrlEncode("/" + spFolder.Url) + "&spListId=" + splist.ID.ToString() + "&SiteId=" + spFolder.UniqueId.ToString();
            string folderUrl = HttpUtility.UrlEncode(spFolder.ServerRelativeUrl).Replace("+", "%20");
            childTreeNode.NavigateUrl = splist.DefaultViewUrl + "?RootFolder=" + folderUrl + "&spListId=" + splist.ID.ToString() + "&SiteId=" + spFolder.UniqueId.ToString();
            childTreeNode.ExpandMode = expandMode;
            childTreeNode.Expanded = expanded;
            childTreeNode.Attributes["Role"] = strPermissions;
            //new add
            childTreeNode.Attributes["OwnerRoot"] = splist.ID.ToString("N");//splist.Title;



            return childTreeNode;
        }

        #endregion


        #region  [New Methods  add by howard 20131015]

        /// <summary>
        /// 重载
        /// </summary>
        /// <returns></returns>
        private void InitTargetChildTreeNodes(RadTreeNode currentTreeNode, SPListItemCollection folderCollection, SPWeb web, SPList list)
        {
            //var result = this.Contains(folderCollection);
            foreach (SPListItem item in folderCollection)
            {
                if (item.FileSystemObjectType == SPFileSystemObjectType.Folder)
                {
                    SPFolder childFolder = item.Folder;
                    var result = this.IsEqualFolder(childFolder);
                    //SPRoleDefinitionBindingCollection usersRoles = item.AllRolesForCurrentUser;
                    // var currentAcount = this.Context.User.Identity.Name.Substring(5, this.Context.User.Identity.Name.Length - 5);
                    //SPPermissionInfo permissionInfo = item.GetUserEffectivePermissionInfo(currentAcount);
                    if (!item.DoesUserHavePermissions(SPBasePermissions.ViewListItems)) continue;

                    //2013.10.11 Xiong Wei, Change show different with quick link Start
                    //if (permissionInfo.RoleAssignments.Count == 0)
                    //{
                    //    continue;
                    //}
                    //2013.10.11 Xiong Wei, Change show different with quick link End

                    string permissions = string.Empty;

                    //判断用户权限
                    // permissions = GetAssignments(permissionInfo);

                    if (string.Equals(childFolder.Name, "Forms", StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }

                    var treeNodeExpandMode = childFolder.SubFolders.Count != 0 ? TreeNodeExpandMode.ServerSideCallBack : TreeNodeExpandMode.ClientSide;
                    var expanded = childFolder.SubFolders.Count != 0;
                    if (this.ContainsTargetFolder(childFolder))
                    {
                        treeNodeExpandMode = TreeNodeExpandMode.ClientSide;

                    }

                    var treeNode = this.CreateChildTreeNode(Convert.ToString(childFolder.UniqueId), childFolder.Name, treeNodeExpandMode, expanded, permissions, list, childFolder);

                    if (this.IsEqualFolder(childFolder.ParentFolder) && result || this.ContainsTargetFolder(childFolder))
                    {

                        //创建查询条件
                        SPQuery spQuery = new SPQuery();
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<Eq><FieldRef Name='ContentType'/><Value Type='Text'>Folder</Value></Eq><OrderBy><FieldRef Name='" + this.SortName + "'  Ascending='" + this.SortType + "'></FieldRef></OrderBy>");
                        spQuery.Query = sb.ToString();
                        spQuery.Folder = childFolder;

                        SPListItemCollection itemCollection = list.GetItems(spQuery);
                        this.InitTargetChildTreeNodes(treeNode, itemCollection, web, list);
                    }

                    currentTreeNode.Nodes.Add(treeNode);
                }
            }
        }


        /// <summary>
        /// UPDATE 2016-3-23
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private bool ContainsTargetFolder(SPFolder folder)
        {
            string tempFolderUrl = this.FolderUrl.TrimStart('/');

            if (tempFolderUrl.StartsWith(folder.Url) || tempFolderUrl.StartsWith(folder.ServerRelativeUrl.TrimStart('/')))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private bool IsEqualFolder(SPFolder folder)
        {
            if (string.Equals(Convert.ToString(folder.UniqueId), this.SiteId, StringComparison.CurrentCultureIgnoreCase))
                return true;
            else
                return false;
        }

        #endregion


    }

    #region Common Class
    public class SPListModel
    {

        public Guid ID
        {
            get;
            set;
        }
        public DateTime CreatedTime
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string TitlePin
        {
            set;
            get;
        }
    }
    #endregion
}
