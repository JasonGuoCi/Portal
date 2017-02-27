charset = "UTF-8";
$.fn.outerHTML = function () {
    return $(this).wrap("<div></div>").parent().html();
}
function hasMap() {
    this.keys = new Array();
    this.data = new Array();
    //添加键值对
    this.set = function (key, value) {
        if (this.data[key] == null) {//如键不存在则身【键】数组添加键名
            this.keys.push(value);
        }
        this.data[key] = value;//给键赋值
    };
    //获取键对应的值
    this.get = function (key) {
        return this.data[key];
    };
    //去除键值，(去除键数据中的键名及对应的值)
    this.remove = function (key) {
        this.keys.remove(key);
        this.data[key] = null;
    };
    //判断键值元素是否为空
    this.isEmpty = function () {
        return this.keys.length == 0;
    };
    //获取键值元素大小
    this.size = function () {
        return this.keys.length;
    };
}
//单选下拉框
$.fn.ruleSingleSelect = function () {
    var singleSelect = function (parentObj) {
        parentObj.addClass("single-select"); //添加样式
        parentObj.children().hide(); //隐藏内容
        var divObj = $('<div class="boxwrap"></div>').prependTo(parentObj); //前插入一个DIV
        //创建元素
        var titObj = $('<a class="select-tit" href="javascript:;"><span></span><i></i></a>').appendTo(divObj);
        var itemObj = $('<div class="select-items"><ul></ul></div>').appendTo(divObj);
        var arrowObj = $('<i class="arrow"></i>').appendTo(divObj);
        var selectObj = parentObj.find("select").eq(0); //取得select对象
        //遍历option选项
        selectObj.find("option").each(function (i) {
            var indexNum = selectObj.find("option").index(this); //当前索引
            var liObj = $('<li>' + $(this).text() + '</li>').appendTo(itemObj.find("ul")); //创建LI
            if ($(this).prop("selected") == true) {
                liObj.addClass("selected");
                titObj.find("span").text($(this).text());
            }
            //检查控件是否启用
            if ($(this).prop("disabled") == true) {
                liObj.css("cursor", "default");
                return;
            }
            //绑定事件
            liObj.click(function () {
                $(this).siblings().removeClass("selected");
                $(this).addClass("selected"); //添加选中样式
                selectObj.find("option").prop("selected", false);
                selectObj.find("option").eq(indexNum).prop("selected", true); //赋值给对应的option
                titObj.find("span").text($(this).text()); //赋值选中值
                arrowObj.hide();
                itemObj.hide(); //隐藏下拉框
                selectObj.trigger("change"); //触发select的onchange事件
            });
        });

        //检查控件是否启用
        if (selectObj.prop("disabled") == true) {
            titObj.css("cursor", "default");
            return;
        }
        //绑定单击事件
        titObj.click(function (e) {
            e.stopPropagation();
            if (itemObj.is(":hidden")) {
                //隐藏其它的下位框菜单
                $(".single-select .select-items").hide();
                $(".single-select .arrow").hide();
                //位于其它无素的上面
                arrowObj.css("z-index", "1");
                itemObj.css("z-index", "1");
                //显示下拉框
                arrowObj.show();
                itemObj.show();
            } else {
                //位于其它无素的上面
                arrowObj.css("z-index", "");
                itemObj.css("z-index", "");
                //隐藏下拉框
                arrowObj.hide();
                itemObj.hide();
            }
        });
        //绑定页面点击事件
        $(document).click(function (e) {
            selectObj.trigger("blur"); //触发select的onblure事件
            arrowObj.hide();
            itemObj.hide(); //隐藏下拉框
        });
    };
    return $(this).each(function () {
        singleSelect($(this));
    });
}

