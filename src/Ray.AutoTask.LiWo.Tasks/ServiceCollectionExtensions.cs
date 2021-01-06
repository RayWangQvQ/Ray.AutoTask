using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ray.AutoTask.LiWo.Tasks
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions()
                .Configure<TaskOptions>(configuration.GetSection("Tasks"));


            return services;
        }
    }
}
