using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using Microsoft.SharePoint;
using Microsoft.Exchange.WebServices.Data;

namespace DisableMemberEmailSend
{
    /// <summary>
    /// 定时邮件发送处理类
    /// </summary>
    class Program
    {
        private static ExchangeService service = null;
        public static void Main(string[] args)
        {

            string domainServer = System.Configuration.ConfigurationManager.AppSettings["EnvisionDomainServer"].ToString();
            string domainName = System.Configuration.ConfigurationManager.AppSettings["EnvisionDomainName"].ToString();
            string domainAdmin = System.Configuration.ConfigurationManager.AppSettings["EnvisionDomainAdmin"].ToString();
            string domainPassword = System.Configuration.ConfigurationManager.AppSettings["EnvisionDomainPassword"].ToString();
            string ITManageGroup = System.Configuration.ConfigurationManager.AppSettings["ITManageGroup"].ToString();

            try
            {
                Console.WriteLine("正在检测AD失效的用户...");
                LogService.WriteLog("正在检测AD失效的用户...");
                service = ConnectExchangeService();
                //获取AD域中失效人员
                List<EntryUser> disableUsers = GetADaccountDisable(domainServer, domainName, domainAdmin, domainPassword);
                //获取SharePoint中 IT管理组下的所有人员
                if (ITManageGroup == "") return;
                List<GroupUser> itGroupUsers = GetITGroupUsers(ITManageGroup);
                Console.WriteLine("正在开始发送邮件…");
                LogService.WriteLog("正在开始发送邮件…");
                foreach (var item in disableUsers)
                {
                    string[] userArray = new string[] { domainName + "\\" + item.UAccount };

                    //获取邮件接收对象列表                    
                    List<GroupUser> userList = new List<GroupUser>();
                    List<GroupUser> adminGroupUsers = GetAdminGroupUsers(userArray);
                    userList.AddRange(itGroupUsers);
                    userList.AddRange(adminGroupUsers);

                    //检测过期人员是否还存在于SharePoint组中，存在则发送邮件                    
                    if (adminGroupUsers != null && adminGroupUsers.Count > 0)
                    {
                        //调用邮件模板并发送邮件
                        DisableMemberEmailTemplate(userList, item.UDisplayName, domainName + "\\" + item.UAccount);
                    }
                }
                Console.WriteLine("邮件发送成功!");

            }
            catch (Exception ex)
            {
                LogService.WriteLog("error--" + ex.Message);
                throw ex;
            }

        }

        /// <summary>
        /// 查找并返回 域中指定群组中用户失效的人员信息
        /// </summary>
        /// <param name="strDomainServer">域服务器</param>
        /// <param name="strDomainName">域名</param>
        /// <param name="strAdinnName">域管理员账号</param>
        /// <param name="strPwd">域管理员密码</param>
        private static List<EntryUser> GetADaccountDisable(string strDomainServer, string strDomainName, string strAdinnName, string strPwd)
        {
            try
            {
                List<EntryUser> expiresEntryUsers = new List<EntryUser>();

                DirectoryEntry de = new DirectoryEntry(strDomainServer, strAdinnName, strPwd);
                //DirectorySearcher searcher = new DirectorySearcher(de, "(&(objectClass=group)(cn=" + group + ")(objectCategory=user)(sAMAccountName=*))");
                DirectorySearcher searcher = new DirectorySearcher(de, "(&(objectCategory=user)(sAMAccountName=*))");
                SearchResultCollection src = searcher.FindAll();
               
                LogService.WriteLog(src.Count.ToString());

                foreach (SearchResult rs in src)
                {
                    if (rs != null)
                    {
                        DirectoryEntry entry = rs.GetDirectoryEntry();
                        string ADAccount = (entry.Properties["sAMAccountName"].Value == null) ? string.Empty : rs.GetDirectoryEntry().Properties["sAMAccountName"].Value.ToString();
                        string CNName = (entry.Properties["name"].Value == null) ? string.Empty : rs.GetDirectoryEntry().Properties["name"].Value.ToString();
                        string Email = (entry.Properties["mail"].Value == null) ? string.Empty : rs.GetDirectoryEntry().Properties["mail"].Value.ToString();
                        string DisplayName = (entry.Properties["displayName "].Value == null) ? string.Empty : rs.GetDirectoryEntry().Properties["displayName"].Value.ToString();
                        //string AccountExpires = (entry.Properties["accountExpires"].Value == null) ? string.Empty : rs.GetDirectoryEntry().Properties["accountExpires"].Value.ToString();
                        //用户帐户控制 userAccountControl (启用：512，禁用：514， 密码永不过期：66048) 
                        string userAccountControl = (entry.Properties["userAccountControl"].Value == null) ? string.Empty : rs.GetDirectoryEntry().Properties["userAccountControl"].Value.ToString();
                        //LogService.WriteLog("the user name is:" + CNName + Email + "禁用code:"+userAccountControl);
                        if (entry.Properties["userAccountControl"].Value.ToString() == "514" || entry.Properties["userAccountControl"].Value.ToString() == "546")
                        {
                            EntryUser user = new EntryUser();
                            user.UName = CNName;
                            user.UAccount = ADAccount;
                            user.UDisplayName = DisplayName;
                            expiresEntryUsers.Add(user);
                        }
                    }
                }
                searcher.Dispose();
                de.Dispose();

                return expiresEntryUsers;
            }
            catch (Exception)
            {
                throw;
            }

        }


