using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            var appAssembly = typeof(ServiceCollectionExtensions).Assembly;
            services.AddMediatR(conf => conf.RegisterServicesFromAssembly(appAssembly));

            services.AddAutoMapper(appAssembly);
            services.AddValidatorsFromAssembly(appAssembly)
                .AddFluentValidationAutoValidation();
        }
    }
}
