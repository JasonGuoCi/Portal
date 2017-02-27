<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ITSiteTypeView.aspx.cs" Inherits="Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.ITMonitor.ITSiteTypeView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>IT全局监控</title>
    <script src="../../Scripts/jquery.min.js" type="text/javascript"></script>
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

        function ExcelExport() {
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
    <form id="mainfrom" runat="server">
        <div class="permissioncontent">
            <div class="itmbox">
                <ul class="cbox">
                    <li class='c_li c_t1'>
                        <p class='p_div'>
                            名称：<asp:Literal ID="ltlName" runat="server"></asp:Literal>
                        </p>
                        <p class='p_div'>
                            子站点数：<asp:Literal ID="ltlChildSiteCount" runat="server"></asp:Literal>
                        </p>
                        <p class='p_div'>
                            文档库数：<asp:Literal ID="ltlDocumentLibraryCount" runat="server"></asp:Literal>
                        </p>
                        <p class='p_div'>
                            当前容量：<asp:Literal ID="ltlCurrentStorage" runat="server"></asp:Literal>
                        </p>
                    </li>

                </ul>
                <ul class="cbox">
                    <li class="c_li c_t2">
                        <a id="btnpermissoinLink" class="btn" href="javascript:void(0);" style="width: 100px">管理权限</a>
                        <asp:LinkButton ID="btnMonitorExport" runat="server" CssClass="btn" OnClientClick="ExcelExport();" OnClick="btnMonitorExport_Click" Width="100px">导出统计报表</asp:LinkButton>
                    </li>
                </ul>
            </div>
        </div>
        <div id="divMask" class="alertMessageBg">
            <div style="width: 145px; position: absolute; left: 43%; top: 40%">
                <img src="../../Skins/Default/images/loading.gif" />
            </div>
        </div>
        <asp:HiddenField ID="currentWebUrl" runat="server" />
        <asp:HiddenField ID="hidDownLoadURL" runat="server" />
    </form>
    <script type="text/javascript">
        $(document).ready(function () {
            parent.envdoc.methods.closeLayerLoad();
            $('body').css('background', 'none');
            var host = $("#currentWebUrl").val();
            $("#btnpermissoinLink").click(function () {
                var url = host + "/_layouts/15/user.aspx";
                window.open(url);
            });
        });
    </script>
</body>
</html>
