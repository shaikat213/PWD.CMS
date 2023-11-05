using PWD.CMS.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace PWD.CMS.Interfaces
{
    public interface IComplainAppService : IApplicationService
    {
        Task<IEnumerable<ComplaintListDto>> GetComplainListAsync(ComplainSearchDto search, FilterModel filterModel);
        Task<bool> UpdateTenantFeedbackAsync(int id, string feedback);
        Task<List<ComplainSearchResultDto>> GetComplainByMobileNoAsync(string mobileNo);
    }
}
