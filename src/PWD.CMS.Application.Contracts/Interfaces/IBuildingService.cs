using PWD.CMS.DtoModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace PWD.CMS.Interfaces
{
    public interface IBuildingService : IApplicationService
    {
        Task<List<BuildingDto>> GetListAsync(FilterModel filterModel);
        Task<BuildingDto> GetAsync(int id);
        Task<BuildingDto> CreateAsync(BuildingDto input);
        Task<BuildingDto> UpdateAsync(BuildingDto input);
        Task DeleteAsync(int id);
    }
}
