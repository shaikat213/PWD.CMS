using PWD.CMS.CMSEnums;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace PWD.CMS.DtoModels
{
    public class ComplainDto : FullAuditedEntityDto<int>
    {
        public int ApartmentId { get; set; }
        public int TenantId { get; set; }        
        public DateTime Date { get; set; }
        public string Remark { get; set; }
        public string ComplainPhotos { get; set; }
        public string RepairPhotos { get; set; }
        public double? Cost { get; set; }
        public string MaterialsUsed { get; set; }
        public int? AllotmentId { get; set; }
        public int? PostingId { get; set; }
        public int ProblemTypeId { get; set; }
        public ComplainStatus ComplainStatus { get; set; }
        public string ComplainStatusName { get; set; }
        public string TenantFeedback { get; set; }
        public bool IsCivil { get; set; }
        public string TicketNumber { get; set; }
        public string Description { get; set; }
        public Guid? OrganizationalUnitId { get; set; }
        public ICollection<ComplainHistoryDto> ComplainHistories { get; set; }
        public string TenantName { get; set; }
        public string TenantMobile { get; set; }
        public string Address { get; set; }
    }

    public class ComplainHistoryDto : EntityDto<int>
    {
        public int ComplainId { get; set; }
        public DateTime Date { get; set; }
        public string Remark { get; set; }
        public int? PostingId { get; set; }
        public int AllotmentId { get; set; }
        public ComplainStatus ComplainStatus { get; set; }
        public string UpdatedBy { get; set; }

    }
}
