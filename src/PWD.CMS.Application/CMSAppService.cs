using System;
using System.Collections.Generic;
using System.Text;
using PWD.CMS.Localization;
using Volo.Abp.Application.Services;

namespace PWD.CMS;

/* Inherit your application services from this class.
 */
public abstract class CMSAppService : ApplicationService
{
    protected CMSAppService()
    {
        LocalizationResource = typeof(CMSResource);
    }
}
