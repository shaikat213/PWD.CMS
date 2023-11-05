﻿using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace PWD.CMS.Data;

/* This is used if database provider does't define
 * ICMSDbSchemaMigrator implementation.
 */
public class NullCMSDbSchemaMigrator : ICMSDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
