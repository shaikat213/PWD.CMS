using PWD.CMS.DtoModels;
using PWD.CMS.Models;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PWD.CMS.Services
{
    public class BuildingTypeService : CrudAppService<BuildingType, BuildingTypeDto, int>
    {
        private readonly IRepository<BuildingType, int> buildingTypeRepository;
        public BuildingTypeService(IRepository<BuildingType, int> repository,
            IRepository<BuildingType, int> buildingTypeRepository) : base(repository)
        {
            this.buildingTypeRepository = buildingTypeRepository;
        }
        public async Task<int> GetCountAsync()
        {
            return (await buildingTypeRepository.GetListAsync()).Count;
        }

        public async Task<List<BuildingTypeDto>> GetSortedListAsync(FilterModel filterModel)
        {
            var buildingTypes = await buildingTypeRepository.WithDetailsAsync();
            buildingTypes = buildingTypes.Skip(filterModel.Offset)
                            .Take(filterModel.Limit);
            return ObjectMapper.Map<List<BuildingType>, List<BuildingTypeDto>>(buildingTypes.ToList());
        }
    }
}
