using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PWD.CMS.EntityFrameworkCore;
using PWD.CMS.MultiTenancy;
using StackExchange.Redis;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileSystem;

namespace PWD.CMS;

[DependsOn(
    typeof(CMSHttpApiModule),
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
    typeof(CMSApplicationModule),
    typeof(CMSEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
)]
public class CMSHttpApiHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        ConfigureConventionalControllers();
        ConfigureAuthentication(context, configuration);
        ConfigureLocalization();
        ConfigureCache(configuration);
        ConfigureVirtualFileSystem(context);
        ConfigureDataProtection(context, configuration, hostingEnvironment);
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context, configuration);
    }

    private void ConfigureCache(IConfiguration configuration)
    {
        Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = "CMS:"; });
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<CMSDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}PWD.CMS.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<CMSDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}PWD.CMS.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<CMSApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}PWD.CMS.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<CMSApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}PWD.CMS.Application"));
            });
        }
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(CMSApplicationModule).Assembly);
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["AuthServer:Authority"];
                options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                options.Audience = "CMS";
            });
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"],
            new Dictionary<string, string>
            {
                    {"CMS", "CMS API"}
            },
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "CMS API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("en-GB", "en-GB", "English (UK)"));
            //options.Languages.Add(new LanguageInfo("ar", "ar", "العربية"));
            //options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
            //options.Languages.Add(new LanguageInfo("fi", "fi", "Finnish"));
            //options.Languages.Add(new LanguageInfo("fr", "fr", "Français"));
            //options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi", "in"));
            //options.Languages.Add(new LanguageInfo("is", "is", "Icelandic", "is"));
            //options.Languages.Add(new LanguageInfo("it", "it", "Italiano", "it"));
            //options.Languages.Add(new LanguageInfo("ro-RO", "ro-RO", "Română"));
            //options.Languages.Add(new LanguageInfo("hu", "hu", "Magyar"));
            //options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
            //options.Languages.Add(new LanguageInfo("ru", "ru", "Русский"));
            //options.Languages.Add(new LanguageInfo("sk", "sk", "Slovak"));
            //options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
            //options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            //options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
            //options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "Deutsch", "de"));
            //options.Languages.Add(new LanguageInfo("es", "es", "Español", "es"));
        });
    }

    private void ConfigureDataProtection(
        ServiceConfigurationContext context,
        IConfiguration configuration,
        IWebHostEnvironment hostingEnvironment)
    {
        var dataProtectionBuilder = context.Services.AddDataProtection().SetApplicationName("CMS");
        if (!hostingEnvironment.IsDevelopment())
        {
            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            dataProtectionBuilder.PersistKeysToStackExchangeRedis(redis, "CMS-Protection-Keys");
        }
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.RemovePostFix("/"))
                            .ToArray()
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();
        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "CMS API");

            var configuration = context.GetConfiguration();
            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
            options.OAuthScopes("CMS");
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseUnitOfWork();
        app.UseConfiguredEndpoints();
    }
}
