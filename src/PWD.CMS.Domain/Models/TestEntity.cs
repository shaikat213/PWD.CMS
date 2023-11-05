using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace PWD.CMS.Models
{
    public class TestEntity : Entity<Guid>
    {
        public string TestName { get; set; }
        public string TestDescription { get; set; }
    }

}
