using PWD.CMS.CMSEnums;
using Volo.Abp.Application.Dtos;

namespace PWD.CMS.DtoModels
{
    public class ProblemTypeDto : FullAuditedEntityDto<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DepartmentType Type { get; set; }
    }
}
