using PWD.CMS.DtoModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace PWD.CMS.Interfaces
{
    public interface IAlloteeService : IApplicationService
    {
        Task<List<TenantDto>> GetListAsync();
        Task<TenantDto> GetAsync(int id);
        Task<TenantDto> CreateAsync(TenantDto input);
        Task<TenantDto> UpdateAsync(TenantDto input);
        Task DeleteAsync(int id);
    }
}
