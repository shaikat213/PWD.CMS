using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace PWD.CMS.Models
{
    public class OrganizaitonUnit : FullAuditedEntity<Guid>
    {
        public Guid ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
    }
}
