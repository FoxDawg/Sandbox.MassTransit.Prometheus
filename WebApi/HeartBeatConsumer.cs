using MassTransit;

namespace WebApi;

public class HeartBeatConsumer : IConsumer<HeartBeat>
{
    private readonly ILogger<HeartBeatConsumer> logger;

    public HeartBeatConsumer(ILogger<HeartBeatConsumer> logger)
    {
        this.logger = logger;
    }

    public Task Consume(ConsumeContext<HeartBeat> context)
    {
        logger.LogInformation("Received heartbeat with ID {Id} and timestamp {Timestamp}", context.Message.Identifier, context.Message.Timestamp);
        return Task.CompletedTask;
    }
}