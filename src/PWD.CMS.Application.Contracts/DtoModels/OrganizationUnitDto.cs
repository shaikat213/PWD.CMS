using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace PWD.CMS.DtoModels
{
    public class OrganizationUnitDto
    {
        public virtual Guid? id { get; set; }
        public virtual Guid? parentId { get; set; }
        public virtual Guid? userId { get; set; }
        public virtual string code { get; set; }
        public virtual string displayName { get; set; }
        public virtual string civilEm { get; set; }
        public virtual List<OrgRoleConsumeDto> roles { get; set; }
        public virtual List<string> roleNames { get; set; } = new List<string>();

    }

    public class PostingDto : FullAuditedEntityDto<Guid>
    {
        public int PostingId { get; set; }
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string NameBn { get; set; }
        public string Post { get; set; }
        public string Designation { get; set; }
        public string DesignationBn { get; set; }
        public string Office { get; set; }
        public string OfficeBn { get; set; }
        public Guid? OrgUniId { get; set; }
        public string EmpPhoneMobile { get; set; }
        public string EmpEmail { get; set; }

    }
    public class PostingConsumeDto
    {
        public Guid id { get; set; }
        public Guid orgUniId { get; set; }
        public int postingId { get; set; }
        public int employeeId { get; set; }
        public string name { get; set; }
        public string nameBn { get; set; }
        public string post { get; set; }
        public string designation { get; set; }
        public string designationBn { get; set; }
        public string office { get; set; }
        public string officeBn { get; set; }

    }

    public class EmployeeDto
    {
        public int id { get; set; }
        public int employeeId { get; set; }
        public string fullName { get; set; }
        public string phoneMobile { get; set; }
    }
}
