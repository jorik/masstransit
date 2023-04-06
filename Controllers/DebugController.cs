using concurrency.Components.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace concurrency.Controllers;

[ApiController]
[Route("[controller]")]
public class DebugController : ControllerBase
{
    [HttpGet(Name = "Debug")]
    public async Task<IActionResult> Get(IPublishEndpoint publishEndpoint)
    {
        var correlationId = NewId.NextSequentialGuid();
        await publishEndpoint.Publish(new CreateInstance(correlationId));

        // give the state machine time to process the message
        await Task.Delay(1000);

        await publishEndpoint.Publish(new FinalizeInstance(correlationId));
        await publishEndpoint.Publish(new FinalizeInstance(correlationId));

        return Ok();
    }
}
