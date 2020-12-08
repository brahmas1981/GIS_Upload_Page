using GIS_Upload_Page.Classes.Parsers;
using GIS_Upload_Page.Data;
using GIS_Upload_Page.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GIS_Upload_Page.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // per request- scoped
            services.AddScoped<IUploadPageContext, Upload_PageContext>();
            services.AddScoped<IParser<MasterCcViewModel>, MasterCcCsvParser>();

           services.AddScoped<IDataService<UploadViewModel>, UploadDataService>();
           services.AddScoped<IDataService<MasterCcViewModel>, MasterCcDataService>();
            // transient

            return services;
        }
    }
}
