charset = "UTF-8";
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
    }
})(jQuery, window, document);