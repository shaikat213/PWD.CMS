using Volo.Abp.Application.Dtos;

namespace PWD.CMS.DtoModels
{
    public class BuildingClassDto : FullAuditedEntityDto<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
