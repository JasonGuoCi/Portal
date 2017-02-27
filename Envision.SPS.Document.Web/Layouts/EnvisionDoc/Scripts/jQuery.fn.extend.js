charset = "UTF-8";


// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (o.hasOwnProperty(k)) if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length === 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

$.event.special.valuechange = {
    teardown: function (namespaces) {
        $(this).unbind(".valuechange");
    },
    handler: function (e) {
        $.event.special.valuechange.triggerChanged($(this));
    },
    add: function (obj) {
        $(this).on("keyup.valuechange cut.valuechange paste.valuechange input.valuechange", obj.selector, $.event.special.valuechange.handler);
    },
    triggerChanged: function (element) {
        var current = element[0].contentEditable === "true" ? element.html() : element.val()
            , previous = typeof element.data("previous") === "undefined" ? element[0].defaultValue : element.data('previous');
        if (current !== previous) {
            element.trigger("valuechange", [element.data("previous")]);
            element.data("previous", current);
        }
    }
}
; (function ($, window, document) {
    $.dateFormat = function (date, format) {
        var array = {
            "M+": date.getMonth() + 1,
            "d+": date.getDate(),
            "h+": date.getHours(),
            "m+": date.getMinutes(),
            "s+": date.getSeconds(),
            "q+": Math.floor((date.getMonth() + 3) / 3),
            "S": date.getMilliseconds()
        };
        if (/(y+)/.test(format)) {
            format = format.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
        }
        for (var key in array) {
            if (array.hasOwnProperty(key)) {
                var value = array[key];
                if (new RegExp("(" + key + ")").test(format)) {
                    format = format.replace(RegExp.$1, RegExp.$1.length === 1 ? value : ("00" + value).substr(("" + value).length));
                }
            }
        }
        return format;
    }
    $.getUniqueId = function () {
        return $.dateFormat(new Date(), "yyyyMMddhhmmss");
    }
    $.getQueryString = function (name, url) {
        if (!url) {
            url = window.location.search;
        }
        var index = url.indexOf("?");
        if (index !== -1) {
            url = url.substr(index + 1);
        }
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = url.match(reg);
        return r != null && r.length === 4 ? r[2] : null;
    }
    $.removeFromArrary = function (arrary, element) {
        for (var i = 0, len = arrary.length; i < len; i++) {
            if (arrary[i] === element) {
                arrary.splice(i, 1);
                break;
            }
        }
    },
    $.replaceAll = function (strSource, strTarget, strReplace) {
        strSource = strSource || "";
        var reg = new RegExp(strTarget, "i");
        return strSource.replace(reg, strReplace);
    }
    $.download = function (url, data, method) {
        if (url && data) {
            var html = "";
            for (var item in data) {
                if (data.hasOwnProperty(item)) {
                    html += "<input type=\"hidden\" name=\"" + item + "\" value=\"" + data[item] + "\" />";
                }
            }
            $("<form action=\"" + url + "\" method=\"" + (method || "post") + "\">" + html + "</form>")
            .appendTo("body").submit().remove();
        };
    };
    $.selectAll = function (selectedAllSelector, selectItemSelector, callback) {
        var $selectAll = $(selectedAllSelector);
        $selectAll.unbind("click").click(function () {
            $(this).toggleClass("checked");
            if ($(this).hasClass("checked")) {
                $(selectItemSelector).removeClass("checked").addClass("checked");
            } else {
                $(selectItemSelector).removeClass("checked");
            }
            callback($(selectItemSelector));
        });
        $(selectItemSelector).die().live("click", function () {
            $(this).toggleClass("checked");
            if ($(this).hasClass("checked")) {
                if ($(selectItemSelector).length === $(selectItemSelector).filter(".checked").length) {
                    $selectAll.removeClass("checked").addClass("checked");
                } else {
                    $selectAll.removeClass("checked");
                }
            } else {
                $selectAll.removeClass("checked");
            }
            callback($(this));
        });
    }
    $.fn.outerHTML = function () {
        return $(this).wrap("<div></div>").parent().html();
    }
    $.fn.placeHolder = function () {
        return this.each(function () {
            var $input = $(this);
            var defaultValue = $input.attr("placeHolder");
            if ($.trim($input.val()) === "") {
                $input.val(defaultValue);
            }
            $input.focusin(function () {
                if ($(this).val() === defaultValue) {
                    $(this).val("");
                }
            })
            .focusout(function () {
                if ($(this).val() === "") {
                    $(this).val(defaultValue);
                }
            });
        });
    }
    $.fn.contextMenu = function (options) {
        var defaults = {
            eventPosX: "clientX",
            eventPosY: "clientY",
            onContextMenu: null,
            onShowMenu: null,
            itemSelector: "li",
            itemClick: null,
            itemParam: {}
        };
        var opts = $.extend(defaults, options);
        return this.each(function () {
            $(this).bind("contextmenu", function (e) {
                e = window.event ? window.event : e;
                $(".contextMenuBox").hide();
                var bShowContext = (!!opts.onContextMenu) ? opts.onContextMenu(e, opts.itemParam) : true;
                if (bShowContext) {
                    var $menu = $(opts.selector);
                    if (!!opts.onShowMenu) {
                        $menu = opts.onShowMenu(e, $menu, opts.itemParam);
                    }
                    var left = e[opts.eventPosX];
                    var top = e[opts.eventPosY] - 64;
                    var maxWidth = $(document).width();
                    var maxHeight = $(document).height();
                    if (left + $menu.outerWidth() > maxWidth) {
                        left = left - $menu.outerWidth();
                    }
                    if (top + $menu.outerHeight() > maxHeight) {
                        top = top - $menu.outerHeight();
                    }
                    $menu.css({ left: left, top: top }).show();
                    var type = e.type;
                    $(document).one("click", function () {
                        $(".contextMenuBox").hide();
                        if (type === "click") {
                            $menu.show();
                            $(document).one("click", function () {
                                $(".contextMenuBox").hide();
                            });
                        }
                    });
                    var $itemList = $menu.find(opts.itemSelector);
                    for (var i = 0, len = $itemList.length; i < len ; i++) {
                        $($itemList[i]).unbind("click").click(function () {
                            if (opts.itemClick) {
                                opts.itemClick($(this), opts.itemParam);
                            }
                        });
                    }
                }
                return false;
            });
        });
    }
    $.fn.autoComplete = function (callback) {
        return this.each(function () {
            var isChange = true;
            function show($input) {
                if ($input.next().find("li").length !== 0) {
                    $input.next().show();
                } else {
                    $input.next().hide();
                }
            }
            function setValues($input) {
                $input.next().empty();
                if (!callback) {
                    return;
                }
                var values = callback();
                for (var i = 0, len = values.length ; i < len ; i++) {
                    $("<li></li>")
                        .html(values[i])
                        .unbind("click").click(function () {
                            $input.val($(this).text());
                            $input.next().hide();
                        })
                        .appendTo($input.next());
                }
            }
            $(this).on("valuechange", function (e, previous) {
                if (!isChange) {
                    return;
                }
                var $input = $(this);
                setValues($input);
                show($input);
            });
            $(this).hover(function () {
                isChange = true;
                setValues($(this));
            }, function () { });
            $(this).unbind("click").click(function () {
                isChange = true;
                setValues($(this));
            }, function () { });
            $(this).keydown(function (event) {
                if (event.keyCode !== 38 && event.keyCode !== 40) {
                    isChange = true;
                    return;
                }
                isChange = false;
                var index = $(this).next().find("li").index($(this).next().find(".selected"));
                switch (event.keyCode) {
                    case 38:
                        index = index - 1;
                        break;
                    case 40:
                        index = index + 1;
                        break;
                }
                if (index < 0 || index > $(this).next().find("li").length - 1) {
                    index = 0;
                }
                $(this).next().find("li:eq(" + index + ")").siblings().removeClass("selected");
                $(this).next().find("li:eq(" + index + ")").addClass("selected");
                $(this).val($(this).next().find("li:eq(" + index + ")").text());
            });
            $(this).parent().hover(function () { }, function () {
                $(this).find("ul").hide();
            });
        });
    }
    $.fn.dataGrid = function (options, callback) {
        var dataGrid = $(this);
        var defaults = {
            delay: 1,
            pageSize: 15,
            pageIndex: 1,
            sortName: "",
            sortType: ""
        };
        var opts = $.extend(defaults, options);
        dataGrid.sortName = opts.sortName;
        dataGrid.sortType = opts.sortType;
        dataGrid.pageSize = opts.pageSize;
        dataGrid.pageIndex = opts.pageIndex;
        dataGrid.action = function () {
            $(this).find(".dataGridSort")
                .unbind("click").click(function () {
                    opts.sortName = $(this).attr("sortname");
                    switch (opts.sortType) {
                        case "ASC":
                            $(this).attr("sorttype", "DESC");
                            break;
                        case "DESC":
                            $(this).attr("sorttype", "ASC");
                            break;
                    }
                    opts.sortType = $(this).attr("sorttype");
                    opts.pageIndex = 1;
                    var loadingId = $.layer.loading({
                        dom: dataGrid,
                        msg: "正在处理..."
                    });
                    dataGrid.init();
                    dataGrid.callback();
                    $.layer.close(loadingId);
                });
            dataGrid.moreInfo.unbind("click").click(function () {
                opts.pageIndex += 1;
                dataGrid.callback();
            });
        }
        dataGrid.init = function () {
            var dataGridSortLinks = $(this).find(".dataGridSort");
            for (var i = 0, len = dataGridSortLinks.length ; i < len ; i++) {
                var $dataGridSortLink = dataGridSortLinks.eq(i);
                var sortName = $dataGridSortLink.attr("sortname");
                if (sortName === opts.sortName) {
                    $dataGridSortLink.find("i").attr({ "class": "icon" });
                    switch (opts.sortType) {
                        case "ASC":
                            $dataGridSortLink.find("i").addClass("iconSortDesc");
                            break;
                        case "DESC":
                            $dataGridSortLink.find("i").addClass("iconSortAsc");
                            break;
                    }
                } else {
                    $dataGridSortLink.find("i").attr({ "class": "icon iconSort" });
                }
            }
        }
        dataGrid.callback = function () {
            dataGrid.sortName = opts.sortName;
            dataGrid.sortType = opts.sortType;
            dataGrid.pageSize = opts.pageSize;
            dataGrid.pageIndex = opts.pageIndex;
            if (callback) {
                callback(opts);
            }
        }
        dataGrid.clear = function () {
            dataGrid.find(".dataGridBody tbody").empty();
        }
        dataGrid.moreInfo = $(this).find(".moreInfo a");
        dataGrid.init();
        dataGrid.callback();
        dataGrid.action();
        return dataGrid;
    }
    $.fn.tree = function (options) {
        var tree = $(this).find(".treeBox").empty();
        var defaults = {
            nodes: [],
            seed: 0,
            increment: 16,
            open: false,
            isAddNode: false
        };
        var opts = $.extend(defaults, options);
        tree.hasChildren = function (pId, nodes) {
            for (var i = 0, len = nodes.length; i < len; i++) {
                var node = nodes[i];
                if (pId && node.ParentId && node.ParentId === pId) {
                    return true;
                }
            }
            return false;
        }
        tree.getChildrenNodes = function (pId, nodes) {
            var childrenNodes = [];
            for (var i = 0, len = nodes.length; i < len; i++) {
                var node = nodes[i];
                if ((!pId && !node.ParentId) || (pId && node.ParentId && node.ParentId === pId)) {
                    childrenNodes.push(node);
                }
            }
            return childrenNodes;
        }
        tree.load = function () {
            tree.addChildNodes(null, opts.nodes);
        }
        tree.refreshChildNodes = function (pId, nodes) {
            var $pNode = tree.getNode(pId);
            $pNode.find("ul:first").remove();
            tree.addChildNodes(pId, nodes);
            if (this.hasChildren(pId, nodes) && opts.open) {
                if (opts.open) {
                    $pNode.removeClass("collapsed").addClass("expanded");
                } else {
                    $pNode.removeClass("expanded").addClass("collapsed");
                }
            }
        }
        tree.addChildNodes = function (pId, nodes) {
            var childrenNodes = tree.getChildrenNodes(pId, nodes);
            for (var i = 0, len = childrenNodes.length; i < len; i++) {
                var node = childrenNodes[i];
                var hasChildren = this.hasChildren(node.Id, nodes);
                tree.appendNode(pId, node, hasChildren);
                if (hasChildren) {
                    tree.addChildNodes(node.Id, nodes);
                }
                if (pId && i === 0) {
                    var $pNode = tree.getNode(pId);
                    if (!$pNode.hasClass("expanded") && opts.open && hasChildren) {
                        $pNode.addClass("expanded");
                    }
                }
            }
        }
        tree.appendNode = function (pId, node, hasChildren) {
            var $node = $("<li></li>")
                .addClass("treeNode")
                .attr("id", node.Id);
            if (opts.open === true && hasChildren) {
                $node.addClass("expanded");
            } else if (hasChildren) {
                $node.addClass("collapsed");
            }
            var $nodeContents = $("<div></div>")
                .addClass("nodeContents")
                .addClass("ellipsis")
                .appendTo($node);
            var $nodeSwitch = $("<span></span>")
                .addClass("nodeSwitch")
                .html("<i class=\"icon\"></i>")
                .appendTo($nodeContents);
            $nodeSwitch.unbind("click").click(function () {
                tree.click($(this).parent().parent(), node);
            });
            $nodeContents.unbind("dblclick").dblclick(function () {
                tree.click($(this).parent(), node);
            });
            if (node.ImageUrl) {
                $("<img/>").addClass("nodeImage")
                    .attr({
                        "src": node.ImageUrl
                    })
                    .appendTo($nodeContents);
            } else {
                $("<span></span>")
                    .addClass("nodeIcon")
                    .html("<i class=\"icon\"></i>")
                    .appendTo($nodeContents);
            }
            var title = $.trim(node.Description) !== "" ? node.Description : node.Title;
            $("<a></a>").attr({
                "href": "javascript:void(0);",
                "title": title
            })
                .addClass("nodeTitle")
                .html(node.Title)
                .appendTo($nodeContents);
            if (!pId) {
                $nodeContents.css({ "padding-left": opts.seed });
                if (opts.isAddNode) {
                    var $lastNode = tree.find(">li:last");
                    if ($lastNode.length !== 0)
                        $lastNode.before($node);
                    else
                        tree.append($node);
                }
                else
                    tree.append($node);
            } else {
                var $pNode = this.getNode(pId);
                $nodeContents.css({ "padding-left": parseInt($pNode.find(".nodeContents:first").css("padding-left")) + opts.increment });
                if ($pNode.find("ul:first").length === 0) {
                    $pNode.append($("<ul></ul>"));
                }
                $pNode.find("ul:first").append($node);
            }
            if ($.isFunction(opts.itemBind)) {
                opts.itemBind($node, node);
            }
        }
        tree.isParentNode = function (id) {
            return tree.getNode(id).find("li").length !== 0;
        }
        tree.click = function ($node, node) {
            if ($.isFunction(opts.click)) {
                opts.click($node, node);
                return;
            }
            tree.change($node);
        }
        tree.change = function ($node) {
            if ($node.hasClass("collapsed")) {//展开
                var $childNodes = $("#" + $node.attr("id") + ">ul>li");
                for (var i = 0, len = $childNodes.length; i < len; i++) {
                    var $childNode = $childNodes.eq(i);
                    if (tree.isParentNode($childNode.attr("id"))) {
                        $childNode.removeClass("expanded").addClass("collapsed");
                    }
                }
                $node.removeClass("collapsed").addClass("expanded");
            } else if ($node.hasClass("expanded")) {//折叠
                $node.removeClass("expanded").addClass("collapsed");
            }
        }
        tree.selectedNode = function (id) {
            tree.find(".nodeContents").removeClass("selected");
            var $node = tree.getNode(id);
            $node.find(">.nodeContents").addClass("selected");
            var $curNode = $node;
            for (var i = 0; ; i++) {
                $curNode = $curNode.parent().parent();
                if (!$curNode.hasClass("treeNode")) {
                    break;
                }
                $curNode.removeClass("collapsed").addClass("expanded");
            }
        }
        tree.addNode = function (pId, node) {
            opts.isAddNode = true;
            if (pId) {
                tree.getNode(pId).removeClass("collapsed").addClass("expanded");
            }
            tree.appendNode(pId, node, false);
        }
        tree.removeNode = function (id) {
            var $node = tree.getNode(id);
            var $pNode = $node.parent().parent();
            $node.remove();
            if (!$pNode.hasClass("treeNode")) {
                return;
            }
            if ($pNode.find(".treeNode").length === 0) {
                $pNode.removeClass("collapsed").removeClass("expanded");
            } else {
                $pNode.removeClass("collapsed").addClass("expanded");
            }
        }
        tree.editNode = function (node) {
            var $node = tree.getNode(node.Id);
            var title = $.trim(node.Description) !== "" ? node.Description : node.Title;
            $node.find(">.nodeContents>.nodeTitle")
                .attr({
                    "title": title
                })
                .html(node.Title);
        }
        tree.getNode = function (id) {
            return tree.find("#" + id);
        }
        tree.getSelectNode = function () {
            return tree.find(".selected").parent();
        }
        tree.load();
        return tree;
    }
    $.layer = {
        loading: function (options) {
            var defaults = {
                msg: "",
                hasHeader: false,
                hasFooter: false,
                drag: false
            }
            var opts = $.extend(defaults, options);
            var html = "<div class=\"layerLoading\">";
            html += "<i class=\"icon\"></i>";
            html += opts.msg;
            html += "</div>";
            opts.html = html;
            return this.dialog(defaults);
        },
        alert: function (msg, ok, dom) {
            return $.layer.dialog({
                title: "提示框",
                showOk: true,
                ok: ok,
                drag: false,
                dom: dom,
                html: "<div class=\"layer-alert\">" + msg + "</div>"
            });
        },
        confirm: function (msg, ok, cancel, dom) {
            return $.layer.dialog({
                title: "确认框",
                showOk: true,
                ok: ok,
                showCancel: true,
                cancel: cancel,
                drag: false,
                dom: dom,
                html: "<div class=\"layer-confirm\">" + msg + "</div>"
            });
        },
        dialog: function (options) {
            var defaults = {
                id: $.getUniqueId(),
                zIndex: 20141016,
                dom: document,
                isDocument: true,
                title: "",
                html: "",
                height: null,
                width: null,
                hasHeader: true,
                hasFooter: true,
                showSubmit: false,
                submit: null,
                showOk: false,
                ok: null,
                showSave: false,
                save: null,
                showCancel: false,
                cancel: null,
                showClose: false,
                close: null,
                drag: true
            }
            var opts = $.extend(defaults, options);
            opts.isDocument = !options.dom;
            opts.zIndex = $.layer.getZIndex(defaults.zIndex);
            var $dom = $(opts.dom);
            function drag($layerContents) {
                if (!opts.hasHeader && !opts.hasFooter) { return; }
                $(document).mousemove(function (e) {
                    if (!!this.move) {
                        var posix = !document.move_target ? { 'x': 0, 'y': 0 } : document.move_target.posix,
                            callback = document.call_down || function () {
                                $(this.move_target).css({
                                    'top': e.pageY - posix.y,
                                    'left': e.pageX - posix.x
                                });
                            };
                        callback.call(this, e, posix);
                    }
                }).mouseup(function (e) {
                    if (!!this.move) {
                        var callback = document.call_up || function () { };
                        callback.call(this, e);
                        $.extend(this, {
                            'move': false,
                            'move_target': null,
                            'call_down': false,
                            'call_up': false
                        });
                    }
                });
                if (opts.hasHeader) {
                    $layerContents.find(".layerHeader").mousedown(function (e) {
                        var offset = $layerContents.offset();
                        $layerContents.posix = { 'x': e.pageX - offset.left, 'y': e.pageY - offset.top };
                        $.extend(document, {
                            'move': true, 'move_target': $layerContents, 'call_down': function (e, posix) {
                                var top = e.pageY - posix.y;
                                var left = e.pageX - posix.x;
                                var minWidth = 0, minHeight = 0;
                                var maxWidth = $(window).height() - $layerContents.height();
                                var maxHeight = $(window).width() - $layerContents.width();
                                top = top < minWidth ? minWidth : top;
                                top = top > maxWidth ? maxWidth : top;
                                left = left < minHeight ? minHeight : left;
                                left = left > maxHeight ? maxHeight : left;
                                $layerContents.css({
                                    'top': top,
                                    'left': left
                                });
                            }
                        });
                    });
                }
                if (opts.hasFooter) {
                    var minWidth = $layerContents.width();
                    var minHeight = $layerContents.height();
                    $layerContents.on('mousedown', '.layerResizable', function (e) {
                        var posix = {
                            'w': $layerContents.width(),
                            'h': $layerContents.height(),
                            'x': e.pageX,
                            'y': e.pageY
                        };
                        $.extend(document, {
                            'move': true, 'call_down': function (e) {
                                var offset = $layerContents.offset();
                                var width = e.pageX - posix.x + posix.w;
                                var height = e.pageY - posix.y + posix.h;
                                var maxWidth = $(window).width() - offset.left;
                                var maxHeight = $(window).height() - offset.top;
                                width = width < minWidth ? minWidth : width;
                                width = width > maxWidth ? maxWidth : width;
                                height = height < minHeight ? minHeight : height;
                                height = height > maxHeight ? maxHeight : height;
                                $layerContents.css({
                                    'width': width,
                                    'height': height
                                });
                                $layerContents.find(".layerBody").css({
                                    'width': width - 2 - 20,
                                    'height': height - 32 - 20 - 54
                                });
                                if ($.isFunction(opts.dragCallback)) {
                                    opts.dragCallback($layerContents, width - 2 - 20, height - 32 - 20 - 54);
                                }
                            }
                        });
                        return false;
                    });
                }
            }
            function createLayerContnets() {
                var $layerContents = $("<div></div>")
                    .attr({ id: "layerContents" + opts.id })
                    .addClass("layerContents")
                    .hide();
                if (opts.hasHeader) {
                    var $layerHeader = $("<div></div>")
                        .addClass("layerHeader")
                        .appendTo($layerContents);
                    $("<div></div>")
                        .addClass("layerTitle")
                        .html(opts.title)
                        .appendTo($layerHeader);
                    $("<div></div>")
                        .addClass("layerClose")
                        .attr({ title: "关闭对话框" })
                        .html("X")
                        .unbind("click").click(function () {
                            if ($.isFunction(opts.cancel)) {
                                opts.cancel($layerContents);
                                return;
                            }
                            $.layer.close(opts.id);
                        })
                        .appendTo($layerHeader);
                }
                $("<div></div>")
                    .addClass("layerBody")
                    .html(opts.html)
                    .appendTo($layerContents);
                if (opts.hasFooter) {
                    var $layerFooter = $("<div></div>")
                        .addClass("layerFooter")
                        .appendTo($layerContents);
                    var $layerButtons = $("<div></div>")
                        .addClass("layerButtons")
                        .appendTo($layerFooter);
                    if (opts.showSubmit) {
                        $("<span></span>")
                        .addClass("layerButton")
                        .html("提交")
                        .unbind("click").click(function () {
                            if ($.isFunction(opts.submit)) {
                                opts.submit($layerContents);
                                return;
                            }
                            $.layer.close(opts.id);
                        })
                        .appendTo($layerButtons);
                    }
                    if (opts.showOk) {
                        $("<span></span>")
                        .addClass("layerButton")
                        .html("确定")
                        .unbind("click").click(function () {
                            if ($.isFunction(opts.ok)) {
                                opts.ok($layerContents);
                                return;
                            }
                            $.layer.close(opts.id);
                        })
                        .appendTo($layerButtons);
                    }
                    if (opts.showSave) {
                        $("<span></span>")
                        .addClass("layerButton")
                        .html("保存")
                        .unbind("click").click(function () {
                            if ($.isFunction(opts.save)) {
                                opts.save($layerContents);
                                return;
                            }
                            $.layer.close(opts.id);
                        })
                        .appendTo($layerButtons);
                    }
                    if (opts.showCancel) {
                        $("<span></span>")
                        .addClass("layerButton")
                        .html("取消")
                        .unbind("click").click(function () {
                            if ($.isFunction(opts.cancel)) {
                                opts.cancel($layerContents);
                                return;
                            }
                            $.layer.close(opts.id);
                        })
                        .appendTo($layerButtons);
                    }
                    if (opts.showClose) {
                        $("<span></span>")
                        .addClass("layerButton")
                        .html("关闭")
                        .unbind("click").click(function () {
                            if ($.isFunction(opts.close)) {
                                opts.close($layerContents);
                                return;
                            }
                            $.layer.close(opts.id);
                        })
                        .appendTo($layerButtons);
                    }
                    if (opts.showBuy) {
                        $("<span></span>")
                        .addClass("layerButton layerBuyButton")
                        .html("购买")
                        .unbind("click").click(function () {
                            if ($.isFunction(opts.buy)) {
                                opts.buy($layerContents);
                                return;
                            }
                            $.layer.close(opts.id);
                        })
                        .appendTo($layerButtons);
                    }
                    if (opts.drag) {
                        $("<div></div>")
                            .addClass("layerResizable")
                            .attr({ title: "可拖拽" })
                            .appendTo($layerFooter);
                    }
                }
                return $layerContents;
            }
            function render() {
                var offset = $dom.offset();
                var top = options.dom ? offset.top : 0;
                var left = options.dom ? offset.left : 0;
                var width = options.dom ? $dom.outerWidth() : $(document).width();
                var height = options.dom ? $dom.outerHeight() : $(document).height();
                $("<div></div>").addClass("layerMask").attr({
                    "id": "layerMask" + opts.id
                }).css({
                    "top": top,
                    "left": left,
                    "width": width,
                    "height": height,
                    "z-index": opts.zIndex
                }).appendTo($("body"));
                var $layerContents = createLayerContnets().appendTo($("body"));
                if (!opts.width) {
                    opts.width = $layerContents.outerWidth();
                }
                if (!opts.height) {
                    opts.height = $layerContents.outerHeight();
                }
                left += ($dom.outerWidth() - opts.width) / 2;
                top += ($dom.outerHeight() - opts.height) / 2;
                $layerContents.css({
                    zIndex: opts.zIndex + 1,
                    top: top,
                    left: left,
                    width: opts.width,
                    height: opts.height
                });
                if (opts.before) {
                    opts.before($layerContents);
                }
                $layerContents.show();
                if (opts.after) {
                    opts.after($layerContents);
                }
                if (opts.drag) {
                    drag($layerContents);
                }
            }
            render();
            return opts.id;
        },
        getZIndex: function (zIndex) {
            var $layerMasks = $(".layerMask");
            var zIndexs = [];
            for (var i = 0, len = $layerMasks.length ; i < len ; i++) {
                zIndexs[i] = $layerMasks.eq(i).css("zIndex");
            }
            if (zIndexs.length === 0) {
                return zIndex + 1;
            }
            zIndexs.sort(function (a, b) {
                return b - a;
            });
            return zIndexs[0] + 1;
        },
        close: function (id) {
            if (!id) {
                return;
            }
            $("#layerMask" + id + ",#layerContents" + id).remove();
        }
    }
})(jQuery, window, document);