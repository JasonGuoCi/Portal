<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupsMaintenance.aspx.cs" Inherits="Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.OtherManager.GroupsMaintenance"%>

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
                            <td class="type w_1" style="width:145px;">操作</td>
                        </tr>
                        <asp:Repeater ID="RepGroupsList" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td class="t_left" style='width: 150px;'><%#DataBinder.Eval(Container.DataItem,"Name")%></td>
                                    <td class="t_left"><%#DataBinder.Eval(Container.DataItem,"Owner")%></td>
                                    <td class="t_left"><%#DataBinder.Eval(Container.DataItem,"Description")%></td>
                                    <td>
                                        <asp:HyperLink ID="linkedit" runat="server" Text="编辑" style="cursor:pointer;" onclick="envdoc.methods.editGroups(this)" CssClass="nor_link" pid='<%#DataBinder.Eval(Container.DataItem,"Id")%>' gname='<%#DataBinder.Eval(Container.DataItem,"Name")%>'></asp:HyperLink>
                                        <asp:HyperLink ID="linkAddUser" runat="server" style="cursor:pointer;" onclick="addUserToGroup(this)" uid='<%#Eval("id") %>' Target="_blank" Text="组成员管理" ToolTip='<%#DataBinder.Eval(Container.DataItem,"IsPermmsion")%>' CssClass="nor_link"></asp:HyperLink>

                                    </td>
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
        <div style="display:none;">
        <div id="AddGroups" class="tab-content">
            <dl>
                <dt>群组名称</dt>
                <dd>
                    <input id="txtGroupsName" name="txtGroupsName" datatype="*" class="input normal" />
                </dd>
            </dl>
            <dl>
                <dt>群组描述</dt>
                <dd>
                    <textarea id="txtGroupsDescription" name="txtGroupsDescription" class="input" style="width: 300px;"></textarea>
                </dd>
            </dl>
        </div>
        <div id="EditGroups" class="tab-content">
            <dl>
                <dt>群组名称</dt>
                <dd>
                    <input id="txtEditGroupsName" name="txtEditGroupsName" datatype="*" class="input normal" />
                </dd>
            </dl>
            <dl>
                <dt>群组所有者</dt>
                <dd>
                    <input id="txtEditGroupsOwner" name="txtEditGroupsOwner" readonly="readonly" datatype="*" class="input normal" />
                    <span style="line-height: 20px; margin-bottom: 0px; vertical-align: middle;">
                        <input type="button" value="..." id="btnEditChoseGroups" style="width: 20px; height: 30px; background: #F5F5F5; border: 0px;" />
                    </span>
                </dd>
            </dl>
            <dl>
                <dt>群组描述</dt>
                <dd>
                    <textarea id="txtEditGroupsDescription" name="txtEditGroupsDescription" class="input" style="width: 300px;"></textarea>
                </dd>
            </dl>
        </div>
        <div id="AddGroupsOwners" class="tab-content">
            <dl>
                <dt>群组所有者</dt>
                <dd>
                    <input id="txtGroupsOwner" name="txtGroupsOwner" readonly="readonly" datatype="*" class="input normal" errormsg="请设置群组所有者" nullmsg="请设置群组所有者" />
                    <span style="line-height: 20px; margin-bottom: 0px; vertical-align: middle;">
                        <input type="button" value="..." id="btnChoseGroups" style="width: 20px; height: 30px; background: #F5F5F5; border: 0px;" />
                    </span>
                </dd>
            </dl>
             <div style="padding-top:20px;padding-left:80px;">
                    <span style="color:red; font-size:14px;">群组所有者：小组的所有者，可以维护小组成员</span>
                </div>
        </div>

        <div id="existingGroupAddhtml" class="tab-content">
            <div class="itmbox">
                <div class="zqxz" id="selectGroups">
                </div>
                <div class="ss">
                    <span>群组名称：</span>
                    <input name="" type="text" class="input" id="txtChoseGroupsName" />
                    <a style="cursor: pointer;" class="btn" id="btnChoseSearchGroups"></a>
                </div>
                <table border="0" cellspacing="0" cellpadding="0" class="tbox">
                    <tr>
                        <td class="type t_left">群组名称</td>
                        <td class="type w_1">操作</td>
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
            envdoc.methods.cginit();
            $('body').css('background', 'none');

        });
        function addUserToGroup(obj) {

            window.open($("#hidCurrentWebUrl").val() + "/_layouts/15/people.aspx?MembershipGroupId=" + $(obj).attr("uid"));
        }
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