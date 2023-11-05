using System;
using System.Collections.Generic;
using System.Text;

namespace PWD.CMS.CMSEnums
{
    public enum ComplainStatus
    {
        None = 0,
        New = 1,
        //Seen = 2,
        SiteVisit = 2,
        InProgress = 3,
        Complete = 4,
        TenantFeedback = 5,
        Cancel = 6
    }
}
