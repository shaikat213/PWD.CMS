using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace PWD.CMS.DtoModels
{
    public class BuildingDto : FullAuditedEntityDto<int>
    {
        public Guid OrganizaitonUnitId { get; set; }
        public int QuarterId { get; set; }
        public string QuarterName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CivilCircleId { get; set; }
        public Guid CivilDivisionId { get; set; }
        public Guid? CivilSubDivisionId { get; set; }
        public string CivilSubDivisionName { get; set; }
        public Guid EmCircleId { get; set; }
        public Guid EmDivisionId { get; set; }
        public Guid? EmSubDivisionId { get; set; }
        public string EmSubDivisionName { get; set; }
        public Guid CivilOfficeId { get; set; }
        public Guid EMOfficeId { get; set; }
        public List<ApartmentDto> Apartments { get; set; }

    }

   
}
