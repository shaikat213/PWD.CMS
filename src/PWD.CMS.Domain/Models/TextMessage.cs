using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace PWD.CMS.Models
{

    public class TextMessage : FullAuditedEntity<int>
    {
        public int ApartmentId { get; set; }
        public int ComplainId { get; set; }
        public string ToNumber { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }

    }
}
