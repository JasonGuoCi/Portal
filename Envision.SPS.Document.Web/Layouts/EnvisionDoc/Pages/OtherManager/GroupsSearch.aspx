<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupsSearch.aspx.cs" Inherits="Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.OtherManager.GroupsSearch"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>文档库管理</title>
    <link href="../../Skins/Default/css/indexframe.css" rel="stylesheet" />
    <link href="../../Skins/Default/css/pagination.css" rel="stylesheet" />
    <link href="../../Skins/Default/css/custome.css" rel="stylesheet" />
    <script src="../../Scripts/jquery.min.js"></script>
    <script src="../../Scripts/Validform_v5.3.2_min.js"></script>
    <script src="../../Scripts/json2.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="contant">
            <div class="right" style="margin-left: 0px !important;">
                <div class="itmbox" style="padding: 0 40px 40px 40px !important; margin-top: 0px !important;">
                    <div class="t_search" style="padding: 10px 10px 10px 10px !important;">
                        <span class="type">群组名称：</span>
                        <asp:TextBox ID="txtGroupName" runat="server" CssClass="s_iput"></asp:TextBox>
                        <asp:Button Text="搜索" CssClass="s_btn" ID="btnSearchGroups" runat="server" OnClick="btnSearchGroups_Click" />
                    </div>
                    <table border="0" cellspacing="0" cellpadding="0" class="tbox">
                        <tr>
                            <td class="type t_left" style="width:300px;">群组名称</td>
                            <td class="type t_left" style="width:300px;">群组所有者</td>
                             <td class="type t_left">描述</td>
                        </tr>
                        <asp:Repeater ID="RepGroupsList" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td class="t_left" style='width: 150px;'><%#DataBinder.Eval(Container.DataItem,"Name")%></td>
                                    <td class="t_left"><%#DataBinder.Eval(Container.DataItem,"Owner")%></td>
                                    <td class="t_left"><%#DataBinder.Eval(Container.DataItem,"Description")%></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>


                        <tr>
                            <td colspan="6" class="page_part">
                                <div class="pagelist">
                                    <div id="PageContent" runat="server" class="default" style="float: left;"></div>
                                     <div class="l-btns">
                                        <span>显示</span><asp:TextBox ID="txtPageNum" runat="server" CssClass="pagenum"  onkeydown="return checkNumber(event);" ontextchanged="txtPageNum_TextChanged" AutoPostBack="True"></asp:TextBox><span>条/页</span>
                                         <span id="total" runat="server"></span>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
       
        <asp:HiddenField ID="hidCurrentWebUrl" Value="" runat="server" />
    </form>
    <script src="../../Scripts/jQuery.fn.extend.js"></script>
    <script src="../../Scripts/layer/layer.js"></script>
    <script src="../../Scripts/global.js"></script>
    <script src="../../Skins/Default/scripts/encisiondoc.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('body').css('background', 'none');
        });
        function checkNumber(e) {
            if (isFirefox = navigator.userAgent.indexOf("Firefox") > 0) {  //FF 
                if (!((e.which >= 48 && e.which <= 57) || (e.which >= 96 && e.which <= 105) || (e.which == 8) || (e.which == 46)))
                    return false;
            } else {
                if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105) || (event.keyCode == 8) || (event.keyCode == 46)))
                    event.returnValue = false;
            }
        }
    </script>
</body>
</html>