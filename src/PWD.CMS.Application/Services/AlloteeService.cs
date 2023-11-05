using PWD.CMS.DtoModels;
using PWD.CMS.Models;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using PWD.CMS.CMSEnums;
using System;
using PWD.CMS.Interfaces;
using Volo.Abp.ObjectMapping;
using System.ComponentModel;
using System.Reflection;

namespace PWD.CMS.Services
{
    public class AlloteeService : CMSAppService, IAlloteeService//CrudAppService<PwdTenant, TenantDto, int>
    {
        private readonly IRepository<Quarter, int> quarterRepository;
        private readonly IRepository<Building, int> buildingRepository;
        private readonly IRepository<Apartment, int> apartmentRepository;
        private readonly IRepository<Allotment, int> allotmentRepository;
        private readonly IRepository<PwdTenant, int> tenantRepository;

        public AlloteeService(
            //IRepository<PwdTenant, int> repository,
            IRepository<Quarter, int> quarterRepository,
            IRepository<Building, int> buildingRepository,
            IRepository<Apartment, int> apartmentRepository,
            IRepository<Allotment, int> allotmentRepository,
            IRepository<PwdTenant, int> tenantRepository) //: base(repository)
        {
            this.quarterRepository = quarterRepository;
            this.buildingRepository= buildingRepository;
            this.apartmentRepository = apartmentRepository;
            this.allotmentRepository = allotmentRepository;
            this.tenantRepository = tenantRepository;
        }

        public async Task<TenantDto> CreateAsync(TenantDto input)
        {
            var tenant = await tenantRepository.WithDetailsAsync();
            var item = tenant.Where(t => t.Mobile == input.Mobile && t.IsDeleted == false).FirstOrDefault();
            if (item == null)
            {
                var newItem = ObjectMapper.Map<TenantDto, PwdTenant>(input);
                var allotee = await tenantRepository.InsertAsync(newItem);
                return ObjectMapper.Map<PwdTenant, TenantDto>(allotee);
            }
            return null;

        }
        public async Task<TenantDto> GetTenantInfo(int apartmentId)
        {
            var apartment = await apartmentRepository.FirstOrDefaultAsync(x => x.Id == apartmentId);
            if (apartment != null && apartment.AllotmentId != null)
            {
                var tenant = await tenantRepository.FirstOrDefaultAsync(x => x.AllotmentId == apartment.AllotmentId);
                if (tenant != null && tenant.AllotmentId != null)
                {
                    return new TenantDto { Name = tenant.Name, 
                        Mobile = tenant.Mobile, 
                        Email = tenant.Email, 
                        IdNumber = tenant.IdNumber, 
                        IdType = ((IdType)tenant.IdType), 
                        IdTypeStr = ((IdType)tenant.IdType).ToString() };
                }
            }

            return null;
        }

        public async Task<int> GetCountAsync()
        {
            return (await tenantRepository.GetListAsync()).Count;
        }

        public async Task<List<TenantDto>> GetSortedListAsync(FilterModel filterModel)
        {
            var tenants = await tenantRepository.WithDetailsAsync();
            tenants = tenants.Skip(filterModel.Offset)
                            .Take(filterModel.Limit);
            return ObjectMapper.Map<List<PwdTenant>, List<TenantDto>>(tenants.ToList());
        }
        public async Task<TenantDto> GetTenantInfoByMobile(string mobile)
        {
            //var apartment = await apartmentRepository.FirstOrDefaultAsync(x => x == apartmentId);
            if (mobile != null)// && apartment.AllotmentId != null)
            {
                var tenant = await tenantRepository.FirstOrDefaultAsync(x => x.Mobile == mobile && x.IsDeleted == false);
                if (tenant != null && tenant.AllotmentId != null)
                {
                    return ObjectMapper.Map<PwdTenant, TenantDto>(tenant);
                    //return new TenantDto { 
                    //    Name = tenant.Name, 
                    //    Mobile = tenant.Mobile, 
                    //    Email = tenant.Email, 
                    //    IdNumber = tenant.IdNumber, 
                    //    IdType = ((IdType)tenant.IdType), 
                    //    IdTypeStr = ((IdType)tenant.IdType).ToString() 
                    //};
                }
            }

            return null;
        }

