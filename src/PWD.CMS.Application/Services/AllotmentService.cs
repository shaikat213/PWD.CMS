using PWD.CMS.DtoModels;
using PWD.CMS.Models;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using PWD.CMS.InputDtos;
using PWD.CMS.Interfaces;
using Volo.Abp.Uow;

namespace PWD.CMS.Services
{
    public class AllotmentService : CrudAppService<Allotment, AllotmentDto, int>
    {
        private readonly IRepository<Allotment, int> allotmentRepository;
        public AllotmentService(IRepository<Allotment, int> repository,
            IRepository<Allotment, int> allotmentRepository) : base(repository)
        {
            this.allotmentRepository = allotmentRepository;
        }
        public async Task<AllotmentDto> GetAllotmentAsync(int id)
        {
            var item = await allotmentRepository.GetAsync(x => x.Id == id);

            return ObjectMapper.Map<Allotment, AllotmentDto>(item);
        }
        public async Task<int> GetCountAsync()
        {
            return (await allotmentRepository.GetListAsync()).Count;
        }

        public async Task<List<AllotmentDto>> GetSortedListAsync(FilterModel filterModel)
        {
            var allotments = await allotmentRepository.WithDetailsAsync();
            allotments = allotments.Skip(filterModel.Offset)
                            .Take(filterModel.Limit);
            return ObjectMapper.Map<List<Allotment>, List<AllotmentDto>>(allotments.ToList());
        }
    }
}
