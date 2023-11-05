using PWD.CMS.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace PWD.CMS;

[DependsOn(
    typeof(CMSEntityFrameworkCoreTestModule)
    )]
public class CMSDomainTestModule : AbpModule
{

}
