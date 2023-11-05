using PWD.CMS.DtoModels;
using PWD.CMS.Models;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace PWD.CMS.Services
{
    public class BuildingClassService : CrudAppService<BuildingClass, BuildingClassDto, int>
    {
        public BuildingClassService(IRepository<BuildingClass, int> repository) : base(repository)
        {

        }
    }
}
