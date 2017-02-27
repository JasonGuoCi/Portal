<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PermissionsReport.aspx.cs" Inherits="Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.Reports.PermissionsReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../../Scripts/layer/skin/layer.css" rel="stylesheet" />
    <script src="../../Scripts/jquery.min.js"></script>
    <script src="../../Scripts/jquery.nicescroll.min.js"></script>
    <script src="../../Scripts/layer/layer.js"></script>
    <link href="../../Skins/Default/css/index.css" rel="stylesheet" />
    <style>
        .alertMessageBg {
            position: fixed;
            _position: absolute;
            width: 100%;
            height: 100%;
            left: 0;
            top: 0;
            background: #242424;
            opacity: 0.2;
            -moz-opacity: 0.2;
            filter: alpha(opacity=20);
            z-index: 97;
            display: none;
        }
    </style>
    <script>
        ////check点击事件
        var checkvalue = "";
        function ClickCheck() {
            jQuery(function () {
                var Tile = "";
                var Guid = "";
                $(".checkbox").each(function () {
                    if ($(this).is(":checked")) {
                        Tile += $(this).attr("title") + ",";
                        Guid += $(this).attr("value") + ",";
                    }
                })

                Tile = Tile.substring(0, Tile.length - 1);
                Guid = Guid.substring(0, Guid.length - 1);
                if (Tile == "" || Guid == "") {
                    layer.msg("请选择您需要导出权限表格!");
                    return false;
                }
                $("#HidCheckSelectTile").val(Tile);
                $("#HidCheckSelectGuid").val(Guid);
                showLoading();
            })
        }
        function showLoading() {
            $("#divMask").show();
        }
        function hideLoading() {
            $("#divMask").hide();
            //window.open('get',$("#hidDownLoadURL").val(),true);
            // parent.location.href = $("#hidDownLoadURL").val();
        }

    </script>
    <title>权限报表</title>
</head>

<body style="background:none !important;">
    <form runat="server">
    <div class="permissioncontent">
        <div class="itmbox">
            <ul id="ULDocumentLibrary" class="cbox" runat="server"></ul>
            <ul class="cbox">

                <li class="c_li c_t2">
                    
                        <asp:LinkButton ID="ButSaveExcel" class="btn" OnClientClick="ClickCheck()" runat="server" Text="导出" Style="width: 150px" OnClick="ButSaveExcel_Click">导出</asp:LinkButton>
                        <input type="hidden" id="HidCheckSelectTile" runat="server" /><input type="hidden" id="HidCheckSelectGuid" runat="server" />
                   
                </li>
                <li class="c_li"></li>
                <li class="c_li"></li>
            </ul>
        </div>
        <div id="divMask" class="alertMessageBg">
            <div style="width: 145px; position: absolute; left: 43%; top: 40%">
                <img src="../../Skins/Default/images/loading.gif" style="width: 70px; height: 70px;" />
            </div>
        </div>
    </div>
         </form>
    <script type="text/javascript">
        $(document).ready(function () {
            $('body').css('background', 'none');
            var height = $(window).height();
            $("#ULDocumentLibrary").height(height - 65);
            $("#ULDocumentLibrary").find("li").eq(0).height(height - 88);
            $("#ULDocumentLibrary").find("li").eq(0).niceScroll({ arrows: false, touchbehavior: false, cursorcolor: "#ddd", cursorborder: "none"});
        });

    </script>
</body>
</html>

