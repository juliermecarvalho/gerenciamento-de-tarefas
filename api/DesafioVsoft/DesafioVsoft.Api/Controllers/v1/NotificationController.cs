using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;

namespace DesafioVsoft.Api.Controllers.v1;



[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly Channel<string> _eventChannel;

    public NotificationController(Channel<string> eventChannel)
    {
        _eventChannel = eventChannel;
    }

    [HttpGet("stream")]
    public async Task Stream(CancellationToken cancellationToken)
    {
        Response.Headers.Add("Content-Type", "text/event-stream");

        var reader = _eventChannel.Reader;

        while (!cancellationToken.IsCancellationRequested)
        {
            while (reader.TryRead(out var message))
            {
                await Response.WriteAsync($"data: {message}\n\n", cancellationToken);
                await Response.Body.FlushAsync(cancellationToken);
            }

            await Task.Delay(100, cancellationToken);
        }
    }
}

