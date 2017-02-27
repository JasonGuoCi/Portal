charset = "UTF-8";
var docLeftMenuDrag = {
    methods: {
        setSize: function () {
            this.getWindowSize(0, 0, function (width, height) {
                //$("#contentRow,#sideNavBox,#handleBarSection,#contentBox").height(height - 66 - 30);
                //$("#sideNavBox,#handleBarSection,#contentBox").height(height);
                var bodyheight = $("#sideNavBox").height();
                if (bodyheight < height) {
                    $("#handleBarSection").height(height + 22);
                } else {
                    $("#handleBarSection").height(bodyheight + 22);
                }
                //$(".divtree").height(height - 190);
                //$(".RadTreeView").height(height - 190);
                //$("#handleBarSection").height($("#sideNavBox").height()+20);
                //if ($("#sideNavBox").height() > (height - 170)) {
                //$("#handleBarSection").height($("body").height());
                //}
                //$(".RadTreeView").niceScroll({ arrows: false, touchbehavior: false, cursorcolor: "#ddd", cursorborder: "none" });
            });
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
        drag: function (drag) {
            drag.onmousedown = function (e) {
                var disX = (e || event).clientX;
                drag.left = drag.offsetLeft;
                document.onmousemove = function (e) {
                    var width = drag.left + ((e || event).clientX - disX);
                    width < 120 && (width = 44);
                    if (width <= 350) {
                        docLeftMenuDrag.methods.executeDrag(width);
                    }
                    return false;
                };
                document.onmouseup = function () {
                    document.onmousemove = null;
                    document.onmouseup = null;
                    drag.releaseCapture && drag.releaseCapture();
                };
                drag.setCapture && drag.setCapture();
                return false;
            };
        },
        executeDrag: function (width) {
            $("#sideNavBox").width(width);
            $("#handleBarSection").css({ left: width });
            $("#contentBox").css({
                "margin-left": width + 40
            });


            switch (width) {
                case 44:
                    $("#sideNavBox").find("div").eq(0).hide();
                    $("#handleBarSection").show();
                    break;
                case 260:
                    $("#sideNavBox").find("div").eq(0).show();
                    $("#handleBarSection").show();
                    break;
                default:
                    $("#sideNavBox").find("div").eq(0).show();
                    $("#handleBarSection").show();
                    break;
            }

        },
        instance: function () {
            this.setSize();
            $(window).resize(function () {
                docLeftMenuDrag.methods.setSize();
            });
            setInterval("docLeftMenuDrag.methods.setSize()", 10);
            this.drag($("#handleBarSection")[0]);
        }

    }
}