        public async Task<int> GetCountBySDIdAsync(Guid? civilSDId, Guid? emSDId)
        {
            var allotees = await tenantRepository.WithDetailsAsync();
            if (civilSDId != null)
            {
                allotees = allotees.Where(q => q.CreatorId == civilSDId);
            }
            return allotees.Count();
        }
        //public async Task<List<TenantDto>> GetSortedListBySDIdAsync(Guid? civilSDId, Guid? emSDId, FilterModel filterModel)
        //{
        //    List<TenantDto> allotees = null;
        //    var items = await tenantRepository.WithDetailsAsync();
        //    if (items.Any())
        //    {
        //        if (civilSDId != null)
        //        {
        //            items = items.Where(q => q.CreatorId == civilSDId);
        //        }
        //        items = items.Skip(filterModel.Offset)
        //                       .Take(filterModel.Limit);
        //        //allotees = new List<TenantDto>();
        //        //foreach (var item in items)
        //        //{
        //        //    allotees.Add(new TenantDto()
        //        //    {
        //        //        Id = item.Id,
        //        //        Name = item.Name,
        //        //        Description = item.,
        //        //        QuarterId = item.QuarterId,
        //        //        QuarterName = item.Quarter?.Name,
        //        //        CivilSubDivisionId = item.CivilSubDivisionId,
        //        //        EmSubDivisionId = item.EmSubDivisionId,
        //        //    });
        //        //}
        //    }
        //    return ObjectMapper.Map<List<PwdTenant>, List<TenantDto>>(items.ToList());
        //}

        public async Task<List<TenantDto>> GetSortedListBySDIdAsync(Guid? civilSDId, Guid? emSDId, FilterModel filterModel)
        {
            List<TenantDto> allotees = null;
            string departmentName = "";
            string allotmentName = "";
            var items = await tenantRepository.WithDetailsAsync(d => d.Department);
            var allotments = await allotmentRepository.WithDetailsAsync(p => p.PwdTenant);
            var apartment = await apartmentRepository.WithDetailsAsync(a => a.Building);
            var building = await buildingRepository.WithDetailsAsync(b=>b.Quarter);
            var quarter = await quarterRepository.WithDetailsAsync();
            //items = await tenantRepository.WithDetailsAsync(a=> a.Allotment);
            if (items.Any())
            {
                if (civilSDId != null)
                {
                    items = items.Where(q => q.CreatorId == civilSDId);
                }
                items = items.Skip(filterModel.Offset)
                               .Take(filterModel.Limit);
                allotees = new List<TenantDto>();
                foreach (var item in items)
                {
                    if (item.DepartmentId > 0)
                    {
                        departmentName = item.Department.Name;
                    }
                    if (item.AllotmentId > 0)
                    {
                        var ap = apartment.Where(a => a.AllotmentId == item.AllotmentId).FirstOrDefault();
                        var buil = ap != null ? building.Where(b => b.Id == ap.BuildingId).FirstOrDefault() : null;
                        var quat = buil != null ? quarter.Where(q => q.Id == buil.QuarterId).FirstOrDefault() : null;

                        allotmentName = quat.Name + "_" + buil.Name + "_" + ap.Name;
                    }
                    allotees.Add(new TenantDto()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Mobile = item.Mobile,
                        Email = item.Email,
                        IdNumber = item.IdNumber,
                        DepartmentId = item?.DepartmentId,
                        DepartmentName = !string.IsNullOrEmpty(departmentName) ? departmentName : "NA",
                        AllotmentId = item?.AllotmentId,
                        AllotmentName = !string.IsNullOrEmpty(allotmentName) ? allotmentName : "NA",
                    });
                }
            }
            return allotees;//ObjectMapper.Map<List<PwdTenant>, List<TenantDto>>(items.ToList());
        }

