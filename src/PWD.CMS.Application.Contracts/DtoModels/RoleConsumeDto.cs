using System;

namespace PWD.CMS.DtoModels
{
    public class RoleConsumeDto
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public bool isPublic { get; set; }
        public bool isDefault { get; set; }
    }
}