var envdoc = {
    urls: {
        //左侧菜单
        getLeftMeunsUrl: 0,
        //新建文档库
        addDocumentLibrary: 1,
        //获取用户组
        getUserGroups: 2,
        isExistDocumentLibrary: 3,
        addSPGroupsUrl: 4,
        getSPGroupsInfoUrl: 5,
        editSPGroupsUrl: 6,
        getDocumentLibraryUrl: 7,
        editDocumentLibraryUrl: 8,
        isExistSPGroupsUrl: 9,
        isExistEditDocumentLibraryUrl: 10,
        getTreeNodesUrl: 11,
        getTreeChildNodesUrl: 12,
        getItTreeNodesUrl: 13,
        getItTreeChildNodesUrl: 14,
        addOwnersToGroupUrl: 15,
        IsExistEditSPGroupsUrl:17
    },
    datas: {
        validform: null,
        CreateDocumentParam: {
            DocumentLibraryName: "",
            BeforeDocumentLibraryName: "",
            DocumentLibraryTemplateId: "",
            DocumentLibraryTemplateName: "",
            DocumentVersionControlId: "",
            DocumentVersionControlName: "",
            DocumentDefaultCheckOutId: "",
            DocumentDefaultCheckOutName: "",
            spuserGroups: [],
            folderNames: [],
            listId: null,
            listItemId: null,
            currentWebUrl: ""
        },
        UserGroupRoles: function () {
            var p = new Object();
            p.Id = "";
            p.Name = "";
            p.GroupRole = "";
            return p;
        },
        spUserGroups: [],
        setGroupsData: {
            GroupsId:"",
            GroupsName: "",
            GroupsOwnerId: "",
            GroupsOwnerName: "",
            GroupsDescription: ""
        },
        zNodes: null,
        itzNodes: null,
        addSPGroupsByDocLayerConent: null,
        ChoseExistingGroupsLayerConent: null,
        EditChoseExistingGroupsLayerContent:null

    },
    config: {
        setting: {
            view: {
                showLine: false
            },
            data: {
                simpleData: {
                    enable: true
                }
            }, async: {
                enable: true,
                url: "/_layouts/15/EnvisionDoc/Handlers/AjaxHandler.ashx",
                autoParam: ["id"],
                otherParam: {},
                dataFilter: null //异步返回后经过Filter 
            }, callback: {
                onClick: null,
                beforeAsync: null,
                asyncSuccess: null,//异步加载成功的fun 
                asyncError: null, //加载错误的fun 
                beforeClick: null //捕获单击节点之前的事件回调函数 
            }
        }
    },
    cdhtmls: {
        createDocumenthtml: "",
        createGroupRoleshtml: "",
        existingGroupAddhtml: "",
        docCreateLastStepHtml: "",
        editDocumentLibraryHtml: "",
        addGroupsHtml: "",
        addGroupOwnersHtml: "",
        existingGroupAddToGrouphtml: "",
        init: function () {
            this.createDocumenthtml = $("#createDocumenthtml").show().remove().outerHTML();
            this.createGroupRoleshtml = $("#createGroupRolehtml").show().remove().outerHTML();
            this.existingGroupAddhtml = $("#existingGroupAddhtml").show().remove().outerHTML();
            this.docCreateLastStepHtml = $("#docCreateLastStep").show().remove().outerHTML();
            this.editDocumentLibraryHtml = $("#editDocumentLibrary").show().remove().outerHTML();

            this.addGroupsHtml = $("#AddGroups").show().remove().outerHTML();
            this.addGroupOwnersHtml = $("#AddGroupsOwners").show().remove().outerHTML();
            this.existingGroupAddToGrouphtml = $("#existingGroupAddToGrouphtml").show().remove().outerHTML();
        }
    },
    doms: {
        treeDom: null,
        zTreeObj: null,
        selectTreeDom: null,
        layerload: null
    },
    cghtmls: {
        exisGroupsAddhtml: "",
        addGroupsHtml: "",
        addGroupOwnersHtml: "",
        editGroupsHtml: "",
        init: function () {
            this.addGroupsHtml = $("#AddGroups").show().remove().outerHTML();
            this.exisGroupsAddhtml = $("#existingGroupAddhtml").show().remove().outerHTML();
            this.addGroupOwnersHtml = $("#AddGroupsOwners").show().remove().outerHTML();
            this.editGroupsHtml = $("#EditGroups").show().remove().outerHTML();
        }
    },
    methods: {
        validform: function ($layercontainer, selector) {
            envdoc.datas.validform = $layercontainer.Validform({
                tiptype: 3,
                btnSubmit: global.settings.layerSelector + " " + selector
            });
        },
        accordion_init: function () {
            $(".m_pd").click(function () {
                $(".m_pd").parent().siblings().hide();
                $(".m_pd").parent().parent().attr("class", "c_p");
                $(this).parent().siblings().show();
                $(this).parent().parent().attr("class", "o_p");
            })
        },
        isSelected: function (items, item) {
            var result = false;
            $.each(items, function (i, n) {
                if (n.Id == item.Id) {
                    result = true;
                    return true;
                }
            });

            return result;
        },
        initDocumentParam: function () {
            envdoc.datas.CreateDocumentParam.DocumentLibraryName = "";
            envdoc.datas.CreateDocumentParam.DocumentLibraryTemplateId = "";
            envdoc.datas.CreateDocumentParam.DocumentLibraryTemplateName = "";
            envdoc.datas.CreateDocumentParam.DocumentVersionControlId = "";
            envdoc.datas.CreateDocumentParam.DocumentVersionControlName = "";
            envdoc.datas.CreateDocumentParam.DocumentDefaultCheckOutId = "";
            envdoc.datas.CreateDocumentParam.DocumentDefaultCheckOutName = "";
            envdoc.datas.CreateDocumentParam.spuserGroups = [];
            envdoc.datas.spUserGroups = [];
        },
        getIsExistDocumentLibrary: function () {
            var cuscheck = false;
            var currentWebUrl = $("#hidCurrentWebUrl").val();
            var ajaxUrl = "/_layouts/15/EnvisionDoc/Handlers/AjaxHandler.ashx?MethodName=" + envdoc.urls.isExistDocumentLibrary + "&param=" + encodeURIComponent($("#txtDocName").val());
            ajaxUrl = (currentWebUrl == undefined ? "" : currentWebUrl) + ajaxUrl;
            $.ajax({
                type: "POST",  //提交方式
                url: ajaxUrl,//路径
                async: false,
                success: function (result) {//返回数据根据结果进行相应的处理
                    var d = JSON.parse(result)
                    if (d.status == "y") {
                        cuscheck = true;
                    } else {
                        cuscheck = false;
                    }
                }
            });
            return cuscheck;
        },
        getIsExistSPGroups: function () {
            var cuscheck = false;
            var currentWebUrl = $("#hidCurrentWebUrl").val();
            var ajaxUrl = "/_layouts/15/EnvisionDoc/Handlers/AjaxHandler.ashx?MethodName=" + envdoc.urls.isExistSPGroupsUrl + "&param=" + encodeURIComponent($("#txtGroupsName").val());
            ajaxUrl = (currentWebUrl == undefined ? "" : currentWebUrl) + ajaxUrl;

            $.ajax({
                type: "POST",  //提交方式
                url: ajaxUrl,//路径
                async: false,
                success: function (result) {//返回数据根据结果进行相应的处理
                    var d = JSON.parse(result)
                    if (d.status == "y") {
                        cuscheck = true;
                    } else {
                        cuscheck = false;
                    }
                }
            });
            return cuscheck;
        },
        getIsExistSPGroupsDoc: function () {
            var cuscheck = false;
            var currentWebUrl = $("#hidCurrentWebUrl").val();
            var ajaxUrl = "/_layouts/15/EnvisionDoc/Handlers/AjaxHandler.ashx?MethodName=" + envdoc.urls.isExistSPGroupsUrl + "&param=" + encodeURIComponent($("#txtAddGroupsName").val());
            ajaxUrl = (currentWebUrl == undefined ? "" : currentWebUrl) + ajaxUrl;

            $.ajax({
                type: "POST",  //提交方式
                url: ajaxUrl,//路径
                async: false,
                success: function (result) {//返回数据根据结果进行相应的处理
                    var d = JSON.parse(result)
                    if (d.status == "y") {
                        cuscheck = true;
                    } else {
                        cuscheck = false;
                    }
                }
            });
            return cuscheck;
        },
        getIsExistEditDocumentLibrary: function (obj) {
            var cuscheck = false;
            var currentWebUrl = $("#hidCurrentWebUrl").val();
            var ajaxUrl = "/_layouts/15/EnvisionDoc/Handlers/AjaxHandler.ashx?MethodName=" + envdoc.urls.isExistEditDocumentLibraryUrl + "&beforeDocLibName=" + encodeURIComponent($(obj).attr("dname")) + "&param=" + encodeURIComponent($("#txtEditDoclibName").val());
            ajaxUrl = (currentWebUrl == undefined ? "" : currentWebUrl) + ajaxUrl;

            $.ajax({
                type: "POST",  //提交方式
                url: ajaxUrl,//路径
                async: false,
                success: function (result) {//返回数据根据结果进行相应的处理
                    var d = JSON.parse(result)
                    if (d.status == "y") {
                        cuscheck = true;
                    } else {
                        cuscheck = false;
                    }
                }
            });
            return cuscheck;
        },
        getIsExistEditGroups: function (obj) {
            var cuscheck = false;
            var currentWebUrl = $("#hidCurrentWebUrl").val();
            var ajaxUrl = "/_layouts/15/EnvisionDoc/Handlers/AjaxHandler.ashx?MethodName=" + envdoc.urls.IsExistEditSPGroupsUrl + "&beforeGroupsName=" + encodeURIComponent($(obj).attr("gname")) + "&param=" + encodeURIComponent($("#txtEditGroupsName").val());
            ajaxUrl = (currentWebUrl == undefined ? "" : currentWebUrl) + ajaxUrl;

            $.ajax({
                type: "POST",  //提交方式
                url: ajaxUrl,//路径
                async: false,
                success: function (result) {//返回数据根据结果进行相应的处理
                    var d = JSON.parse(result)
                    if (d.status == "y") {
                        cuscheck = true;
                    } else {
                        cuscheck = false;
                    }
                }
            });
            return cuscheck;
        },
        getLeftMenus: function () {
            global.methods.ajax(envdoc.urls.getLeftMeunsUrl, null, null, function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    return;
                }
                var listItems = response.Result.menutab || [];
                var htmlMenu = "";
                var leftMenus = $("#leftMenu");
                var o = 0;
                var k = 0;
                $.each(listItems, function (i, n) {
                    if ($("#ctl00_PlaceHolderMain_hidIsWebManager").val() == "FALSE" && n.name == "站点设置") {
                        return true;
                    }
                    if (o == 0) {
                        o = 1;
                        k = 1;
                        htmlMenu += "<dl class=\"o_p\">";

                    } else {
                        k = 0;
                        htmlMenu += "<dl class=\"c_p\">";
                    }

                    htmlMenu += " <dt id=\"one" + i + "\"><a class=\"menu m_pd\" href=\"#\">" + n.name + "</a></dt><dd>";

                    $.each(n.menu, function (j, m) {
                        if (j == 0) {
                            htmlMenu += "<a class=\"menu hover\" href='#' name=\"" + m.name + "\" id=\"" + m.name + "\" url='" + m.url + "' onclick=\"ShowTaskPage('" + m.url + "',this);\">" + m.name + "</a>";
                            if (k == 1) {
                                $("#mainframe").attr("src", "");
                                $("#mainframe").attr("src", m.url);
                                $(".filebox").html("<a>" + n.name + "</a><span>&gt;</span><a>" + m.name + "</a>");
                            }
                        } else {
                            htmlMenu += "<a class=\"menu\" href='#' name=\"" + m.name + "\" id=\"" + m.name + "\" url='" + m.url + "' onclick=\"ShowTaskPage('" + m.url + "',this);\">" + m.name + "</a>";
                        }

                    });
                    htmlMenu += "</dd></dl>";

                });
                leftMenus.html(htmlMenu);
                envdoc.methods.accordion_init();
            }, null);
        },
        getUserGroups: function (pageindex) {
            global.methods.ajax(envdoc.urls.getUserGroups, {
                category: 1,
                pageindex: pageindex,
                keyword: encodeURIComponent($("#txtGroupsName").val())
            }, null, function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    return;
                }
                var listItems = response.Result || [];
                var PageHtml = response.PageContent;
                var existingGroups = $("#existingGroupAddhtml");
                var existingGroupsTable = existingGroups.find("table");
                existingGroups.find("table tr:not(:first)").html("");
                $("#selectGroups").html("");

                $.each(listItems, function (i, n) {
                    var row = $("<tr></tr>");
                    var column1 = $("<td style='text-align:left;'>" + n.Name + "</td>");
                    //var column2 = $("<td></td>");
                    var column3 = $("<td></td>");
                    var link = $("<a style='cursor: pointer;' class='cz_itm' gid='" + n.Id + "' id='x" + n.Id + "'>选 择</a>");
                    link.click(function () {

                        if (!envdoc.methods.isSelected(envdoc.datas.spUserGroups, n)) {
                            link.attr("class", "cz_itms");
                            var UserGroupsRoles = new envdoc.datas.UserGroupRoles();
                            UserGroupsRoles.Id = n.Id;
                            UserGroupsRoles.Name = n.Name;
                            UserGroupsRoles.GroupRole = "1";
                            envdoc.datas.spUserGroups.push(UserGroupsRoles);
                            envdoc.methods.addSelectGroups(UserGroupsRoles, i);
                        }
                    });
                    link.appendTo(column3);
                    column1.appendTo(row);
                    //column2.appendTo(row);
                    column3.appendTo(row);
                    row.appendTo(existingGroupsTable);
                });
                var rowPage = $("<tr></tr>");
                var columnPage = $("<td colspan=\"2\" class='page_part'><div class=\"pagelist\"><div id=\"PageContent\" runat=\"server\" class=\"default\">" + PageHtml + "</div></div></td>");
                columnPage.appendTo(rowPage);
                rowPage.appendTo(existingGroupsTable);

                $.each(envdoc.datas.spUserGroups, function (i, n) {
                    $("#x" + n.Id).attr("class", "cz_itms");
                    envdoc.methods.addSelectGroups(n, i);
                });
            }, null);
        },
        GroupsPages: function (pageindex, category) {
            if(category==1){
            //getUserGroups
                envdoc.methods.getUserGroups(pageindex);
            }else if(category==2)
            {
                envdoc.methods.ChoseExistingGroupsByDoc(envdoc.datas.addSPGroupsByDocLayerConent,pageindex)
            } else if (category == 3) {
                envdoc.methods.ChoseExistingGroups(envdoc.datas.ChoseExistingGroupsLayerContent, pageindex);
            }else if(category==4)
            {
                envdoc.methods.EditChoseExistingGroups(envdoc.datas.EditChoseExistingGroupsLayerContent, pageindex);
            }
        },
        setDocumentLibraryParam: function () {
            envdoc.datas.CreateDocumentParam.DocumentLibraryName = $("#txtDocName").val();
            envdoc.datas.CreateDocumentParam.DocumentLibraryTemplateId = $("#ddlDocTemplate").find("option:selected").val();
            envdoc.datas.CreateDocumentParam.DocumentLibraryTemplateName = $("#ddlDocTemplate").find("option:selected").text();
        },
        setEditDocumentLibraryParam: function (obj) {
            envdoc.datas.CreateDocumentParam.listId = $(obj).attr("id");
            envdoc.datas.CreateDocumentParam.DocumentLibraryName = $("#txtEditDoclibName").val();
            envdoc.datas.CreateDocumentParam.DocumentVersionControlId = $("#ddlEditVersionControl").find("option:selected").val();
            envdoc.datas.CreateDocumentParam.DocumentVersionControlName = $("#ddlEditVersionControl").find("option:selected").text();
            envdoc.datas.CreateDocumentParam.DocumentDefaultCheckOutId = $("input[name='rblEditCheckOut']:checked").val();
            envdoc.datas.CreateDocumentParam.DocumentDefaultCheckOutName = $("input[name='rblEditCheckOut']:checked").next().text();
        },
        setDocumentLibraryParamOther: function () {
            envdoc.datas.CreateDocumentParam.DocumentVersionControlId = $("#ddlVersionControl").find("option:selected").val();
            envdoc.datas.CreateDocumentParam.DocumentVersionControlName = $("#ddlVersionControl").find("option:selected").text();
            envdoc.datas.CreateDocumentParam.DocumentDefaultCheckOutId = $("input[name='rblCheckOut']:checked").val();
            envdoc.datas.CreateDocumentParam.DocumentDefaultCheckOutName = $("input[name='rblCheckOut']:checked").next().text();
        },
        setCreateDocumentLibraryParam: function () {
            if (envdoc.datas.CreateDocumentParam.DocumentLibraryName != "") {
                $("#txtDocName").val(envdoc.datas.CreateDocumentParam.DocumentLibraryName);
            }
            if (envdoc.datas.CreateDocumentParam.DocumentLibraryTemplateId != "") {
                $('select#ddlDocTemplate').attr('value', envdoc.datas.CreateDocumentParam.DocumentLibraryTemplateId);
            }
        },
        showDocumentLibraryParam: function () {
            if (envdoc.datas.CreateDocumentParam.DocumentLibraryName != "") {
                $("#labDoclibName").text(envdoc.datas.CreateDocumentParam.DocumentLibraryName);
            }
        },
        showDocumentLibraryParamOther: function () {
            if (envdoc.datas.CreateDocumentParam.DocumentVersionControlId != "") {
                $('select#ddlVersionControl').attr('value', envdoc.datas.CreateDocumentParam.DocumentVersionControlId);
            }
            if (envdoc.datas.CreateDocumentParam.DocumentDefaultCheckOutId != "") {
                $("#rblCheckOut input[value='" + envdoc.datas.CreateDocumentParam.DocumentDefaultCheckOutId + "']").attr("checked", "checked");
            }
        },
        addDocumentLibrary: function () {
            var result = false;
            global.methods.ajaxt(envdoc.urls.addDocumentLibrary, {
                createDocumentattr: encodeURIComponent(JSON.stringify(envdoc.datas.CreateDocumentParam))
            }, null, function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    layer.close(loadingId);
                    layer.msg("创建失败！ " + response.Result)
                } else {
                    layer.closeAll();
                    layer.msg("创建成功！");
                    setTimeout("location.reload()", 3000);
                }
            }, null, false);
        },
        addSPGroups: function (layerindex) {
            var result = false;
            global.methods.ajaxt(envdoc.urls.addSPGroupsUrl, {
                createSPGroupsattr: encodeURIComponent(JSON.stringify(envdoc.datas.setGroupsData))
            }, null, function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    layer.close(loadingId);
                    //layer.msg("创建失败");
                } else {
                    layer.close(loadingId);
                    layer.close(layerindex);
                    envdoc.datas.setGroupsData.GroupsId = response.Result.GroupsId;
                    envdoc.methods.showFaseAddGroupsOwnersLayer();
                    envdoc.methods.validform($("#AddGroupsOwners"), ".layui-layer-btn0");
                    $("#btnChoseGroups").click(function () {
                        var layerContent = envdoc.methods.ChoseExistingGroupAddHtml();
                        envdoc.datas.ChoseExistingGroupsLayerConent = layerContent;

                        envdoc.methods.ChoseExistingGroups(layerContent, 0);

                        $("#btnChoseSearchGroups").click(function () {
                            envdoc.methods.ChoseExistingGroups(layerContent,0);
                        });
                    });

                    //layer.closeAll();
                    //layer.msg("创建成功");
                    //setTimeout("location.reload()", 3000);
                }
            }, null, false);
        },

        editSPGroups: function () {
            var result = false;
            global.methods.ajaxt(envdoc.urls.editSPGroupsUrl, {
                editspGroupsStr: encodeURIComponent(JSON.stringify(envdoc.datas.setGroupsData))
            }, null, function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    layer.close(loadingId);
                    layer.msg("保存失败！ " + response.Result);
                } else {
                    layer.closeAll();
                    layer.msg("保存成功！");
                    setTimeout("location.reload()", 3000);
                }
            }, null, false);
        },

        addSelectGroups: function (item, index) {
            var selectGroups = $("#selectGroups");
            var gspan = $("<span></span>")
            var alink = $("<a style='cursor: pointer;'></a>");
            alink.click(function () {
                $("#x" + item.Id).attr("class", "cz_itm");
                var currentParent = $(this).parent();
                currentParent.remove();
                envdoc.datas.spUserGroups.splice($.inArray(item, envdoc.datas.spUserGroups), 1);
            });
            gspan.attr("id", "n" + item.Id);
            gspan.attr("gid", item.Id);
            gspan.html(item.Name);
            gspan.attr("index", index);
            alink.appendTo(gspan.appendTo(selectGroups));
        },
        showGroupsToRoles: function () {
            var createGroupRole = $("#createGroupRolehtml");
            var GroupsRoleTable = createGroupRole.find("table");
            createGroupRole.find("table tr:not(:first)").html("");
            $.each(envdoc.datas.spUserGroups, function (i, n) {
                var row = $("<tr></tr>");
                var column1 = $("<td class=\"t_left\">" + n.Name + "</td>");
                var column2 = $("<td></td>");
                var column3 = $("<td></td>");
                var deleteItem = $("<a style='cursor: pointer;' class='cz_itm'>删 除</a>");
                var selectBox = $("<div class='select_box'></div>");
                var selectItem = $("<select class='select'><option value=\"1\">仅查看</option><option value=\"2\">读取</option> <option value=\"3\">编辑</option><option value=\"4\">完全控制</option></select>");
                if (n.GroupRole != "") {
                    selectItem.attr('value', n.GroupRole);
                }
                selectItem.change(function () {
                    var UserGroupsRoles = new envdoc.datas.UserGroupRoles();
                    UserGroupsRoles = n;
                    UserGroupsRoles.GroupRole = $(this).children('option:selected').val();
                    envdoc.datas.spUserGroups.splice($.inArray(n, envdoc.datas.spUserGroups), 1, UserGroupsRoles);
                    n = UserGroupsRoles;

                });

                selectItem.appendTo(selectBox);
                selectBox.appendTo(column2);
                deleteItem.appendTo(column3);
                column1.appendTo(row);
                column2.appendTo(row);
                column3.appendTo(row);
                row.appendTo(GroupsRoleTable);
                deleteItem.click(function () {
                    $(this).parent().parent().remove();
                    envdoc.datas.spUserGroups.splice($.inArray(n, envdoc.datas.spUserGroups), 1);

                });
            });
        },
        showCreateDocLibHtml: function () {
            var index = layer.open({
                type: 1
                , title: '一键创建文档库'
                , offset: '10px'
                , skin: 'layui-layer-rim' //加上边框
                , area: ['550px', '370px']
                , shade: [0.2, '#fff']
                , content: envdoc.cdhtmls.createDocumenthtml
                , btn: ['下一步']
                , yes: function () {
                    if (!envdoc.datas.validform.check()) {
                        return true;
                    }

                    if (!envdoc.methods.getIsExistDocumentLibrary()) {
                        return true;

                    }

                    //确认按钮
                    envdoc.methods.setDocumentLibraryParam();
                    layer.close(index);
                    envdoc.methods.showGroupRolesHtml();
                    envdoc.methods.showGroupsToRoles();
                    $("#existingGroupAdd").click(function () {
                        envdoc.methods.showExistingGroupAddHtml();
                        envdoc.methods.getUserGroups();

                        $("#btnSearchGroups").click(function () {
                            envdoc.methods.getUserGroups()
                        });
                    });
                    //doc创建新的群组
                    $("#CreateNewGroupAdd").click(function () {
                        envdoc.datas.setGroupsData = {};
                        envdoc.methods.showFaseAddGroupsLayer();

                        var currentWebUrl = $("#hidCurrentWebUrl").val();
                        var ajaxUrl = "/_layouts/15/EnvisionDoc/Handlers/AjaxHandler.ashx?MethodName=9";
                        ajaxUrl = (currentWebUrl == undefined ? "" : currentWebUrl) + ajaxUrl;

                        $("#txtAddGroupsName").attr("ajaxurl", ajaxUrl)
                        $("#txtAddGroupsName").blur(function () {
                            $("#txtAddGroupsName").attr("ajaxurl", ajaxUrl)
                        });
                        envdoc.methods.validform($("#AddGroups"), ".layui-layer-btn0");
                    });
                }
            });
        },
        //doc弹出新建窗口
        showFaseAddGroupsLayer: function () {
            var index = layer.open({
                type: 1
               , title: '新建群组'
                , skin: 'layui-layer-rim' //加上边框
               , area: ['550px', '300px']
               , offset: '20px'
               , shade: [0.2, '#fff']
               , content: envdoc.cdhtmls.addGroupsHtml
               , btn: ['下一步']
               , yes: function () {
                   if (!envdoc.datas.validform.check()) {
                       return true;
                   }
                   if (!envdoc.methods.getIsExistSPGroupsDoc()) {
                       return true;
                   }
                   //确认按钮
                   var firm = layer.confirm('确定创建此用户组？', {
                       offset: '70px',
                       skin: 'layui-layer-rim' //加上边框
                       ,btn: ['确定', '取消 '] //按钮
                   }, function () {
                       layer.close(firm);
                       //layer.close(index);txtAddGroupsName
                       envdoc.datas.setGroupsData.GroupsName = $("#txtAddGroupsName").val();
                       envdoc.datas.setGroupsData.GroupsDescription = $("#txtAddGroupsDescription").val();
                       envdoc.methods.addSPGroupsByDoc(index);

                   }, function () {

                   });
               }
            });
        },
        //doc 保存群组信息并加载 选择 群组所有者 窗口
        addSPGroupsByDoc: function (layerindex) {
            var result = false;
            global.methods.ajaxt(envdoc.urls.addSPGroupsUrl, {
                createSPGroupsattr: encodeURIComponent(JSON.stringify(envdoc.datas.setGroupsData))
            }, null, function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    layer.close(loadingId);
                    //layer.msg("创建失败");
                } else {
                    layer.close(loadingId);
                    layer.close(layerindex);
                    envdoc.datas.setGroupsData.GroupsId = response.Result.GroupsId;
                    envdoc.methods.showFaseAddGroupsOwnersLayerByDoc();
                    envdoc.methods.validform($("#AddGroupsOwners"), ".layui-layer-btn0");
                    $("#btnAddChoseGroups").click(function () {
                        var layerContent = envdoc.methods.ChoseExistingGroupAddLayerDoc();
                        envdoc.datas.addSPGroupsByDocLayerConent = layerContent;
                        envdoc.methods.ChoseExistingGroupsByDoc(layerContent,0);
                        $("#btnChoseSearchGroups").click(function () {
                            envdoc.methods.ChoseExistingGroupsByDoc(layerContent,0);
                        });
                    });

                    //layer.closeAll();
                    //layer.msg("创建成功");
                    //setTimeout("location.reload()", 3000);
                }
            }, null, false);
        },
        //doc显示群组所有者 layer
        showFaseAddGroupsOwnersLayerByDoc: function () {

            var index = layer.open({
                type: 1
               , title: '设置群组所有者'
                , skin: 'layui-layer-rim' //加上边框
               , area: ['550px', '300px']
               , offset: '20px'
               , shade: [0.2, '#fff']
               , content: envdoc.cdhtmls.addGroupOwnersHtml
               , btn: ['完成']
               , yes: function () {
                   if (!envdoc.datas.validform.check()) {
                       return true;
                   }
                   if (!envdoc.methods.getIsExistSPGroupsDoc()) {
                       return true;
                   }
                   //确认按钮
                   var firm = layer.confirm('确定设置群组所有者？', {
                       offset: '70px',
                       skin: 'layui-layer-rim' //加上边框
                       ,btn: ['确定', '取消 '] //按钮
                   }, function () {
                       layer.close(firm);
                       envdoc.methods.editSPGroupsByDoc(index);
                   }, function () {

                   });
               }
            });
        },
        //doc编辑群组 并 保存群组所有者信息
        editSPGroupsByDoc: function (layerindex) {
            var result = false;
            global.methods.ajaxt(envdoc.urls.editSPGroupsUrl, {
                editspGroupsStr: encodeURIComponent(JSON.stringify(envdoc.datas.setGroupsData))
            }, null, function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    layer.close(loadingId);
                    layer.msg("保存失败" + response.Result);
                } else {
                    var n = new envdoc.datas.UserGroupRoles();
                    n.Id = response.Result.Id;
                    n.Name = envdoc.datas.setGroupsData.GroupsName;
                    if (!envdoc.methods.isSelectedByName(envdoc.datas.spUserGroups, n)) {
                        var UserGroupsRoles = new envdoc.datas.UserGroupRoles();
                        UserGroupsRoles.Id = n.Id;
                        UserGroupsRoles.Name = n.Name;
                        UserGroupsRoles.GroupRole = "1";
                        envdoc.datas.spUserGroups.push(UserGroupsRoles);
                    }
                    envdoc.methods.showGroupsToRoles();
                    layer.close(loadingId);
                    layer.close(layerindex);
                }
            }, null, false);
        },
        //Doc根据群组name判断是否有重复选择
        isSelectedByName: function (items, item) {
            var result = false;
            $.each(items, function (i, n) {
                if (n.GroupsName == item.Name) {
                    result = true;
                    return true;
                }
            });

            return result;
        },
        //文档库中创建新群组 选择群组owners
        ChoseExistingGroupsByDoc: function (layerContent,pageindex) {
            global.methods.ajax(envdoc.urls.getUserGroups, {
                category: 2,
                pageindex: pageindex,
                keyword: encodeURIComponent($("#txtChoseGroupsName").val())
            }, null, function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    return;
                }
                var listItems = response.Result || [];
                var PageHtml = response.PageContent;

                var existingGroups = $("#existingGroupAddToGrouphtml");
                var existingGroupsTable = existingGroups.find("table");
                existingGroups.find("table tr:not(:first)").html("");
                $("#selectGroupsDoc").html("");
                $.each(envdoc.datas.spUserGroups, function (i, n) {
                    envdoc.methods.addSelectGroups(n, i);
                });
                $.each(listItems, function (i, n) {
                    var row = $("<tr></tr>");
                    var column1 = $("<td style='text-align:left;'>" + n.Name + "</td>");
                    //var column2 = $("<td></td>");
                    var column3 = $("<td></td>");
                    var link = $("<a style='cursor: pointer;' class='cz_itm' gid='" + n.Id + "'>选 择</a>");
                    link.click(function () {
                        layer.close(layerContent);
                        envdoc.datas.setGroupsData.GroupsOwnerId = n.Id;
                        envdoc.datas.setGroupsData.GroupsOwnerName = n.Name;
                        $("#txtAddGroupsOwner").val(n.Name);
                    });
                    link.appendTo(column3);
                    column1.appendTo(row);
                    //column2.appendTo(row);
                    column3.appendTo(row);
                    row.appendTo(existingGroupsTable);
                });

                var rowPage = $("<tr></tr>");
                var columnPage = $("<td colspan=\"2\" class='page_part'><div class=\"pagelist\"><div id=\"PageContent\" runat=\"server\" class=\"default\">" + PageHtml + "</div></div></td>");
                columnPage.appendTo(rowPage);
                rowPage.appendTo(existingGroupsTable);

            }, null);
        },
        showGroupRolesHtml: function () {
            var index = layer.open({
                type: 1
                , title: '权限设置'
                , skin: 'layui-layer-rim' //加上边框
                , offset: '10px'
                , area: ['600px', '400px']
                , shade: [0.2, '#fff']
                , content: envdoc.cdhtmls.createGroupRoleshtml
                , btn: ['下一步', '上一步']
                , yes: function () { //或者使用btn2
                    //确认按钮
                    if (envdoc.datas.spUserGroups.length <= 0) {
                        layer.msg("请添加群组")
                        return true;
                    }
                    layer.close(index);
                    envdoc.methods.showDocCreateLastStepHtml();
                    envdoc.methods.showDocumentLibraryParam();
                    envdoc.methods.showDocumentLibraryParamOther();
                }
                , btn2: function (index) {
                    //按钮【按钮二】的回调
                    layer.close(index);
                    envdoc.methods.showCreateDocLibHtml();
                    envdoc.methods.setCreateDocumentLibraryParam();
                    envdoc.methods.validform($("#createDocumenthtml"), ".layui-layer-btn0");

                    var currentWebUrl = $("#hidCurrentWebUrl").val();
                    var ajaxUrl = "/_layouts/15/EnvisionDoc/Handlers/AjaxHandler.ashx?MethodName=3";
                    ajaxUrl = (currentWebUrl == undefined ? "" : currentWebUrl) + ajaxUrl;

                    $("#txtDocName").attr("ajaxurl", ajaxUrl);
                    $("#txtDocName").blur(function () {
                        $("#txtDocName").attr("ajaxurl", ajaxUrl);
                    });
                }
            });
        },
        //从现有群组添加layer
        showExistingGroupAddHtml: function () {
            var index = layer.open({
                type: 1
           , title: '选择群组'
                , skin: 'layui-layer-rim' //加上边框
           , offset: '10px'
           , area: ['500px', '400px']
            , shade: [0.2, '#fff']
           , content: envdoc.cdhtmls.existingGroupAddhtml
           , btn: ['确定']
           , yes: function () {
               //确认按钮
               layer.close(index);
               envdoc.methods.showGroupsToRoles();
           }
            });
        },
        showDocCreateLastStepHtml: function () {
            var index = layer.open({
                type: 1
                , title: '创建文档库完成'
                , skin: 'layui-layer-rim' //加上边框
                 , offset: '10px'
                , area: ['600px', '400px']
                , shade: [0.2, '#fff']
                , content: envdoc.cdhtmls.docCreateLastStepHtml
                , btn: ['完成', '上一步']
                , yes: function () {
                    //确认按钮
                    var firm = layer.confirm('是否要创建此文档库？', {
                        offset: '50px',
                        skin: 'layui-layer-rim' //加上边框
                        ,btn: ['确定', '取消 '] //按钮
                    }, function () {
                        layer.close(firm);
                        envdoc.methods.setDocumentLibraryParamOther();
                        envdoc.datas.CreateDocumentParam.spuserGroups = envdoc.datas.spUserGroups;
                        envdoc.methods.addDocumentLibrary();
                    }, function () {

                    });

                }, btn2: function () {
                    //确认按钮
                    envdoc.methods.setDocumentLibraryParamOther();
                    layer.close(index);
                    envdoc.methods.showGroupRolesHtml();
                    envdoc.methods.showGroupsToRoles();
                    $("#existingGroupAdd").click(function () {
                        envdoc.methods.showExistingGroupAddHtml();
                        envdoc.methods.getUserGroups();
                        $("#btnSearchGroups").click(function () {
                            envdoc.methods.getUserGroups()
                        });
                    });
                }
            });
        },
        showFaseAddGroupsOwnersLayer: function () {

            var index = layer.open({
                type: 1
               , title: '设置群组所有者'
                , skin: 'layui-layer-rim' //加上边框
               , area: ['600px', '400px']
               , offset: '10px'
               , shade: [0.2, '#fff']
               , content: envdoc.cghtmls.addGroupOwnersHtml
               , btn: ['完成']
               , yes: function () {
                   if (!envdoc.datas.validform.check()) {
                       return true;
                   }
                   if (!envdoc.methods.getIsExistSPGroups()) {
                       return true;
                   }
                   //确认按钮
                   var firm = layer.confirm('确定设置群组所有者？', {
                       offset: '50px',
                       skin: 'layui-layer-rim' //加上边框
                       ,btn: ['确定', '取消 '] //按钮
                   }, function () {
                       layer.close(firm);
                       envdoc.methods.editSPGroups();
                   }, function () {

                   });
               }
            });
        },

        showFaseAddGroupsHtml: function () {
            var index = layer.open({
                type: 1
               , title: '新建群组'
                , skin: 'layui-layer-rim' //加上边框
               , area: ['600px', '400px']
               , offset: '10px'
               , shade: [0.2, '#fff']
               , content: envdoc.cghtmls.addGroupsHtml
               , btn: ['下一步']
               , yes: function () {
                   if (!envdoc.datas.validform.check()) {
                       return true;
                   }
                   if (!envdoc.methods.getIsExistSPGroups()) {
                       return true;
                   }
                   //确认按钮
                   var firm = layer.confirm('确定创建此用户组？', {
                       offset: '50px',
                       skin: 'layui-layer-rim' //加上边框
                       ,btn: ['确定', '取消 '] //按钮
                   }, function () {
                       layer.close(firm);
                       //layer.close(index);
                       envdoc.datas.setGroupsData.GroupsName = $("#txtGroupsName").val();
                       envdoc.datas.setGroupsData.GroupsDescription = $("#txtGroupsDescription").val();
                       envdoc.methods.addSPGroups(index);

                   }, function () {

                   });
               }
            });
        },

        ChoseExistingGroupAddHtml: function () {

            var index = layer.open({
                type: 1
           , title: '选择群组'
                , skin: 'layui-layer-rim' //加上边框
            , offset: '10px'
           , area: ['500px', '400px']
            , shade: [0.2, '#fff']
           , content: envdoc.cghtmls.exisGroupsAddhtml
            });
            return index;
        },
        ChoseExistingGroupAddLayerDoc: function () {

            var index = layer.open({
                type: 1
           , title: '选择群组所有者'
                , skin: 'layui-layer-rim' //加上边框
            , offset: '10px'
           , area: ['500px', '400px']
            , shade: [0.2, '#fff']
           , content: envdoc.cdhtmls.existingGroupAddToGrouphtml
            });
            return index;
        },
        ChoseExistingGroups: function (layerContent,pageindex) {
            global.methods.ajax(envdoc.urls.getUserGroups, {
                category:3,
                pageindex:pageindex,
                keyword: encodeURIComponent($("#txtChoseGroupsName").val())
            }, null, function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    return;
                }
                var listItems = response.Result || [];
                var PageHtml = response.PageContent;

                var existingGroups = $("#existingGroupAddhtml");
                var existingGroupsTable = existingGroups.find("table");
                existingGroups.find("table tr:not(:first)").html("");
                $("#selectGroups").html("");
                $.each(envdoc.datas.spUserGroups, function (i, n) {
                    envdoc.methods.addSelectGroups(n, i);
                });
                $.each(listItems, function (i, n) {
                    var row = $("<tr></tr>");
                    var column1 = $("<td style='text-align:left;'>" + n.Name + "</td>");
                    //var column2 = $("<td></td>");
                    var column3 = $("<td></td>");
                    var link = $("<a style='cursor: pointer;' class='cz_itm' gid='" + n.Id + "'>选 择</a>");
                    link.click(function () {
                        layer.close(layerContent);
                        envdoc.datas.setGroupsData.GroupsOwnerId = n.Id;
                        envdoc.datas.setGroupsData.GroupsOwnerName = n.Name;
                        $("#txtGroupsOwner").val(n.Name);
                    });
                    link.appendTo(column3);
                    column1.appendTo(row);
                    //column2.appendTo(row);
                    column3.appendTo(row);
                    row.appendTo(existingGroupsTable);
                });

                var rowPage = $("<tr></tr>");
                var columnPage = $("<td colspan=\"2\" class='page_part'><div class=\"pagelist\"><div id=\"PageContent\" runat=\"server\" class=\"default\">" + PageHtml + "</div></div></td>");
                columnPage.appendTo(rowPage);
                rowPage.appendTo(existingGroupsTable);

            }, null);
        },

        EditChoseExistingGroups: function (layerContent,pageindex) {
            global.methods.ajax(envdoc.urls.getUserGroups, {
                category:4,
                pageindex: pageindex,
                keyword: encodeURIComponent($("#txtChoseGroupsName").val())
            }, null, function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    return;
                }
                var listItems = response.Result || [];
                var PageHtml = response.PageContent;

                var existingGroups = $("#existingGroupAddhtml");
                var existingGroupsTable = existingGroups.find("table");
                existingGroups.find("table tr:not(:first)").html("");
                $("#selectGroups").html("");
                $.each(envdoc.datas.spUserGroups, function (i, n) {
                    envdoc.methods.addSelectGroups(n, i);
                });
                $.each(listItems, function (i, n) {
                    var row = $("<tr></tr>");
                    var column1 = $("<td style='text-align:left;'>" + n.Name + "</td>");
                    //var column2 = $("<td></td>");
                    var column3 = $("<td></td>");
                    var link = $("<a style='cursor: pointer;' class='cz_itm' gid='" + n.Id + "'>选 择</a>");
                    link.click(function () {
                        layer.close(layerContent);
                        envdoc.datas.setGroupsData.GroupsOwnerId = n.Id;
                        envdoc.datas.setGroupsData.GroupsOwnerName = n.Name;
                        $("#txtEditGroupsOwner").val(n.Name);
                    });
                    link.appendTo(column3);
                    column1.appendTo(row);
                    //column2.appendTo(row);
                    column3.appendTo(row);
                    row.appendTo(existingGroupsTable);
                });

                var rowPage = $("<tr></tr>");
                var columnPage = $("<td colspan=\"2\" class='page_part'><div class=\"pagelist\"><div id=\"PageContent\" runat=\"server\" class=\"default\">" + PageHtml + "</div></div></td>");
                columnPage.appendTo(rowPage);
                rowPage.appendTo(existingGroupsTable);

            }, null);
        },
        getGroupsInfoByGroupsName: function () {
            global.methods.ajax(envdoc.urls.getSPGroupsInfoUrl, {
                spgroupsname: encodeURIComponent($("#txtEditGroupsName").val())
            }, null, function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    return;
                }
                var listItems = response.Result || [];
                envdoc.datas.setGroupsData.GroupsOwnerName = listItems.GroupsOwnerName;
                envdoc.datas.setGroupsData.GroupsOwnerId = listItems.GroupsOwnerId;
                $("#txtEditGroupsOwner").val(listItems.GroupsOwnerName);
                $("#txtEditGroupsDescription").val(listItems.GroupsDescription);
            }, null);
        },
        showEditGroupsLayer: function (obj) {
            var index = layer.open({
                type: 1
              , title: '编辑群组'
                , skin: 'layui-layer-rim' //加上边框
                , offset: '10px'
              , area: ['600px', '400px']
              , shade: [0.2, '#fff']
              , content: envdoc.cghtmls.editGroupsHtml
              , btn: ['保存']
              , yes: function () {
                  if (!envdoc.datas.validform.check()) {
                      return true;
                  }
                  if (!envdoc.methods.getIsExistEditGroups(obj)) {
                      return true;
                  }

                  //确认按钮
                  var firm = layer.confirm('是否要保存修改此用户组？', {
                      offset: '50px',
                      skin: 'layui-layer-rim' //加上边框
                      ,btn: ['确定', '取消 '] //按钮
                  }, function () {
                      layer.close(firm);
                      envdoc.datas.setGroupsData.GroupsDescription = $("#txtEditGroupsDescription").val();
                      envdoc.datas.setGroupsData.GroupsName = $("#txtEditGroupsName").val();
                      envdoc.methods.editSPGroups();
                  }, function () {

                  });
              }
            });
        },
        //一件创建群组“编辑”
        editGroups: function (obj) {
            envdoc.methods.showEditGroupsLayer(obj);
            //$("#txtEditGroupsName").attr("readonly", "readonly");
            var gname = $(obj).attr("gname");
            var pid = $(obj).attr("pid");
            $("#txtEditGroupsName").val(gname);
            envdoc.datas.setGroupsData.GroupsName = gname;
            envdoc.datas.setGroupsData.GroupsId = pid;
            envdoc.methods.getGroupsInfoByGroupsName();
            envdoc.methods.validform($("#EditGroups"), ".layui-layer-btn0");

            var currentWebUrl = $("#hidCurrentWebUrl").val();
            var ajaxUrl = "/_layouts/15/EnvisionDoc/Handlers/AjaxHandler.ashx?MethodName=17&beforeGroupsName=" + gname;
            ajaxUrl = (currentWebUrl == undefined ? "" : currentWebUrl) + ajaxUrl;

            $("#txtEditGroupsName").attr("ajaxurl", ajaxUrl);
            //检查文档名是否已经存在
            $("#txtEditGroupsName").blur(function () {
                $("#txtEditGroupsName").attr("ajaxurl", ajaxUrl);
            });

            $("#btnEditChoseGroups").click(function () {
                var layerContent = envdoc.methods.ChoseExistingGroupAddHtml();
                envdoc.datas.EditChoseExistingGroupsLayerContent = layerContent
                envdoc.methods.EditChoseExistingGroups(layerContent,0);
                $("#btnChoseSearchGroups").click(function () {
                    envdoc.methods.EditChoseExistingGroups(layerContent,0);
                });
            });
        },
        editDocumentLibraryLayer: function (obj) {
            var index = layer.open({
                type: 1
                , offset: '10px'
              , title: '编辑文档库'
                , skin: 'layui-layer-rim' //加上边框
              , area: ['600px', '400px']
              , shade: [0.2, '#fff']
              , content: envdoc.cdhtmls.editDocumentLibraryHtml
              , btn: ['保存']
              , yes: function () {
                  if (!envdoc.datas.validform.check()) {
                      return true;
                  }
                  if (!envdoc.methods.getIsExistEditDocumentLibrary(obj)) {
                      return true;
                  }
                  //确认按钮
                  var firm = layer.confirm('确定要保存修改此文档库？', {
                      offset: '70px',
                      skin: 'layui-layer-rim' //加上边框
                      ,btn: ['确定', '取消 '] //按钮
                  }, function () {
                      layer.close(firm);
                      envdoc.methods.setEditDocumentLibraryParam(obj);
                      envdoc.methods.editSaveDocumentLibrary();
                  }, function () {

                  });
              }
            });
        },
        editSaveDocumentLibrary: function () {
            var result = false;
            global.methods.ajaxt(envdoc.urls.editDocumentLibraryUrl, {
                editDocumentattr: encodeURIComponent(JSON.stringify(envdoc.datas.CreateDocumentParam))
            }, null, function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    layer.close(loadingId);
                    layer.msg("保存失败");
                } else {
                    layer.closeAll();
                    layer.msg("保存成功");
                    setTimeout("location.reload()", 3000);
                }
            }, null, false);
        },
        getDocumentLibraryInfo: function (obj) {
            var dname = $(obj).attr("dname");
            var did = $(obj).attr("id");
            $("#txtEditDoclibName").val(dname);

            var currentWebUrl = $("#hidCurrentWebUrl").val();
            var ajaxUrl = "/_layouts/15/EnvisionDoc/Handlers/AjaxHandler.ashx?MethodName=10&beforeDocLibName=" + dname;
            ajaxUrl = (currentWebUrl == undefined ? "" : currentWebUrl) + ajaxUrl;

            $("#txtEditDoclibName").attr("ajaxurl", ajaxUrl);
            //检查文档名是否已经存在
            $("#txtEditDoclibName").blur(function () {
                $("#txtEditDoclibName").attr("ajaxurl", ajaxUrl);
            });

            global.methods.ajax(envdoc.urls.getDocumentLibraryUrl, {
                doclibName: encodeURIComponent(did)
            }, null, function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    return;
                }
                var listItems = response.Result || [];
                envdoc.datas.CreateDocumentParam = listItems;
                if (envdoc.datas.CreateDocumentParam.DocumentVersionControlId != "") {
                    $('select#ddlEditVersionControl').attr('value', envdoc.datas.CreateDocumentParam.DocumentVersionControlId);
                }
                if (envdoc.datas.CreateDocumentParam.DocumentDefaultCheckOutId != "") {
                    $("#rblEditCheckOut input[value='" + envdoc.datas.CreateDocumentParam.DocumentDefaultCheckOutId + "']").attr("checked", "checked");
                }
            }, null);
        },
        editDocumentLibrary: function (obj) {
            envdoc.methods.editDocumentLibraryLayer(obj);
            envdoc.methods.validform($("#editDocumentLibrary"), ".layui-layer-btn0");
            envdoc.methods.getDocumentLibraryInfo(obj);
        },
        //document role settings begin
        treeSettingInit: function () {
            envdoc.config.setting.async.autoParam = ["id", "listId"],
            envdoc.datas.currentWebUrl = $("#hidCurrentWebUrl").val();
            envdoc.datas.currentWebUrl = envdoc.datas.currentWebUrl == undefined ? "" : envdoc.datas.currentWebUrl;
            envdoc.config.setting.async.url = envdoc.datas.currentWebUrl + "/_layouts/15/EnvisionDoc/Handlers/AjaxHandler.ashx";
            envdoc.config.setting.async.otherParam = { "MethodName": envdoc.urls.getTreeChildNodesUrl };
            envdoc.config.setting.async.dataFilter = filter;
            envdoc.config.setting.callback.onClick = zOnClick;
            envdoc.config.setting.callback.beforeAsync = zbeforeAsync;
            envdoc.config.setting.callback.asyncSuccess = zTreeOnAsyncSuccess;
            envdoc.config.setting.callback.asyncError = zTreeOnAsyncError;
            envdoc.config.setting.callback.beforeClick = beforeClick;
            function filter(treeId, parentNode, childNodes) {
                if (!childNodes) return null;
                childNodes = childNodes.Result;
                for (var i = 0, l = childNodes.length; i < l; i++) {
                    childNodes[i].name = childNodes[i].name;
                }
                return childNodes;
            };
            function beforeClick(treeId, treeNode) {

                if (!treeNode.isParent) {
                    alert("请选择父节点");
                    return false;
                } else {
                    return true;
                }
            };
            function zbeforeAsync(treeId, treeNode) {
                //envdoc.config.setting.async.otherParam = { "MethodName": envdoc.urls.getTreeChildNodesUrl, "listId": treeNode.listId };
                //alert(treeNode.listId);
            }
            function zTreeOnAsyncError(event, treeId, treeNode) {
                alert("异步加载失败!");
            };
            function zTreeOnAsyncSuccess(event, treeId, treeNode, msg) {

            };
            function zOnClick(event, treeId, treeNode, clickFlag) {
                var url = treeNode.currentWebUrl + "/_layouts/User.aspx?List=" + treeNode.listId;
                if (treeNode.listItemId != "") {
                    url += ("&obj=" + treeNode.listId + "," + treeNode.listItemId + ",LISTITEM");
                }
                window.open(url);
            }
        },
        treeload: function () {
            global.methods.ajax(envdoc.urls.getTreeNodesUrl, {
            }, null, function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    return;
                }
                var listItems = response.Result || [];
                if (response.Result.length > 0) {
                    envdoc.datas.zNodes = listItems;

                    envdoc.methods.treeSettingInit();
                    envdoc.doms.zTreeObj = $.fn.zTree.init($("#treeDemo"), envdoc.config.setting, envdoc.datas.zNodes);
                }
            }, null);
        },
        //document role settings end
        //it setting begin
        itTreeSettingInit: function () {
            envdoc.config.setting.async.autoParam = ["id", "siteId", "webId", "listId", "IdType"],
            envdoc.config.setting.async.otherParam = { "MethodName": envdoc.urls.getItTreeChildNodesUrl };
            envdoc.config.setting.async.dataFilter = filter;
            envdoc.config.setting.callback.onClick = zOnClick;
            envdoc.config.setting.callback.beforeAsync = zbeforeAsync;
            envdoc.config.setting.callback.asyncSuccess = zTreeOnAsyncSuccess;
            envdoc.config.setting.callback.asyncError = zTreeOnAsyncError;
            envdoc.config.setting.callback.beforeClick = beforeClick;
            function filter(treeId, parentNode, childNodes) {
                if (!childNodes) return null;
                childNodes = childNodes.Result;
                for (var i = 0, l = childNodes.length; i < l; i++) {
                    childNodes[i].name = childNodes[i].name;
                }
                return childNodes;
            };
            function beforeClick(treeId, treeNode) {

                if (!treeNode.isParent) {
                    alert("请选择父节点");
                    return false;
                } else {
                    return true;
                }
            };
            function zbeforeAsync(treeId, treeNode) {

            }
            function zTreeOnAsyncError(event, treeId, treeNode) {
                alert("异步加载失败!");
            };
            function zTreeOnAsyncSuccess(event, treeId, treeNode, msg) {

            };
            function zOnClick(event, treeId, treeNode, clickFlag) {

                if ($(event.srcElement).attr("id") != "treeDemo_2_span") {
                    $("#treeDemo_2_a").attr("class", "level1");
                }
                var url = "";
                if (treeNode.IdType == 5) {
                    url = "EnvisionDoc/Pages/ITMonitor/ITSiteTypeView.aspx?siteId=" + treeNode.siteId + "&webId=" + treeNode.webId;
                }
                else if (treeNode.IdType == 0) {
                    url = "EnvisionDoc/Pages/ITMonitor/ITLibTypeView.aspx?siteId=" + treeNode.siteId + "&webId=" + treeNode.webId + "&listId=" + treeNode.listId + "&listName=" + encodeURIComponent(treeNode.name);
                }
                $("#mainframe").attr("src", "");
                if (url != "") {
                    envdoc.doms.layerload = layer.load(1);
                    $("#mainframe").attr("src", url);
                }
            }
        },
        closeLayerLoad: function () {
            layer.close(envdoc.doms.layerload);
        },
        itTreeLoad: function () {
            global.methods.ajax(envdoc.urls.getItTreeNodesUrl, {
            }, null, function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    return;
                }
                var listItems = response.Result || [];
                envdoc.datas.itzNodes = listItems;
                envdoc.methods.itTreeSettingInit();
                envdoc.doms.zTreeObj = $.fn.zTree.init($("#treeDemo"), envdoc.config.setting, envdoc.datas.itzNodes);
                if (listItems.length > 1) {
                    var firstItem = listItems[1];
                    $("#treeDemo_2_a").attr("class", "level1 curSelectedNode");
                    var url = "";
                    if (firstItem.IdType == 5) {
                        url = "EnvisionDoc/Pages/ITMonitor/ITSiteTypeView.aspx?siteId=" + firstItem.siteId + "&webId=" + firstItem.webId;
                    }
                    else if (firstItem.IdType == 0) {
                        url = "EnvisionDoc/Pages/ITMonitor/ITLibTypeView.aspx?siteId=" + firstItem.siteId + "&webId=" + firstItem.webId + "&listId=" + firstItem.listId + "&listName=" + encodeURIComponent(firstItem.name);
                    }
                    $("#mainframe").attr("src", "");
                    if (url != "") {
                        envdoc.doms.layerload = layer.load(1);
                        $("#mainframe").attr("src", url);
                    }
                }

            }, null);
        },
        //it setting end
        instance: function () {
            envdoc.methods.getLeftMenus();


        },
        cdinit: function () {
            envdoc.cdhtmls.init();
            $("#createDocLib").click(function () {
                envdoc.methods.initDocumentParam();
                envdoc.methods.showCreateDocLibHtml();
                envdoc.methods.validform($("#createDocumenthtml"), ".layui-layer-btn0");

                var currentWebUrl = $("#hidCurrentWebUrl").val();
                var ajaxUrl = "/_layouts/15/EnvisionDoc/Handlers/AjaxHandler.ashx?MethodName=3";
                ajaxUrl = (currentWebUrl == undefined ? "" : currentWebUrl) + ajaxUrl;

                $("#txtDocName").attr("ajaxurl", ajaxUrl);
                //检查文档名是否已经存在
                $("#txtDocName").blur(function () {
                    //console.log(envdoc.methods.getIsExistDocumentLibrary());
                    $("#txtDocName").attr("ajaxurl", ajaxUrl);
                });
            });
        },
        cginit: function () {
            envdoc.cghtmls.init();
            $("#FaseAddGroups").click(function () {
                envdoc.datas.setGroupsData = {};
                envdoc.methods.showFaseAddGroupsHtml();

                var currentWebUrl = $("#hidCurrentWebUrl").val();
                var ajaxUrl = "/_layouts/15/EnvisionDoc/Handlers/AjaxHandler.ashx?MethodName=9";
                ajaxUrl = (currentWebUrl == undefined ? "" : currentWebUrl) + ajaxUrl;

                $("#txtGroupsName").attr("ajaxurl", ajaxUrl)
                $("#txtGroupsName").blur(function () {
                    $("#txtGroupsName").attr("ajaxurl", ajaxUrl)
                });
                envdoc.methods.validform($("#AddGroups"), ".layui-layer-btn0");
            });
        },
        treeinit: function () {
            this.treeload();
        },
        itTreeInit: function () {
            this.itTreeLoad();
        }
    }
};


