using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace Envision.SPS.Utility.Utilities
{
    public class BaseWebPart : UserControl
    {
        #region Constructors

        protected BaseWebPart()
        {
            this.InitData();
        }

        #endregion

        #region Properties

        /// <summary>
        /// 当前站点
        /// </summary>
        protected SPWeb Web
        {
            get;
            private set;
        }

        /// <summary>
        /// 当前站点Url
        /// </summary>
        protected string RootUrl
        {
            get;
            private set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            this.Web = SPContext.Current.Web;
            this.RootUrl = this.Web.Url + "/";
        }

        /// <summary>
        /// 获取列表中超链接的路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected string GetListUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

            var arr = url.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length != 2)
            {
                return url;
            }

            return arr[1].Trim();
        }

        /// <summary>
        /// 获取列表中图片的路径
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        protected string GetListImage(string image)
        {
            if (string.IsNullOrEmpty(image))
            {
                return string.Empty;
            }

            var arr = image.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length != 2)
            {
                return image;
            }

            return arr[0].Trim();
        }

        /// <summary>
        /// 获取列表中人员名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string GetListName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            var arr = name.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length != 2)
            {
                return name;
            }

            return arr[1].TrimStart('#');
        }

        /// <summary>
        /// 出现异常事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnError(EventArgs e)
        {
            base.OnError(e);
        }

        #endregion
    }
}
