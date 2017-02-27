using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envision.SPS.Utility.Models
{
    public class DocumentLibraryAttr
    {
        public string DocumentLibraryName { get; set; }
        public string BeforeDocumentLibraryName { get; set; }
        public string DocumentLibraryTemplateId { get; set; }
        public string DocumentLibraryTemplateName { get; set; }
        public string DocumentVersionControlId { get; set; }
        public string DocumentVersionControlName { get; set; }
        public string DocumentDefaultCheckOutId { get; set; }
        public string DocumentDefaultCheckOutName { get; set; }

        public string listId { get; set; }

        public List<SPUserGroupAttr> spuserGroups { get; set; }
    }

    public class SPUserGroupAttr
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string GroupRole { get; set; }
    }
}
