<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditReport.aspx.cs" Inherits="Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.Reports.AuditReport" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>文档库审计报表</title>
    <link href="../../Scripts/layer/skin/layer.css" rel="stylesheet" />
    <script src="../../Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="../../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.nicescroll.min.js"></script>
    <link href="../../Scripts/layer/skin/layer.css" rel="stylesheet" />
    <script src="../../Scripts/layer/layer.js" type="text/javascript"></script>
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

        $(document).ready(function () {

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
<body style="background: none !important;">
    <form id="form1" runat="server">
        <asp:HiddenField ID="hidSelectedValue" runat="server" />
        <asp:HiddenField ID="hidSelectedText" runat="server" />
        <asp:HiddenField ID="hidDownLoadURL" runat="server" />
        <div class="permissioncontent">
            <div class="itmbox">
                <ul id="ULDocumentLibrary" runat="server" class="cbox"></ul>
                <ul class="cbox">
                    <li class="c_li c_t2">
                        <div class="selectbox">
                            <asp:DropDownList ID="ddlOperType" runat="server" class="select">
                                <asp:ListItem Value="查看"></asp:ListItem>
                                <asp:ListItem Value="删除"></asp:ListItem>
                                <asp:ListItem Value="修改"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:LinkButton ID="btnExport" runat="server" OnClientClick="return ShowDateDiv();" class="btn"> 导出</asp:LinkButton>
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
        <div id="divDate" class="divDate" style="display: none;">
            <div style="width: 300px; height: 200px; position: absolute; left: 40%; top: 35%">
                <asp:TextBox ID="calStart" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" CssClass="time" MaxLength="10" Width="100px" />
                <br />
                <br />
                <asp:TextBox ID="calEnd" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" CssClass="time" MaxLength="10" Width="100px" /><br />
                <br />
                <asp:LinkButton ID="btnAduitExport" runat="server" CssClass="btn" OnClientClick="return AuditExcelExport();" Width="100px"></asp:LinkButton>
                <asp:Button ID="btnAduitExports" runat="server" Style="display: none;" OnClick="btnExport_Click" />
                <a href="#" class="btn" onclick="HideDateDiv()">返 回</a>
            </div>
        </div>
        <div id="setDate" style="display: none;">
            <div>
                <table style="margin: 0 auto; line-height: 300%; padding: 30px; width: 100%;">
                    <tr>
                        <td>审计开始日期：
                        </td>
                        <td>
                            <input id="startDate" class="time" maxlength="10" readonly="readonly" style="width: 150px;" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd', maxDate: '%y-%M-%d' });" />
                        </td>
                    </tr>
                    <tr>
                        <td>审计结束日期：

                        </td>
                        <td>
                            <input id="endDate" class="time" maxlength="10" readonly="readonly" style="width: 150px;" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd', maxDate: '%y-%M-%d', minDate: '#F{$dp.$D(\'startDate\')}' });" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><span style="color:red;">时间范围不能超过3个月！</span></td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
    <script src="../../Scripts/jQuery.fn.extend.js"></script>
    <script type="text/javascript">
        var setdivDate = null;
        $(document).ready(function () {
            $('body').css('background', 'none');
            var height = $(window).height();
            $("#ULDocumentLibrary").height(height - 65);
            $("#ULDocumentLibrary").find("li").eq(0).height(height - 88);
            $("#ULDocumentLibrary").find("li").eq(0).niceScroll({ arrows: false, touchbehavior: false, cursorcolor: "#ddd", cursorborder: "none" });
            setdivDate = $("#setDate").show().remove().outerHTML();
        });
       
        function checkSetDate() {

            var start = $("#startDate").val();
            var end = $("#endDate").val();
            if (start && end) {
                if (end > start) {
                    var Startobj = $dp.$D('startDate', { 'M': 3 });
                    var Endobj = $dp.$D('endDate');
                    var StartobjValue = Startobj.y + '-' + Startobj.M + '-' + Startobj.d
                    var EndobjValue = Endobj.y + '-' + Endobj.M + '-' + Endobj.d
                    if (StartobjValue <= EndobjValue) {
                        layer.msg("时间范围不能超过3个月！");
                        return false;
                    } else {
                        $("#calStart").val(start);
                        $("#calEnd").val(end);
                        return true;
                    }
                } else {
                    layer.msg("审计结束日期必须大于审计开始日期!");
                    return false;
                }
            } else {
                layer.msg("请选择导出审计报表的起始日期!");
                return false;
            }
        }

        function ExcelExporti() {
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
            return true;
        }

        function ShowDateDiv() {
            if (!ExcelExporti()) {
                return false;
            }
            $("#calStart").val("");
            $("#calEnd").val("");
            $("#startDate").val("");
            $("#endDate").val("");
            var index = layer.open({
                type: 1
               , title: '设置报表时间段'
               , offset: '10px'
               , area: ['400px', '300px']
               , shade: [0.2, '#fff']
               , content: setdivDate
               , btn: ['导出审计报表', '返 回']
                , yes: function () { //或者使用btn2
                    //确认按钮
                    if (checkSetDate()) {
                        $("#btnAduitExports").click();
                        layer.close(index);
                        showLoading();
                    }
                }
                , btn2: function (index) {
                    //按钮【按钮二】的回调
                    layer.close(index);
                }
            });
            return false;
        }
    </script>
</body>
</html>


