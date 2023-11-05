using System;
using Volo.Abp.Application.Dtos;

namespace PWD.CMS.DtoModels
{
    public class ApartmentLookupDto : EntityDto<int>
    {
        public string Name { get; set; }
    }
}
