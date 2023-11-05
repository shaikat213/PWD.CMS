using PWD.CMS.CMSEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace PWD.CMS.Models
{
    public class Complain : FullAuditedEntity<int>
    {
        public string TicketNumber { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public string MaterialsUsed { get; set; }

        //public string ComplainPhotos { get; set; }
        //public string RepairPhotos { get; set; }
        public double? Cost { get; set; }
        public int? PostingId { get; set; }
        public bool IsCivil { get; set; }
        public bool NotifyDivision { get; set; }
        public DateTime NotifyDivisionDate { get; set; }
        public bool NotifyCircle { get; set; }
        public DateTime NotifyCircleDate { get; set; }
        public bool NotifyZone { get; set; }
        public DateTime NotifyZoneDate { get; set; }
        public Guid? OrganizationalUnitId { get; set; }
        public ComplainStatus ComplainStatus { get; set; }
        public string TenantFeedback { get; set; }

        // Foreign key
        public int AllotmentId { get; set; }
        public Allotment Allotment { get; set; }
        public int ProblemTypeId { get; set; }
        public ProblemType ProblemType { get; set; }
        public ICollection<ComplainHistory> ComplainHistories { get; set; }

    }

    public class ComplainHistory : FullAuditedEntity<int>
    {
        public int ComplainId { get; set; }
        public DateTime Date { get; set; }
        public string Remark { get; set; }
        public int? PostingId { get; set; }
        public int AllotmentId { get; set; }
        public ComplainStatus ComplainStatus { get; set; }

    }
}
