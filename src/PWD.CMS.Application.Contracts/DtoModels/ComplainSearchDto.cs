using System;
using System.Collections.Generic;
using System.Text;

namespace PWD.CMS.DtoModels
{
    public class ComplainSearchDto
    {
        public int? ComplaintId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int? StatusId { get; set; }
        public Guid? CircleId { get; set; }
        public Guid? DivisionId { get; set; }
        public Guid? SubDivisionId { get; set; }
        public int? QuarterId { get; set; }
        public int? BuildingId { get; set; }
    }
}
