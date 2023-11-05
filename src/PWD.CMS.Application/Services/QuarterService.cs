using PWD.CMS.DtoModels;
using PWD.CMS.Interfaces;
using PWD.CMS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System;

namespace PWD.CMS.Services
{
    public class QuarterService : CMSAppService, IQuarterService
    {
        private readonly IRepository<Quarter> repository;
        private readonly IRepository<Quarter> quarterRepository;

        public QuarterService(IRepository<Quarter> repository, IRepository<Quarter> quarterRepository)
        {
            this.repository = repository;
            this.quarterRepository = quarterRepository;
        }
        public async Task<QuarterDto> CreateAsync(QuarterDto input)
        {
            var newItem = ObjectMapper.Map<QuarterDto, Quarter>(input);

            var item = await repository.InsertAsync(newItem);

            return ObjectMapper.Map<Quarter, QuarterDto>(item);
        }
        public async Task DeleteAsync(int id)
        {
            await repository.DeleteAsync(x => x.Id == id);
        }
        public async Task<QuarterDto> GetAsync(int id)
        {
            var item = await repository.GetAsync(x => x.Id == id);

            return ObjectMapper.Map<Quarter, QuarterDto>(item);
        }
        public async Task<List<QuarterDto>> GetListAsync()
        {
            List<QuarterDto> list = null;
            var items = await repository.WithDetailsAsync(p => p.District);
            if (items.Any())
            {
                list = new List<QuarterDto>();
                foreach (var item in items)
                {
                    list.Add(new QuarterDto()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        DistrictId = item.DistrictId,
                        DistrictName = item.District?.Name,
                        CivilSubDivisionId = item.CivilSubDivisionId,
                        EmSubDivisionId = item.EmSubDivisionId,
                    });
                }
            }

            return list;
        }
        public async Task<List<QuarterDto>> GetListByDistrictAsync(int id)
        {
            List<QuarterDto> list = null;
            var items = await repository.WithDetailsAsync(p => p.District);
            items = items.Where(i => i.DistrictId == id);
            if (items.Any())
            {
                list = new List<QuarterDto>();
                foreach (var item in items)
                {
                    list.Add(new QuarterDto()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        DistrictId = item.DistrictId,
                        DistrictName = item.District?.Name,
                        CivilSubDivisionId = item.CivilSubDivisionId,
                        EmSubDivisionId = item.EmSubDivisionId,
                    });
                }
            }

            return list;
        }
        public async Task<QuarterDto> UpdateAsync(QuarterDto input)
        {
            var updateItem = ObjectMapper.Map<QuarterDto, Quarter>(input);

            var item = await repository.UpdateAsync(updateItem);

            return ObjectMapper.Map<Quarter, QuarterDto>(item);
        }
        public async Task<int> GetCountAsync()
        {
            return (await quarterRepository.GetListAsync()).Count;
        }
        public async Task<List<QuarterDto>> GetSortedListAsync(FilterModel filterModel)
        {
            var quarters = await quarterRepository.WithDetailsAsync();
            quarters = quarters.Skip(filterModel.Offset)
                            .Take(filterModel.Limit);
            return ObjectMapper.Map<List<Quarter>, List<QuarterDto>>(quarters.ToList());
        }
        //public async Task<int> GetCountBySDIdAsync(Guid? civilSDId, Guid? emSDId)
        public async Task<int> GetCountBySDIdAsync(Guid? sdId)
        {
            var quarters = await quarterRepository.WithDetailsAsync();
            //if (civilSDId != null && emSDId != null)
            //{
            //    quarters = quarters.Where(q => q.CivilSubDivisionId == civilSDId && q.EmSubDivisionId == emSDId);
            //}
            if (sdId != null)
            {
                quarters = quarters.Where(q => q.CivilSubDivisionId == sdId || q.EmSubDivisionId == sdId);
            }
            //else if (emSDId != null)
            //{
            //    quarters = quarters.Where(q => q.EmSubDivisionId == emSDId);
            //}
            return quarters.Count();
        }
        //public async Task<List<QuarterDto>> GetSortedListBySDIdAsync(Guid? civilSDId, Guid? emSDId, FilterModel filterModel)
        public async Task<List<QuarterDto>> GetSortedListBySDIdAsync(Guid? sdId, FilterModel filterModel)
        {
            var quarters = await quarterRepository.WithDetailsAsync();
            //if (civilSDId != null && emSDId != null)
            //{
            //    quarters = quarters.Where(q => q.CivilSubDivisionId == civilSDId && q.EmSubDivisionId == emSDId);
            //}
            //else if (civilSDId != null)
            //{
            if (sdId != null)
                quarters = quarters.Where(q => q.CivilSubDivisionId == sdId || q.EmSubDivisionId == sdId);
            //}
            //else if (emSDId != null)
            //{
            //    quarters = quarters.Where(q => q.EmSubDivisionId == emSDId);
            //}
            quarters = quarters.Skip(filterModel.Offset)
                            .Take(filterModel.Limit);
            return ObjectMapper.Map<List<Quarter>, List<QuarterDto>>(quarters.ToList());
        }
        public async Task<List<QuarterDto>> GetListBySDIdAsync(Guid? sdId)
        {
            var quarters = await quarterRepository.WithDetailsAsync();
            if (sdId != null)
            {
                quarters = quarters.Where(q => q.CivilSubDivisionId == sdId || q.EmSubDivisionId == sdId);
            }
            return ObjectMapper.Map<List<Quarter>, List<QuarterDto>>(quarters.ToList());
        }
    }
}
