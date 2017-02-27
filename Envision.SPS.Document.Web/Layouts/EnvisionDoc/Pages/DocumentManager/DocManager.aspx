<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" Namespace="Microsoft.SharePoint.WebControls" TagPrefix="SP" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocManager.aspx.cs" Inherits="Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.DocumentManager.DocManager" %>
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
        <asp:HiddenField ID="hidCurrentWebUrl" Value="" runat="server" />
        <div class="contant">
            <div class="right" style="margin-left: 0px !important;">
                <%-- <div class="file_patch"><a href="#">站点设置</a><span>&gt;</span><a href="#">目录树设置</a></div>--%>
                <div class="itmbox" style="padding:0 40px 40px 40px !important; min-height:450px;margin-top:0px !important;">
                    <div class="t_search" style="padding:10px 10px 10px 10px !important;">
                        <a style="cursor: pointer;" class="add_dp" id="createDocLib" runat="server">一键创建文档库</a>
                        <span class="type">文档库名称：</span>
                        <%--<input name="" type="text" class="s_iput" />--%>
                         <asp:TextBox ID="txtDoclibName" runat="server" CssClass="s_iput"></asp:TextBox>
                         <asp:Button Text="搜索" CssClass="s_btn" ID="Button1" runat="server" OnClick="btnSearchDoclib_Click"/>
                      <%--  <a href="#" class="s_btn">搜索</a>--%>
                    </div>
                    <table border="0" cellspacing="0" cellpadding="0" class="tbox" id="documentLibList">
                        <tr>
                            <td class="type t_left" style="cursor:pointer;" id="DocName">文档库名称<i></i></td>
                            <td class="type w_1" style="cursor:pointer;width:190px;" id="DocCreateTime">创建时间<i></i></td>
                            <td class="type w_1" style="width:100px;">版本控制</td>
                            <td class="type w_1" style="width:100px;">是否默认签出</td>
                            <td class="type w_1" style="width:100px;">是否继承权限</td>
                            <td class="type w_1">操作</td>
                        </tr>
                        <asp:Repeater ID="RepDocumentList" runat="server">
                            <ItemTemplate>
                                <tr HasUniqueRoleAssignments="<%#Eval("HasUniqueRoleAssignments") %>">
                                    <td class="t_left"><%#DataBinder.Eval(Container.DataItem,"Title")%></td>
                                    <td><%#DataBinder.Eval(Container.DataItem,"Created")%></td>
                                    <td><%#DataBinder.Eval(Container.DataItem,"EnableVersioning")%></td>
                                    <td><%#GetForceCheckout(Eval("ForceCheckout").ToString()) %></td>
                                    <td><%#GetHasUniqueRoleAssignments(Eval("HasUniqueRoleAssignments").ToString()) %></td>
                                    <td><a style="cursor:pointer;" class="edit" id="<%#Eval("Id") %>" dname="<%#Eval("Title") %>" onclick="envdoc.methods.editDocumentLibrary(this)">编辑</a></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>


                        <tr>
                            <td colspan="6" class="page_part">
                                <div class="pagelist">
                                    
                                    <div id="PageContent" runat="server" class="default" style="float: left;"></div>
                                    <div class="l-btns">
                                        <span>显示</span><asp:TextBox ID="txtPageNum" runat="server" CssClass="pagenum" onkeydown="return checkNumber(event);" ontextchanged="txtPageNum_TextChanged" AutoPostBack="True"></asp:TextBox><span>条/页</span>
                                    <span id="total" runat="server"></span>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div style="display:none;">
        <div id="createDocumenthtml" class="tab-content">
            <dl>
                <dt>文档库名称</dt>
                <dd>
                    <input id="txtDocName" name="txtDocName" datatype="*" class="input normal" />
                </dd>
            </dl>
            <dl>
                <dt>文档库模板</dt>
                <dd>
                     <div class="se_box">
                        <asp:DropDownList ID="ddlDocTemplate" runat="server" CssClass="select">
                        </asp:DropDownList>
                    </div>
                </dd>
            </dl>
        </div>
        <div id="createGroupRolehtml" class="tab-content">
            <div class="itmbox">
                <div class="t_search">
                    <a style="cursor: pointer;" class="add_dp" id="existingGroupAdd">从现有群组添加</a>
                    <a style="cursor: pointer;" class="add_dp" id="CreateNewGroupAdd">新建群组并添加</a>
                </div>
                <table border="0" cellspacing="0" cellpadding="0" class="tbox">
                    <tr>
                        <td class="type t_left">群组名称</td>
                        <td class="type w_1">权限级别</td>
                        <td class="type w_1">操作</td>
                    </tr>

                </table>
            </div>

        </div>
        <div id="existingGroupAddhtml" class="tab-content">
            <div class="itmbox">
                <div class="zqxz" id="selectGroups">
                </div>
                <div class="ss">
                    <span>群组名称：</span>
                    <input name="" type="text" class="input" id="txtGroupsName"/>
                    <a style="cursor:pointer;" class="btn" id="btnSearchGroups"></a>
                </div>
                <table border="0" cellspacing="0" cellpadding="0" class="tbox">
                    <tr>
                        <td class="type t_left">群组名称</td>
                        <td class="type w_1">操作</td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="docCreateLastStep" class="tab-content">
            <dl>
                <dt>文档库名称:</dt>
                <dd>
                    <label id="labDoclibName"></label>
                </dd>
            </dl>
            <dl>
                <dt>版本控制:</dt>
                <dd>
                    <div class="se_box">
                        <select name="" class="select" id="ddlVersionControl">
                            <option value="0">无</option>
                            <option value="1">主版本</option>
                            <option value="2">次版本</option>
                        </select>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>默认签出:</dt>
                <dd>
                    <asp:RadioButtonList ID="rblCheckOut" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="radio_btn">
                        <asp:ListItem Value="1" Text="是"></asp:ListItem>
                        <asp:ListItem Value="0" Text="否"></asp:ListItem>
                    </asp:RadioButtonList>
                </dd>
            </dl>
        </div>
        <div id="editDocumentLibrary" class="tab-content">
            <dl>
                <dt>文档库名称:</dt>
                <dd>
                    <input id="txtEditDoclibName" name="txtEditDoclibName" datatype="*" class="input normal" />
                </dd>
            </dl>
            <dl>
                <dt>版本控制:</dt>
                <dd>
                    <div class="se_box">
                        <select name="" class="select" id="ddlEditVersionControl">
                            <option value="0">无</option>
                            <option value="1">主版本</option>
                            <option value="2">次版本</option>
                        </select>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>编辑强制签出:</dt>
                <dd>
                    <asp:RadioButtonList ID="rblEditCheckOut" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="radio_btn">
                        <asp:ListItem Value="1" Text="是"></asp:ListItem>
                        <asp:ListItem Value="0" Text="否"></asp:ListItem>
                    </asp:RadioButtonList>
                </dd>
            </dl>
        </div>

        <div id="AddGroups" class="tab-content">
            <dl>
                <dt>群组名称</dt>
                <dd>
                    <input id="txtAddGroupsName" name="txtAddGroupsName" datatype="*" class="input normal" />
                </dd>
            </dl>
             <dl>
                <dt>群组描述</dt>
                <dd>
                    <textarea id="txtAddGroupsDescription" name="txtAddGroupsDescription" class="input" style="width:300px;"></textarea>
                </dd>
            </dl>
        </div>
        <div id="AddGroupsOwners" class="tab-content">
             <dl>
                <dt>群组所有者</dt>
                <dd>
                    <input id="txtAddGroupsOwner" name="txtAddGroupsOwner" readonly="readonly" datatype="*" class="input normal" errormsg="请设置群组所有者" nullmsg="请设置群组所有者" />
                    <span style="line-height: 20px; margin-bottom: 0px; vertical-align: middle;">
                       <input type="button" value="..." id="btnAddChoseGroups" style="width: 20px; height: 30px;background:#F5F5F5;border:0px;"/>
                    </span>
                </dd>
                 
            </dl>
             <div style="padding-top:20px;padding-left:80px;">
                    <span style="color:red; font-size:14px;">群组所有者：小组的所有者，可以维护小组成员</span>
                </div>
        </div>
        <div id="existingGroupAddToGrouphtml" class="tab-content">
            <div class="itmbox">
                <div class="zqxz" id="selectGroupsDoc">
                </div>
                <div class="ss">
                    <span>群组名称：</span>
                    <input name="" type="text" class="input" id="txtChoseGroupsName"/>
                    <a style="cursor:pointer;" class="btn" id="btnChoseSearchGroups"></a>
                </div>
                <table border="0" cellspacing="0" cellpadding="0" class="tbox">
                    <tr>
                        <td class="type t_left">群组名称</td>
                        <%--<td class="type w_1">创建时间</td>--%>
                        <td class="type w_1">操作</td>
                    </tr>

                </table>
            </div>
        </div>
        </div>
        <asp:Button ID="DocCreateTimeSort" runat="server" style="display:none;" Text="0" OnClick="DocCreateTimeSort_Click"/>
        <asp:Button ID="DocNameSort" runat="server" style="display:none;" Text="0" OnClick="DocNameSort_Click"/>
    </form>
    <script src="../../Scripts/jQuery.fn.extend.js"></script>
    <script src="../../Scripts/layer/layer.js"></script>
    <script src="../../Scripts/global.js"></script>
    <script src="../../Skins/Default/scripts/encisiondoc.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            envdoc.methods.cdinit();
            $('body').css('background', 'none');
            var documentLibList = $("#documentLibList").find("tr");
            $.each(documentLibList, function (i,n) {
                var tr = $(this);
                if (tr.attr("HasUniqueRoleAssignments") == "False") {
                    $(this).css({ "background-color": "#FEFF99" });
                }
            });
            $("#DocName").click(function () {
                $("#DocNameSort").click();
            });
            $("#DocCreateTime").click(function () {
                $("#DocCreateTimeSort").click();
            });
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

