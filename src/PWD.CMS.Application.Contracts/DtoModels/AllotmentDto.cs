using System;
using Volo.Abp.Application.Dtos;

namespace PWD.CMS.DtoModels
{
    public class AllotmentDto : FullAuditedEntityDto<int>
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
        public int PwdTenantId { get; set; }
        public int ApartmentId { get; set; }
    }
}
