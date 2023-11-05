using PWD.CMS.DtoModels;
using PWD.CMS.InputDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace PWD.CMS.Interfaces
{
    public interface IAllotmentService: IApplicationService
    {
        Task<List<AllotmentDto>> GetListAsync();
        Task<List<AllotmentListDto>> GetSortedListAsync(FilterModel filterModel);
        Task<AllotmentDto> GetAsync(int id);
        Task<int> GetCountAsync();
        Task<AllotmentDto> CreateAsync(AllotmentDto input);
        Task<AllotmentDto> UpdateAsync(AllotmentDto input);
        Task DeleteAsync(int id);
    }
}
