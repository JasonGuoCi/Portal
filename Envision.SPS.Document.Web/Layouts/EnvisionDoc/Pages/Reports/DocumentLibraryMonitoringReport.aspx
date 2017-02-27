<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentLibraryMonitoringReport.aspx.cs" Inherits="Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.Reports.DocumentLibraryMonitoringReport" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>文档库监控报表</title>
    <link href="../../Scripts/layer/skin/layer.css" rel="stylesheet" />
    <script src="../../Scripts/jquery.min.js" type="text/javascript"></script>
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
            background: rgba(229, 229, 229, 2);
            opacity: 0.3;
            -moz-opacity: 0.3;
            filter: alpha(opacity=30);
            z-index: 97;
            display: none;
        }
    </style>
    <script type="text/javascript">
        $(function () {

        });


        function ExcelExport() {
            var selval = '';
            var seltext = '';
            $("[name='LIDocumentLibrary']:checkbox").each(function () {
                if ($(this).attr('checked')) {
                    selval += $(this).val() + ',';
                    seltext += $(this).attr("title") + ',';
                }
            });
            selval = selval.length > 0 ? selval.substr(0, selval.length - 1) : '';
            seltext = seltext.length > 0 ? seltext.substr(0, seltext.length - 1) : '';
            $("#hidSelectedValue").val(selval);
            $("#hidSelectedText").val(seltext);
            if (selval == "") {
                //alert("请至少选择一项后再进行导出!");
                layer.msg("请至少选择一项后再进行导出!");
                return false;
            }
            showLoading();
        }

        function showLoading() {
            $("#divMask").show();
        }

        function hideLoading() {            
            $("#divMask").hide();
            parent.location.href = $("#hidDownLoadURL").val();
        }

    </script>
</head>
<body style="background:none !important;">
    <form id="form1" runat="server">
        <asp:HiddenField ID="hidSelectedValue" runat="server" />
        <asp:HiddenField ID="hidSelectedText" runat="server" />
        <asp:HiddenField ID="hidDownLoadURL" runat="server" />
        <div class="permissioncontent">
            <div class="itmbox">
                <ul id="ULDocumentLibrary" runat="server" class="cbox"></ul>
                <ul class="cbox">
                    <li class="c_li c_t2">
                        <asp:LinkButton ID="btnExport" runat="server" OnClientClick="return ExcelExport();" OnClick="btnExport_Click" class="btn" Style="width: 150px"> 导出</asp:LinkButton>
                    </li>
                    <li class="c_li"></li>
                </ul>
            </div>

        </div>
        <div id="divMask" class="alertMessageBg">
            <div style="width: 145px; position: absolute; left: 43%; top: 40%">
                <img src="../../Skins/Default/images/loading.gif" />
            </div>
        </div>
    </form>
    <script type="text/javascript">
        $(document).ready(function () {
            $('body').css('background', 'none');
            var height = $(window).height();
            $("#ULDocumentLibrary").height(height - 65);
            $("#ULDocumentLibrary").find("li").eq(0).height(height - 88);
            $("#ULDocumentLibrary").find("li").eq(0).niceScroll({ arrows: false, touchbehavior: false, cursorcolor: "#ddd", cursorborder: "none" });
        });
    </script>
</body>
</html>

