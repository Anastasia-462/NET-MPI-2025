using ComplexTask_LINQ.Database;
using ComplexTask_LINQ.Provider.CustomProvider;
using ComplexTask_LINQ.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplexTask_LINQ.Test.Fakes
{
    public static class DependencyInjectionConfig
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Регистрация подключения к базе данных
            services.AddSingleton<IPgDatabaseConnection>(_ =>
                new PgDatabaseConnection("YourConnectionStringHere"));

            // Регистрация CustomQueryProvider через DI
            services.AddSingleton<ICustomQueryProvider, CustomQueryProvider>();

            // Регистрация SearchService
            services.AddTransient<SearchService>();
        }
    }
}
