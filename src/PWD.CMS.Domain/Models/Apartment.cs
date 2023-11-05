using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace PWD.CMS.Models
{
    public class Apartment : FullAuditedEntity<int>
    {
        public int BuildingId { get; set; }
        public Building Building { get; set; }
        public int Floor { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? AllotmentId { get; set; }

        //[ForeignKey("AllotmentId")]
        //public Allotment Allotment { get; set; }

    }
}
