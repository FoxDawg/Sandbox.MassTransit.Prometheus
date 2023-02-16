using MassTransit;

namespace WebApi;

public class HeartBeatSender : IHostedService
{
    private readonly IBus messageBus;
    private readonly ILogger<HeartBeatSender> logger;
    private Task task;

    public HeartBeatSender(IBus messageBus, ILogger<HeartBeatSender> logger)
    {
        this.messageBus = messageBus;
        this.logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.task = Task.Run(async () =>
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
                    var heartBeat = new HeartBeat();
                    await messageBus.Publish(heartBeat, cancellationToken);
                    this.logger.LogInformation("Sent heartbeat {Timestamp}", heartBeat.Timestamp);
                    
                }
                catch (OperationCanceledException)
                {
                    logger.LogWarning("Task cancelled");
                }
            }
        }, cancellationToken);
        return Task.CompletedTask;
        
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await this.task;
    }
}