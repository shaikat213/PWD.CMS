using PWD.CMS.DtoModels;
using PWD.CMS.Models;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PWD.CMS.Services
{
    public class DepartmentService : CrudAppService<Department, DepartmentDto, int>
    {
        private readonly IRepository<Department, int> departmentRepository;
        public DepartmentService(IRepository<Department, int> repository,
            IRepository<Department, int> departmentRepository) : base(repository)
        {
            this.departmentRepository = departmentRepository;
        }

        public async Task<int> GetCountAsync()
        {
            return (await departmentRepository.GetListAsync()).Count;
        }

        public async Task<List<DepartmentDto>> GetSortedListAsync(FilterModel filterModel)
        {
            var departments = await departmentRepository.WithDetailsAsync();
            departments = departments.Skip(filterModel.Offset)
                            .Take(filterModel.Limit);
            return ObjectMapper.Map<List<Department>, List<DepartmentDto>>(departments.ToList());
        }

    }   
}
