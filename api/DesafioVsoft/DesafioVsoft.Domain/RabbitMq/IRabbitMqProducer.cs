using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioVsoft.Domain.RabbitMq;

public interface IRabbitMqProducer
{
    Task PublishUserChangedAsync(string message);
}
