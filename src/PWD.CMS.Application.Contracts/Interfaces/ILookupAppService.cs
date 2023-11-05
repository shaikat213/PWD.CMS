using PWD.CMS.DtoModels;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PWD.CMS.Interfaces
{
    public interface ILookupAppService:IApplicationService
    {
        Task<ListResultDto<ApartmentLookupDto>> GetApartmentByBuildingLookupAsync(int buildingId);
        Task<ListResultDto<BuildingLookupDto>> GetBuilduingLookupAsync();
        Task<ListResultDto<BuildingLookupDto>> GetBuilduingByQuarterLookupAsync(int quarterId);
        Task<ListResultDto<DistrictLookupDto>> GetDistrictLookupAsync();
        Task<ListResultDto<QuarterLookupDto>> GetQuarterLookupAsync(int districtId);
    }
}
