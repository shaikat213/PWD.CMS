using PWD.CMS.CMSEnums;
using Volo.Abp.Application.Dtos;

namespace PWD.CMS.DtoModels
{
    public class AttachmentDto : FullAuditedEntityDto<int>
    {
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string Path { get; set; }
        public EntityType EntityType { get; set; }
        public int? ImporterId { get; set; }
        public int? EntityId { get; set; }
        public AttachmentType AttachmentType { get; set; }
    }
}
