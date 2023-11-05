using System;

namespace PWD.CMS.DtoModels
{
    public class AllotmentInputDto
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
