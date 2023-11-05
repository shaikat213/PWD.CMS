using PWD.CMS.DtoModels;
using PWD.CMS.Interfaces;
using PWD.CMS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using System;

namespace PWD.CMS.Services
{
    public class BuildingService : CMSAppService, IBuildingService
    {
        private readonly IRepository<Building, int> repository;
        public BuildingService(
            IRepository<Building, int> repository,
            IRepository<Apartment, int> apartmentRepository)
        {
            this.repository = repository;
            //this.apartmentRepository = apartmentRepository;
        }

        public async Task<BuildingDto> CreateAsync(BuildingDto input)
        {
            var newItem = ObjectMapper.Map<BuildingDto, Building>(input);

            var item = await repository.InsertAsync(newItem);

            return ObjectMapper.Map<Building, BuildingDto>(item);
        }
        public async Task DeleteAsync(int id)
        {
            await repository.DeleteAsync(x => x.Id == id);
        }
        public async Task<BuildingDto> GetAsync(int id)
        {
            var item = await repository.GetAsync(x => x.Id == id);

            return ObjectMapper.Map<Building, BuildingDto>(item);
        }

        public async Task<List<BuildingDto>> GetBuildingListAsync()
        {
            List<BuildingDto> list = null;
            var items = await repository.WithDetailsAsync(p => p.Quarter);
            if (items.Any())
            {
                list = new List<BuildingDto>();
                foreach (var item in items)
                {
                    list.Add(new BuildingDto()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        QuarterId = item.QuarterId,
                        QuarterName = item.Quarter?.Name,
                        CivilSubDivisionId = item.CivilSubDivisionId,
                        EmSubDivisionId = item.EmSubDivisionId,
                    });
                }
            }
            return list;
        }
        public async Task<List<BuildingDto>> GetListAsync(FilterModel filterModel)
        {
            List<BuildingDto> list = null;
            var items = await repository.WithDetailsAsync(p => p.Quarter);
            if (items.Any())
            {
                items = items.Skip(filterModel.Offset)
                               .Take(filterModel.Limit);
                list = new List<BuildingDto>();
                foreach (var item in items)
                {
                    list.Add(new BuildingDto()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        QuarterId = item.QuarterId,
                        QuarterName = item.Quarter?.Name,
                        CivilSubDivisionId = item.CivilSubDivisionId,
                        EmSubDivisionId = item.EmSubDivisionId,
                    });
                }
            }
            return list;
        }
        public async Task<List<BuildingDto>> GetListByQuarterAsync(int id)
        {
            List<BuildingDto> list = null;
            var items = await repository.WithDetailsAsync(p => p.Quarter);
            items = items.Where(i => i.QuarterId == id);
            if (items.Any())
            {
                list = new List<BuildingDto>();
                foreach (var item in items)
                {
                    list.Add(new BuildingDto()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        QuarterId = item.QuarterId,
                        QuarterName = item.Quarter?.Name,
                        CivilSubDivisionId = item.CivilSubDivisionId,
                        EmSubDivisionId = item.EmSubDivisionId,
                    });
                }
            }

            return list;
        }
        public async Task<BuildingDto> UpdateAsync(BuildingDto input)
        {
            var updateItem = ObjectMapper.Map<BuildingDto, Building>(input);

            var item = await repository.UpdateAsync(updateItem);

            return ObjectMapper.Map<Building, BuildingDto>(item);
        }
        public async Task<int> GetCountAsync()
        {
            return (await repository.GetListAsync()).Count;
        }
        public async Task<List<BuildingDto>> GetSortedListAsync(FilterModel filterModel)
        {
            var buildings = await repository.WithDetailsAsync();
            buildings = buildings.Skip(filterModel.Offset)
                            .Take(filterModel.Limit);
            return ObjectMapper.Map<List<Building>, List<BuildingDto>>(buildings.ToList());
        }
        //public async Task<int> GetCountBySDIdAsync(Guid? civilSDId, Guid? emSDId)
        public async Task<int> GetCountBySDIdAsync(Guid? sdId)
        {
            var buildings = await repository.WithDetailsAsync();
            if (sdId != null)//civilSDId != null && emSDId != null)
            {
                buildings = buildings.Where(q => q.CivilSubDivisionId == sdId || q.EmSubDivisionId == sdId);
            }
            //else if (civilSDId != null)
            //{
            //    buildings = buildings.Where(q => q.CivilSubDivisionId == civilSDId);
            //}
            //else if (emSDId != null)
            //{
            //    buildings = buildings.Where(q => q.EmSubDivisionId == emSDId);
            //}
            return buildings.Count();
        }
        //public async Task<List<BuildingDto>> GetSortedListBySDIdAsync(Guid? civilSDId, Guid? emSDId, FilterModel filterModel)
        public async Task<List<BuildingDto>> GetSortedListBySDIdAsync(Guid? sdId, FilterModel filterModel)
        {
            List<BuildingDto> buildings = null;
            var items = await repository.WithDetailsAsync(p => p.Quarter);
            if (items.Any())
            {
                if (sdId != null)// && emSDId != null)
                {
                    items = items.Where(q => q.CivilSubDivisionId == sdId || q.EmSubDivisionId == sdId);
                }
                //else if (civilSDId != null)
                //{
                //    items = items.Where(q => q.CivilSubDivisionId == civilSDId);
                //}
                //else if (emSDId != null)
                //{
                //    items = items.Where(q => q.EmSubDivisionId == emSDId);
                //}
                items = items.Skip(filterModel.Offset)
                               .Take(filterModel.Limit);
                buildings = new List<BuildingDto>();
                foreach (var item in items)
                {
                    buildings.Add(new BuildingDto()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        QuarterId = item.QuarterId,
                        QuarterName = item.Quarter?.Name,
                        CivilSubDivisionId = item.CivilSubDivisionId,
                        EmSubDivisionId = item.EmSubDivisionId,
                    });
                }
            }
            return buildings;
        }

        //public async Task<List<BuildingDto>> GetListBySDIdAsync(Guid? civilSDId)
        public async Task<List<BuildingDto>> GetListBySDIdAsync(Guid? sdId)
        {
            List<BuildingDto> buildings = null;
            var items = await repository.WithDetailsAsync(p => p.Quarter);
            if (items.Any())
            {
                if (sdId != null)
                {
                    items = items.Where(q => q.CivilSubDivisionId == sdId || q.EmSubDivisionId == sdId);
                }

                buildings = new List<BuildingDto>();
                foreach (var item in items)
                {
                    buildings.Add(new BuildingDto()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        QuarterId = item.QuarterId,
                        QuarterName = item.Quarter?.Name,
                        CivilSubDivisionId = item.CivilSubDivisionId,
                        EmSubDivisionId = item.EmSubDivisionId,
                    });
                }
            }
            return buildings;
        }
        public async Task<int> GetCountByQuarterIdAsync(int qId)
        {
            var buildings = await repository.WithDetailsAsync();
            if (qId > 0)
            {
                buildings = buildings.Where(q => q.QuarterId == qId);
            }
            return buildings.Count();
        }
        public async Task<List<BuildingDto>> GetListByQuarterIdAsync(int qId, FilterModel filterModel)
        {
            List<BuildingDto> list = null;
            var items = await repository.WithDetailsAsync(p => p.Quarter);
            items = items.Where(i => i.QuarterId == qId);
            if (items.Any())
            {
                items = items.Skip(filterModel.Offset)
                               .Take(filterModel.Limit);
                list = new List<BuildingDto>();
                foreach (var item in items)
                {
                    list.Add(new BuildingDto()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        QuarterId = item.QuarterId,
                        QuarterName = item.Quarter?.Name,
                        CivilSubDivisionId = item.CivilSubDivisionId,
                        EmSubDivisionId = item.EmSubDivisionId,
                    });
                }
            }
            return list;
        }
    }
}
