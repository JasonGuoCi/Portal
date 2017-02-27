<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocSettings.aspx.cs" Inherits="Envision.SPS.Document.Web.Layouts.DocSettings" MasterPageFile="EnvisionDoc/Master/EnvisionSeattle.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
     <link href="/_layouts/15/EnvisionDoc/Scripts/layer/skin/layer.css" rel="stylesheet" />
    <link rel="stylesheet" href="/_layouts/15/Styles/EnvisionDoc/global.css" charset="UTF-8" />
    <link rel="stylesheet" href="/_layouts/15/Styles/EnvisionDoc/app.css" charset="UTF-8" />
    <link rel="stylesheet" href="/_layouts/15/Styles/EnvisionDoc/document.css" charset="UTF-8" />
    <link rel="stylesheet" href="/_layouts/15/EnvisionDoc/Plugins/FontAwesome/css/font-awesome.min.css"
        charset="UTF-8" />
    <link rel="stylesheet" href="/_layouts/15/EnvisionDoc/Plugins/Validform/css/style.css"
        charset="UTF-8" />
    <link href="/_layouts/15/EnvisionDoc/Plugins/My97DatePicker/skin/WdatePicker.min.css" rel="stylesheet" />
    <link href="EnvisionDoc/Skins/Default/css/index.css" rel="stylesheet" />
<%--    <link href="EnvisionDoc/Skins/Default/css/accordion.css" rel="stylesheet" />--%>
    <style type="text/css">
        #s4-titlerow, #sideNavBox, #fullscreenmodebox {
            display: none !important;
        }

        #contentBox {
            margin: 0 !important;
        }

        #s4-bodyContainer, #contentRow {
            padding: 0 !important;
        }

        .sbar span {
            position: absolute;
            top: 0;
            left: 70px;
            right: 40px;
            text-align: center;
            line-height: 32px;
            color: red;
            display: none;
        }

            .sbar span em {
                margin: 0 6px;
            }
    </style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <a href="javascript:void(0);" id="actionLink" style="display: none;"></a>
    <div class="secpage_box document_box">
        <div class="contentsSection">
            <div id="leftSection">
                <div id="tree">
                    <div class="contant">
                        <div class="left">
                            <div class="submenu" id="leftMenu">

                            </div>
                          </div>
                      </div>
                  </div>

            </div>
            <div id="handleBarSection">
            </div>
            <div id="rightSection">
                <div class="header ">
                    <div class="rightpane_file" style="padding-bottom: 0;">
                        <div class="filebox">
                        </div>
                    </div>
                </div>
                <div id="rightFrameZone" style="margin-top: 10px;">
                    <iframe id="mainframe" style="height: 100%;" width="100%" scrolling="auto" frameborder="0"></iframe>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript" src="/_layouts/15/EnvisionDoc/Scripts/jquery.min.js" charset="UTF-8"></script>
    <script src="/_layouts/15/EnvisionDoc/Scripts/layer/layer.js"></script>
    <script src="/_layouts/15/EnvisionDoc/Scripts/jquery.nicescroll.min.js"></script>
    <script type="text/javascript" src="/_layouts/15/EnvisionDoc/Scripts/jQuery.fn.extend.js" charset="UTF-8"></script>
    <script type="text/javascript" src="/_layouts/15/EnvisionDoc/Scripts/global.js" charset="UTF-8"></script>
    <script type="text/javascript" src="/_layouts/15/EnvisionDoc/Scripts/layoutsDrag.js" charset="UTF-8"></script>

    <script type="text/javascript" src="EnvisionDoc/Skins/Default/scripts/encisiondoc.js"></script>

    <script type="text/javascript" src="/_layouts/15/EnvisionDoc/Scripts/document.js" charset="UTF-8"></script>
    <script type="text/javascript" charset="UTF-8">
        $(document).ready(function () {
           
            layoutsDray.methods.instance();
            envdoc.methods.instance();
            udoc.methods.instance();

        });

        function ShowTaskPage(url, _this) {
            $("#mainframe").attr("src", "");
            $("#mainframe").attr("src", url);
            var sonname = $(_this).attr("id");

            var parname = $(_this).parent().siblings().children().html();
            $(".hover").attr("class", "menu");
            $(_this).attr("class", "menu hover");
            $(".filebox").html("<a href='#'>" + parname + "</a><span>&gt;</span><a href='javascript:void(0);'>" + sonname + "</a>");
        }
    </script>
    <asp:HiddenField ID="hidCurrentWebUrl" Value="" runat="server"/>
    <asp:HiddenField ID="hidIsWebManager" Value="" runat="server" />
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Envision Document Site Setting
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Envision Document Site Setting 
</asp:Content>
