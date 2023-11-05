using System;
using System.Collections.Generic;
using System.Text;

namespace PWD.CMS.InputDtos
{
    public class AllotmentListDto
    {
        public int? Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int? ApartmentId { get; set; }
        public string ApartmentName { get; set; }
        public int? BuildingId { get; set; }
        public string BuildingName { get; set; }
        public int? TenantId { get; set; }
        public string TenantName { get; set; }
        public string TenantMobile { get; set; }
        public string TenantEmail{ get; set; }
        public string TenantNid { get; set; }        
    }
}
