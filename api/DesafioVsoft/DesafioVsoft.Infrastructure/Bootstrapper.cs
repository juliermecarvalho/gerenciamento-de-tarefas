using DesafioVsoft.Domain.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DesafioVsoft.Infrastructure;

public static class Bootstrapper
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IRabbitMqProducer, RabbitMqProducer>();
        services.AddSingleton(Channel.CreateUnbounded<string>());
        services.AddHostedService<RabbitMqConsumerService>();
    }
}
