<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ITSettings.aspx.cs" Inherits="Envision.SPS.Document.Web.Layouts.ITSettings" MasterPageFile="EnvisionDoc/Master/EnvisionSeattle.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link href="/_layouts/15/EnvisionDoc/Scripts/layer/skin/layer.css" rel="stylesheet" />
    <link rel="stylesheet" href="/_layouts/15/Styles/EnvisionDoc/global.css" charset="UTF-8" />
    <link rel="stylesheet" href="/_layouts/15/Styles/EnvisionDoc/app.css" charset="UTF-8" />
    <link rel="stylesheet" href="/_layouts/15/Styles/EnvisionDoc/document.css" charset="UTF-8" />
    <%--<link rel="stylesheet" href="/_layouts/15/EnvisionDoc/Plugins/FontAwesome/css/font-awesome.min.css" charset="UTF-8" />
    <link rel="stylesheet" href="/_layouts/15/EnvisionDoc/Plugins/Validform/css/style.css"  charset="UTF-8" />
    <link href="/_layouts/15/EnvisionDoc/Plugins/My97DatePicker/skin/WdatePicker.min.css" rel="stylesheet" />--%>
    <link href="/_layouts/15/EnvisionDoc/Scripts/zTree/css/demo.css" rel="stylesheet" />
    <link href="/_layouts/15/EnvisionDoc/Scripts/zTree/css/zTreeStyle/zTreeStyle.css" rel="stylesheet" />
    <link href="/_layouts/15/EnvisionDoc/Skins/Default/css/index.css" rel="stylesheet" />
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

                <div class="leftPane">
                    <div id="tree" style="overflow: hidden;">
                        <div class="treeBox">
                             <ul id="treeDemo" class="ztree"></ul>
                        </div>
                    </div>
                </div>

            </div>
            <div id="handleBarSection" style="z-index:10;"> </div>
            <div id="rightSection">
                    <iframe id="mainframe" style="height: 100%;" width="100%" scrolling="auto" frameborder="0"></iframe>
            </div>
        </div>
    </div>

    <script type="text/javascript" src="/_layouts/15/EnvisionDoc/Scripts/jquery.min.js" charset="UTF-8"></script>
    <script src="/_layouts/15/EnvisionDoc/Scripts/jquery.nicescroll.min.js"></script>
    <script src="/_layouts/15/EnvisionDoc/Scripts/layer/layer.js"></script>
    <script type="text/javascript" src="/_layouts/15/EnvisionDoc/Scripts/jQuery.fn.extend.js" charset="UTF-8"></script>
    <script src="EnvisionDoc/Scripts/zTree/js/jquery.ztree.all.js"></script>
    <script type="text/javascript" src="/_layouts/15/EnvisionDoc/Scripts/global.js" charset="UTF-8"></script>
    <script type="text/javascript" src="/_layouts/15/EnvisionDoc/Scripts/layoutsDrag.js" charset="UTF-8"></script>
    <%--<script type="text/javascript" src="/_layouts/15/EnvisionDoc/Scripts/Tree.js"></script>--%>
    <script src="EnvisionDoc/Skins/Default/scripts/encisiondoc.js"></script>

    <script type="text/javascript" charset="UTF-8">
        $(document).ready(function () {
            //$("#leftSection").height(document.body.clientHeight);
            $(".treeBox").niceScroll({ arrows: false, touchbehavior: false, cursorcolor: "#7C7C7C", cursorborder: "none", cursoropacitymax: 0, cursorwidth: 0 });
            //$(".leftPane").niceScroll({ touchbehavior: false, cursorcolor: "#7C7C7C", cursoropacitymax: 0, cursorwidth: 0 });
            layoutsDray.methods.itinstance();
            envdoc.methods.itTreeInit();
        });

    </script>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Envision IT Document Site Setting
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Envision IT Document Site Setting 
</asp:Content>
