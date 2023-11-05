using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace PWD.CMS.Models
{
    // Data Entry [Setup]
    public class Quarter : FullAuditedEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int DistrictId { get; set; }
        public District District { get; set; }
        public int? ExternalId { get; set; }

        //public Guid CircleId { get; set; }        
        //public Guid DivisionId { get; set; }
        //public Guid SubDivisionId { get; set; }
        public Guid? CivilSubDivisionId { get; set; }
        public Guid? EmSubDivisionId { get; set; }
        public List<Building> Buildings { get; set; }
    }
}
