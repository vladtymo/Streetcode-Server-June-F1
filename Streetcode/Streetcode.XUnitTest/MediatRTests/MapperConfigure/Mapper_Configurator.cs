using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.XUnitTest.MediatRTests.MapperConfigure
{
    public static class Mapper_Configurator
    {
        public static IMapper Create<TMapProfile>()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddAutoMapper(typeof(TMapProfile));
            return services.BuildServiceProvider().GetService<IMapper>();
        }
    }
}
