namespace NetMQPubSubExample;

using System.Text.Json;

internal class Program
{

	private readonly List<string> topics = new List<string> { "TopicA", "TopicB", "TopicC" };

	static void Main(string[] args)
	{
		new Program().Run();
	}

	private void Run()
	{
		Console.Write("Press Enter to begin.  Once running, press Enter again to stop.");
		Console.ReadLine();

		var cancellationTokenSource = new CancellationTokenSource();
		var server = Task.Run(() => Subscriber(cancellationTokenSource.Token));
		var client = Task.Run(() => Publisher(cancellationTokenSource.Token));

		Console.ReadLine();

		cancellationTokenSource.Cancel();
		Task.WaitAll(server, client);
	}

	private void Subscriber(CancellationToken cancelToken)
	{
		// or use "inproc://{name}" for in-process (e.g. inproc://job-service)
		var addr = "tcp://localhost:12345"; 

		Console.WriteLine("Subscriber socket connecting...");
		using IMessageSubscriber subscriber = new MessageSubscriber();
		subscriber.Options.ReceiveHighWatermark = 1000;
		subscriber.Connect(addr);
		topics.ForEach(topic => { subscriber.TopicSubscribe(topic); });

		const int maxSecondsDelayBeforeCancelCheck = 2;
		var timeout = TimeSpan.FromSeconds(maxSecondsDelayBeforeCancelCheck);
		do
		{
			if (subscriber.TryReceiveTopicMessage(timeout, out var messageTopicReceived, out var moreFrames))
			{
				if (moreFrames)
				{
					TestMessage? messageReceived = null;
					messageReceived = subscriber.ReceiveMessage<TestMessage>();
					Console.WriteLine($"<== Received message for topic \"{messageTopicReceived}\". Message: {JsonSerializer.Serialize(messageReceived)}");
				}
			}

		} while (!cancelToken.IsCancellationRequested);

		subscriber.Disconnect(addr);
		Console.WriteLine("<== Server done!");
	}

	private void Publisher(CancellationToken cancelToken)
	{
		// or use "inproc://{name}" for in-process (e.g. inproc://job-service)
		var addr = "tcp://localhost:12345";

		using IMessagePublisher publisher = new MessagePublisher();
		publisher.Options.SendHighWatermark = 1000;

		Console.WriteLine("Publisher socket binding...");
		publisher.Bind(addr);

		// now that we've bound a socket, give subscriber a bit of time to get started
		// before we begin sending messages
		Thread.Sleep(1000);

		var counter = 0;
		var rand = new Random();
		do
		{
			var randomizedTopic = rand.Next(this.topics.Count);
			var topic = this.topics[randomizedTopic];

			++counter;

			Console.WriteLine($"==> Sending message for topic \"{topic}\". Message: #{counter}");
			publisher.SendTopicMessage(topic, new TestMessage() { Counter = counter });

			// simulate a bit of processing
			Thread.Sleep(500);

		} while (!cancelToken.IsCancellationRequested);

		publisher.Unbind(addr);
		Console.WriteLine("==> Client done!");
	}
}