        public async Task<int> GetCountBySearchAsync(Guid? civilSDId, string name, string mobile, string idnumber)
        {
            var allotees = await tenantRepository.WithDetailsAsync();
            if (civilSDId != null)
            {
                allotees = allotees.Where(q => q.CreatorId == civilSDId);

                if (!string.IsNullOrEmpty(name))
                {
                    allotees = allotees.Where(i => i.Name == name);
                }

                if (!string.IsNullOrEmpty(mobile))
                {
                    allotees = allotees.Where(i => i.Mobile == mobile);
                }

                if (!string.IsNullOrEmpty(idnumber))
                {
                    allotees = allotees.Where(i => i.IdNumber == idnumber);
                }
            }
            return allotees.Count();
        }
        public async Task<List<TenantDto>> GetSortedListBySearchAsync(Guid? civilSDId, string name, string mobile, string idnumber, FilterModel filterModel)
        {
            List<TenantDto> allotees = null;
            string departmentName = "";
            string allotmentName = "";
            var items = await tenantRepository.WithDetailsAsync(d => d.Department);
            var allotments = await allotmentRepository.WithDetailsAsync(p => p.PwdTenant);
            var apartment = await apartmentRepository.WithDetailsAsync(a => a.Building);
            var building = await buildingRepository.WithDetailsAsync(b => b.Quarter);
            var quarter = await quarterRepository.WithDetailsAsync();
            //items = await tenantRepository.WithDetailsAsync(a=> a.Allotment);
            if (items.Any())
            {
                if (civilSDId != null)
                {
                    items = items.Where(q => q.CreatorId == civilSDId);

                    if(!string.IsNullOrEmpty(name))
                    {
                        items = items.Where(i => i.Name == name);
                    }

                    if (!string.IsNullOrEmpty(mobile))
                    {
                        items = items.Where(i => i.Mobile == mobile);
                    }

                    if (!string.IsNullOrEmpty(idnumber))
                    {
                        items = items.Where(i => i.IdNumber == idnumber);
                    }
                }
                items = items.Skip(filterModel.Offset)
                               .Take(filterModel.Limit);
                allotees = new List<TenantDto>();
                foreach (var item in items)
                {
                    if (item.DepartmentId > 0)
                    {
                        departmentName = item.Department.Name;
                    }
                    if (item.AllotmentId > 0)
                    {
                        var ap = apartment.Where(a => a.AllotmentId == item.AllotmentId).FirstOrDefault();
                        var buil = ap != null ? building.Where(b => b.Id == ap.BuildingId).FirstOrDefault() : null;
                        var quat = buil != null ? quarter.Where(q => q.Id == buil.QuarterId).FirstOrDefault() : null;

                        allotmentName = quat.Name + "_" + buil.Name + "_" + ap.Name;
                    }
                    allotees.Add(new TenantDto()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Mobile = item.Mobile,
                        Email = item.Email,
                        IdNumber = item.IdNumber,
                        DepartmentId = item?.DepartmentId,
                        DepartmentName = !string.IsNullOrEmpty(departmentName) ? departmentName : "NA",
                        AllotmentId = item?.AllotmentId,
                        AllotmentName = !string.IsNullOrEmpty(allotmentName) ? allotmentName : "NA",
                    });
                }
            }
            return allotees;//ObjectMapper.Map<List<PwdTenant>, List<TenantDto>>(items.ToList());
        }

        public async Task<List<TenantDto>> GetListAsync()
        {
            List<TenantDto> list = null;
            var items = await tenantRepository.WithDetailsAsync();
            //if (items.Any())
            //{
            //    list = new List<QuarterDto>();
            //    foreach (var item in items)
            //    {
            //        list.Add(new QuarterDto()
            //        {
            //            Id = item.Id,
            //            Name = item.Name,
            //            Description = item.Description,
            //            DistrictId = item.DistrictId,
            //            DistrictName = item.District?.Name,
            //            CivilSubDivisionId = item.CivilSubDivisionId,
            //            EmSubDivisionId = item.EmSubDivisionId,
            //        });
            //    }
            //}

            return ObjectMapper.Map<List<PwdTenant>, List<TenantDto>>(items.ToList());
        }

        public async Task<TenantDto> GetAsync(int id)
        {
            var item = await tenantRepository.GetAsync(x => x.Id == id);

            return ObjectMapper.Map<PwdTenant, TenantDto>(item);
        }

        public async Task<TenantDto> UpdateAsync(TenantDto input)
        {
            var updateItem = ObjectMapper.Map<TenantDto, PwdTenant>(input);

            var item = await tenantRepository.UpdateAsync(updateItem);

            return ObjectMapper.Map<PwdTenant, TenantDto>(item);
        }

        public async Task DeleteAsync(int id)
        {
            await tenantRepository.DeleteAsync(x => x.Id == id);
        }


    }
}
