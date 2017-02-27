<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Envision.SPS.Portal.Web.Layouts.EnvisionPortal.Home" MasterPageFile="Master/EnvisionPortal.master" %>

<%@ Import Namespace="Microsoft.SharePoint.WebPartPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="TopNavigation" Src="~/_controltemplates/15/EnvisionPortal/TopNavigation.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="CompanyInfo" Src="~/_controltemplates/15/EnvisionPortal/CompanyInfo.ascx" %>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Envision
</asp:Content>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PlaceHolderPageHead" runat="server">
    <link href="Scripts/layer/skin/layer.css" rel="stylesheet" />
    <link href="/_layouts/15/EnvisionPortal/Skins/Default/css/Portal.css" rel="stylesheet" />
    <link href="/_layouts/15/EnvisionPortal/Skins/Envision/CN/css/envision.css" rel="stylesheet" />
  <style type="text/css">
		#zz1_TopNavigationMenu_NavMenu_Edit{
				display:none;
			}
	</style>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="PlaceHolderPageMain" runat="server">
    <div class="bg_part">
        <img src="Skins/Default/images/bg.JPG" />
    </div>
    <div class="top">
        <div class="search_box" style="right:280px !important;">
            <input name="txtSearch" type="text" class="input" id="txtSearch"/>
        </div>
        <div class="user_box" style="width:220px !important;">欢迎您，<asp:Literal ID="ltlUser" runat="server"></asp:Literal></div>
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
        <%--<dl class="mbox_1">
            <dt></dt>
            <dd>
                <p>平台简介</p>
                <div>
                    远景能源以"为人类的可持续未来解决挑战"为使命，致力于引领全球能源行业的智慧变革。远景能源成立至今连续多年业务高速增长，已经成为全球领先的智慧能源技术服务提供商，业务包括智能风机的研发与销售、智慧风场软件和技术服务，研发能力和技术水平处于全球领先地位。目前集团员工总数接近700人，国际员工占20%，硕士和博士超过60%，研发及技术人员达到80%。<br>
                    远景能源以"为人类的可持续未来解决挑战"为使命，致力于引领全球能源行业的智慧变革。
                </div>
            </dd>
        </dl>--%>
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
                            <img src="Skins/default/images/pp_2.png" /></span>
                        <span class="jFlowControl1">
                            <img src="Skins/default/images/pp_2.png" /></span>
                        <span class="jFlowControl1">
                            <img src="Skins/default/images/pp_2.png" /></span>
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
    <asp:HiddenField ID="hidCurrentWebUrl" runat="server" />
    <script src="Scripts/jquery.min.js"></script>
    <script src="Scripts/jquery.nicescroll.min.js"></script>
    <script src="Scripts/layer/layer.js"></script>
    <script src="Scripts/jquery.flexslider-min.js"></script>
    <script src="Scripts/jquery.flow.1.2.auto.js"></script>
    <script src="Scripts/jQuery.fn.extend.js"></script>
    <script src="Scripts/global.js"></script>
    <script src="Scripts/EnvisionPortal.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            envportal.methods.homeInit();
            $("#companyProfile").niceScroll({ arrows: false, touchbehavior: false, cursorcolor: "#7C7C7C", cursorborder: "none",cursoropacitymax: 0, cursorwidth: 05 });
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
