using MassTransit;

namespace WebApi;

public class HeartBeatConsumer : IConsumer<HeartBeat>
{
    private readonly ILogger<HeartBeatConsumer> logger;
    private readonly Random random;

    public HeartBeatConsumer(ILogger<HeartBeatConsumer> logger)
    {
        this.logger = logger;
        this.random = new Random();
    }

    public async Task Consume(ConsumeContext<HeartBeat> context)
    {
        await Task.Delay(TimeSpan.FromSeconds(this.random.Next(10)), context.CancellationToken);
        logger.LogInformation("Received heartbeat with ID {Id} and timestamp {Timestamp}", context.Message.Identifier, context.Message.Timestamp);
    }
}