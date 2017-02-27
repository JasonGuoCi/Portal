<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ToolKitLinks.aspx.cs" Inherits="Envision.SPS.Portal.Web.Layouts.EnvisionPortal.Pages.ToolKitLinks" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../Skins/Default/css/toolkit.css" rel="stylesheet" />
    <title>常用系统入口</title>
</head>

<body>
    <div class="top">
        <div class="box">
            <a href="/" class="h_btn"></a><span>欢迎您, <asp:Literal ID="ltlUser" runat="server"></asp:Literal></span><h4>&nbsp;</h4>
        </div>
    </div>
    <%--<div class="b_nav_box">
        <div>
            <a href="#">常用系统入口</a><em>&gt;</em><a href="#" class="hover">submenu</a>
        </div>
    </div>--%>
    <asp:Repeater ID="RepCategory" runat="server" OnItemDataBound="RepCategory_ItemDataBound">
       
        <ItemTemplate>
                <div class="contant">
            <h4 class="t_type"><asp:Literal ID="categoryTitle" runat="server" Text='<%#Eval("category") %>'></asp:Literal></h4>
           
            <asp:Repeater ID="RepSystemLinks" runat="server" OnItemDataBound="RepSystemLinks_ItemDataBound">
                <HeaderTemplate>
                    <div style="float:left;margin-left:70px;">
                    <table border="0" cellspacing="0" cellpadding="0" class="app_box">
                        <tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <td align="left"><a href="<%#Eval("linkUrl") %>" target="_blank" class="itm"><i>
                        <img src="<%#Eval("imageUrl") %>" img1="<%#Eval("imageUrl") %>" img2="<%#Eval("imageUrl2") %>" alt="" /></i><p><%#Eval("title") %></p>
                    </a></td>
                </ItemTemplate>
                <FooterTemplate>
                    </tr>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
                    </div>
                    </div>
        </ItemTemplate>
    </asp:Repeater>
   <%-- <div class="contant">
        <h4 class="t_type">Office App</h4>
        <table border="0" cellspacing="0" cellpadding="0" class="app_box">
            <tr>
                <td><a href="#" class="itm"><i>
                    <img src="img/toolkit_icon1_1.png" alt="" /></i><p>设备管理</p>
                </a></td>
                <td><a href="#" class="itm"><i>
                    <img src="img/toolkit_icon1_1.png" alt="" /></i><p>供应商管理</p>
                </a></td>
                <td><a href="#" class="itm"><i>
                    <img src="img/toolkit_icon1_1.png" alt="" /></i><p>常用模板</p>
                </a></td>
                <td><a href="#" class="itm"><i>
                    <img src="img/toolkit_icon1_1.png" alt="" /></i><p>会议室预定</p>
                </a></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </div>
    <div class="contant">
        <h4 class="t_type">Office App</h4>
        <table border="0" cellspacing="0" cellpadding="0" class="app_box">
            <tr>
                <td><a href="#" class="itm"><i>
                    <img src="img/toolkit_icon1_1.png" alt="" /></i><p>行政部门</p>
                </a></td>
                <td><a href="#" class="itm"><i>
                    <img src="img/toolkit_icon1_1.png" alt="" /></i><p>内部宣传部</p>
                </a></td>
                <td><a href="#" class="itm"><i>
                    <img src="img/toolkit_icon1_1.png" alt="" /></i><p>IT部门</p>
                </a></td>
                <td><a href="#" class="itm"><i>
                    <img src="img/toolkit_icon1_1.png" alt="" /></i><p>财务部</p>
                </a></td>
                <td><a href="#" class="itm"><i>
                    <img src="img/toolkit_icon1_1.png" alt="" /></i><p>人事部</p>
                </a></td>
                <td><a href="#" class="itm"><i>
                    <img src="img/toolkit_icon1_1.png" alt="" /></i><p>人事部</p>
                </a></td>
                <td><a href="#" class="itm"><i>
                    <img src="img/toolkit_icon1_1.png" alt="" /></i><p>人事部</p>
                </a></td>
            </tr>
            <tr>
                <td><a href="#" class="itm"><i>
                    <img src="img/toolkit_icon1_1.png" alt="" /></i><p>法务部</p>
                </a></td>
                <td><a href="#" class="itm"><i>
                    <img src="img/toolkit_icon1_1.png" alt="" /></i><p>采购部</p>
                </a></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </div>--%>
    <script src="../Scripts/jquery.min.js"></script>
    <script>
        $(".app_box").find("a").each(function () {
            $(this).hover(function () {
                $(this).css({ "color": "#fff", "background": "#00a2e4" });
               var img= $(this).find("i").eq(0).find("img").eq(0);
               var hoverimg = img.attr("img2");
               img.attr("src", hoverimg);
            }, function () {
                $(this).css({ "color": "#2FCAC2", "background": "#fff" });
                var img = $(this).find("i").eq(0).find("img").eq(0);
                var removerimg = img.attr("img1");
                img.attr("src", removerimg);
            });
        });
    </script>
</body>
    
</html>
