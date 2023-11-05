using System;
using System.Collections.Generic;
using System.Text;

namespace PWD.CMS.DtoModels
{
    public class SmsRequestInput
    {
        //public string api_token { get; } = "pnij27vt-wixyw8qu-a4rwdexc-kqvpgude-tt32mdiv";
        //public string sid { get; } = "PWDNONAPI";
        public string Msisdn { get; set; }
        public string Sms { get; set; }
        public string CsmsId { get; set; }
    }
}
