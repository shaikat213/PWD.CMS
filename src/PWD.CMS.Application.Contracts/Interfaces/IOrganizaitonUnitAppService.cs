using PWD.CMS.DtoModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace PWD.CMS.Interfaces
{
    public interface IOrganizaitonUnitAppService : IApplicationService
    {
        Task<List<OrganizationUnitDto>> GetOffices();
        Task<DateTime> LatestOffice();
        Task<PostingDto> GetPosting(string userName);
    }
}
