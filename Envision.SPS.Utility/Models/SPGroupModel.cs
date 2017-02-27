using System;

namespace Envision.SPS.Utility.Models
{
    [Serializable]
    public sealed class SPGroupModel : BaseModel
    {

        public string Name
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public string Owner
        {
            get;
            set;
        }
        public string OwnerType
        {
            get;
            set;
        }
        public bool IsPermmsion { get; set; }
    }

   
}
