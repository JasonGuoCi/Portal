using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envision.SPS.Utility.Models
{
    public class SPGroups
    {
        public int GroupsId { get; set; }
        public string GroupsName { get; set; }
        public int GroupsOwnerId { get; set; }
        public string GroupsOwnerName { get; set; }

        public string GroupsDescription { get; set; }
    }
}