        /// <summary>
        /// 创建并获取失效人员所在的站点和组的行信息
        /// </summary>
        /// <param name="expiresLoginNames"></param>
        /// <returns></returns>
        public static string CreateAdminTRRow(string[] expiresLoginNames)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                var SPSRootSiteUrl = System.Configuration.ConfigurationManager.AppSettings["SPSRootSiteUrl"];
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite spSite = new SPSite(SPSRootSiteUrl))
                    {
                        spSite.AllowUnsafeUpdates = true;
                        using (SPWeb spweb = spSite.OpenWeb())
                        {
                            spweb.AllowUnsafeUpdates = true;

                            GetAdminGroupUsersRowContent(spweb, expiresLoginNames, sb);

                            spweb.AllowUnsafeUpdates = false;
                        }
                        spSite.AllowUnsafeUpdates = false;
                    }
                });

                return sb.ToString();

            }
            catch (Exception)
            {
                throw;
            }


        }


        private static string GetAdminGroupUsersRowContent(SPWeb spweb, string[] expiresLoginNames, StringBuilder sb)
        {

            SPRoleAssignmentCollection spRoleAssignmentCollection = spweb.RoleAssignments;
            foreach (SPRoleAssignment roleItem in spRoleAssignmentCollection)
            {
                SPRoleDefinition roleDefinition = null;
                roleDefinition = spweb.RoleDefinitions.GetByType(SPRoleType.Administrator);
                bool result = roleItem.RoleDefinitionBindings.Contains(roleDefinition);
                if (result && roleItem.Member is SPGroup)
                {
                    SPGroup itemGroup = spweb.SiteGroups[roleItem.Member.Name];
                    //判断失效人员是否在该组中
                    bool inGroup = GroupContainUser(itemGroup, expiresLoginNames);
                    if (inGroup)
                    {
                        sb.Append("<tr><td>" + itemGroup.ParentWeb.Url.Replace("http://escnjyspfront01", "http://sp.envisioncn.com") + "</td><td>" + itemGroup.Name + "</td></tr>");
                    }
                }
            }

            if (spweb.Webs.Count > 0)
            {
                foreach (SPWeb webItem in spweb.Webs)
                {
                    GetAdminGroupUsersRowContent(webItem, expiresLoginNames, sb);
                }
            }
            return sb.ToString();

        }


        /// <summary>
        /// 获取IT管理组下的人员
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="expiresLoginNames"></param>
        /// <returns></returns>
        public static List<GroupUser> GetITGroupUsers(string groupName)
        {
            try
            {
                List<GroupUser> groupUserList = new List<GroupUser>();

                var SPSRootSiteUrl = System.Configuration.ConfigurationManager.AppSettings["SPSRootSiteUrl"];
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite spSite = new SPSite(SPSRootSiteUrl))
                    {
                        spSite.AllowUnsafeUpdates = true;
                        using (SPWeb web = spSite.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            SPGroup group = web.SiteGroups[groupName];
                            if (group != null)
                            {
                                foreach (SPUser item in group.Users)
                                {
                                    GroupUser groupuser = new GroupUser();
                                    groupuser.UAccount = item.LoginName;
                                    groupuser.UDisplayName = item.Name;
                                    groupuser.UEmail = item.Email;
                                    groupuser.GroupName = item.Name;
                                    groupuser.URL = item.ParentWeb.Url;
                                    groupUserList.Add(groupuser);
                                }
                            }
                            web.AllowUnsafeUpdates = false;
                        }
                        spSite.AllowUnsafeUpdates = false;
                    }
                });
                return groupUserList;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 获取过期人员所在的SharePoint管理组下的所有人员
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="expiresLoginNames"></param>
        /// <returns></returns>
        public static List<GroupUser> GetAdminGroupUsers(string[] expiresLoginNames)
        {
            try
            {
                List<GroupUser> adminGroupUserList = new List<GroupUser>();
                var SPSRootSiteUrl = System.Configuration.ConfigurationManager.AppSettings["SPSRootSiteUrl"];
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite spSite = new SPSite(SPSRootSiteUrl))
                    {
                        spSite.AllowUnsafeUpdates = true;
                        using (SPWeb web = spSite.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;

                            GetAdminGroupUsers(web, ref adminGroupUserList, expiresLoginNames);

                            web.AllowUnsafeUpdates = false;
                        }
                        spSite.AllowUnsafeUpdates = false;
                    }
                });
                return adminGroupUserList;
            }
            catch (Exception)
            {
                throw;
            }

        }


        /// <summary>
        ///获取文档库管理组 人员
        /// </summary>
        /// <param name="spweb"></param>
        /// <param name="userList"></param>
        private static void GetAdminGroupUsers(SPWeb spweb, ref List<GroupUser> userList, string[] expiresLoginNames)
        {

            SPRoleAssignmentCollection spRoleAssignmentCollection = spweb.RoleAssignments;
            foreach (SPRoleAssignment roleItem in spRoleAssignmentCollection)
            {
                SPRoleDefinition roleDefinition = null;
                roleDefinition = spweb.RoleDefinitions.GetByType(SPRoleType.Administrator);
                bool result = roleItem.RoleDefinitionBindings.Contains(roleDefinition);
                if (result)
                {
                    if (roleItem.Member is SPUser) continue;
                    SPGroup itemGroup = spweb.SiteGroups[roleItem.Member.Name];
                    //判断失效人员是否在该组中
                    bool inGroup = GroupContainUser(itemGroup, expiresLoginNames);
                    if (inGroup)
                    {
                        foreach (SPUser itemUser in itemGroup.Users)
                        {
                            GroupUser groupuser = new GroupUser();
                            groupuser.UAccount = itemUser.LoginName;
                            groupuser.UDisplayName = itemUser.Name;
                            groupuser.UEmail = itemUser.Email;
                            groupuser.GroupName = itemGroup.Name;
                            groupuser.URL = itemGroup.ParentWeb.Url;
                            if (!userList.Contains(groupuser))
                            {
                                userList.Add(groupuser);
                            }
                        }
                    }
                }
            }

            if (spweb.Webs.Count > 0)
            {
                foreach (SPWeb webItem in spweb.Webs)
                {
                    GetAdminGroupUsers(webItem, ref userList, expiresLoginNames);
                }
            }

        }


        /// <summary>
        /// 判断SP组是否包含USER
        /// </summary>
        /// <param name="spGroup"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>
        private static bool GroupContainUser(SPGroup spGroup, string[] loginName)
        {
            bool result = false;
            foreach (SPUser itemUser in spGroup.Users)
            {
                if (itemUser.LoginName.IndexOf('|') > 0 && itemUser.LoginName.Split('|')[1] == loginName[0])
                {
                    result = true;
                    break;
                }
            }
            return result;
        }



        #region 实体类
        /// <summary>
        /// 自定义 User类，用来存储AD中 过期的User信息
        /// </summary>
        public class EntryUser
        {
            public string UName { get; set; }
            public string UDisplayName { get; set; }
            public string UEmail { get; set; }
            public string UAccount { get; set; }
        }

        /// <summary>
        /// 自定义 User类，用来存储SharePoint组中的人员信息
        /// </summary>
        public class GroupUser
        {
            public string UName { get; set; }
            public string UDisplayName { get; set; }
            public string UEmail { get; set; }
            public string UAccount { get; set; }
            public string GroupName { get; set; }
            public string URL { get; set; }

        }

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

        #region Email

        #region Send Email With Template

        /// <summary>
        /// 构建邮件提醒模板并发送邮件
        /// </summary>
        /// <param name="toMailMessages">邮件接收人列表</param>
        /// <param name="disUserName">过期人员显示名</param>
        /// <param name="disUserAccount">过期人员账号</param>
        private static void DisableMemberEmailTemplate(List<GroupUser> toMailMessages, string disUserName, string disUserAccount)
        {
            try
            {
                string[] disUserArray = new string[] { disUserAccount };
                MailMessage mailT = new MailMessage();
                //mailT.To.Add(toMailMessage);        
                List<string> emailsList = new List<string>();
                foreach (var item in toMailMessages)
                {
                    if (!emailsList.Contains(item.UEmail))
                    {
                        emailsList.Add(item.UEmail);
                    }
                }
                //Just For Test
                //emailsList.Add("1002865216@qq.com");
                //emailsList.Add("2083734251@qq.com");

                for (int i = 0; i < emailsList.Count; i++)
                {
                    if (emailsList[i] != "")
                    {
                        mailT.To.Add(emailsList[i]);
                    }
                }
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                string trContent = CreateAdminTRRow(disUserArray);

                mailT.IsBodyHtml = true;
                mailT.Subject = "SharePoint管理组人员失效提醒";
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

                string mailStaticBody = @" <body>
                                                <p>Dear All,</p>
                                                <p>站点管理组人员{0}（{1}）的AD账号已失效，该用户在如下站点群组中，请及时更新群组人员，以确保系统授权的有效性！</p>
                                                <table class='gridtable'>
                                                <tr><th>站点路径</th><th>群组名称</th>
                                                {2}                                                
                                                </table>
                                                <p></p>
                                                <p>Thanks,</p>
                                                <p>SharePoint系统</p>
                                                <p>{3}</p>
                                                </body>";

                mailT.Body = mailStaticBodyStyle + string.Format(mailStaticBody, disUserName, disUserAccount, trContent, currentDate);
                mailT.BodyEncoding = Encoding.GetEncoding(936);

                SendMailByExchange(mailT);

               
            }
            catch (Exception ex)
            {
                LogService.WriteLog("Error--" + ex.Message);
            }
        }

        #endregion

        #region Send Email By STMP
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailMessage">邮件信息对象</param>
        /// <returns></returns>
        private static bool SendMail(MailMessage mailMessage)
        {
            try
            {
                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["EnvisionMailAddress"], ConfigurationManager.AppSettings["EnvisionMailDisplayName"]);
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["EnvisionMailHost"]);
                smtp.UseDefaultCredentials = true;
                smtp.Port = 25;
                NetworkCredential networkCredential = new NetworkCredential(ConfigurationManager.AppSettings["EnvisionMailUsername"], ConfigurationManager.AppSettings["EnvisionMailPassword"]);
                smtp.Credentials = networkCredential;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(mailMessage);
                LogService.WriteLog("Success--" + System.DateTime.Now + " --发件人：" + mailMessage.From.Address + "--》收件人：" + mailMessage.To.ToString());
                return true;
            }
            catch (Exception ex)
            {
                LogService.WriteLog("Error--" + System.DateTime.Now + " --" + ex.Message + "--》" + ex.StackTrace);

            }
            return false;
        }

        #endregion

        #region  Send Email By Exchange Service
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailMessage">邮件信息对象</param>
        /// <returns></returns>
        private static bool SendMailByExchange(MailMessage mailMessage)
        {
            try
            {

                EmailMessage message = new EmailMessage(service);
                message.Subject = mailMessage.Subject;
                message.Body = mailMessage.Body;
                //message.ToRecipients.Add(mailMessage.To.ToString());
                foreach (MailAddress address in mailMessage.To)
                {
                    if (address.ToString() != "") message.ToRecipients.Add(address.ToString());
                }
                message.Send();

                LogService.WriteLog("Success--》收件人：" + mailMessage.To.ToString());
                LogService.WriteLog("HTML：" + mailMessage.Body.ToString());
                return true;
            }
            catch (Exception ex)
            {
                LogService.WriteLog("Error--" + System.DateTime.Now + " --" + ex.Message + "--》" + ex.StackTrace);

            }
            return false;
        }
        #endregion

        #endregion




    }
}
