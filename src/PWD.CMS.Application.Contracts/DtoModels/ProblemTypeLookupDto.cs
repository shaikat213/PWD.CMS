using Volo.Abp.Application.Dtos;

namespace PWD.CMS.DtoModels
{
    public class ProblemTypeLookupDto : EntityDto<int>
    {
        public string Name { get; set; }
    }
}
