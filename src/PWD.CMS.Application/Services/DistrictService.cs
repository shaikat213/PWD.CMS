using PWD.CMS.DtoModels;
using PWD.CMS.Models;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Authorization;

namespace PWD.CMS.Services
{
    public class DistrictService : CrudAppService<District, DistrictDto, int>
    {
        private readonly IRepository<District, int> districtRepository;
        public DistrictService(IRepository<District, int> repository,
            IRepository<District, int> districtRepository) : base(repository)
        {
            this.districtRepository = districtRepository;
        }

        public async Task Office(string officeId, int id, bool isAdd = true)
        {
            var district = await base.GetAsync(id);
            if (district != null)
            {
                //var offices =new List<string>();
                var offices = district.OrganizationUnits.Split('\u002C').ToList();
                if (isAdd) offices.Add(officeId);
                else offices.Remove(officeId);
                district.OrganizationUnits = string.Join(",", offices);
                await UpdateAsync(id, district);
            }
        }
        public async Task<List<string>> OfficeList( int id)
        {
            var district = await base.GetAsync(id);
            if (district != null)
            {
                var offices = district
                    .OrganizationUnits.Split('\u002C')
                    .Where(o=>o.Length>1).ToList();
                return offices;
            }
            return null;
        }
        
        [AllowAnonymous]
        public async Task<DistrictDto> OfficeDistrict(Guid officeId)
        {
            var ol = await Repository.GetListAsync();
            foreach(var o in ol)
            {
                var ou = o.OrganizationUnits?.Split(',').ToList();
                if (ou!=null && ou.Contains(officeId.ToString())) return new DistrictDto() 
                { Id=o.Id,Name=o.Name,OrganizationUnits=o.OrganizationUnits} ;
            };
            return null;
        }

        //public async Task<DistrictDto> GetDistrictInfo(int apartmentId)
        //{
        //    var apartment = await apartmentRepository.FirstOrDefaultAsync(x => x.Id == apartmentId);
        //    if (apartment != null && apartment.AllotmentId != null)
        //    {
        //        var district = await districtRepository.FirstOrDefaultAsync(x => x.AllotmentId == apartment.AllotmentId);
        //        if (district != null && district.AllotmentId != null)
        //        {
        //            return new DistrictDto { Name = district.Name, Mobile = district.Mobile, Email = district.Email, IdNumber = district.IdNumber, IdType = ((IdType)district.IdType), IdTypeStr = ((IdType)district.IdType).ToString() };
        //        }
        //    }

        //    return null;
        //}

        public async Task<int> GetCountAsync()
        {
            return (await districtRepository.GetListAsync()).Count;
        }

        public async Task<List<DistrictDto>> GetSortedListAsync(FilterModel filterModel)
        {
            var districts = await districtRepository.WithDetailsAsync();
            districts = districts.Skip(filterModel.Offset)
                            .Take(filterModel.Limit);
            return ObjectMapper.Map<List<District>, List<DistrictDto>>(districts.ToList());
        }
    }
}
