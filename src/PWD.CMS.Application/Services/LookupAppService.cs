using PWD.CMS.CMSEnums;
using PWD.CMS.DtoModels;
using PWD.CMS.Interfaces;
using PWD.CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace PWD.CMS.Services
{
    public class LookupAppService : CMSAppService, ILookupAppService
    {
        private readonly IRepository<District> repository;
        private readonly IRepository<Quarter> quarterRepository;
        private readonly IRepository<Building> buildingRepository;
        private readonly IRepository<Apartment> apartmentRepository;
        private readonly IRepository<ProblemType> problemTypeRepository;

        public LookupAppService(IRepository<District> repository,
            IRepository<Quarter> quarterRepository,
            IRepository<Building> buildingRepository,
            IRepository<Apartment> apartmentRepository,
            IRepository<ProblemType> problemTypeRepository)
        {
            this.repository = repository;
            this.quarterRepository = quarterRepository;
            this.buildingRepository = buildingRepository;
            this.apartmentRepository = apartmentRepository;
            this.problemTypeRepository = problemTypeRepository;
        }
        public async Task<ListResultDto<DistrictLookupDto>> GetDistrictLookupAsync()
        {
            var queryable = await repository.GetQueryableAsync();

            return new ListResultDto<DistrictLookupDto>(
                ObjectMapper.Map<List<District>, List<DistrictLookupDto>>(queryable.ToList())
            );
        }

        public async Task<ListResultDto<QuarterLookupDto>> GetQuarterLookupAsync(int districtId)
        {
            var quarters = await quarterRepository.GetListAsync();
            quarters = quarters.Where(x => x.DistrictId == districtId).ToList();
            return new ListResultDto<QuarterLookupDto>(
                ObjectMapper.Map<List<Quarter>, List<QuarterLookupDto>>(quarters)
            );
        }

        public async Task<ListResultDto<BuildingLookupDto>> GetBuilduingLookupAsync()
        {
            var buildings = await buildingRepository.GetQueryableAsync();
            return new ListResultDto<BuildingLookupDto>(
                ObjectMapper.Map<List<Building>, List<BuildingLookupDto>>(buildings.ToList())
            );
        }

        public async Task<ListResultDto<BuildingLookupDto>> GetBuilduingByQuarterLookupAsync(int quarterId)
        {
            var buildings = await buildingRepository.GetListAsync();
            buildings = buildings.Where(x => x.QuarterId == quarterId).ToList();
            return new ListResultDto<BuildingLookupDto>(
                ObjectMapper.Map<List<Building>, List<BuildingLookupDto>>(buildings)
            );
        }

        public async Task<ListResultDto<ApartmentLookupDto>> GetApartmentByBuildingLookupAsync(int buildingId)
        {
            var apartments = await apartmentRepository.GetListAsync();
            apartments = apartments.Where(x => x.BuildingId == buildingId).ToList();
            return new ListResultDto<ApartmentLookupDto>(
                ObjectMapper.Map<List<Apartment>, List<ApartmentLookupDto>>(apartments)
            );
        }

        public async Task<ListResultDto<ProblemTypeLookupDto>> GetProblemTypeLookupAsync(DepartmentType type)
        {
            var queryable = await problemTypeRepository.GetQueryableAsync();
            var problemTypes = queryable.Where(x => x.Type == type).ToList();

            return new ListResultDto<ProblemTypeLookupDto>(
                ObjectMapper.Map<List<ProblemType>, List<ProblemTypeLookupDto>>(problemTypes)
            );
        }
    }
}
