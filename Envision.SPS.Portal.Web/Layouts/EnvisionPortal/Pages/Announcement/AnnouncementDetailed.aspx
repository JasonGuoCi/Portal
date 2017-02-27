<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnnouncementDetailed.aspx.cs" Inherits="Envision.SPS.Portal.Web.Layouts.EnvisionPortal.Pages.Announcement.AnnouncementDetailed" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../../Skins/Default/css/announcementDetailed.css" rel="stylesheet" />
</head>
<body>
    <div class="contant">
        <div class="contatn_right textpage_contant">
            <div class="textpage_title">
                <h1>
                    <asp:Literal ID="ltlTitle" runat="server"></asp:Literal></h1>
                <div><span>
                    <asp:Literal ID="ltltPublistDate" runat="server"></asp:Literal></span></div>
            </div>
            <div class="textpage_text">
                <asp:Literal ID="ltlContetnBox" runat="server"></asp:Literal></div>
            <div class="textpage_attachment" runat="server" id="attachmentDiv" visible="false">
                <table border="0" cellspacing="0" cellpadding="0">
                    <tbody>
                        <tr>
                            <td class="type">附件：</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Repeater ID="RepAttachment" runat="server">
                                    <ItemTemplate>
                                        <div><a href="<%#Eval("url") %>" target="_blank"><%#Eval("name") %></a></div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</body>
</html>


