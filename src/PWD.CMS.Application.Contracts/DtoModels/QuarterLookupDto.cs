using System;
using Volo.Abp.Application.Dtos;

namespace PWD.CMS.DtoModels
{
    public class QuarterLookupDto : EntityDto<int>
    {
        public string Name { get; set; }
    }
}
