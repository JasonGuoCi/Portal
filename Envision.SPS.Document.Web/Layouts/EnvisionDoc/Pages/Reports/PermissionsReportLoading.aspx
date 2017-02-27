<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PermissionsReportLoading.aspx.cs" Inherits="Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.Reports.PermissionsReportLoading" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>PermissionsReportLoading</title>
    <style type='text/css'>
        table.gridtable {
            font-family: verdana,arial,sans-serif;
            font-size: 11px;
            color: #333333;
            border-width: 1px;
            border-color: #666666;
            border-collapse: collapse;
            width: 530px;
        }

            table.gridtable th {
                border-width: 1px;
                padding: 8px;
                border-style: solid;
                border-color: #666666;
                background-color: #dedede;
            }

            table.gridtable td {
                border-width: 1px;
                padding: 8px;
                border-style: solid;
                border-color: #666666;
                background-color: #ffffff;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="float: left; margin: 0 auto; width: 100%; text-align: center;">
            <div style="margin: 0 auto;" id="loading">
                <img src="/_layouts/15/envisionDoc/Scripts/layer/skin/default/loading-2.gif" style="vertical-align: top;" /><span style="font-size: 30px;">报表导出中,请稍等······</span>
            </div>
            <div style="margin: 0 auto; display: none;" id="download">
                <table class='gridtable' style="margin: auto;" id="downtable">
                    <tr>
                        <th>报表名称</th>
                        <th>操作</th>
                    </tr>
                </table>
            </div>
            <div style="margin: 0 auto; display: none;font-family:Microsoft Yahei,微软雅黑, Arial, Helvetica, sans-serif;font-size: 30px;" id="handleerror">
                
            </div>
        </div>
        <asp:HiddenField ID="hidId" runat="server" />
    </form>
    <script src="/_layouts/15/envisionDoc/Scripts/jquery.min.js"></script>
    <script src="/_layouts/15/envisionDoc/Scripts/json2.js"></script>
    <script>
        $(document).ready(function () {
            function GetHandleResult() {
                $.ajax({
                    type: "POST",  //提交方式
                    url: "/_layouts/15/EnvisionDoc/Handlers/AjaxHandler.ashx?MethodName=16&id=" + $("#hidId").val(),//路径
                    async: true,
                    success: function (result) {//返回数据根据结果进行相应的处理
                        var d = JSON.parse(result)
                        result = d.status;
                        if (d.status == "y") {
                            //成功
                            stopInterval();
                            var download = $("#download");
                            var downtable = $("#downtable");
                            $("#loading").hide();
                            $("#handleerror").hide();
                            download.show();
                            download.find("table tr:not(:first)").html("");
                            var fileNameArr = d.result.FileName.split(',');
                            var id = d.result.ID;
                            $.each(fileNameArr, function (i, n) {
                                var row = $("<tr></tr>");
                                var column1 = $("<td>" + fileNameArr[i].split('_')[0] + "</td>");
                                var column2 = $("<td></td>");
                                var link = $("<a target='_blank' href='/_layouts/15/EnvisionDoc/pages/DownLoadFile.aspx?id=" + id + "&FileName=" + encodeURIComponent(fileNameArr[i]) + "'>" + fileNameArr[i].split('_')[0] + "权限报表</a>");
                                link.appendTo(column2);
                                column1.appendTo(row);
                                column2.appendTo(row);
                                row.appendTo(downtable);
                            });
                        } else if (d.status == "error") {
                            //错误
                            stopInterval();
                            $("#loading").hide();
                            $("#download").hide();
                            $("#handleerror").show();
                            $("#handleerror").html("处理失败，发生异常");
                        }else if(d.status=="null")
                        {
                            stopInterval();
                            $("#loading").hide();
                            $("#download").hide();
                            $("#handleerror").show();
                            $("#handleerror").html("没有找到提交的请求");
                        }
                    }
                });
                //return result;
            }
            var intervalId = window.setInterval(GetHandleResult, 10000);
            function stopInterval() {
                clearInterval(intervalId);
            }
        });
    </script>
</body>


</html>
