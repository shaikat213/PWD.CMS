using PWD.CMS.DtoModels;
using PWD.CMS.Models;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PWD.CMS.Services
{
    public class ProblemTypeService : CrudAppService<ProblemType, ProblemTypeDto, int>
    {
        private readonly IRepository<ProblemType, int> problemTypeRepository;
        public ProblemTypeService(IRepository<ProblemType, int> repository,
            IRepository<ProblemType, int> problemTypeRepository) : base(repository)
        {
            this.problemTypeRepository = problemTypeRepository;
        }

        public async Task<int> GetCountAsync()
        {
            return (await problemTypeRepository.GetListAsync()).Count;
        }

        public async Task<List<ProblemTypeDto>> GetSortedListAsync(FilterModel filterModel)
        {
            var problemTypes = await problemTypeRepository.WithDetailsAsync();
            problemTypes = problemTypes.Skip(filterModel.Offset)
                            .Take(filterModel.Limit);
            return ObjectMapper.Map<List<ProblemType>, List<ProblemTypeDto>>(problemTypes.ToList());
        }
    }
}
