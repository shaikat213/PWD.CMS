using System;
using Volo.Abp.Application.Dtos;

namespace PWD.CMS.DtoModels
{
    public class BuildingLookupDto : FullAuditedEntityDto<int>
    {
        public string Name { get; set; }
        public Guid? CivilSubDivisionId { get; set; }
        public Guid? EmSubDivisionId { get; set; }
    }
}
