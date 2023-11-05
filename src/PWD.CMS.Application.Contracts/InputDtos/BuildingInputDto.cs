using System;
using System.Collections.Generic;

namespace PWD.CMS.DtoModels
{
    public class BuildingInputDto
    {
        public Guid OrganizaitonUnitId { get; set; }
        public int QuarterId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CivilCircleId { get; set; }
        public Guid CivilDivisionId { get; set; }
        public Guid CivilSubDivisionId { get; set; }
        public Guid EmCircleId { get; set; }
        public Guid EmDivisionId { get; set; }
        public Guid EmSubDivisionId { get; set; }
        public Guid CivilOfficeId { get; set; }
        public Guid EMOfficeId { get; set; }
        public List<ApartmentInputDto> Apartments { get; set; }

    }
}
