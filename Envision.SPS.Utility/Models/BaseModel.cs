using System;

namespace Envision.SPS.Utility.Models
{
    [Serializable]
    public abstract class BaseModel
    {
        #region Properties


        /// <summary>
        /// 对象ID
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        #endregion
    }
}
