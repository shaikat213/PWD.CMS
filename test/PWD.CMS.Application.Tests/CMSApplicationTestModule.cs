using Volo.Abp.Modularity;

namespace PWD.CMS;

[DependsOn(
    typeof(CMSApplicationModule),
    typeof(CMSDomainTestModule)
    )]
public class CMSApplicationTestModule : AbpModule
{

}
