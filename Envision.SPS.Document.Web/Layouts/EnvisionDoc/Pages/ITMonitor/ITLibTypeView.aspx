<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ITLibTypeView.aspx.cs" Inherits="Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.ITMonitor.ITLibTypeView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>IT全局监控</title>
        <script src="../../Scripts/jquery.min.js" type="text/javascript"></script>
    <link href="../../Skins/Default/css/index.css" rel="stylesheet" />
    <script src="../../Scripts/My97DatePicker/WdatePicker.js"  type="text/javascript"></script>   
     <link href="../../Scripts/layer/skin/layer.css" rel="stylesheet" />
    <script src="../../Scripts/layer/layer.js" type="text/javascript"></script>
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


        .divDate {
            position: fixed;
            _position: absolute;
            width: 100%;
            height: 100%;
            left: 0;
            top: 0;
            background: rgba(229, 229, 229, 2);
            opacity: 0.9;
            -moz-opacity: 0.9;
            filter: alpha(opacity=90);
            z-index: 97;
            display: none;
        }
    </style>
    <script type="text/javascript">
        function ExcelExport() {
            showLoading();
        }

        function AuditExcelExport() {
            var start = $("#calStart").val();
            var end = $("#calEnd").val();
            if (start && end) {
                if (end > start) {
                    showLoading();
                } else {
                    layer.msg("审计结束日期必须大于审计开始日期!");
                    return false;
                }
            } else {
                //alert("请选择审计报表的起始日期!");
                layer.msg("请选择导出审计报表的起始日期!");
                return false;
            }
        }



        function HideDateDiv() {
            $("#divDate").hide();
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
                            文件夹数：<asp:Literal ID="ltlFoldeCount" runat="server"></asp:Literal>
                           </p>
                            <p class='p_div'>
                            文件数：<asp:Literal ID="ltlFilesCount" runat="server"></asp:Literal>
                           </p>
                            <p class='p_div'>
                            当前容量：<asp:Literal ID="ltlCurrentStorage" runat="server"></asp:Literal>
                           </p>
                        </li>

                   </ul>
                    <ul class="cbox">
                        <li class="c_li c_t2">
                               <a  id="btnpermissoinLink" class="btn" href="javascript:void(0);"" style="width:100px"> 管理权限</a>   

                           
                               <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn" OnClientClick="return ShowDateDiv();"  

Width="100px" >导出审计报表</asp:LinkButton>
                               <asp:LinkButton ID="btnPermissionExport" runat="server" CssClass="btn" OnClientClick="ExcelExport();" 

OnClick="btnPermissionExport_Click" Width="100px">导出权限报表</asp:LinkButton>
                               <asp:LinkButton ID="btnMonitorExport" runat="server" CssClass="btn" OnClientClick="ExcelExport();" 

OnClick="btnMonitorExport_Click" Width="100px">导出统计报表</asp:LinkButton>
                        </li>                       
                    </ul>


                
               </div>
            
         </div>
         <div id="divMask" class="alertMessageBg">
            <div style="width: 145px; position: absolute; left: 43%; top: 40%">
                <img src="../../Skins/Default/images/loading.gif"/>
            </div>
        </div>

        <div id="divDate" class="divDate" style="display:none;">
            <div style="width: 300px;height:200px; position: absolute; left: 40%; top: 35%">
                 审计开始日期：<asp:TextBox ID="calStart" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" CssClass="time" MaxLength="10" Width="100px"/>
                <br/><br/>审计结束日期：<asp:TextBox ID="calEnd" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" CssClass="time" MaxLength="10"  Width="100px"/><br/><br/>
                <asp:LinkButton ID="btnAduitExport" runat="server" CssClass="btn" OnClientClick="return AuditExcelExport();" OnClick="btnAduitExport_Click" Width="100px" >导出审计报表</asp:LinkButton>
                <asp:Button ID="btnAduitExports" runat="server" style="display:none;" OnClick="btnAduitExport_Click" />
                <a href="#" class="btn" onclick="HideDateDiv()" >返 回</a>       
                 </div>
        </div>
        <div id="setDate" style="display:none;">
           <div>
            <table style="margin:0 auto; line-height:300%; padding:30px; width:100%;"><tr><td>
                审计开始日期：
                       </td><td>
                           <input id="startDate" class="time" maxlength="10" style="width:150px;" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd', maxDate: '%y-%M-%d' });" />
                            </td></tr>
                <tr><td>审计结束日期：

                    </td><td>
                         <input id="endDate" class="time" maxlength="10" style="width:150px;" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd', maxDate: '%y-%M-%d', minDate: '#F{$dp.$D(\'startDate\')}' });"/>
                             </td></tr>
                <tr>
                        <td colspan="2"><span style="color:red;">时间范围不能超过3个月！</span></td>
                    </tr>
            </table>
               </div>
        </div>
        <asp:HiddenField ID="currentWebUrl" runat="server" />
        <asp:HiddenField ID="hidDownLoadURL" runat="server" />
    </form>
    <script src="../../Scripts/jQuery.fn.extend.js"></script>
     <script type="text/javascript">
         var setdivDate = null;
         $(document).ready(function () {
             parent.envdoc.methods.closeLayerLoad();
             $('body').css('background', 'none');
             var host = $("#currentWebUrl").val();
             var listId = $.getQueryString("listId");
             $("#btnpermissoinLink").click(function () {
                 var url = host + "/_layouts/15/user.aspx?obj=" + listId + ",doclib&List=" + listId;
                 window.open(url);
             });
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
         function ShowDateDiv() {
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
