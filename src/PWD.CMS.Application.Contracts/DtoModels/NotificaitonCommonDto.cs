using PWD.CMS.CMSEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PWD.CMS.DtoModels
{
    public class NotificaitonCommonDto
    {
        public string Message { get; set; }
        public string CreatorName { get; set; }
        public string ReceiverName { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid CreatedFor { get; set; }
    }
}
