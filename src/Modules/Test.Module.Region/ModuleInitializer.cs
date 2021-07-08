using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SimplCommerce.Infrastructure;
using SimplCommerce.Infrastructure.Modules;
using Test.Module.Region.Services;

namespace Test.Module.Region
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IRegionService, RegionService>();
            GlobalConfiguration.RegisterAngularModule("test.region");
        }
    }
}
