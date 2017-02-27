charset = "UTF-8";

var global = {
    settings: {
        noneffect: false,
        layerSelector: ".layerContents:last"
    },
    enums: {
        responseStatus: {
            success: 0, //成功
            exception: 1, //异常
            failure: 2, //失败,
            noneffect: 3 //无效
        }
    },
    methods: {
        getQueryString: function (name, url) {
            if (!url) {
                url = window.location.search;
            }
            var index = url.indexOf("?");
            if (index !== -1) {
                url = url.substr(index + 1);
            }
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = url.match(reg);
            return r != null && r.length === 4 ? decodeURIComponent(r[2]) : null;
        },
        getWindowSize: function (minWidth, minHeight, callBack) {
            var width = $(window).width();
            if (width < minWidth) {
                width = minWidth;
            }
            var height = $(window).height();
            if (height < minHeight) {
                height = minHeight;
            }
            if ($.isFunction(callBack)) {
                callBack(width, height);
            }
        },
        encrypt: function (contents) {
            return contents;
        },
        decrypt: function (contents) {
            return contents;
        },
        parseData: function (data) {
            data = data || {};
            return data;
        },
        ajax: function (url, data, selector, success, error) {
            var loadingId = $.getUniqueId();
            var currentWebUrl = $("#hidCurrentWebUrl").val();
            data = data || {};
            var ajaxUrl = "/_layouts/15/EnvisionPortal/Handlers/AjaxHandler.ashx?MethodName=" + url + "&Time=" + $.getUniqueId();
            for (var item in data) {
                if (data.hasOwnProperty(item)) {
                    var value = data[item];
                    ajaxUrl += ("&" + item + "=" + ((value === null || value === undefined) ? "" : value));
                }
            }
            ajaxUrl = (currentWebUrl == undefined ? "" : currentWebUrl) + ajaxUrl;
           
            $.ajax({
                type: "GET",   //访问WebService使用Post方式请求
                contentType: "application/json", //WebService 会返回Json类型
                url: ajaxUrl, //"/_layouts/UDoc/Handlers/AjaxHandler.ashx", //调用WebService的地址和方法名称组合 ---- WsURL/方法名
                //data: data,         //这里是要传递的参数，格式为 data: "{paraName:paraValue}",下面将会看到       
                dataType: "json",
                beforeSend: function () {
                    if (selector) {
                        global.methods.loading(selector, null, loadingId);
                    }
                }, //发送请求
                success: function (response) {
                    if ($.isFunction(success)) {
                        success(loadingId, response);
                    } else {
                        if (selector) {
                            global.methods.closeLoading(loadingId);
                        }
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrown) {
                    if ($.isFunction(error)) {
                        error(loadingId, xmlHttpRequest, textStatus, errorThrown);
                        return;
                    }
                    if (selector) {
                        global.methods.closeLoading(loadingId);
                    }
                    $.layer.alert(textStatus);
                }
            });
        },
        ajaxg: function (url, data, selector, success, error, asyncn) {
            var loadingId = $.getUniqueId();
          
            data = data || {};
            var ajaxUrl = "/_layouts/15/EnvisionPortal/Handlers/AjaxHandler.ashx?MethodName=" + url + "&Time=" + $.getUniqueId();
            for (var item in data) {
                if (data.hasOwnProperty(item)) {
                    var value = data[item];
                    ajaxUrl += ("&" + item + "=" + ((value === null || value === undefined) ? "" : value));
                }
            }
            var currentWebUrl = $("#hidCurrentWebUrl").val();
            
            ajaxUrl = (currentWebUrl == undefined ? "" : currentWebUrl) + ajaxUrl;
           
            $.ajax({
                type: "GET",   //访问WebService使用Post方式请求
                contentType: "application/json", //WebService 会返回Json类型
                url: ajaxUrl, //"/_layouts/UDoc/Handlers/AjaxHandler.ashx", //调用WebService的地址和方法名称组合 ---- WsURL/方法名
                //data: data,         //这里是要传递的参数，格式为 data: "{paraName:paraValue}",下面将会看到       
                dataType: "json",
                async: asyncn || true,
                beforeSend: function () {
                    if (selector) {
                        global.methods.loading(selector, null, loadingId);
                    }
                }, //发送请求
                success: function (response) {
                    if ($.isFunction(success)) {
                        success(loadingId, response);
                    } else {
                        if (selector) {
                            global.methods.closeLoading(loadingId);
                        }
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrown) {
                    if ($.isFunction(error)) {
                        error(loadingId, xmlHttpRequest, textStatus, errorThrown);
                        return;
                    }
                    if (selector) {
                        global.methods.closeLoading(loadingId);
                    }
                    $.layer.alert(textStatus);
                }
            });
        },
        ajaxt: function (url, data, selector, success, error, asyncn) {
            var loadings = null;
            data = data || {};
            var ajaxUrl = "/_layouts/15/EnvisionPortal/Handlers/AjaxHandler.ashx?MethodName=" + url + "&Time=" + $.getUniqueId();
            for (var item in data) {
                if (data.hasOwnProperty(item)) {
                    var value = data[item];
                    ajaxUrl += ("&" + item + "=" + ((value === null || value === undefined) ? "" : value));
                }
            }
            var currentWebUrl = $("#hidCurrentWebUrl").val();
            ajaxUrl = (currentWebUrl == undefined ? "" : currentWebUrl) + ajaxUrl;
          
            $.ajax({
                type: "GET",   //访问WebService使用Post方式请求
                contentType: "application/json", //WebService 会返回Json类型
                url: ajaxUrl, //"/_layouts/UDoc/Handlers/AjaxHandler.ashx", //调用WebService的地址和方法名称组合 ---- WsURL/方法名
                //data: data,         //这里是要传递的参数，格式为 data: "{paraName:paraValue}",下面将会看到       
                async: asyncn || true,
                dataType: "json",
                beforeSend: function () {
                    loadings = global.methods.loadingStart();
                }, //发送请求
                success: function (response) {
                    if ($.isFunction(success)) {
                        success(loadings, response);
                    } else {
                        global.methods.loadingClose(loadings);
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrown) {
                    global.methods.loadingClose(loadings);
                }
            });
        },
        loading: function (selector, zIndex, id) {
            zIndex = $.layer.getZIndex(zIndex || 20141016);
            var $ele = $(selector);
            var offset = $ele.offset();
            var top = selector ? offset.top : 0;
            var left = selector ? offset.left : 0;
            var width = selector ? $ele.outerWidth() : $(document).width();
            var height = selector ? $ele.outerHeight() : $(document).height();
            $("<div></div>").addClass("layerMask").attr({
                "id": "layerMask_" + id
            }).css({
                "top": top,
                "left": left,
                "width": width,
                "height": height,
                "z-index": zIndex
            }).appendTo($("body"));
            $("<div></div>").addClass("loading").attr({
                "id": "loading_" + id
            }).css({
                "top": top + (height - 10) / 2,
                "left": left + (width - 10) / 2,
                "z-index": zIndex + 1
            }).appendTo($("body"));
        },
        loadingStart:function()
        {
            return layer.load(1);
        },
        loadingClose:function(id)
        {
            layer.close(id);
        },
        closeLoading: function (id) {
            $("#layerMask_" + id + ",#loading_" + id).remove();
        },
        addKeyword: function (keywords, keyword) {
            keywords.push(keyword);
        },
        removeKeyword: function (keywords, id) {
            for (var i = 0, len = keywords.length; i < len; i++) {
                var keyword = keywords[i];
                if (keyword.split("|")[0] !== id) {
                    continue;
                }
                $.removeFromArrary(keywords, keyword);
                break;
            }
        },
        editKeyword: function (keywords, keyword, id) {
            global.methods.removeKeyword(keywords, id);
            global.methods.addKeyword(keywords, keyword);
        },
        getKeywords: function (keywords, keyword) {
            if ($.trim(keyword) === "") {
                return [];
            }
            var values = [];
            for (var i = 0, len = keywords.length; i < len && values.length < 10; i++) {
                var arrary = keywords[i].split("|");
                for (var j = 1; j < arrary.length && values.length < 10; j++) {
                    var value = arrary[j];
                    if (value === "") {
                        continue;
                    }
                    if (value.toLowerCase().indexOf(keyword.toLowerCase()) === -1 && value.toUpperCase().indexOf(keyword.toUpperCase()) === -1) {
                        continue;
                    }
                    value = global.methods.setHighlighter(value, keyword);
                    if ($.inArray(value, values) === -1) {
                        values.push(value);
                    }
                }
            }
            return values;
        },
        setHighlighter: function (contents, keyword) {
            if (contents === "" || keyword === "") {
                return contents;
            }
            return $.replaceAll(contents, keyword, "<font color='red'>" + keyword + "</font>");
        },
        getUserLink: function (userId, userName) {
            var url = "/_layouts/UserDisp.aspx?ID=" + userId + "";
            return "<a href=\"" + url + "\" target=\"_blank\" class=\"link\">" + userName + "</a>";
        },
        instance: function () {
            $("a").live("focus", function () {
                $(this).blur();
            });
        }
    }
}
$(document).ready(function () {
    //global.methods.instance();
});