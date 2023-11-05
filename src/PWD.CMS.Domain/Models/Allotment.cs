using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace PWD.CMS.Models
{
    public class Allotment : FullAuditedEntity<int>
    {
        public string PreAllotment { get; set; }
        public string PostAllotment { get; set; }
        //public string PrePhotos { get; set; }
        //public string PostPhotos { get; set; }
        public string PrePhotos { get; set; }
        public string PostPhotos { get; set; }
        public string Remarks { get; set; }        
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        // Foreign key        
        public int PwdTenantId { get; set; }
        [ForeignKey("PwdTenantId")]
        public virtual PwdTenant PwdTenant { get; set; }
        public int ApartmentId { get; set; }
        [ForeignKey("ApartmentId")]
        public virtual Apartment Apartment { get; set; }
    }
}
