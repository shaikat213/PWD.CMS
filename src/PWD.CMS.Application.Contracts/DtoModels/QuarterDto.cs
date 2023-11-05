using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace PWD.CMS.DtoModels
{
    public class QuarterDto : FullAuditedEntityDto<int>
    {
        public Guid OrganizaitonUnitId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }        
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public Guid? CivilCircleId { get; set; }
        public Guid? CivilDivisionId { get; set; }
        public Guid? CivilSubDivisionId { get; set; }
        public string CivilSubDivisionName { get; set; }        
        public Guid? EmCircleId { get; set; }
        public Guid? EmDivisionId { get; set; }
        public Guid? EmSubDivisionId { get; set; }
        public string EmSubDivisionName { get; set; }
        public List<BuildingDto> Buildings { get; set; }
    }
}
