using Microsoft.EntityFrameworkCore;
using PWD.CMS.Models;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace PWD.CMS.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class CMSDbContext :
    AbpDbContext<CMSDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }

    // Tenant Management
   // public DbSet<Models.Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    // Test Entity
    public DbSet<TestEntity> TestEntities { get; set; }

    public DbSet<Volo.Abp.TenantManagement.Tenant> Tenants { get; set; }
    #endregion

    public DbSet<ProblemType> ProblemTypes { get; set; }
    public DbSet<BuildingType> BuildingTypes { get; set; }
    //public DbSet<BuildingClass> BuildingClasses { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<OrganizaitonUnit> OrganizaitonUnits { get; set; }

    // Data Entry [Setup]
    public DbSet<Quarter> Quarters { get; set; }
    public DbSet<Building> Buildings { get; set; }
    public DbSet<Apartment> Apartments { get; set; }
    public DbSet<PwdTenant> PwdTenants { get; set; }
    public DbSet<Allotment> Allotments { get; set; }
    public DbSet<Complain> Complains { get; set; }
    public DbSet<ComplainHistory> ComplainHistories { get; set; }
    public DbSet<TextMessage> Messages { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Otp> Otps { get; set; }
    public CMSDbContext(DbContextOptions<CMSDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        //builder.Entity<TestEntity>(b => b.ToTable("TestEntities"));
        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(CMSConsts.DbTablePrefix + "YourEntities", CMSConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
    }
}
