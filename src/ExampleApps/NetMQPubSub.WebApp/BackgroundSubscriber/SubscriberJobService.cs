namespace NetMQPubSub.WebApp.BackgroundSubscriber;

using NetMQPubSub.Subscriber;

public class SubscriberJobService : BackgroundService
{
	private const string MessageTopic = "JobMessage";
	private const string ConnectAddress = "tcp://localhost:54321";
	private readonly ILogger<SubscriberJobService> logger;
	private MessageSubscriber? subscriber = null;
	private Task? mainTask = null;

	public SubscriberJobService(
		 ILogger<SubscriberJobService> logger)
	{
		this.logger = logger;
	}

	public override Task StartAsync(CancellationToken cancellationToken)
	{
		return base.StartAsync(cancellationToken);
	}

	protected override Task ExecuteAsync(CancellationToken cancelToken)
	{
		this.mainTask = Task.Run(() =>
		{
			logger.LogInformation($"<== {nameof(SubscriberJobService)} is starting.");
			InitializeSubscriber();

			var timeout = TimeSpan.FromSeconds(1);
			do
			{
				if (this.subscriber!.TryReceiveStringMessage(timeout, out var topic, out string? message))
				{
					this.logger.LogInformation($"<== Received topic {topic} message: \"{message}\"");
				}
			} while (!cancelToken.IsCancellationRequested);
		}, cancelToken);

		return Task.CompletedTask;
	}

	public override async Task StopAsync(CancellationToken stoppingToken)
	{
		if (this.mainTask is not null && this.subscriber is not null)
		{
			logger.LogInformation($"<== {nameof(SubscriberJobService)} is stopping.");
			this.subscriber.Close();
		}

		await base.StopAsync(stoppingToken);
	}

	private void InitializeSubscriber()
	{
		this.subscriber = new MessageSubscriber();
		this.subscriber.Connect(ConnectAddress);
		this.subscriber.TopicSubscribe(MessageTopic);
	}
}
