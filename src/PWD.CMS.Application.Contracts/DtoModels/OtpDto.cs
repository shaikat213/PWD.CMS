using PWD.CMS.CMSEnums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace PWD.CMS.DtoModels
{
    public class OtpDto : FullAuditedEntityDto<int>
    {
        public int OtpNo { get; set; }
        public string MobileNo { get; set; }
        public OtpStatus OtpStatus { get; set; }
        public int? MaxAttempt { get; set; }
    }
}
