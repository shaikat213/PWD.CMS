using PWD.CMS.CMSEnums;

namespace PWD.CMS.DtoModels
{
    public class ProblemTypeInputDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DepartmentType Type { get; set; }
    }
}
