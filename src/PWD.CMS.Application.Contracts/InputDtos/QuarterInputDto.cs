using System;
using System.Collections.Generic;

namespace PWD.CMS.DtoModels
{
    // Data Entry [Setup]
    public class QuarterInputDto
    {
        public Guid OrganizaitonUnitId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DistrictId { get; set; }
        public Guid CircleId { get; set; }
        public Guid DivisionId { get; set; }
        public Guid SubDivisionId { get; set; }
        public Guid CivilOfficeId { get; set; }
        public Guid EMOfficeId { get; set; }
        public List<BuildingInputDto> Buildings { get; set; }
    }
}
