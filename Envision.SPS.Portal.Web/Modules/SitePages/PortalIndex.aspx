<%@ Assembly Name="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Page Language="C#" Inherits="Microsoft.SharePoint.WebPartPages.WikiEditPage"  MasterPageFile="~/_catalogs/masterpage/Envision/EnvisionPortal.master" meta:webpartpageexpansion="full" meta:progid="SharePoint.WebPartPage.Document" %>
<%@ Import Namespace="Microsoft.SharePoint.WebPartPages" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="TopNavigation" Src="~/_controltemplates/15/EnvisionPortal/TopNavigation.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="CompanyInfo" Src="~/_controltemplates/15/EnvisionPortal/CompanyInfo.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="WelcomeLoginName" Src="~/_controltemplates/15/EnvisionPortal/WecomeLoginName.ascx" %>
<asp:Content ID="ContentTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Envision 远景能源门户
</asp:Content>
<asp:Content ID="ContentHead" ContentPlaceHolderID="PlaceHolderPageHead" runat="server">
    <link href="/_layouts/15/EnvisionPortal/Scripts/layer/skin/layer.css" rel="stylesheet" />
    <link href="/_layouts/15/EnvisionPortal/Skins/Default/css/Portal.css" rel="stylesheet" />
    <link href="/_layouts/15/EnvisionPortal/Skins/Envision/CN/css/envisionsp.css" rel="stylesheet" />
      <style type="text/css">
		#zz1_TopNavigationMenu_NavMenu_Edit{
				display:none;
			}
	</style>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="PlaceHolderPageMain" runat="server">
    <div class="bg_part">
        <img src="/_layouts/15/EnvisionPortal/Skins/Default/images/bg.JPG" />
    </div>
    <div class="top">
        <div class="search_box" style="right:280px !important;">
            <input name="txtSearch" type="text" class="input" id="txtSearch"/>
        </div>
        <div class="user_box" style="width:220px !important;">欢迎您， <wssuc:WelcomeLoginName id="IdWelcomeLoginName" runat="server" enableviewstate="false" /></div>
        <div class="nav_box">
           <%-- <wssuc:TopNavigation id="IdTopNavigation" runat="server" enableviewstate="false" />--%>
               <div id="topnavAreaRow" class="ms-tableRow">
                                <div id="topnavMiddleAreaCell" class="ms-tableCell ms-verticalAlignMiddle">
                                    <SharePoint:AjaxDelta id="DeltaTopNavigation" BlockElement="true" CssClass="ms-displayInline" runat="server">
									
                                    <SharePoint:DelegateControl runat="server" ControlId="TopNavigationDataSource" Id="topNavigationDelegate">
                                    <Template_Controls>
                                    <asp:SiteMapDataSource ShowStartingNode="False" SiteMapProvider="SPNavigationProvider" id="topSiteMap" runat="server" StartingNodeUrl="sid:1002" />
                                    </Template_Controls>
                                    </SharePoint:DelegateControl>
                                    <a name="startNavigation">
                                    </a>
                                   
                                    <SharePoint:AspMenu ID="TopNavigationMenu" Runat="server" EnableViewState="false" DataSourceID="topSiteMap" AccessKey="&lt;%$Resources:wss,navigation_accesskey%&gt;" UseSimpleRendering="true" UseSeparateCss="false" Orientation="Horizontal" StaticDisplayLevels="2" AdjustForShowStartingNode="true" MaximumDynamicDisplayLevels="5" SkipLinkText="" />
                                    
                                          </SharePoint:AjaxDelta>
                                </div>
                            </div>
        </div>

    </div>
    <div class="contant">
        <wssuc:CompanyInfo id="EnvisionCompanyInfo" runat="server" enableviewstate="false" />
        <div class="mbox_2">
            <div class="metro_1 metro_color_1"><a href="#" class="icon_4"><span>预留位置</span></a></div>
            <div class="metro_2 metro_color_2">
                <div class="weather_app">
                    <div id="slides1">
                    </div>
                    <div class="pp" id="myController1">
                        <span class="prev"></span>
                        <span class="next"></span>
                        <span class="jFlowControl1">
                            <img src="/_layouts/15/EnvisionPortal/Skins/default/images/pp_2.png" /></span>
                        <span class="jFlowControl1">
                            <img src="/_layouts/15/EnvisionPortal/Skins/default/images/pp_2.png" /></span>
                        <span class="jFlowControl1">
                            <img src="/_layouts/15/EnvisionPortal/Skins/default/images/pp_2.png" /></span>
                    </div>
                </div>
               
            </div>
            <div class="metro_3 metro_color_3">
                <p><a href="/Lists/EnvsionAnnouncement/AllItems.aspx" target="_blank"></a>公告</p>
                <div id="Announcement" style="height: 72px;display: block; overflow: hidden;">
                    
                </div>
            </div>
            <div class="metro_1 metro_color_4"><a href="/_layouts/envisionportal/pages/ToolKitLinks.aspx" class="icon_1"><span>常用系统入口</span></a></div>
            <div class="metro_1 metro_color_5"><a href="/_layouts/envisionportal/pages/DepartmentSupport/List.aspx" class="icon_3"><span>部门知识指导</span></a></div>
            <div class="metro_1 metro_color_6"><a style="cursor:pointer;" class="icon_2" id="helpDesk"><span>联系管理员</span></a></div>
        </div>
    </div>
    <asp:HiddenField ID="hidCurrentWebUrl" runat="server" Value="/" />
    <script src="/_layouts/15/EnvisionPortal/Scripts/jquery.min.js"></script>
    <script src="/_layouts/15/EnvisionPortal/Scripts/jquery.nicescroll.min.js"></script>
    <script src="/_layouts/15/EnvisionPortal/Scripts/layer/layer.js"></script>
    <script src="/_layouts/15/EnvisionPortal/Scripts/jquery.flexslider-min.js"></script>
    <script src="/_layouts/15/EnvisionPortal/Scripts/jquery.flow.1.2.auto.js"></script>
    <script src="/_layouts/15/EnvisionPortal/Scripts/jQuery.fn.extend.js"></script>
    <script src="/_layouts/15/EnvisionPortal/Scripts/global.js"></script>
    <script src="/_layouts/15/EnvisionPortal/Scripts/EnvisionPortal.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            envportal.methods.homeInit();
            $("#companyProfile").niceScroll({ arrows: false, touchbehavior: false, cursorcolor: "#7C7C7C", cursorborder: "none", cursoropacitymax: 0, cursorwidth: 05 });
        });
        function addNavtigationCss(_this) {
            $(".hover").attr("class", "");
            $(_this).children("dt").children("a").attr("class", "hover");

        }
        function removeNavtigationCss(_this) {
            $(_this).children("a").attr("class", "");
        }
    </script>
</asp:Content>