using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envision.SPS.Utility.Models
{
    public class EnvisionPortalConfig
    {
        /// <summary>
        /// Portal地址
        /// </summary>
        public string EnvisionPortalWebUrl { get; set; }
        /// <summary>
        /// Domain
        /// </summary>
        public string EnvisionPortalDomain { get; set; }
        /// <summary>
        /// 公告
        /// </summary>
        public string EnvsionAnnouncement { get; set; }
        /// <summary>
        ///公司简介
        /// </summary>
        public string EnvisionCompanyInfo { get; set; }
        /// <summary>
        /// 常用系统链接category
        /// </summary>
        public string EnvisionLinksCategory { get; set; }
        /// <summary>
        /// 常用系统链接
        /// </summary>
        public string EnvisionSystemLink { get; set; }
        /// <summary>
        /// 部门知识指导（部门表）
        /// </summary>
        public string EnvisionDepartments { get; set; }
        /// <summary>
        /// 部门知识指导
        /// </summary>
        public string EnvisionKnowledgeInfo { get; set; }
    }
}
