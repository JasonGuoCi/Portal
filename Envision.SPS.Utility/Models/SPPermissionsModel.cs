using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envision.SPS.Utility.Models
{
    public class SPPermissionsModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string JTile { get; set; }
        /// <summary>
        /// 用户组
        /// </summary>
        public string JAssignmentCollection { get; set; }
        /// <summary>
        /// 小组成员
        /// </summary>
        public string JAssignmentCy { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public string JDefinitionCollection { get; set; }
        /// <summary>
        ///路径
        /// </summary>
        public string JPath { get; set; }
        /// <summary>
        /// 是否拥有继承权
        /// </summary>
        public string JType { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string JDescription { get; set; }
    }
}