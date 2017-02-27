var envportal = {
    urls: {
        GetWeatherUrl: 0,
        GetAnnouncementUrl: 1,
        GetAnnouncementDetailedUrl: 2
    },
    methods: {
        getWeather: function () {
            var Weather = $("#slides1");
            Weather.html("Loading...");
            global.methods.ajax(envportal.urls.GetWeatherUrl, null, null, function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    return;
                }
                var listItems = response.Result || [];
                var weatherHtml = "";
                if (listItems != null) {
                    $.each(listItems, function (i, n) {
                        weatherHtml += " <div class=\"slide_wrap1\"><dl style=\"height:100px;\"><dt style=\"margin-top:10px;\"><img src=\"/_layouts/15/EnvisionPortal/Skins/weather/" + n.Img + "\" /></dt><dd><div>" + n.Location + "</div><div style=\"font-size:20px;\" id=\"" + envportal.methods.StrRemove(n.Location, "g") + "_" + i + "\">" + n.CurrentTime + "</div><div style=\"font-size:14px;line-height:18px;\">" + n.Temperature + " " + n.Conditions + "</div><span id=\"" + envportal.methods.StrRemove(n.Location, "g") + "_" + i + "_2\">" + n.CurrentDate + "</span></dd></dl></div>";
                    });
                }
                Weather.html(weatherHtml);
                envportal.methods.regWeatherShow();
                envportal.methods.regWeather();
            }, null);
        },
        GetAreaTime: function () {
            global.methods.ajax(envportal.urls.GetWeatherUrl, null, null, function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    return;
                }
                var listItems = response.Result || [];
                if (listItems != null) {
                    $.each(listItems, function (i, n) {
                        $("#" + envportal.methods.StrRemove(n.Location, "g") + "_" + i).html(n.CurrentTime);
                        $("#" + envportal.methods.StrRemove(n.Location, "g") + "_" + i + "_2").text(n.CurrentDate);
                    });
                }
            }, null);
        },
        getAnnouncement: function () {
            var Announcement = $("#Announcement");
            Announcement.html("Loading...");
            global.methods.ajax(envportal.urls.GetAnnouncementUrl, null, null, function (loadingId, response) {
                if (response.Status !== global.enums.responseStatus.success) {
                    return;
                }
                var listItems = response.Result || [];
                var announcementHtml = "";
                if (listItems != null) {
                    $.each(listItems, function (i, n) {
                        var li = $("<li></li>");
                        var em = $("<em></em>");
                        var alink = $("<a style='cursor:pointer;' title='" + n.title + "' onclick='envportal.methods.announcementDetailed(\"" + n.id + "\",\"" + n.title + "\")'></a>");
                        em.html(n.publishedDate);
                        alink.html(n.title);
                        em.appendTo(li);
                        alink.appendTo(li);
                        announcementHtml += "<li>" + li.html() + "</li>";
                    });
                    Announcement.html("<ul>" + announcementHtml + "</ul>");
                    $.Scroll(Announcement, { line: 3, speed: 1000, timer: 5000 });
                }
            }, null);
        },
        announcementDetailed: function (id, title) {
            envportal.methods.announcementLayer(id, title);
        },
        announcementLayer: function (id, title) {
            layer.open({
                title: '公告',
                type: 2,
                shade: 0.8,
                area: ['900px', '90%'], //宽高
                content: '/_layouts/15/envisionportal/pages/announcement/announcementDetailed.aspx?listItemId=' + id + ''
            });
        },
        StrRemove: function (str, is_global) {
            var result;
            result = str.replace(/(^\s+)|(\s+$)/g, "");
            if (is_global.toLowerCase() == "g")
                result = result.replace(/\s/g, "");
            return result;
        },
        regWeather: function () {
            $("#myController1").jFlow({
                slides: "#slides1",
                controller: ".jFlowControl1",
                slideWrapper: "#jFlowSlide3",
                selectedWrapper: "jFlowSelected3",
                auto: true,
                duration: 500,
                width: "285px",
                height: "100px",
                prev: ".prev",
                next: ".next"
            });
        },
        regWeatherHide: function () {
            $("#myController1").hide();
        },
        regWeatherShow: function () {
            $("#myController1").show();
        },
        supportDepartType: function (obj) {
            //window.location = "/_layouts/15/envisionportal/pages/DepartmentSupport/List.aspx#";
            $("#hidType").val(obj);
            $("#hidBtnSeach").click();
        },
        helpDesk: function () {
            $("#helpDesk").click(function () {
                layer.open({
                    title: '管理员信息',
                    type: 2,
                    shade: 0.8,
                    area: ['900px', '90%'], //宽高
                    content: '/_layouts/15/envisionportal/pages/HelpDesk.aspx'
                });
            });
        },
        homeInit: function () {
            this.helpDesk();
            this.getAnnouncement();
            this.regWeatherHide();
            this.getWeather();
            window.setInterval(this.GetAreaTime, 30000);
            document.onkeydown = function (event) {
                var e = event || window.event || arguments.callee.caller.arguments[0];
                var obj = e.target || e.srcElement;//获取事件源 
                if (e && e.keyCode == 13 && $(obj).attr("id") == "txtSearch") { // enter 键
                    //要做的事情
                    window.open("/_layouts/15/osssearchresults.aspx?k=" + $("#txtSearch").val());
                    return false;
                }
            };
        },
        suppInit: function () {

        }
    }
}