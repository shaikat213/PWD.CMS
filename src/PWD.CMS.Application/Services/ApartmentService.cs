using PWD.CMS.DtoModels;
using PWD.CMS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System;
using PWD.CMS.InputDtos;

namespace PWD.CMS.Services
{
    public class ApartmentService : CrudAppService<Apartment, ApartmentDto, int>
    {
        private readonly IRepository<Apartment, int> apartmentRepository;
        private readonly IRepository<Allotment, int> allotmentRepository;
        public ApartmentService(IRepository<Apartment, int> repository,
            IRepository<Apartment, int> apartmentRepository,
            IRepository<Allotment, int> allotmentRepository) : base(repository)
        {
            this.apartmentRepository = apartmentRepository;
            this.allotmentRepository = allotmentRepository;
        }
        public async Task<int> GetCountAsync()
        {
            return (await apartmentRepository.GetListAsync()).Count;
        }
        public async Task<List<ApartmentDto>> GetSortedListAsync(FilterModel filterModel)
        {
            var apartments = await apartmentRepository.WithDetailsAsync(p => p.Building);
            apartments = apartments.Skip(filterModel.Offset)
                            .Take(filterModel.Limit);
            return ObjectMapper.Map<List<Apartment>, List<ApartmentDto>>(apartments.ToList());
        }
        public async Task<int> GetCountBySDIdAsync(Guid? sdId)//, Guid? emSDId)
        {
            var apartments = await apartmentRepository.WithDetailsAsync(p => p.Building);

            apartments = apartments.Where(a => a.Building.CivilSubDivisionId == sdId || a.Building.EmSubDivisionId == sdId);
            return apartments.Count();
        }
        public async Task<List<ApartmentDto>> GetSortedListBySDIdAsync(Guid? sdId, FilterModel filterModel)
        {
            var apartments = await apartmentRepository.WithDetailsAsync(p => p.Building);

            apartments = apartments.Where(a => a.Building.CivilSubDivisionId == sdId || a.Building.EmSubDivisionId == sdId);

            apartments = apartments.Skip(filterModel.Offset)
                            .Take(filterModel.Limit);
            return ObjectMapper.Map<List<Apartment>, List<ApartmentDto>>(apartments.ToList());
        }
        public async Task<List<ApartmentDto>> GetSortedListByBuildingIdAsync(int buildingId, FilterModel filterModel)
        {
            var apartments = await apartmentRepository.WithDetailsAsync();
            var aptByBuilding = apartments.Where(a => a.BuildingId == buildingId);
            aptByBuilding = aptByBuilding.Skip(filterModel.Offset)
                            .Take(filterModel.Limit);
            return ObjectMapper.Map<List<Apartment>, List<ApartmentDto>>(aptByBuilding.ToList());
        }
        public async Task<int> GetCountByQuarterIdAsync(int qId)
        {
            var buildings = await apartmentRepository.WithDetailsAsync();
            if (qId > 0)
            {
                buildings = buildings.Where(q => q.Building.QuarterId == qId);
            }
            return buildings.Count();
        }
        public async Task<List<ApartmentDto>> GetListByQuarterIdAsync(int qId, FilterModel filterModel)
        {
            List<ApartmentDto> list = null;
            var items = await apartmentRepository.WithDetailsAsync(p => p.Building.Quarter);
            items = items.Where(i => i.Building.QuarterId == qId);
            if (items.Any())
            {
                items = items.Skip(filterModel.Offset)
                               .Take(filterModel.Limit);
                list = new List<ApartmentDto>();
                foreach (var item in items)
                {
                    list.Add(new ApartmentDto()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Floor = item.Floor,
                        BuildingId = item.BuildingId,
                        BuildingName = item.Building.Name
                    });
                }
            }
            return list;
        }
        public async Task<int> GetCountByBuildingAsync(int buildingId)
        {
            var apartments = await apartmentRepository.WithDetailsAsync();
            var aptByBuilding = apartments.Where(a => a.BuildingId == buildingId);
            var count = aptByBuilding.Count();
            return count;// aptByBuilding.Count();//(await apartmentRepository.GetListAsync()).Count;
        }
        public async Task<List<ApartmentDto>> GetApartmentListByBuildingIdAsync(int bId, FilterModel filterModel)
        {
            List<ApartmentDto> list = null;
            var items = await apartmentRepository.WithDetailsAsync(p => p.Building);
            items = items.Where(i => i.BuildingId == bId);
            if (items.Any())
            {
                items = items.Skip(filterModel.Offset)
                               .Take(filterModel.Limit);
                list = new List<ApartmentDto>();
                foreach (var item in items)
                {
                    list.Add(new ApartmentDto()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Floor=item.Floor,
                        BuildingId = item.BuildingId,
                        BuildingName = item.Building.Name
                    });
                }
            }
            return list;
        }
        public async Task<List<AllotmentListDto>> GetListByBuildingIdAsync(int bId, FilterModel filterModel)
        {
            List<AllotmentListDto> list = null;
            var items = await apartmentRepository.WithDetailsAsync(p => p.Building);
            var allotments = await allotmentRepository.WithDetailsAsync(t => t.PwdTenant);
            if (items.Any())
            {                
                var joinData = from ap in items.Where(i => i.BuildingId == bId)
                               join al in allotments on ap.AllotmentId equals al.Id into handover
                               from x in handover.DefaultIfEmpty()
                               select new AllotmentListDto()
                               {
                                   ApartmentId = ap.Id,
                                   ApartmentName = ap.Name,
                                   BuildingId = ap.BuildingId,
                                   BuildingName = ap.Building.Name,
                                   StartDate = x.DateFrom.ToString("dd/MM/yyyy"),
                                   EndDate = x.DateTo.HasValue ? x.DateTo.Value.ToString("dd/MM/yyyy") : "",
                                   TenantId = x.PwdTenant != null ? x.PwdTenant.Id : null,
                                   TenantName = x.PwdTenant != null ? x.PwdTenant.Name : "",
                                   TenantMobile = x.PwdTenant != null ? x.PwdTenant.Mobile : "",
                                   TenantEmail = x.PwdTenant != null ? x.PwdTenant.Email : "",
                                   TenantNid = x.PwdTenant != null ? x.PwdTenant.IdNumber : ""
                               };

                joinData = joinData.Skip(filterModel.Offset)
                        .Take(filterModel.Limit);   
                
                return joinData.ToList();
            }
            return list;
        }
        public async Task<ApartmentDto> GetApartmentByAllotmentAsync(int allotmentId)
        {
            //var apartment = await apartmentRepository.FirstOrDefaultAsync(x => x == apartmentId);
            if (allotmentId > 0)// && apartment.AllotmentId != null)
            {
                var apartment = await apartmentRepository.FirstOrDefaultAsync(x => x.AllotmentId == allotmentId);
                if (apartment != null && apartment.AllotmentId != null)
                {
                    return ObjectMapper.Map<Apartment, ApartmentDto>(apartment);
                    //return new ApartmentDto
                    //{
                    //    Id = apartment.Id,
                    //    Name = apartment.Name,
                    //    Description = apartment.Description,
                    //    BuildingId = apartment.BuildingId
                    //};
                }
            }

            return null;
        }
    }
}
