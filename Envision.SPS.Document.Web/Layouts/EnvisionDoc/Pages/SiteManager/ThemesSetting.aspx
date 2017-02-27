<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
 <%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ThemesSetting.aspx.cs" Inherits="Envision.SPS.Document.Web.Layouts.EnvisionDoc.Pages.SiteManager.ThemesSetting"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <title></title>
           <link href="../../Skins/Default/css/index.css"  rel="stylesheet" />
           <link href="../../Scripts/layer/skin/layer.css" rel="stylesheet" />
           <script src="../../Scripts/jquery.min.js"></script>
        <script>
            jQuery(function () {
                var radioId=<%=RadioSelectId%>;
                $(radioId).attr("checked","checked");

            })
            ///拿取皮肤主题
            function ClickRad(obj) {
                document.getElementById("ThemeVlues").value = "";
                document.getElementById("ThemeVlues").value = obj.value;
            }

        </script>
<style>

      body {
          margin: 0 auto;
          background: #fff;
      }
</style>
    </head>
    <body> 
     <div class="maincontains">
            <div class="right" style="width:1031px; margin-top:20px;">
               
                <div class="itmbox">
                    <ul class="change_skin">
                        <li><img src="../../../images/EnvisionDoc/Themes/Theme_01.png" /><p>办公室(蓝色)</p><em><input type="radio" runat="server" value="/_catalogs/theme/15/palette001.spcolor" id="theme1" class="theme" name="theme" /></em></li>                                                                                                                       
                        <li><img src="../../../images/EnvisionDoc/Themes/Theme_02.png" /><p>春天里(绿色)</p><em><input type="radio" runat="server" value="/_catalogs/theme/15/palette013.spcolor" id="theme2" class="theme" name="theme" /></em></li>
                        <li><img src="../../../images/EnvisionDoc/Themes/Theme_03.png" /><p>大海怪(橘红)</p><em><input type="radio" runat="server" value="/_catalogs/theme/15/palette005.spcolor" id="theme3" class="theme" name="theme" /></em></li>
                    </ul>
                </div>
                 <form runat="server"><input type="hidden" value="" id="ThemeVlues" runat="server" /><asp:Button ID="BtnSaveTheme" name="BtnSaveTheme" runat="server" Text="保存皮肤"  style="margin-bottom: 10px;margin-left: 430px;padding: 0px 40px;border: 0px;margin-right: 20px;text-align: center;line-height: 40px;color: #fff;font-size: 14px;background: #39f;"  class="btn1" OnClientClick="changetheme(this);return false" OnClick="BtnSaveTheme_Click"  /></form>
            </div>
         </div>

    </body>
<script src="../../Scripts/layer/layer.js"></script>
    <script>
        function changetheme(_this)
        {
           var index= layer.confirm("确定更换皮肤风格?",{btn:['确定','取消'],id:'layui-layer2'},function(){
                $(".theme").each(function(){
                    if($(this).attr("checked")=="checked")
                    {
                        $("#ThemeVlues").val($(this).attr("value"));
                    }
                })
                if( $("#ThemeVlues").val()=="") return false;
               $("#BtnSaveTheme").attr("disable","disable");
               layer.close(index);
               layer.load(1);
               _this.onclick="";
               _this.click();
            });
        }
        <%=this.ThemResult%>
    </script>
</html>
