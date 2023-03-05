using MassTransit;

namespace WebApi;

public class HeartBeatSender : IHostedService
{
    private readonly ILogger<HeartBeatSender> logger;
    private readonly IBus messageBus;
    private readonly Random random;
    private CancellationTokenSource cts;
    private Task task;

    public HeartBeatSender(IBus messageBus, ILogger<HeartBeatSender> logger)
    {
        this.messageBus = messageBus;
        this.logger = logger;
        random = new Random();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        cts = new CancellationTokenSource();
        task = Task.Run(async () =>
        {
            while (cts.Token.IsCancellationRequested == false)
            {
                try
                {
                    var numberOfMessages = random.Next(10);
                    var messages = Enumerable.Range(0, numberOfMessages).Select(o => new HeartBeat());
                    await messageBus.PublishBatch(messages, cts.Token);
                    logger.LogInformation("Sent {Number} heartbeats", numberOfMessages);

                    var wait = TimeSpan.FromSeconds(random.Next(10));
                    await Task.Delay(wait, cts.Token);
                }
                catch (OperationCanceledException)
                {
                    logger.LogWarning("Task cancelled");
                }
            }
        }, cts.Token);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        cts.Cancel();
        await task;
    }
}