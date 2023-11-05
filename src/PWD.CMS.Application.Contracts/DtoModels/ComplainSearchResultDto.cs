using System;
using System.Collections.Generic;
using System.Text;

namespace PWD.CMS.DtoModels
{
    public class ComplainSearchResultDto
    {
        public int? Id { get; set; }
        public string TicketNo { get; set; }
        public string ProblemDescription { get; set; }
        public string ComplainStatus { get; set; }
        public string SubmitedDate { get; set; }
        public int? TenantId { get; set; }
        public string TenantName { get; set; }
        public string TenantMobileNo { get; set; }
        public int? ProblemTypeId { get; set; }
        public string ProblemTypeStr { get; set; }
        public int? QuarterId { get; set; }
        public string QuarterName { get; set; }
        public int? BuildingId { get; set; }
        public string BuildingName { get; set; }
        public int? ApartmentId { get; set; }
        public string ApartmentName { get; set; }
        public int? DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string TenantFeedback { get; set; }

    }
}
