using System;

namespace Envision.SPS.Utility.Models
{
    [Serializable]
    public sealed class TopMenuItem
    {

         public int ID
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string Url
        {
            get;
            set;
        }
        public int ParentId
        {
            get;
            set;
        }
       
    }
}
