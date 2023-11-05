using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using PWD.CMS.CMSEnums;
using Volo.Abp.Domain.Entities.Auditing;

namespace PWD.CMS.Models
{
    public class PwdTenant : FullAuditedEntity<int>
    {
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public IdType? IdType { get; set; }
        public string IdNumber { get; set; }
        public string PermanentAddress { get; set; }        
        public string Designation { get; set; }
        public string Note { get; set; }        
        public int? AllotmentId { get; set; }
        [ForeignKey("AllotmentId")]
        public virtual Allotment Allotment { get; set; }       
    }
}
