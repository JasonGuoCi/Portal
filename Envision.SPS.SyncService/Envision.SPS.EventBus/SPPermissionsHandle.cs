using Envision.SPS.DataAccess;
using Envision.SPS.Utility.Models;
using Envision.SPS.Utility.Utilities;
using Microsoft.SharePoint;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Exchange.WebServices.Data;

namespace Envision.SPS.EventBus
{
    /// <summary>
    /// 权限表处理类
    /// </summary>
    public class SPPermissionsHandle
    {
        //EFDBBase efdbb = new EFDBBase();
        EventBusDAL dal = new EventBusDAL();

        #region 1取数据
        public DataAccess.SPS_EventBus GetEnventBusItem()
        {
            DataAccess.SPS_EventBus enventItem = null;
            try
            {
                enventItem = dal.SP_Envision_GetEventBusModel(); ///获取单条数据实体
            }
            catch (Exception ex)
            {
                LogService.WriteLog("运行失败!错误原因：" + ex.Message);
            }
            return enventItem;
        }
        #endregion


        #region 2Excel处理
        public void StartExportExcel(DataAccess.SPS_EventBus enventItem)
        {
            try
            {
                string oFilePath = System.Configuration.ConfigurationManager.AppSettings["TemplateExcelPath"].ToString();
                if (enventItem == null)
                {
                    LogService.WriteLog("运行失败!错误原因：实体数据为空!");
                }
                else
                {
                    string[] labrarys = enventItem.ListID.Split(',');
                    SPSecurity.RunWithElevatedPrivileges(() =>
                    {
                        using (SPSite site = new SPSite(new Guid(enventItem.SiteID)))
                        {
                            using (SPWeb web = site.OpenWeb(new Guid(enventItem.WebID)))
                            {
                                for (int i = 0; i < labrarys.Length; i++)
                                {
                                    ///拷贝
                                    LogService.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始拷贝模板文件!");
                                    System.IO.File.Copy(oFilePath, enventItem.FilePath + "/" + enventItem.FileName.Split(',')[i], true);
                                    FileStream file = new FileStream(enventItem.FilePath + "/" + enventItem.FileName.Split(',')[i], FileMode.Open, FileAccess.Read);
                                    XSSFWorkbook hssfworkbook = new XSSFWorkbook(file);
                                    hssfworkbook.RemoveSheetAt(0);
                                    XSSFSheet DataSheet = (XSSFSheet)hssfworkbook.CreateSheet("" + web.Lists[new Guid(labrarys[i])].Title + "权限表");
                                    ICellStyle cellStyle = hssfworkbook.CreateCellStyle();
                                    LogService.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "XSSFSheet,ICellStyle创建成功!");
                                    this.ExcelPermissionsExport(oFilePath, enventItem.FilePath, cellStyle, DataSheet, new Guid(labrarys[i]), hssfworkbook, web);
                                    FileStream file2 = null;
                                    file2 = new FileStream(enventItem.FilePath + "/" + enventItem.FileName.Split(',')[i], FileMode.Create);
                                    hssfworkbook.Write(file2);
                                    file2.Close();
                                }
                            }
                        }
                    });
                    LogService.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始发送邮件!");
                    SendEmail("", enventItem);
                }
            }
            catch (Exception ex)
            {
                LogService.WriteLog("运行失败!错误原因：" + ex.Message);
                dal.UpdateSPS_EventBusStateError(enventItem);
            }
        }
        #endregion

