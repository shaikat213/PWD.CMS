using PWD.CMS.CMSEnums;
using Volo.Abp.Application.Dtos;

namespace PWD.CMS.DtoModels
{
    public class TenantDto : FullAuditedEntityDto<int>
    {
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public IdType IdType { get; set; }
        public string IdTypeStr { get; set; }
        public string IdNumber { get; set; }
        public string PermanentAddress { get; set; }
        public string Designation { get; set; }
        public string Note { get; set; }
        public int? AllotmentId { get; set; }        
        public string AllotmentName { get; set; }
    }
}
