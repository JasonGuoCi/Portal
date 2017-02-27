charset = "UTF-8";
var layoutsDray = {
    methods: {
        setSize: function () {
            global.methods.getWindowSize(0, 0, function (width, height) {
                $(".document_box").height(height - 66);
                $(".contentsSection,#leftSection,.leftPane,#handleBarSection,#rightSection,.treeBox").height(height - 66);
                //$("#listItemsDataGrid .dataGridContents").height(height - 66 - 30 - 128);
                $("#mainframe").height(height - 66 - 60);
                $(".treeBox").height(height - 86);
                $(".treeBox").niceScroll({ arrows: false, touchbehavior: false, cursorcolor: "#7C7C7C", cursorborder: "none", cursoropacitymax: 0, cursorwidth: 0 });
            });
        },
        drag: function (drag) {
            drag.onmousedown = function (e) {
                var disX = (e || event).clientX;
                drag.left = drag.offsetLeft;
                document.onmousemove = function (e) {
                    var width = drag.left + ((e || event).clientX - disX);
                    width < 120 && (width = 44);
                    if (width <= 350 || width<=120) {
                        layoutsDray.methods.executeDrag(width);
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
            $("#leftSection").width(width);
            $(".leftPane").width(width);
            $("#handleBarSection").css({ left: width });
           
            $("#rightSection").css({ "margin-left": width + 6 });
            switch (width) {
                case 44:
                    $("#leftSection").find(".searchbox").hide();
                    $("#tree").hide();
                    $("#handleBarSection").show();
                    break;
                case 60:
                    $("#leftSection").find(".searchbox").show();
                    $("#tree").show();
                    $("#handleBarSection").show();
                    break;
                default:
                    $("#tree").show();
                    $("#leftSection").show();
                    $("#handleBarSection").show();
                    break;
            }
        },
        instance: function () {
            this.setSize();
            $(window).resize(function () {
                layoutsDray.methods.setSize();
            });
            //this.drag($("#handleBarSection")[0]);
        },
        itinstance: function () {
            this.setSize();
            $(window).resize(function () {
                layoutsDray.methods.setSize();
            });
            //this.drag($("#handleBarSection")[0]);
        }
    }
}
