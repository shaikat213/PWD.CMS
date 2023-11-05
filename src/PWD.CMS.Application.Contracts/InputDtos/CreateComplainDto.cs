using System;

namespace PWD.CMS.DtoModels
{
    public class CreateComplainDto
    {
        public int ApartmentId { get; set; }
        public int? AllotmentId { get; set; }
        public int ProblemTypeId { get; set; }
        public int? ComplainStatusId { get; set; }
        public string Description { get; set; }
        public Guid? OrganizationalUnitId { get; set; }
        public int? PostingId { get; set; }
        public int? TokenNo { get; set; }
        public string District { get; set; }
        public string Quarter { get; set; }
        public string Building { get; set; }
        public string Apartment { get; set; }
        public string TenantName { get; set; }
        public string TenantEmail { get; set; }
        public string TenantMobile { get; set; }
        public string ProblemTypeStr { get; set; }

    }


}
