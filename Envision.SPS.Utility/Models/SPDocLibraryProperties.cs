using System;

namespace Envision.SPS.Utility.Models
{
    [Serializable]
    public sealed class SPDocLibraryProperties : BaseModel
    {

        public string Title
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
       
        public string EnableVersioning
        {
            get;
            set;
        }
        public string ForceCheckout
        {
            get;
            set;
        }
        public string HasUniqueRoleAssignments
        {
            get;
            set;
        }
        public string BaseType
        {
            get;
            set;
        }
        public DateTime? Created
        {
            get;
            set;
        }
    }
}
