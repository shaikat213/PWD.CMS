﻿using Volo.Abp.Domain.Entities.Auditing;

namespace PWD.CMS.Models
{
    public class BuildingType : FullAuditedEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
