charset = "UTF-8";

var udoc = {
    enums: {
        listItemType: {
            documentLibrary: 0, //文档库
            folder: 1, //文件夹
            documentSet: 2, //文档集
            file: 3, //文件
            fileGroup: 4, //组合
            skyDrive: 5//个人网盘
        },
        downloadType: {
            documentLibrary: 0, //文档库
            folder: 1, //文件夹
            documentSet: 2, //文档集
            file: 3, //文件
            fileGroup: 4 //文件组合
        },
        actionType: {
            move: 0, //移动
            copy: 1 //复制
        },
        permissionKind: {
            emptyMask: 0,
            viewListItems: 1, //查看项目  -  查看列表中的项目、文档库中的文档和查看 Web 讨论评论。
            addListItems: 2, //添加项目  -  向列表中添加项目，向文档库中添加文档，以及添加 Web 讨论评论。
            editListItems: 3, //编辑项目  -  编辑列表中的项目、文档库中的文档、文档中的 Web 讨论评论以及自定义文档库中的 Web 部件页。
            deleteListItems: 4, //删除项目  -  从列表中删除项目、从文档库中删除文档，以及删除文档中的 Web 讨论评论。
            approveItems: 5, //批准项目  -  批准列表项或文档的次要版本。
            openItems: 6, //打开项目  -  使用服务器端文件处理程序查看文档源。
            viewVersions: 7, //查看版本  -  查看列表项或文档的以前版本。
            deleteVersions: 8, //删除版本  -  删除列表项或文档的以前版本。
            cancelCheckout: 9, //替代签出版本  -  放弃或签入已由其他用户签出的文档。 
            managePersonalViews: 10, //管理个人视图  -  创建、更改和删除列表的个人视图。
            manageLists: 12, //管理列表  -  创建和删除列表，添加或删除列表中的栏，以及添加或删除列表的公共视图。
            viewFormPages: 13, //查看应用程序页面  -  查看表单、视图和应用程序页面。枚举列表。
            anonymousSearchAccessList: 14,
            open: 17, //打开  -  允许用户打开网站、列表或文件夹，以便访问该容器中的项目。
            viewPages: 18, //查看网页  -  查看网站中的网页。
            addAndCustomizePages: 19, //添加和自定义网页  -  添加、更改或删除 HTML 网页或 Web 部件页，并使用与 Windows SharePoint Services 兼容的编辑器编辑网站。
            applyThemeAndBorder: 20, //应用主题和边框  -  将主题或边框应用于整个网站。
            applyStyleSheets: 21, //应用样式表  -  将样式表(.CSS 文件)应用于网站。
            viewUsageData: 22, //查看使用率数据  -  查看有关网站使用率的报告。
            createSSCSite: 23,
            manageSubwebs: 24, //创建子网站  -  创建子网站，例如工作组网站、会议工作区网站和文档工作区网站。
            createGroups: 25, //创建用户组  -  创建一个用户组，该用户组可用于网站集中的任何位置。
            managePermissions: 26, //管理权限  -  创建和更改网站上的权限级别，并为用户和用户组分配权限。
            browseDirectories: 27, //浏览目录  -  使用 SharePoint Designer 和 Web DAV 接口枚举网站中的文件和文件夹。 
            browseUserInfo: 28, //浏览用户信息  -  查看有关网站用户的信息。
            addDelPrivateWebParts: 29, //添加/删除个人 Web 部件  -  在 Web 部件页中添加或删除个人 Web 部件。 
            updatePersonalWebParts: 30, //更新个人 Web 部件  -  更新 Web 部件以显示个性化信息
            manageWeb: 31, //管理网站  -  授予执行该网站的所有管理任务并管理内容的能力。
            anonymousSearchAccessWebLists: 32,
            useClientIntegration: 37, //使用客户端集成功能  -  使用启动客户端应用程序的功能。如果没有此权限，用户必须本地处理文档并上载更改。
            useRemoteAPIs: 38, //使用远程接口  -  使用 SOAP、Web DAV 或 SharePoint Designer 接口访问网站。
            manageAlerts: 39, //管理通知  -  管理网站中所有用户的通知。
            createAlerts: 40, //创建通知  -  创建电子邮件通知。
            editMyUserInfo: 41, //编辑个人用户信息  -  允许用户更改个人用户信息，例如添加图片。
            enumeratePermissions: 63, //枚举权限  -  枚举网站、列表、文件夹、文档或列表项中的权限。
            fullMask: 65
        },
        logGroup: {
            none: -1,
            documentLibrary: 0, //文档库
            folder: 1, //文件夹
            documentSet: 2, //文档集
            document: 3 //文件
        },
        logType: {
            none: -1,
            addDocumentLibrary: 0,
            editDocumentLibrary: 1,
            removeDocumentLibrary: 2,
            viewDocumentLibrary: 3,
            downloadDocumentLibrary: 4,
            addFolder: 5,
            editFolder: 6,
            removeFolder: 7,
            viewFolder: 8,
            downloadFolder: 9,
            moveFolder: 10,
            copyFolder: 11,
            addDocumentSet: 12,
            editDocumentSet: 13,
            removeDocumentSet: 14,
            viewDocumentSet: 15,
            downloadDocumentSet: 16,
            moveDocumentSet: 17,
            copyDocumentSet: 18,
            addDocument: 19,
            editDocument: 20,
            removeDocument: 21,
            viewDocument: 22,
            downloadDocument: 23,
            moveDocument: 24,
            copyDocument: 25,
            openDocument: 26,
            onlineViewDocument: 27,
            onlineEditDocument: 28,
            checkInDocument: 29,
            checkOutDocument: 30,
            undoCheckOutDocument: 31,
            removeDocumentVersion: 32,
            retrieveDocumentVersion: 33,
            viewDocumentVersion: 34,
            viewDocumentVersions: 35
        }
    },
    urls: {
        writeLogUrl: 26,//"Api/Document/WriteLog",
        getTreeNodesUrl: 0,
        getSubTreeNodesUrl: 29,
        getSelectTreeNodesUrl: 23,
        getSubSelectTreeNodesUrl: 24,
        hasManageUserUrl: "Api/Document/HasManageUser",
        moveUrl: 11, // "Api/Document/ListItem/Move",
        copyUrl: 12, //"Api/Document/ListItem/Copy",
        getListItemsUrl: 1, //"Api/Document/ListItem/GetListItems",
        checkWebPermissionUrl: 6, // "Api/Document/Web/CheckPermission",
        addDocumentLibraryUrl: 2, // "Api/Document/List/AddDocumentLibrary",
        removeDocumentLibraryUrl: 3, // "Api/Document/List/RemoveDocumentLibrary",
        editDocumentLibraryUrl: 4, // "Api/Document/List/EditDocumentLibrary",
        getDocumentLibraryUrl: 5, // "Api/Document/List/GetDocumentLibrary",
        addFolderListItemUrl: 7, // "Api/Document/ListItem/AddFolderListItem",
        removeListItemsUrl: 8, //"Api/Document/ListItem/RemoveListItems",
        editFolderListItemUrl: 9, //"Api/Document/ListItem/EditFolderListItem",
        getFolderListItemUrl: 10, // "Api/Document/ListItem/GetFolderListItem",
        checkOutUrl: 13, //"Api/Document/ListItem/CheckOut",
        checkInUrl: 14, //"Api/Document/ListItem/CheckIn",
        undoCheckOutUrl: 15, //"Api/Document/ListItem/UndoCheckOut",
        uploadUrl: "/_layouts/15/UDoc/Handlers/UploadHandler.ashx",
        getFilesUrl: 22, // "Api/Document/ListItem/GetFiles",
        editFileUrl: 16, //"Api/Document/ListItem/EditFile",
        getFileUrl: 17, //"Api/Document/ListItem/GetFile",
        downloadUrl: "/_layouts/15/UDoc/Handlers/DownloadHandler.ashx",
        removeFileVersionsUrl: 18, // "Api/Document/ListItem/RemoveFileVersions",
        retrieveFileVersionUrl: 19, //"Api/Document/ListItem/RetrieveFileVersion",
        getFileVersionsUrl: 20, //"Api/Document/ListItem/GetFileVersions",
        showWeChatUrl: "/_layouts/15/UDoc/Handlers/ShowWeChat.ashx",
        openLocationUrl: 21, //"Api/Document/ListItem/OpenLocation",
        getAuthorityUrl: "Api/Common/GetAuthority",
        getUsersUrl: "Api/Common/GetUsers",
        getUserUrl: "Api/Common/GetUser",
        getModulesUrl: "Api/Common/GetModules",
        addUserUrl: "Api/Common/AddUser",
        editUserUrl: "Api/Common/EditUser",
        removeUsersUrl: "Api/Common/RemoveUsers",
        getLogSearchResultsUrl: 27,//"Api/Document/GetLogSearchResults",
        exportLogToExcelUrl: "/_layouts/15/UDoc/Handlers/ExportLogToExcelHandler.ashx",
        swfUrl: "/_layouts/15/UDoc/Plugins/WebUploader/Resources/Uploader.swf",
        getListInfoUrl: 25, //"Api/Document/ListItem/GetFileVersions"
        checkLicensingUrl: 28,
        getReportDataUrl: 30,
        editFileMarkUrl: 31,
        getFileMarkUrl: 32,
        GetDocumentLibrariesListUrl:33
    },
   
    datas: {
        folderNames: [],
        listId: null,
        listItemId: null,
        listItemBasePermissions: [],
        documetnSetListItemId: null,
        documetnSetBasePermissions: [],
        fileListItemId: null,
        fileBasePermissions: [],
        listItemNames: [],
        folderKeyword: "",
        selectFolderKeyword: "",
        listItemKeyword: "",
        selectListId: null,
        selectListItemId: null,
        listItemFirst: true,
        customerId: null,
        logListId: null,
        logGroupJson: null,
        logTypeJson: null,
        logDataGrid: null,
        fileChanged: false,
        treeNodes: null,
        selectParam: null,
        rowColumns: [],
        init: function () {
        }
    },
    doms: {
        treeDom: null,
        selectTreeDom: null,
        uploaderDom: null
    },
    methods: {     

        loadListsGridItems: function (data) {
            udoc.datas.listItemFirst = true;
            var listItemsDataGrid = $("#listItemsDataGrid").dataGrid({
                sortName: "Created",
                sortType: "DESC",
                pageSize: 16,
                pageIndex: 1
            }, function (options) {
                //var $container = $("#listItemsDataGrid").find(".dataGridBody tbody");
                //var $container = $(data).find(".dataGridBody tbody");
                var container = $(data).find("#rowsContentBody");
                var html;
                var keyword = udoc.datas.listItemKeyword;
                $("#txtListItemKeyword").val(keyword === "" ? $("#txtListItemKeyword").attr("placeholder") : keyword);
                global.methods.ajax(udoc.urls.GetDocumentLibrariesListUrl, {
                    Keyword: encodeURIComponent(global.methods.encrypt(keyword)),
                    isFirst: udoc.datas.listItemFirst
                }, "#rightSection", function (loadingId, response) {
                    if (response.Status !== global.enums.responseStatus.success) {
                        global.methods.closeLoading(loadingId);
                        $.layer.alert(response.Result, null, "#rightSection");
                        return;
                    }
                    var i, len;

                    listItemsDataGrid.clear();

                    var listItems = response.Result || [];
                    if (listItems.length === 0) {
                        html = "";
                        html += "<tr>";
                        html += "<td colspan=\"7\" class=\"firstCell lastCell\">";
                        html += "没有任何数据.";
                        html += "</td>";
                        html += "</tr>";
                        container.append(html);
                        global.methods.closeLoading(loadingId);
                        return;
                    }
                    var selectAll = $("#listItemsDataGrid").find(".dataGridHeader .select .icon").hasClass("checked");
                    for (i = 0, len = listItems.length; i < len; i++) {
                        var listItem = listItems[i];
                        var $row = udoc.methods.buildListsGridItemRow(listItem, selectAll, listItem.ListItemType, "#listItemsDataGrid");
                        container.append($row);
                    }
                    $(data).find("#rowsContentBody").append(container);
                    global.methods.closeLoading(loadingId);
                }, null);
            });
            $("#listItemsDataGrid").find(".dataGridBody tbody").hover(function () {
                var height = 0;
                var $dataGridContents = $(this).parent().parent();
                var $rows = $dataGridContents.find("tr");
                for (var i = 0; i < $rows.length; i++) {
                    height += $rows.eq(i).outerHeight();
                }
                if (height > $dataGridContents.height()) {
                    $dataGridContents.addClass("scroll");
                    $dataGridContents.css({ "overflow-y": "auto" });
                }
            }, function () {
                var $dataGridContents = $(this).parent().parent();
                $dataGridContents.removeClass("scroll");
                $dataGridContents.css({ "overflow-y": "hidden" });
            });
            $.selectAll("#listItemsDataGrid .dataGridHeader .select .icon", "#listItemsDataGrid .dataGridBody .select .icon", function ($checkbox) {
                $checkbox.each(function () {
                    if ($(this).hasClass("checked")) {
                        $(this).parent().parent().removeClass("selected").addClass("selected");
                    } else {
                        $(this).parent().parent().removeClass("selected");
                    }
                });
            });
        },
        buildListsGridItemRow: function (listItem, selectAll, listItemType, dataGridSelector) {
            var html = "";
            html += "<tr  ListId=\"" + listItem.ListId + ">";
            html += "<td class=\"select firstCell\">";
            html += "<i class=\"icon" + (selectAll ? " checked" : "") + "\"></i>";
            html += "</td>";
            html += "<td class=\"fileIcon\">";
            //var title = listItem.FileLeafRef + (listItem.Description === "" ? "" : "\r\n" + listItem.Description);
            //html += "<img src=\""  + "\" alt=\"" + title + "\" title=\"" + title + "\">";
            html += "</td>";
            html += "<td>";
            html += "<a href=\"javascript:void(0);\" class=\"ListItemName\" title=\"" + (listItem.Description === "" ? listItem.Title : listItem.Description) + "\">";
            title = listItem.Title;
            //if (dataGridSelector === "#listItemsDataGrid") {
            //    var keyword = udoc.datas.listItemKeyword;
            //    $("#txtListItemKeyword").val(keyword === "" ? $("#txtListItemKeyword").attr("placeholder") : keyword);
            //    title = global.methods.setHighlighter(title, keyword);
            //}
            html += title;
            html += "</a>";
            html += "</td>";
            html += "<td class=\"date\">";
            html += listItem.Created;
            html += "</a>";
            html += "</td>";
            html += "<td class=\"user\">";
            html += listItem.EnableVersioning;
            html += "</td>";
            html += "<td class=\"date\">";
            html += listItem.EnableVersioning;
            html += "</a>";
            html += "</td>";
            html += "<td class=\"date\">";
            html += listItem.HasUniqueRoleAssignments;
            html += "</a>";
            html += "</td>";
            html += "<td class=\"user lastCell\">";
            html += listItem.Created;
            html += "</td>";
            html += "</tr>";
            var $row = $(html);
            return $row;
        },
        instance: function () {
           
        }
    }
}