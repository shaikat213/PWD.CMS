using System;

namespace PWD.CMS.DtoModels
{
    public class CreateUpdateOrganizaitonUnitDto
    {
        public Guid ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
    }
}
