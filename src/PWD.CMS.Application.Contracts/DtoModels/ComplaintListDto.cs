using System;

namespace PWD.CMS.DtoModels
{
    public class ComplaintListDto
    {
        public int? Id { get; set; }
        public string TicketNumber { get; set; }
        public int? TenantId { get; set; }
        public string TenantName { get; set; }
        public string Address { get; set; }
        public string ProblemDescription { get; set; }
        public int? ComplainStatusId { get; set; }
        public string ComplainStatusStr { get; set; }
        public string FeedBack { get; set; }
        public string SubmittedDate { get; set; }
        public string MobileNo { get; set; }
        public string Comment { get; set; }
        public int? ProblemTypeId { get; set; }
        public string ProblemTypeStr { get; set; }
        public DateTime? CreateDate { get; set; }
        public double? Cost { get; set; }
        public string MaterialsUsed { get; set; }

    }
}
