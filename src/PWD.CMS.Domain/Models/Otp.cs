﻿using PWD.CMS.CMSEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace PWD.CMS.Models
{
    public class Otp : FullAuditedEntity<int>
    {
        public int OtpNo { get; set; }
        public string MobileNo { get; set; }
        public DateTime ExpireDateTime { get; set; }
        public OtpStatus OtpStatus { get; set; }
        public int? MaxAttempt { get; set; }
    }
}
