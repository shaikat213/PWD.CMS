using PWD.CMS.CMSEnums;
using PWD.CMS.DtoModels;
using PWD.CMS.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace PWD.CMS.Services
{
    public class AttachmentService : CrudAppService<Attachment, AttachmentDto, int>
    {
        private readonly IRepository<Attachment, int> repository;
        public AttachmentService(IRepository<Attachment, int> repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<List<AttachmentDto>> GetAttachmentInfo(string entityType, int? entityId, string attachmentType)
        {
            var attachment = await repository.GetListAsync(x => x.EntityType == (EntityType)Enum.Parse(typeof(EntityType), entityType) && x.EntityId == entityId && x.AttachmentType == (AttachmentType)Enum.Parse(typeof(AttachmentType), attachmentType));            
            if (attachment != null)
            {
                return ObjectMapper.Map<List<Attachment>, List<AttachmentDto>>((List<Attachment>)attachment);
            }

            return null;
        }
    }
}
