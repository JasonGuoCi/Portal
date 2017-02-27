<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocCurrentSiteLeftTree.ascx.cs" Inherits="Envision.SPS.Document.Web.ControlTemplates.EnvisionDoc.DocCurrentSiteLeftTree" %>
<%@ Register Assembly="Telerik.Web.UI,Version=2010.02.0713.0, Culture=neutral, PublicKeyToken=29ac1a93ec063d92" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<div class="divtree" style="overflow-y:auto;overflow-x:hidden">
    <telerik:RadTreeView Skin="Office2007" ID="rtvDocumentTreeView" runat="server" Width="100%"
         OnNodeExpand="rtvDocumentTreeView_NodeExpand" OnClientNodeExpanding="OnClientNodeExpanding" OnClientNodeClicking="OnClientNodeClicking"
        OnClientLoad="OnClientLoad" style="overflow:hidden !important;">
        <ContextMenus>
            <telerik:RadTreeViewContextMenu ID="MainContextMenu" runat="server">
                <Items>
                    <%--<telerik:RadMenuItem  Value="UpLoadFile" Text="UpLoadFile">
                            </telerik:RadMenuItem>
                            <telerik:RadMenuItem Value="CheckInForAll" Text="CheckInForAll">
                            </telerik:RadMenuItem>
                             <telerik:RadMenuItem Value="GetFolderPath" runat="Server" Id="idGetFolderPath" Text="GetFolderPath">
                            </telerik:RadMenuItem>
                             <telerik:RadMenuItem Value="ManagerFolder" runat="Server" Id="idManagerFolder" Text="ManagerFolder">
                            </telerik:RadMenuItem>--%>
                </Items>
                <CollapseAnimation Type="none"></CollapseAnimation>
            </telerik:RadTreeViewContextMenu>
        </ContextMenus>
    </telerik:RadTreeView>
    <script type="text/javascript">

        var cookie = {
            set: function (key, val) {//设置cookie方法
                var date = new Date(); //获取当前时间
                //var expiresDays = time;  //将date设置为n天以后的时间
                //date.setTime(date.getTime() + expiresDays * 24 * 3600 * 1000); //格式化为cookie识别的时间
                document.cookie = key + "=" + val + ";path=/";  //设置cookie
            },
            get: function (key) {//获取cookie方法
                /*获取cookie参数*/
                var getCookie = document.cookie.replace(/[ ]/g, "");  //获取cookie，并且将获得的cookie格式化，去掉空格字符
                var arrCookie = getCookie.split(";")  //将获得的cookie以"分号"为标识 将cookie保存到arrCookie的数组中
                var tips;  //声明变量tips
                for (var i = 0; i < arrCookie.length; i++) {   //使用for循环查找cookie中的tips变量
                    var arr = arrCookie[i].split("=");   //将单条cookie用"等号"为标识，将单条cookie保存为arr数组
                    if (key == arr[0]) {  //匹配变量名称，其中arr[0]是指的cookie名称，如果该条变量为tips则执行判断语句中的赋值操作
                        tips = arr[1];   //将cookie的值赋给变量tips
                        break;   //终止for循环遍历
                    }
                }
                return tips;
            }, del: function (key) {
                document.cookie = key + "=; expires=Fri, 21 Dec 1976 04:31:24 GMT;";
            }
        }



        //节点点击事件
        function OnClientNodeClicking(sender, e) {
            var rtvDocumentTreeView = $find("<%=rtvDocumentTreeView.ClientID%>");
            var treeNode = e.get_node();
            var treeNodeValue = treeNode.get_value();
            var treeNodeCategory = treeNode.get_category();
            cookie.del("treeNodeValue");
            cookie.set("treeNodeValue", treeNodeValue);
        }

        function OnClientNodeExpanding(sender, e) {
            //左侧拖动条初始化
            docLeftMenuDrag.methods.setSize();
         }


        function OnClientLoad() {
            //alert("OnClientLoad");
            var currentNodeValue = $(".breadcrumbCurrentNode").text();
            if (currentNodeValue.toUpperCase() == "HOME") {
                cookie.del("treeNodeValue");
                cookie.set("treeNodeValue", "");
            }
            var currentSelectValue = cookie.get("treeNodeValue");
            var rtvDocumentTreeView = $find("<%=rtvDocumentTreeView.ClientID%>");
            var treeNode = rtvDocumentTreeView.findNodeByValue(currentSelectValue);
            if (treeNode != null) {
                treeNode.select(); //Focus当前节点
                treeNode.expand();
            } else {
                ;
            }
        }

        function refreshTreeNode(treeNode) {
            treeNode.select();
            treeNode.get_nodes().clear();
            treeNode.set_expanded(false);
            treeNode.set_expandMode(Telerik.Web.UI.TreeNodeExpandMode.ServerSideCallBack)
            treeNode.expand();
        }


        function openWindow(width, height, actionUrl) {
            var features = "dialogWidth:" + width + "px;dialogHeight:" + height + "px;scroll:yes;status:no;location:no;resizable:no;";
            return window.showModalDialog(actionUrl, window, features);
        }

    </script>
</div>
