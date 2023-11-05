using Volo.Abp.Domain.Entities.Auditing;

namespace PWD.CMS.Models
{
    public class District : FullAuditedEntity<int>
    {
        public string Name { get; set; }
        public string OrganizationUnits { get; set; }
    }
}
