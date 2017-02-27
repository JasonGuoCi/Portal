<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentPropterySetting.aspx.cs" Inherits="Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.SiteManager.DocumentPropterySetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <title>文档库属性设置</title>
    <link href="../../Skins/Default/css/index.css" rel="stylesheet"/>
    <script src="../../Scripts/jquery.min.js" type="text/javascript"></script>
   
</head>
<body>
    <form id="form1" runat="server">
        <div class="maincontain">
        <div class="right">
            <div class="itmbox" style="padding-top: 20px;">
                <dl class="nor_itm">
                    <dt>排序规则：</dt>
                    <dd>
                        <asp:RadioButtonList ID="rblCollations" runat="server" RepeatDirection="Horizontal" Width="300" TextAlign="Left"  >
                            <asp:ListItem Text="按名称排序" Value="按名称"></asp:ListItem>
                            <asp:ListItem Text="按创建时间排序" Value="按创建日期"></asp:ListItem>
                        </asp:RadioButtonList></dd>
                </dl>
                <dl class="nor_itm">
                    <dt>升序/降序：</dt>
                    <dd>
                        <asp:RadioButtonList ID="rblAscDesc" runat="server" RepeatDirection="Horizontal" Width="300" TextAlign="Right"  >
                            <asp:ListItem Text="升序" Value="升序"></asp:ListItem>
                            <asp:ListItem Text="降序" Value="降序"></asp:ListItem>
                        </asp:RadioButtonList></dd>
                </dl>
                <dl class="nor_itm">
                    <dt>默认展开层级：</dt>
                    <dd>
                        <asp:RadioButtonList ID="rblOpen" runat="server" RepeatDirection="Horizontal" Width="300" TextAlign="Right"  >
                            <asp:ListItem Text="收起" Value="False"></asp:ListItem>
                            <asp:ListItem Text="展开" Value="True"></asp:ListItem>
                        </asp:RadioButtonList></dd>
                </dl>
                <div class="nor_btnarea">
                    <asp:Button ID="btnSaveSet" runat="server" Style="float: left; padding: 0px 40px; border: 0px; margin-right: 20px; text-align: center; line-height: 40px; color: #fff; font-size: 14px; background: #39f;"
                        Text="保存设置" OnClick="btnSavaSet_Click" />

                </div>
            </div>
        </div>
            </div>
    </form>
<script src="../../Scripts/layer/layer.js"></script>
     <script type="text/javascript">
         $(document).ready(function () {
             $('body').css('background', 'none');
         });
         <%=Result%>
    </script>
</body>
</html>
