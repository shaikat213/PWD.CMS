using PWD.CMS.DtoModels;
using PWD.CMS.InputDtos;
using PWD.CMS.Interfaces;
using PWD.CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace PWD.CMS.Services
{
    public class TestEntityAppService : ApplicationService, ITestEntityAppService
    {
        private readonly IRepository<TestEntity, Guid> _tesRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        
        public TestEntityAppService(IRepository<TestEntity, Guid> tesRepository,
                                    IUnitOfWorkManager unitOfWorkManager)
        {
            _tesRepository = tesRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<TestEntityDto> CreateAsync(TestEntityInputDto input)
        {
            var newEntity = ObjectMapper.Map<TestEntityInputDto, TestEntity>(input);

            var testEntity = await _tesRepository.InsertAsync(newEntity);

            await _unitOfWorkManager.Current.SaveChangesAsync();

            return ObjectMapper.Map<TestEntity, TestEntityDto>(testEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _tesRepository.DeleteAsync(id);
        }

        public async Task<TestEntityDto> GetAsync(Guid id)
        {
            var testEntities = await _tesRepository.GetListAsync();
            var testEntity = testEntities.FirstOrDefault(i => i.Id == id);
            return ObjectMapper.Map<TestEntity, TestEntityDto>(testEntity);
        }

        public async Task<List<TestEntityDto>> GetListAsync()
        {
            var tests = await _tesRepository.GetListAsync();
            return ObjectMapper.Map<List<TestEntity>, List<TestEntityDto>>(tests);
        }

        public async Task<TestEntityDto> UpdateAsync(TestEntityInputDto input)
        {
            var updateEntity = ObjectMapper.Map<TestEntityInputDto, TestEntity>(input);

            var testEntity = await _tesRepository.UpdateAsync(updateEntity);

            return ObjectMapper.Map<TestEntity, TestEntityDto>(testEntity);
        }
    }
}
