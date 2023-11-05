using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace PWD.CMS.DtoModels
{
    public class DistrictLookupDto : EntityDto<int>
    {
        public string Name { get; set; }
    }

    public class DistrictDto : FullAuditedEntityDto<int>
    {
        public string Name { get; set; }
        public string OrganizationUnits { get; set; }
    }
}