        #region 3发邮件
        private bool SendEmail(string mailTemplater, DataAccess.SPS_EventBus enventItem)
        {
            bool sendResult = true;
            try
            {
                MailMessage mailT = new MailMessage();
                List<string> emailsList = new List<string>();

                if (!string.IsNullOrEmpty(enventItem.UserEmail))
                {
                    emailsList.Add(enventItem.UserEmail);
                    //Just For Test
                    for (int i = 0; i < emailsList.Count; i++)
                    {
                        if (emailsList[i] != "")
                        {
                            mailT.To.Add(emailsList[i]);
                        }
                    }
                    string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                    mailT.IsBodyHtml = true;
                    mailT.Subject = "权限报表导出信息";
                    string mailStaticBodyStyle = @"<style type='text/css'>

                                                body{font-family: verdana,arial,sans-serif;}

                                                table.gridtable {
	                                                font-family: verdana,arial,sans-serif;
	                                                font-size:11px;
	                                                color:#333333;
	                                                border-width: 1px;
	                                                border-color: #666666;
	                                                border-collapse: collapse;
                                                        width:530px;
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
                                                </style>";

                    StringBuilder mailStaticBody = new StringBuilder();
                    mailStaticBody.Append("<body>");
                    mailStaticBody.Append("<p>Dear " + enventItem.UserName + ",</p>");
                    mailStaticBody.Append("<p>您于" + enventItem.EventTime.ToString() + ",权限报表已经生成成功!</p>");
                    mailStaticBody.Append("<table class='gridtable'>");
                    mailStaticBody.Append("<tr><th>报表名称</th><th>操作</th></tr>    ");
                    for (int i = 0; i < enventItem.FileName.Split(',').Length; i++)
                    {
                        mailStaticBody.Append("<tr><td>" + enventItem.FileName.Split(',')[i].Split('_')[0] + "</td><td><a href=\"" + System.Configuration.ConfigurationManager.AppSettings["ServerUrl"].ToString() + "_layouts/15/EnvisionDoc/pages/DownLoadFile.aspx?id=" + enventItem.ID + "&FileName=" + HttpUtility.UrlEncode(enventItem.FileName.Split(',')[i]) + "\">" + enventItem.FileName.Split(',')[i].Split('_')[0] + "权限报表</a></td></tr>");
                    }
                    mailStaticBody.Append("</table>");
                    mailStaticBody.Append("<p>Thanks,</p>");
                    mailStaticBody.Append("<p>SharePoint系统</p>");
                    mailStaticBody.Append("</body>");
                    mailT.Body = mailStaticBodyStyle + mailStaticBody;
                    mailT.BodyEncoding = Encoding.GetEncoding(936);
                    sendResult = SendMail(mailT);
                }
                if (sendResult) { LogService.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "邮件发送成功,修改当前数据为成功状态!"); UpdateEnventItem(enventItem); } else { LogService.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "邮件发送失败!"); dal.UpdateSPS_EventBusStateError(enventItem); }


            }
            catch (Exception ex)
            {
                dal.UpdateSPS_EventBusStateError(enventItem);
                LogService.WriteLog("运行失败!错误原因：" + ex.Message);
            }
            return sendResult;
        }
        private static ExchangeService service = null;
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailMessage">邮件信息对象</param>
        /// <returns></returns>
        private static bool SendMail(MailMessage mailMessage)
        {
            try
            {
                service = ConnectExchangeService();
                EmailMessage message = new EmailMessage(service);
                message.Subject = mailMessage.Subject;
                message.Body = mailMessage.Body;
                foreach (MailAddress address in mailMessage.To)
                {
                    if (address.ToString() != "") message.ToRecipients.Add(address.ToString());
                }
                message.Send();
                return true;
            }
            catch (Exception ex)
            {
                LogService.WriteLog("运行失败!错误原因：" + ex.Message);
                return false;
            }

        }
        #endregion

        #region 4更新状态
        private bool UpdateEnventItem(DataAccess.SPS_EventBus item)
        {
            bool result = false;
            try
            {

                result = dal.UpdateSPS_EventBus(item);
            }
            catch (Exception ex)
            {
                LogService.WriteLog("运行失败!错误原因：" + ex.Message);
            }
            return result;

        }
        #endregion


        #region SharePoint Common Method

        /// <summary>
        /// 判断用户是否为每个人
        /// </summary>
        /// <param name="name">用户组名称</param>
        /// <returns></returns>
        private bool IsMemberName(string name)
        {
            if (name == "每个人" || name.ToLower() == "everyone")
                return true;
            else
                return false;
        }
        private List<SPPermissionsModel> ReadFileBrary(Guid libraryName, SPWeb web)
        {

            List<SPPermissionsModel> customizePermissionCollection = new List<SPPermissionsModel>();
            SPPermissionsModel jumodel = new SPPermissionsModel();

            SPList currentList = web.Lists[libraryName];
            jumodel.JTile = currentList.Title;
            jumodel.JType = currentList.HasUniqueRoleAssignments == true ? "独有权限" : "继承"; ///获取文件权限是否为继承上一级的
            jumodel.JPath = currentList.ParentWebUrl + "" + jumodel.JTile + "";

            ///获取文件库的权限小组（循环获取）
            SPRoleAssignmentCollection sprCollection = currentList.RoleAssignments;
            StringBuilder sbu = new StringBuilder();
            StringBuilder sbtwo = new StringBuilder();
            foreach (SPRoleAssignment Spritem in sprCollection)
            {
                string newSbtwo = "";
                ///获取权限级别信息
                SPRoleDefinitionBindingCollection sprdbc = Spritem.RoleDefinitionBindings;
                foreach (SPRoleDefinition sprrditem in sprdbc)
                {
                    newSbtwo += sprrditem.Name + ";";
                }
                if (newSbtwo == "受限访问;") continue;
                if (Spritem.Member.Name == "样式资源读者") continue;
                sbtwo.Append(newSbtwo + "&");
                jumodel.JAssignmentCollection += Spritem.Member.Name + "&";
                if (!IsMemberName(Spritem.Member.Name))
                {
                    //获取用户组下的用户名称
                    if (Spritem.Member is SPUser)
                    {
                        SPUser user = Spritem.Member as SPUser;
                        sbu.Append("" + user.Name + ";");
                    }
                    else
                    {
                        SPGroup spgroup = web.SiteGroups[Spritem.Member.Name];  ///获取该用户组下的用户名称
                        if (spgroup.Users.Count == 0)
                        {
                            sbu.Append("暂无用户;");
                        }
                        else
                        {
                            //获取用户组下的用户名称
                            foreach (SPUser spusersitem in spgroup.Users)
                            {
                                sbu.Append("" + spusersitem.Name + ";");
                            }
                        }
                    }
                    sbu.Append("&");
                }
                else
                {
                    sbu.Append("每个人;");
                    sbu.Append("&");
                }
            }
            //统一去掉model内属性字段内的字符串的最后一个字符(‘&’)
            jumodel.JDefinitionCollection = sbtwo.ToString();
            jumodel.JAssignmentCy = sbu.ToString();
            if (jumodel.JAssignmentCy != null && jumodel.JAssignmentCy != "") jumodel.JAssignmentCy = jumodel.JAssignmentCy.Substring(0, jumodel.JAssignmentCy.Length - 1); else jumodel.JAssignmentCy = jumodel.JAssignmentCy;
            if (jumodel.JAssignmentCollection != null && jumodel.JAssignmentCollection != "") jumodel.JAssignmentCollection = jumodel.JAssignmentCollection.ToString().Substring(0, jumodel.JAssignmentCollection.Length - 1); else jumodel.JAssignmentCollection = jumodel.JAssignmentCollection;
            if (jumodel.JDefinitionCollection != null && jumodel.JAssignmentCollection != "") jumodel.JDefinitionCollection = jumodel.JDefinitionCollection.Substring(0, jumodel.JDefinitionCollection.Length - 1); else jumodel.JDefinitionCollection = jumodel.JDefinitionCollection;
            customizePermissionCollection.Add(jumodel);
            if (currentList.RootFolder.SubFolders.Count > 0)
            {
                ReadFileBraryBySubFolder(web, currentList, currentList.RootFolder, ref customizePermissionCollection);
            }
            return customizePermissionCollection;
        }

        private void ReadFileBraryBySubFolder(SPWeb web, SPList splist, SPFolder folder, ref List<SPPermissionsModel> customizePermissionCollection)
        {
            SPListItemCollection folderItems = GetSubFolderItems(splist, folder);
            try
            {
                foreach (SPListItem folderItem in folderItems)
                {

                    if (folderItem.Name == "Forms" || folderItem.FileSystemObjectType != SPFileSystemObjectType.Folder) continue;
                    SPPermissionsModel jurcModel = new SPPermissionsModel();
                    StringBuilder sbuJAssignmentCy = new StringBuilder();
                    StringBuilder sbuDefinition = new StringBuilder();
                    //SPListItem currentFolder = currentList.Folders[i];
                    jurcModel.JTile = folderItem.Name; ///获取文件夹de名称
                    jurcModel.JType = folderItem.HasUniqueRoleAssignments == true ? "独有权限" : "继承"; //判断权限是否授权
                    jurcModel.JPath = folderItem.Url;
                    //获取用户组名称
                    SPRoleAssignmentCollection sprAssignment = folderItem.RoleAssignments;
                    foreach (SPRoleAssignment spritem in sprAssignment)
                    {
                        string newSbtwo = "";
                        ///获取权限级别信息
                        SPRoleDefinitionBindingCollection sprdbc = spritem.RoleDefinitionBindings;
                        foreach (SPRoleDefinition sprdfitem in sprdbc)
                        {
                            newSbtwo += sprdfitem.Name + ";";
                        }
                        if (newSbtwo == "受限访问;") continue;
                        if (spritem.Member.Name == "样式资源读者") continue;
                        sbuDefinition.Append(newSbtwo + "&");
                        jurcModel.JAssignmentCollection += spritem.Member.Name + ";&";
                        if (!IsMemberName(spritem.Member.Name)) //判断用户组的名称是否等于“每个人”
                        {
                            if (spritem.Member is SPUser)
                            {
                                SPUser user = spritem.Member as SPUser;
                                sbuJAssignmentCy.Append("" + user.Name + ";");
                            }
                            else
                            {
                                //获取用户组内包含的用户
                                SPGroup spgroup = web.SiteGroups[spritem.Member.Name];
                                if (web.SiteGroups[spritem.Member.Name].Users.Count == 0)
                                {
                                    sbuJAssignmentCy.Append("暂无用户;");
                                }
                                else
                                {
                                    //获取用户组内包含的用户
                                    foreach (SPUser spusers in spgroup.Users)
                                    {
                                        sbuJAssignmentCy.Append("" + spusers.Name + ";");
                                    }
                                }
                            }
                            sbuJAssignmentCy.Append("&");
                        }
                        else
                        {
                            sbuJAssignmentCy.Append("每个人;");
                            sbuJAssignmentCy.Append("&");
                        }
                    }
                    //统一去掉model内属性字段内的字符串的最后一个字符(‘&’)
                    jurcModel.JDefinitionCollection = sbuDefinition.ToString();
                    jurcModel.JAssignmentCy = sbuJAssignmentCy.ToString();
                    if (jurcModel.JAssignmentCy != null && jurcModel.JAssignmentCy != "") jurcModel.JAssignmentCy = jurcModel.JAssignmentCy.Substring(0, jurcModel.JAssignmentCy.Length - 1); else jurcModel.JAssignmentCy = jurcModel.JAssignmentCy;
                    if (jurcModel.JAssignmentCollection != null && jurcModel.JAssignmentCollection != "") jurcModel.JAssignmentCollection = jurcModel.JAssignmentCollection.ToString().Substring(0, jurcModel.JAssignmentCollection.Length - 1); else jurcModel.JAssignmentCollection = jurcModel.JAssignmentCollection;
                    if (jurcModel.JDefinitionCollection != null && jurcModel.JDefinitionCollection != "") jurcModel.JDefinitionCollection = jurcModel.JDefinitionCollection.Substring(0, jurcModel.JDefinitionCollection.Length - 1); else jurcModel.JDefinitionCollection = jurcModel.JDefinitionCollection;
                    customizePermissionCollection.Add(jurcModel);
                    if (folderItem.Folder.SubFolders.Count > 0) { ReadFileBraryBySubFolder(web, splist, folderItem.Folder, ref customizePermissionCollection); }
                }
                LogService.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "文件夹信息获取完毕!");
            }
            catch (Exception ex) { }


        }
        private SPListItemCollection GetSubFolderItems(SPList list, SPFolder folder)
        {
            //创建查询条件
            SPQuery spQuery = new SPQuery();
            StringBuilder sb = new StringBuilder();
            sb.Append("<Eq><FieldRef Name='ContentType'/><Value Type='Text'>Folder</Value></Eq>");
            spQuery.Query = sb.ToString();
            spQuery.Folder = folder;
            return list.GetItems(spQuery);
        }
        #endregion

        #region Excel Common Method

        #region 生成权限表
        public void SetExcelTitle(XSSFSheet DataSheet, XSSFWorkbook hssfworkbook, ICellStyle cellStyle, string webTitle, string weburl, string listTile)
        {
            IRow r1 = DataSheet.CreateRow(0);
            IRow r2 = DataSheet.CreateRow(1);
            IRow r3 = DataSheet.CreateRow(3);
            IRow r4 = DataSheet.CreateRow(4);
            ///设置单元格宽度
            DataSheet.SetColumnWidth(4, 100 * 60);
            DataSheet.SetColumnWidth(5, 100 * 80);
            ///设置样式
            ICellStyle headCellStyle = hssfworkbook.CreateCellStyle();
            headCellStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            headCellStyle.FillForegroundColor = HSSFColor.PALE_BLUE.index;
            ///填写标题
            setCell(r1, 0, "站点名称:", headCellStyle, DataSheet);
            setCell(r1, 1, webTitle, cellStyle, DataSheet);
            setCell(r1, 2, "导出日期:", headCellStyle, DataSheet);
            setCell(r1, 3, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ssss"), cellStyle, DataSheet);
            setCell(r2, 0, "站点路径:", headCellStyle, DataSheet);
            setCell(r2, 1, weburl, cellStyle, DataSheet);
            setCell(r2, 2, "文档库名称:", headCellStyle, DataSheet);
            setCell(r2, 3, listTile, cellStyle, DataSheet);
            ///设置样式
            ICellStyle headCellStyleTwo = hssfworkbook.CreateCellStyle();
            IFont font = hssfworkbook.CreateFont();
            font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
            headCellStyleTwo.SetFont(font);
            setCell(r3, 0, "文档库权限列表", headCellStyleTwo, DataSheet);
            SetCellRangeAddress(DataSheet, 3, 3, 0, 5);
            ///设置样式
            ICellStyle headCellStyleSan = hssfworkbook.CreateCellStyle();
            headCellStyleSan.SetFont(font);
            headCellStyleSan.FillPattern = FillPatternType.SOLID_FOREGROUND;
            headCellStyleSan.FillForegroundColor = HSSFColor.PALE_BLUE.index;

            ///设置标题
            setCell(r4, 0, "名称", headCellStyleSan, DataSheet);
            setCell(r4, 1, "路径", headCellStyleSan, DataSheet);
            setCell(r4, 2, "权限类别", headCellStyleSan, DataSheet);
            setCell(r4, 3, "权限小组", headCellStyleSan, DataSheet);
            setCell(r4, 4, "小组成员", headCellStyleSan, DataSheet);
            setCell(r4, 5, "权限级别", headCellStyleSan, DataSheet);
        }
        /// <summary>
        /// 给合并单元格添加样式
        /// </summary>
        /// <param name="DataSheet"></param>
        public void SetMergeCellStyle(XSSFSheet DataSheet, ICellStyle contentCellStyle, XSSFWorkbook hssfworkbook)
        {
            ICellStyle leftstyle = hssfworkbook.CreateCellStyle();
            ///循环给合并的列添加边框样式
            for (int i = 5; i < DataSheet.LastRowNum + 1; i++)
            {
                //XSSFRow sf = new XSSFRow(DataSheet.GetRow(i),DataSheet);
                IRow r = DataSheet.GetRow(i);
                for (int j = 0; j < r.Cells.Count; j++)
                {
                    ICell cl = HSSFCellUtil.GetCell(r, j);
                    cl.CellStyle = contentCellStyle;
                }
                ICell c1 = r.GetCell(0);
                ICell c2 = r.GetCell(1);
                ICell c3 = r.GetCell(3);
                ICell c4 = r.GetCell(4);
                ICell c5 = r.GetCell(5);

                leftstyle.VerticalAlignment = VerticalAlignment.CENTER;
                leftstyle.Alignment = HorizontalAlignment.LEFT;
                leftstyle.BorderBottom = BorderStyle.THIN;
                leftstyle.BorderLeft = BorderStyle.THIN;
                leftstyle.BorderRight = BorderStyle.THIN;
                leftstyle.BorderTop = BorderStyle.THIN;
                c1.CellStyle = leftstyle;
                c2.CellStyle = leftstyle;
                c3.CellStyle = leftstyle;
                c4.CellStyle = leftstyle;
                c5.CellStyle = leftstyle;
            }


        }
        bool ExcelPermissionsExport(string oFilePath, string createFilePath, ICellStyle cellStyle, XSSFSheet DataSheet, Guid libraryName, XSSFWorkbook hssfworkbook, SPWeb web)
        {
            try
            {
                LogService.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始获取文件夹信息!");
                List<SPPermissionsModel> querylist = this.ReadFileBrary(libraryName, web);
                LogService.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始生成Excel文件!");

                SetExcelTitle(DataSheet, hssfworkbook, cellStyle, web.Title, web.Url, querylist[0].JTile);
                if (querylist == null || querylist.Count == 0) return false;
                int total = 5;
                int cnt = querylist.Count;  //获取list内的个数
                ///给合并的列加上黑色的边框样式
                ICellStyle contentCellStyle = hssfworkbook.CreateCellStyle();
                if (cnt > 0)
                {
                    for (int i = 0; i < cnt; i++)
                    {
                        try
                        {
                            NPOI.SS.UserModel.IRow r = DataSheet.GetRow(i + 5);
                            var item = querylist[i];
                            if (r == null)
                            {
                                r = DataSheet.CreateRow(total);
                            }
                            int itmeSplit = item.JAssignmentCollection.Split('&').Length; /// 获取权限小组数量

                            int MergeCellNuber = total + itmeSplit - 1; //计算合并列数
                            if (r.RowNum == 5)
                            {
                                //合并0，1，2列
                                SetCellRangeAddress(DataSheet, 5, MergeCellNuber, 0, 0);
                                SetCellRangeAddress(DataSheet, 5, MergeCellNuber, 1, 1);
                                SetCellRangeAddress(DataSheet, 5, MergeCellNuber, 2, 2);

                                //填充文件库的信息
                                setCell(DataSheet.GetRow(5), 0, item.JTile, contentCellStyle, DataSheet);
                                setCell(DataSheet.GetRow(5), 1, item.JPath, contentCellStyle, DataSheet);
                                setCell(DataSheet.GetRow(5), 2, item.JType, contentCellStyle, DataSheet);
                                ///填写用户组，用户，权限与权限描述等信息
                                for (int j = 0; j < item.JAssignmentCollection.Split('&').Length; j++)
                                {
                                    //if (item.JDefinitionCollection.Split('&')[j] == "受限访问;") continue;
                                    r = DataSheet.GetRow(total + j);
                                    if (r == null)
                                    {
                                        r = DataSheet.CreateRow(total + j);
                                    }
                                    setCell(r, 3, item.JAssignmentCollection.Split('&')[j], contentCellStyle, DataSheet);
                                    setCell(r, 4, item.JAssignmentCy.Split('&')[j].Replace("系统账户;", ""), contentCellStyle, DataSheet);
                                    setCell(r, 5, item.JDefinitionCollection.Split('&')[j].Replace("受限访问;", ""), contentCellStyle, DataSheet);
                                }
                                ///累加行数
                                total = total + itmeSplit;
                            }
                            else
                            {
                                r = DataSheet.GetRow(total);
                                if (r == null)
                                {
                                    r = DataSheet.CreateRow(total);
                                }
                                ///判断文件夹的权限类型是否为继承上一级的关系
                                if (item.JType == "继承")
                                {
                                    r = DataSheet.GetRow(total);
                                    if (r == null)
                                    {
                                        r = DataSheet.CreateRow(total);
                                    }
                                    ///填写文件夹路径，权限类型,标题等信息
                                    setCell(DataSheet.GetRow(total), 1, item.JPath, contentCellStyle, DataSheet);
                                    setCell(DataSheet.GetRow(total), 2, item.JType + "于" + "上一级目录", contentCellStyle, DataSheet);
                                    setCell(DataSheet.GetRow(total), 0, item.JTile, contentCellStyle, DataSheet);
                                    ///添加数据所在行没有数据列的边框样式
                                    setCell(r, 3, " ", contentCellStyle, DataSheet);
                                    setCell(r, 4, " ", contentCellStyle, DataSheet);
                                    setCell(r, 5, " ", contentCellStyle, DataSheet);
                                    total = total + 1;
                                }
                                else
                                {
                                    ///合并 0,1,2列
                                    SetCellRangeAddress(DataSheet, total, MergeCellNuber, 0, 0);
                                    SetCellRangeAddress(DataSheet, total, MergeCellNuber, 1, 1);
                                    SetCellRangeAddress(DataSheet, total, MergeCellNuber, 2, 2);

                                    ///填充路径，权限类型,标题
                                    setCell(DataSheet.GetRow(total), 1, item.JPath, contentCellStyle, DataSheet);
                                    setCell(DataSheet.GetRow(total), 2, item.JType, contentCellStyle, DataSheet);
                                    setCell(DataSheet.GetRow(total), 0, item.JTile, contentCellStyle, DataSheet);

                                    ///填充用户组信息,填充用户组的用户信息，填充用户组的权限信息，还有权限描述信息
                                    for (int j = 0; j < item.JAssignmentCollection.Split('&').Length; j++)
                                    {
                                        //if (item.JDefinitionCollection.Split('&')[j] == "受限访问;") continue;

                                        r = DataSheet.GetRow(total + j);
                                        if (r == null)
                                        {
                                            r = DataSheet.CreateRow(total + j);
                                        }
                                        setCell(r, 3, item.JAssignmentCollection.Split('&')[j], contentCellStyle, DataSheet);
                                        setCell(r, 4, item.JAssignmentCy.Split('&')[j].Replace("系统账户;", ""), contentCellStyle, DataSheet);
                                        setCell(r, 5, item.JDefinitionCollection.Split('&')[j].Replace("受限访问;", ""), contentCellStyle, DataSheet);
                                    }
                                    total = total + itmeSplit;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogService.WriteLog("运行失败!错误原因：" + ex.Message);
                            return false;
                        }
                    }
                    ////  给合并单元格加上样式
                    SetMergeCellStyle(DataSheet, contentCellStyle, hssfworkbook);
                    LogService.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "Excel文件已经生成成功!");
                } return true;
            }
            catch
            {
                return false;
            }


        }
        #endregion

        #region 操作Excel表的基本方法
        /// <summary>
        /// 包含设置表格背景色
        /// </summary>
        /// <param name="row_cbCenter"></param>
        /// <param name="cellIndex"></param>
        /// <param name="value"></param>
        /// <param name="cellStyle"></param>
        /// <param name="sh"></param>
        private void setCellHead(NPOI.SS.UserModel.IRow row_cbCenter, int cellIndex, object value, ICellStyle cellStyle, ISheet sh)
        {
            NPOI.SS.UserModel.ICell cell_cbCenter = row_cbCenter.GetCell(cellIndex);

            if (cell_cbCenter == null)
            {
                cell_cbCenter = row_cbCenter.CreateCell(cellIndex);
            }
            sh.SetColumnWidth(0, 15 * 256);
            sh.SetColumnWidth(1, 35 * 256);
            sh.SetColumnWidth(2, 15 * 256);
            sh.SetColumnWidth(3, 25 * 256);

            cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LIGHT_CORNFLOWER_BLUE.index;
            cellStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;

            //设置单元格上下左右边框线  
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            //边框颜色  
            cellStyle.BottomBorderColor = HSSFColor.BLACK.index;
            cellStyle.TopBorderColor = HSSFColor.BLACK.index;
            cellStyle.LeftBorderColor = HSSFColor.BLACK.index;
            cellStyle.RightBorderColor = HSSFColor.BLACK.index;



            //文字水平和垂直对齐方式  
            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.CENTER;
            //是否换行  
            //cellStyle.WrapText = true;
            //缩小字体填充  
            //cellStyle.ShrinkToFit = true;

            cell_cbCenter.CellStyle = cellStyle;
            var setValue = string.IsNullOrEmpty(IBUtils.ObjectToStr(value)) ? " " : value.ToString();
            cell_cbCenter.SetCellValue(setValue);
        }
        /// <summary>
        /// 不设置表格背景色
        /// </summary>
        /// <param name="row_cbCenter"></param>
        /// <param name="cellIndex"></param>
        /// <param name="value"></param>
        /// <param name="cellStyle"></param>
        /// <param name="sh"></param>
        private void setCell(NPOI.SS.UserModel.IRow row_cbCenter, int cellIndex, object value, ICellStyle cellStyle, ISheet sh)
        {
            NPOI.SS.UserModel.ICell cell_cbCenter = row_cbCenter.GetCell(cellIndex);

            if (cell_cbCenter == null)
            {

                cell_cbCenter = row_cbCenter.CreateCell(cellIndex);
            }
            sh.SetColumnWidth(0, 15 * 256);
            sh.SetColumnWidth(1, 35 * 256);
            sh.SetColumnWidth(2, 25 * 256);
            sh.SetColumnWidth(3, 25 * 256);

            //cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LIGHT_CORNFLOWER_BLUE.index;
            //cellStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;

            //设置单元格上下左右边框线  
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            //边框颜色  
            cellStyle.BottomBorderColor = HSSFColor.BLACK.index;
            cellStyle.TopBorderColor = HSSFColor.BLACK.index;
            cellStyle.LeftBorderColor = HSSFColor.BLACK.index;
            cellStyle.RightBorderColor = HSSFColor.BLACK.index;



            //文字水平和垂直对齐方式  
            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.CENTER;
            //是否换行  
            //cellStyle.WrapText = true;
            //缩小字体填充  
            //cellStyle.ShrinkToFit = true;

            cell_cbCenter.CellStyle = cellStyle;
            var setValue = string.IsNullOrEmpty(IBUtils.ObjectToStr(value)) ? " " : value.ToString();
            cell_cbCenter.SetCellValue(setValue);
        }


        /// <summary>
        /// Set Cell Value
        /// </summary>
        /// <param name="row_cbCenter"></param>
        /// <param name="cellIndex"></param>
        /// <param name="value"></param>
        private void setCell(NPOI.SS.UserModel.IRow row_cbCenter, int cellIndex, object value, NPOI.SS.UserModel.CellType ctype)
        {
            NPOI.SS.UserModel.ICell cell_cbCenter = row_cbCenter.GetCell(cellIndex);

            if (cell_cbCenter == null)
            {
                cell_cbCenter = row_cbCenter.CreateCell(cellIndex);
            }
            cell_cbCenter.SetCellType(ctype);
            if (value == null) return;
            if (cell_cbCenter.CellType == NPOI.SS.UserModel.CellType.NUMERIC)
            {
                double d = 0;
                Double.TryParse(value.ToString(), out d);
                cell_cbCenter.SetCellValue(d);
            }

        }
        private object mValue = System.Reflection.Missing.Value;
        public static void SetCellRangeAddress(ISheet sheet, int rowstart, int rowend, int colstart, int colend)
        {
            CellRangeAddress cellRangeAddress = new CellRangeAddress(rowstart, rowend, colstart, colend);
            sheet.AddMergedRegion(cellRangeAddress);
        }
        #endregion

        #endregion


        #region Get ExchangeService
        public static ExchangeService ConnectExchangeService()
        {
            string exchangeAdminAccount = System.Configuration.ConfigurationManager.AppSettings["EnvisionMailAddress"].ToString();
            string exchangeAdminPassword = System.Configuration.ConfigurationManager.AppSettings["EnvisionMailPassword"].ToString();

            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013)
            {
                Credentials = new WebCredentials(exchangeAdminAccount, exchangeAdminPassword)
            };

            service.AutodiscoverUrl("sharepoint.admin@envisioncn.com", RedirectionUrlValidationCallback);

            return service;

        }
        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            Uri redirectionUri = new Uri(redirectionUrl);
            return string.Equals(redirectionUri.Scheme, "https", StringComparison.CurrentCultureIgnoreCase);
        }

        #endregion
    }
}
