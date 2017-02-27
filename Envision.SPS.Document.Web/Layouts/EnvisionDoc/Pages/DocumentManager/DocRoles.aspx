<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocRoles.aspx.cs" Inherits="Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.DocumentManager.DocRoles" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
    <link href="../../Scripts/zTree/css/demo.css" rel="stylesheet" />
    <link href="../../Scripts/zTree/css/zTreeStyle/zTreeStyle.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <ul id="treeDemo" class="ztree"></ul>

        </div>
        <asp:HiddenField ID="hidCurrentWebUrl" Value="" runat="server" />
    </form>

    <script src="../../Scripts/jquery.min.js"></script>
    <script src="../../Skins/Default/scripts/jquery.fn.extend.js"></script>
    <script src="../../Scripts/zTree/js/jquery.ztree.all.js"></script>
    <script src="../../Scripts/layer/layer.js"></script>
    <script src="../../Scripts/global.js"></script>
    <script src="../../Skins/Default/scripts/encisiondoc.js"></script>
    <script type="text/javascript" charset="UTF-8">
        $(document).ready(function () {

            envdoc.methods.treeinit();
        });
    </script>
</body>
</html>
