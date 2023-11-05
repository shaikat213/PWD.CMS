﻿using PWD.CMS.CMSEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PWD.CMS.DtoModels
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public string ClientId { get; set; }
        public string Message { get; set; }
        public string CreatorName { get; set; }
        public string ReceiverName { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string ResourceUrl { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid CreatedFor { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Unread { get; set; }
    }
}
