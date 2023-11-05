using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace PWD.CMS.Models
{
    public class Building : FullAuditedEntity<int>
    {
        public int QuarterId { get; set; }
        public Quarter Quarter { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ExternalId { get; set; }
        public Guid? CivilSubDivisionId { get; set; }
        public Guid? EmSubDivisionId { get; set; }
        public List<Apartment> Apartments { get; set; }
        
        //public Guid OrganizaitonUnitId { get; set; }
        //public Guid CivilCircleId { get; set; }
        //public Guid CivilDivisionId { get; set; }
        //public Guid EmCircleId { get; set; }
        //public Guid EmDivisionId { get; set; }
        //public Guid CivilId { get; set; }
        //public Guid EMId { get; set; }


    }
}
