<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Envision.SPS.Portal.Web.Layouts.EnvisionPortal.Pages.DepartmentSupport.List" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../../Skins/Default/css/support.css" rel="stylesheet" />
    <title>部门知识指导</title>
</head>

<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:HiddenField ID="hidType" runat="server" />

        <div style="right: 0; width: 100%; position: fixed; z-index: 99;">
            <div class="top">
                <div class="box">
                    <a href="/" class="h_btn"></a><span>欢迎您,<asp:Literal ID="ltlUser" runat="server"></asp:Literal></span><h4>&nbsp;</h4>
                </div>
            </div>
        </div>
        <div class="contant">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Button ID="hidBtnSeach" runat="server" Style="display: none;" OnClick="hidBtnSeach_Click" />
                    <div style="margin-top: 60px; width: 1200px; position: fixed; background-color: #FFF; margin-left: -800px; left: 870px; z-index: 100;">
                        <ul class="department_choice">
                            <li class="search_part">
                                <asp:TextBox ID="txtkeyword" runat="server" CssClass="input"></asp:TextBox>
                                <asp:Button ID="btnSearchTitle" runat="server" OnClick="btnSearchTitle_Click" CssClass="btn" Style="border: 0px;" />

                            </li>
                            <li class="dep_part">
                                <a style="cursor: pointer;" class="dep" onclick="envportal.methods.supportDepartType('ALL');">ALL</a>
                                <asp:Repeater ID="RepDepartment" runat="server">
                                    <ItemTemplate>
                                        <a style="cursor: pointer;" class="dep" onclick="envportal.methods.supportDepartType('<%#Eval("departName") %>');"><%#Eval("departName") %></a>
                                    </ItemTemplate>
                                </asp:Repeater>

                            </li>
                        </ul>
                    </div>
                    <div class="FAQ_box" style="margin-top: 140px;">
                        <asp:Repeater ID="RepSupportAnswer" runat="server">
                            <ItemTemplate>
                                <dl>
                                    <div style="position: relative; padding: 23px 0px 0px 0px;"></div>
                                    <dt id="b<%#Eval("id") %>" style="margin: 0; padding: 0; position: relative; height: auto; margin: -125px 0 0; border-top: 135px solid #fff; z-index: 1;">
                                        <%#Eval("title") %></dt>
                                    <div style="position: relative; padding: 10px 0px 25px 0px;"></div>
                                    <dd style="margin: 0; padding: 0; position: relative; z-index: 2;"><%#Eval("contentBox")%></dd>
                                    <%#Eval("attachemtns") %>
                                    <div style="position: relative; padding: 12px 0px 25px 0px; border-bottom: 3px #eee solid; z-index: 3;"></div>
                                </dl>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div style="position: fixed; left: 50%; margin-left: 220px; top: 120px;">
                        <dl class="FAQ_list">
                            <dt class="title">Question</dt>
                            <dd id="sidebar-nav" style="height: 350px; overflow: hidden; outline: none;">
                                <asp:Repeater ID="RepSupportTitle" runat="server">
                                    <HeaderTemplate>
                                        <table border="0" cellspacing="0" cellpadding="0">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><a href="#b<%#Eval("id") %>" title="<%#Eval("title") %>" style="display: block; width: 350px; height: 30px; line-height: 30px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" onclick="addtTdClass(this)"><%#Eval("title") %></a></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </dd>
                        </dl>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
    <script src="../../Scripts/jquery.min.js"></script>
    <script src="../../Scripts/jquery.nicescroll.min.js"></script>
    <script src="../../Scripts/jQuery.fn.extend.js"></script>
    <script src="../../Scripts/global.js"></script>
    <script src="../../Scripts/EnvisionPortal.js"></script>
    <script>
        function addtTdClass(obj) {
            var objtb = obj.parentElement.parentElement.parentElement;
            var objtrs = objtb.childNodes;
            for (var i = 0; i < objtrs.length; i++) {
                var objtds = objtrs[i].childNodes;
                for (var j = 0; j < objtds.length; j++) {
                    if (objtds[j].nodeName == "#text" && !/\s/.test(objtds.nodeValue)) {
                        objtrs[i].removeChild(objtds[j]);
                    }
                }
                for (var k = 0; k < objtds.length; k++) {
                    if (objtds[k].getAttribute("class") == "hover" || objtds[k].className == "hover") {
                        objtds[k].className = "";
                    }
                }
            }
            obj.parentElement.className = "hover";
        }
        function init() {
            if ($("#hidType").val() != "") {
                $(".dep_part").find("a").attr("class", "dep");
                $.each($(".dep_part").find("a"), function (i, n) {
                    if ($(n).html() == $("#hidType").val()) {
                        $(n).attr("class", "dep hover");
                    }
                });
            } else {
                $(".dep_part").find("a").eq(0).attr("class", "dep hover");
            }
            $("#sidebar-nav").niceScroll({ touchbehavior: false, cursorcolor: "#7C7C7C", cursoropacitymax: 0, cursorwidth: 0 });
            document.onkeydown = function (event) {
                var e = event || window.event || arguments.callee.caller.arguments[0];
                var obj = e.target || e.srcElement;//获取事件源 
                if (e && e.keyCode == 13 && $(obj).attr("id") == "txtkeyword") { // enter 键
                    //要做的事情
                    $("#btnSearchTitle").click();
                    return false;
                }
            };
        }
        $(document).ready(function () {
            envportal.methods.suppInit();
            init();
        });

    </script>
</body>

</html>
