using PWD.CMS.CMSEnums;
using Volo.Abp.Domain.Entities.Auditing;

namespace PWD.CMS.Models
{
    public class ProblemType : FullAuditedEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DepartmentType Type { get; set; }
    }
}
