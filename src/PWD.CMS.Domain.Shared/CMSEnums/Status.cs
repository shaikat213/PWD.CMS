using System;
using System.Collections.Generic;
using System.Text;

namespace PWD.CMS.CMSEnums
{
    public enum Status
    {
        None = 0,
        Created = 1,
        Processing = 2,
        Completed = 3,
        OnHold = 4,
        Approved = 5,
        Authorized = 6,
        Rejected = 7,
        Cancelled = 8
    }
}
