using PWD.CMS.DtoModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace PWD.CMS.Interfaces
{
    public interface IQuarterService : IApplicationService
    {
        Task<List<QuarterDto>> GetListAsync();
        Task<QuarterDto> GetAsync(int id);
        Task<QuarterDto> CreateAsync(QuarterDto input);
        Task<QuarterDto> UpdateAsync(QuarterDto input);
        Task DeleteAsync(int id);
    }
}
