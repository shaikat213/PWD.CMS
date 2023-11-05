using System;
using Volo.Abp.Application.Dtos;

namespace PWD.CMS.DtoModels
{
    public class OrganizaitonUnitDto : FullAuditedEntityDto<Guid>
    {
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
