charset = "UTF-8";

var treedoc = {
    enums: {
        listItemType: {
            documentLibrary: 0, //文档库
            folder: 1, //文件夹
            documentSet: 2, //文档集
            file: 3, //文件
            fileGroup: 4, //组合
            skyDrive: 5//个人网盘
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
        }
    },
    urls: {
        getTreeNodesUrl: 0,
        getSubTreeNodesUrl: 29,
        getSelectTreeNodesUrl: 23,
        getSubSelectTreeNodesUrl: 24,
        hasManageUserUrl: "Api/Document/HasManageUser",
        getListItemsUrl: 1, //"Api/Document/ListItem/GetListItems",
        checkWebPermissionUrl: 6 // "Api/Document/Web/CheckPermission",
    },

    doms: {
        treeDom: null,
        selectTreeDom: null,
        uploaderDom: null
    },
    datas: {
        folderNames: [],
        siteId: null,
        listId: null,
        listItemId: null,
        listItemBasePermissions: [],
        documetnSetListItemId: null,
        documetnSetBasePermissions: [],
        fileBasePermissions: [],
        listItemNames: [],
        folderKeyword: "",
        selectFolderKeyword: "",
        listItemKeyword: "",
        selectListId: null,
        selectListItemId: null
    },
    methods: {
        addQuickNav: function (node) {
            $("<span></span>")
                .attr({
                    "ListId": node.ListId,
                    "title": $.trim(node.Description) !== "" ? node.Description : node.Title
                })
                .addClass("textLink")
                .html(treedoc.methods.getQuickNavHtml(node.Title))
                .unbind("click")
                .click(function () {
                    if (treedoc.datas.listId === node.ListId && !treedoc.datas.listItemId) {
                        return;
                    }
                    treedoc.doms.treeDom.getNode(node.Id).find(">.nodeContents>.nodeTitle").click();
                })
                 .appendTo($("#quickNavBox"));
        },
        getQuickNavHtml: function (title) {
            var html = "";
            var length = title.length;
            for (var j = length - 1; j >= 0; j--) {
                html += "<span>" + title.substr(j, 1) + "</span>";
            }
            return html;
        },
        treeShowMenu: function (itemParam) {
            $("#tree .nodeContents").removeClass("hover");
            var selector;
            if (itemParam.ListItemId) {
                selector = "#tree .treeNode[ListId='" + itemParam.ListId + "'][ListItemId='" + itemParam.ListItemId + "']>.nodeContents";
            } else {
                selector = "#tree .treeNode[ListId='" + itemParam.ListId + "']>.nodeContents:first";
            }
            $(selector).addClass("hover");
        },
        treeItemClick: function ($menuItem, itemParam) {
            if ($menuItem.hasClass("refresh")) {
                treedoc.methods.loadTree();
                treedoc.methods.loadListItemsDataGrid();
            } else if ($menuItem.hasClass("addDocumentLibrary")) {
                treedoc.methods.addDocumentLibrary(itemParam);
            }
        },
        folderItemClick: function ($menuItem, itemParam) {
            if ($menuItem.hasClass("editFolder")) {
                treedoc.methods.writeLog(treedoc.enums.logGroup.folder, treedoc.enums.logType.editFolder, itemParam.ListId, itemParam.ListItemId, null, null, null, function () {
                    treedoc.methods.editFolder(itemParam);
                });
            } else if ($menuItem.hasClass("removeFolder")) {
                treedoc.methods.removeFolder(itemParam);
            } else if ($menuItem.hasClass("addFolder")) {
                treedoc.methods.addFolder(itemParam);
            } else if ($menuItem.hasClass("download")) {
                treedoc.methods.writeLog(treedoc.enums.logGroup.folder, treedoc.enums.logType.downloadFolder, itemParam.ListId, itemParam.ListItemId, null, null, null, function () {
                    treedoc.methods.download(itemParam);
                });
            } else if ($menuItem.hasClass("promotedAction")) {
                treedoc.methods.promotedAction(itemParam);
            } else if ($menuItem.hasClass("move") || $menuItem.hasClass("copy")) {
                var listItemId = $("#" + itemParam.Id).parent().parent().attr("ListItemId");
                var folderListItemId = (listItemId === "" || listItemId === undefined) ? undefined : parseInt(listItemId);
                if ($menuItem.hasClass("move")) {
                    treedoc.methods.move({
                        id: itemParam.Id,
                        listItemType: treedoc.enums.listItemType.folder,
                        listId: itemParam.ListId,
                        listItemId: itemParam.ListItemId,
                        folderListItemId: folderListItemId
                    });
                } else {
                    treedoc.methods.copy({
                        id: itemParam.Id,
                        listItemType: treedoc.enums.listItemType.folder,
                        listId: itemParam.ListId,
                        listItemId: itemParam.ListItemId,
                        folderListItemId: folderListItemId
                    });
                }
            } else if ($menuItem.hasClass("folderView")) {
                treedoc.methods.writeLog(treedoc.enums.logGroup.folder, treedoc.enums.logType.viewFolder, itemParam.ListId, itemParam.ListItemId, null, null, null, function () {
                    treedoc.methods.showFolder(itemParam);
                });
            } else if ($menuItem.hasClass("folderReport")) {
                treedoc.methods.report(itemParam);
            }
        },
        loadTree: function () {
            var keyword = treedoc.datas.folderKeyword === null ? "" : treedoc.datas.folderKeyword;
            global.methods.ajax(treedoc.urls.getTreeNodesUrl, {
                IsAllLoad: keyword !== "",
                Keyword: encodeURIComponent(global.methods.encrypt(keyword))
            }, ".leftPane", function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    global.methods.closeLoading(loadingId);
                    $.layer.alert(response.Result, null, ".document_box");
                    return;
                }
                var nodes = response.Result;
                treedoc.datas.folderNames = [];
                for (var i = 0, len = nodes.length; i < len; i++) {
                    var node = nodes[i];
                    if (!node.ParentId) {
                        treedoc.methods.addQuickNav(node);
                    } else {
                        global.methods.addKeyword(treedoc.datas.folderNames, node.Id + "|" + node.Title);
                    }
                }
                treedoc.methods.loadTreeNodes(nodes);
                global.methods.closeLoading(loadingId);
            }, null);
            $("#tree").hover(function () {
                $(this).css({ "overflow-y": "auto" });
            }, function () {
                $(this).css({ "overflow-y": "hidden" });
            });
        },
        loadTreeNodes: function (nodes, callBack) {
            var currentNode = null;
            for (var i = 0, len = nodes.length; i < len; i++) {
                var node = nodes[i];
                if (!node.ParentId && node.ListItemType !== treedoc.enums.listItemType.skyDrive) {
                    if (!currentNode && node.ListId === treedoc.datas.listId && !treedoc.datas.listItemId) {
                        currentNode = node;
                    }
                } else {
                    if (!currentNode && node.ListId === treedoc.datas.listId && node.ListItemId === treedoc.datas.listItemId) {
                        currentNode = node;
                    }
                }
            }
            treedoc.doms.treeDom = $("#tree").tree({
                nodes: nodes,
                open: true,
                itemBind: function ($node, node) {
                    if (node.ListItemType === treedoc.enums.listItemType.skyDrive) {
                        $node.css({
                            "margin-top": "10px",
                            "padding-top": "10px",
                            "border-top": "1px solid #cccccc"
                        });
                    }
                    var keyword = udoc.treedoc.folderKeyword === null ? "" : udoc.treedoc.folderKeyword;
                    if (node.ParentId && keyword === "") {
                        $node.removeClass("expanded").addClass("collapsed");
                    }
                    var $nodeTitle = $node.find(".nodeTitle");
                    $nodeTitle.html(global.methods.setHighlighter(node.Title, keyword));
                    var selector, onShowMenu, itemClick, downloadType;
                    if (!node.ParentId && node.ListItemType !== treedoc.enums.listItemType.skyDrive) {
                        itemClick = treedoc.methods.documentLibraryItemClick;
                    } else {
                        itemClick = treedoc.methods.folderItemClick;
                    }
                    var listItemId = $node.parent().parent().attr("ListItemId");
                    var folderListItemId = listItemId ? parseInt(listItemId) : undefined;
                    //$node.find(".nodeContents").contextMenu({
                    //    itemClick: itemClick,
                    //    itemParam: {
                    //        Id: node.Id,
                    //        ListId: node.ListId,
                    //        ListItemId: node.ListItemId,
                    //        FolderListItemId: folderListItemId,
                    //        DownloadType: downloadType,
                    //        BasePermissions: node.BasePermissions,
                    //        ListItemType: node.ListItemType
                    //    }
                    //});
                    $node.attr({ "ListId": node.ListId });
                    if (node.ListItemId) {
                        $node.attr({ "ListItemId": node.ListItemId });
                    }
                    $node.find(">.nodeContents").hover(function () {
                        $("#tree .nodeContents").removeClass("hover");
                    }, function () { });
                    $node.find(">.nodeContents>.nodeTitle").unbind("click").click(function () {
                        $("#tree .nodeContents").removeClass("hover");
                        var $treeNode = $(this).parent().parent();
                        var listId = $treeNode.attr("ListId");
                        var listItemId = $treeNode.attr("ListItemId");
                        if (listId === treedoc.datas.listId) {
                            if (listItemId && treedoc.datas.listItemId) {
                                if (parseInt(listItemId) === treedoc.datas.listItemId) {
                                    return;
                                }
                            } else if (!listItemId && !treedoc.datas.listItemId) {
                                return;
                            }
                        }
                        treedoc.datas.listItemKeyword = "";
                        treedoc.methods.selectedNode(node);
                        treedoc.methods.loadListItemsDataGrid();
                    });
                    if ($.isFunction(callBack)) {
                        callBack();
                    }
                },
                click: treedoc.methods.treeClick
            });
            if (nodes.length !== 0 && !currentNode) {
                currentNode = nodes[0];
            }
            if (nodes.length !== 0) {
                treedoc.methods.selectedNode(currentNode);
            } else {
                $(".toolsbar .actionLink").attr({ "disabled": "disabled" });
            }
        },
        treeClick: function ($node, node) {
            if (!($node.hasClass("collapsed") && node.ParentId && $node.find("ul").length === 0)) { //展开
                treedoc.doms.treeDom.change($node);
                return;
            }
            treedoc.methods.refreshTree(node.ListId, node.ListItemId);
        },
        refreshTree: function (listId, listItemId) {
            var keyword = treedoc.datas.folderKeyword === null ? "" : treedoc.datas.folderKeyword;
            global.methods.ajax(treedoc.urls.getSubTreeNodesUrl, {
                ListId: listId,
                ListItemId: listItemId,
                Keyword: encodeURIComponent(global.methods.encrypt(keyword))
            }, ".leftPane", function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    global.methods.closeLoading(loadingId);
                    $.layer.alert(response.Result, null, ".document_box");
                    return;
                }
                var $node;
                if (!listItemId) {
                    $node = $("#tree").find("li[ListId='" + listId + "']:first");
                } else {
                    $node = $("#tree").find("li[ListId='" + listId + "'][ListItemId='" + listItemId + "']");
                }
                treedoc.doms.treeDom.refreshChildNodes($node.attr("Id"), response.Result);
                if (response.Result.length === 0) {
                    $node.removeClass("collapsed");
                } else {
                    $node.removeClass("collapsed").addClass("expanded");
                }
                global.methods.closeLoading(loadingId);
            }, null);
        },
        loadTreeData: function (callBack) {
            var keyword = treedoc.datas.folderKeyword === null ? "" : treedoc.datas.folderKeyword;
            global.methods.ajax(treedoc.urls.getTreeNodesUrl, {
                IsAllLoad: true,
                Keyword: encodeURIComponent(global.methods.encrypt(keyword))
            }, null, function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    return;
                }
                var nodes = response.Result;
                treedoc.datas.folderNames = [];
                for (var i = 0, len = nodes.length; i < len; i++) {
                    var node = nodes[i];
                    if (node.ParentId) {
                        global.methods.addKeyword(treedoc.datas.folderNames, node.Id + "|" + node.Title);
                    }
                }
                treedoc.datas.treeNodes = nodes;
                if ($.isFunction(callBack)) {
                    callBack();
                }
            }, null);
        },
        selectedNode: function (node) {
            treedoc.datas.listId = node.ListId;
            treedoc.datas.listItemId = node.ListItemId;
            treedoc.methods.checkPermissions();
            $("#quickNavBox").find(".textLink[ListId='" + node.ListId + "']").addClass("selected").siblings().removeClass("selected");
            treedoc.doms.treeDom.selectedNode(node.Id);
            treedoc.datas.listItemBasePermissions = node.BasePermissions;
        },
        checkPermissions: function () {
            // $("#fileListToolsbar a").removeAttr("disabled").show();
            $("#fileListToolsbar a").show();
            var basePermissions = treedoc.datas.listItemBasePermissions;
            for (var i = 0, len = basePermissions.length; i < len; i++) {
                var basePermission = basePermissions[i];
                if (basePermission.HasPermission) {
                    continue;
                }
                switch (basePermission.PermissionKind) {
                    case treedoc.enums.permissionKind.addListItems:
                        //$("#uploadFilesLink,#addDocumentSetLink").attr({ "disabled": "disabled" });
                        $("#uploadFilesLink,#addDocumentSetLink").hide();
                        break;
                    case treedoc.enums.permissionKind.deleteListItems:
                        //$("#removeListItemsLink,#moveListItemsLink").attr({ "disabled": "disabled" });
                        $("#removeListItemsLink,#moveListItemsLink").hide();
                        break;
                    case treedoc.enums.permissionKind.open:
                        //$("#downloadListItemsLink").attr({ "disabled": "disabled" });
                        $("#downloadListItemsLink").hide();
                        break;
                }
            }
        },
        showFolder: function (itemParam) {
            global.methods.ajax(treedoc.urls.getFolderListItemUrl, {
                ListId: itemParam.ListId,
                ListItemId: itemParam.ListItemId,
                ListItemType: treedoc.enums.listItemType.folder
            }, ".document_box", function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    global.methods.closeLoading(loadingId);
                    $.layer.alert(response.Result, null, ".document_box");
                    return;
                }
                var html = treedoc.htmls.foderViewHtml;
                html = $.replaceAll(html, "{FileLeafRef}", response.Result.FileLeafRef);
                html = $.replaceAll(html, "{Title}", response.Result.Title);
                html = $.replaceAll(html, "{SortNumber}", response.Result.SortNumber === undefined || response.Result.SortNumber === null || response.Result.SortNumber === "" ? "" : response.Result.SortNumber);
                html = $.replaceAll(html, "{Created}", response.Result.Created);
                html = $.replaceAll(html, "{LastItemModifiedDate}", response.Result.LastItemModifiedDate);
                global.methods.closeLoading(loadingId);
                $.layer.dialog({
                    dom: ".document_box",
                    title: "查看文件夹",
                    html: html,
                    showClose: true
                });
            }, null);
        },
        excuteAction: function (url) {
            $("#actionLink").attr({ "href": url });
            document.getElementById("actionLink").click();
        },
        initData: function () {
            var hash = location.hash;
            if (!hash) {
                return;
            }
            hash = hash.substr(1);
            treedoc.datas.siteId = global.methods.getQueryString("SiteId", hash);
            treedoc.datas.listId = global.methods.getQueryString("ListId", hash);
            var listItemId = global.methods.getQueryString("ListItemId", hash);
            if (listItemId) {
                treedoc.datas.listItemId = parseInt(listItemId);
            }
        },
        openSelectNodeDialog: function (params, callBack) {
            var title = ((params.action === treedoc.enums.actionType.move ? "移动" : "复制") + "文件-请选择目录");
            var dialogId = $.layer.dialog({
                dom: ".document_box",
                title: title,
                html: treedoc.htmls.selectNodeDialogHtml,
                showOk: true,
                showCancel: true,
                after: function () {
                    treedoc.methods.loadSelectTree(params);
                },
                ok: function ($layerContents) {
                    var $nodeContents = $layerContents.find(".selected");
                    if ($nodeContents.length === 0) {
                        $.layer.alert("请选择目录.", null, ".document_box");
                        return;
                    }
                    var $node = $nodeContents.parent();
                    var id = $node.attr("id");
                    var listId = $node.attr("ListId");
                    var listItemId = $node.attr("ListItemId");
                    if ($.isFunction(callBack)) {
                        callBack({
                            id: id,
                            listId: listId,
                            listItemId: listItemId,
                            dialogId: dialogId
                        });
                    } else {
                        $.layer.close(dialogId);
                    }
                },
                dragCallback: function ($layerContents, width, height) {
                    $layerContents.find("#selectTree").width(width).height(height - 36);
                }
            });
        },
        checkIsAllFile: function (listId, listItemIds) {
            for (var i = 0, length = listItemIds.length; i < length; i++) {
                if ($("tr[ListItemType=\"" + treedoc.enums.listItemType.file + "\"][ListId=\"" + listId + "\"][ListItemId=\"" + listItemIds[i] + "\"]").length === 0) {
                    return false;
                }
            }
            return true;
        },
        loadSelectTree: function (params) {
            treedoc.datas.selectParam = params;
            global.methods.ajax(treedoc.urls.getSelectTreeNodesUrl, {
                Keyword: ""
            }, ".document_box", function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    global.methods.closeLoading(loadingId);
                    $.layer.alert(response.Result, null, ".document_box");
                    return;
                }
                var treeNodes = [];
                var nodes = response.Result;
                for (var i = 0, len = nodes.length; i < len; i++) {
                    var node = nodes[i];
                    if ((treedoc.datas.selectParam.listItemType === treedoc.enums.listItemType.folder
                        || treedoc.datas.selectParam.listItemType === treedoc.enums.listItemType.documentSet
                        || treedoc.datas.selectParam.listItemType === treedoc.enums.listItemType.fileGroup && treedoc.methods.checkIsAllFile(listId, treedoc.datas.selectParam.listItemIds))
                        && node.ListItemType === treedoc.enums.listItemType.documentSet) {
                        continue;
                    }
                    if (node.ListItemType === treedoc.enums.listItemType.documentSet) {
                        node.ImageUrl = "/_layouts/images/" + node.ImageUrl;
                    }
                    treeNodes.push(node);
                }
                treedoc.doms.selectTreeDom = $("#selectTree").tree({
                    nodes: treeNodes,
                    open: true,
                    itemBind: function ($node, node) {
                        var hasAddPermission = false;
                        var basePermissions = node.BasePermissions;
                        for (var i = 0, len = basePermissions.length; i < len; i++) {
                            var basePermission = basePermissions[i];
                            switch (basePermission.PermissionKind) {
                                case treedoc.enums.permissionKind.addListItems:
                                    hasAddPermission = basePermission.HasPermission;
                                    break;
                            }
                        }
                        if (node.ParentId) {
                            $node.removeClass("expanded").addClass("collapsed");
                        }
                        var $nodeTitle = $node.find(".nodeTitle");
                        var title = node.Title;
                        if ((params.listItemType === treedoc.enums.listItemType.folder && node.ListId === params.listId && node.ListItemId === params.listItemId)
                            || (params.listItemType !== treedoc.enums.listItemType.folder && node.ListId === params.listId && node.ListItemId === params.listItemId)) {
                            title += ("(<font color=\"red\">当前目录</font>)");
                            $node.removeClass("collapsed");
                        }
                        $nodeTitle.html(title);
                        $node.attr({ "ListId": node.ListId });
                        if (!hasAddPermission) {
                            $nodeTitle.parent().attr({ "disabled": "disabled" });
                        }
                        if (node.ListItemId) {
                            $node.attr({ "ListItemId": node.ListItemId });
                        }
                        $nodeTitle.unbind("click").click(function () {
                            if ($(this).find("font").length === 1) {
                                return;
                            }
                            $("#selectTree").find(".nodeContents").removeClass("selected");
                            $(this).parent().addClass("selected");
                        });
                    },
                    click: treedoc.methods.selectTreeClick
                });
                global.methods.closeLoading(loadingId);
            }, null);
            $("#selectTree").hover(function () {
                $(this).css({ "overflow-y": "auto" });
            }, function () {
                $(this).css({ "overflow-y": "hidden" });
            });
        },
        selectTreeClick: function ($node, node) {
            if (!($node.hasClass("collapsed") && node.ParentId && $node.find("ul").length === 0)) { //展开
                treedoc.doms.treeDom.change($node);
                return;
            }
            treedoc.methods.refreshSelectTree(node.ListId, node.ListItemId);
        },
        refreshSelectTree: function (listId, listItemId) {
            global.methods.ajax(treedoc.urls.getSubSelectTreeNodesUrl, {
                ListId: listId,
                ListItemId: listItemId,
                Keyword: ""
            }, global.settings.layerSelector, function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    global.methods.closeLoading(loadingId);
                    $.layer.alert(response.Result, null, global.settings.layerSelector);
                    return;
                }
                var $node;
                if (!listItemId) {
                    $node = $("#selectTree").find("li[ListId='" + listId + "']:first");
                } else {
                    $node = $("#selectTree").find("li[ListId='" + listId + "'][ListItemId='" + listItemId + "']");
                }
                var nodes = [];
                for (var i = 0, len = response.Result.length; i < len; i++) {
                    var node = response.Result[i];
                    if ((treedoc.datas.selectParam.listItemType === treedoc.enums.listItemType.folder
                        || treedoc.datas.selectParam.listItemType === treedoc.enums.listItemType.documentSet
                        || treedoc.datas.selectParam.listItemType === treedoc.enums.listItemType.fileGroup && treedoc.methods.checkIsAllFile(listId, treedoc.datas.selectParam.listItemIds))
                        && node.ListItemType === treedoc.enums.listItemType.documentSet) {
                        continue;
                    }
                    nodes.push(node);
                }
                treedoc.doms.selectTreeDom.refreshChildNodes($node.attr("Id"), nodes);
                if (response.Result.length === 0) {
                    $node.removeClass("collapsed");
                } else {
                    $node.removeClass("collapsed").addClass("expanded");
                }
                global.methods.closeLoading(loadingId);
            }, null);
        },

        instance: function () {
            this.initData();
            this.loadTree();
            this.loadTreeData();
        }
    }
}