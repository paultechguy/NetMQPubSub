# NetMQPubSub
NetMQPubSub is a general IPC publish-subscribe library written in NET Core. The underlying IPC transport layer is provided by
[NetMQ](https://github.com/zeromq/netmq).  The number of NetMQ features implemented in NetMQPubSub is limited to
only those required to support messaging between a publisher and subscribers. This is a small number of features
implemented, but we hope to expand those as needed in the future.

## Overview

NetMQPubSub provides support for features such as:
* NET Standard Library - Supports C#, Android, Linux, macOS, Windows, Library
* Multiple subscribers, subscribing to different topics.
* Cancellation of publishers and subscribers using a .NET Core CancellationToken.
* Can be used by both .NET Core web applications and console applications.
* Messages can sent/received as strings or class objects.

## License
NetMQPubSub uses the <a href="https://mit-license.org/" target="_blank_">MIT License</a> model.

## Nuget Packages

For most application you write with NetMQPubSub, you will need to include
the several of these nuget packages (along with [NetMQ](https://github.com/zeromq/netmq)):

* [NetMQPubSub.Common](https://www.nuget.org/packages/NetMQPubSub.Common) - Features that are common across both publisher and subscriber.
* [NetMQPubSub.Core](https://www.nuget.org/packages/NetMQPubSub.Core) - Interfaces for publisher and subscriber related components. This has no dependency on NetMQ libraries.
* [NetMQPubSub.Publisher](https://www.nuget.org/packages/NetMQPubSub.Publisher) - Features for publisher.
* [NetMQPubSub.Subscriber](https://www.nuget.org/packages/NetMQPubSub.Subscriber) - Features for subscriber.

**Note:** Except as noted, the [NetMQ](https://github.com/zeromq/netmq) IPC transport layer libraries must be
included in code projects.

## Example Applications

In addition, the repository also includes source for the following example applications:

* NetMQPubSub.ConsoleApp - This is a console-based application that demonstrates sending continuous messages from a single publisher to a large number of subscribers.  This also demonstrates how to cancel all messaging agents based on a NET Core cancellation token.
The source code for this application is included below.
* NetMQPubSub.WebApp - A web application that demonstrates how to publish a message to a subscriber when a web button is pressed. This also provides the details on how to configure dependency injection for NET Core.

## Community Support
We welcome pull requests from the community to enhance and grow NetMQPubSub.

## NetMQPubSub.ConsoleApp
This sample console application creates a single publisher and 10 subscribers.  The publisher will publish a message,
using one of 10 random topics, every 50ms.  Each subscriber has subscribed to a single topic.
The application will execute until the Enter key is pressed. This provides a suitable demonstration of how
to leverage a NET Core CancellationToken to stop and dispose of the publisher and all subscribers once
the Enter key is pressed. Lastly, it shows the proper way for an application to perform
the required NetMQ cleanup when an application shuts down.  See the code reference
to ```NetMQPubSubHelper.Cleanup()```.

``` csharp
namespace NetMQPubSub.ConsoleApp;

using NetMQPubSub.Publisher;
using NetMQPubSub.Common.Helpers;
using NetMQPubSub.Subscriber;
using System;
using System.Linq;
using System.Text.Json;
using NetMQPubSub.Core.Interfaces;

internal class Program
{
	private const int MaximumSubscribers = 10;
	private readonly List<string> topics = Enumerable
		.Range(0, MaximumSubscribers)
		.Select(t => $"Topic{t}")
		.ToList();

	static void Main()
	{
		new Program().Run();
		NetMQPubSubHelper.Cleanup();
	}

	private void Run()
	{
		Console.Write("Press Enter to begin.  Once running, press Enter again to stop.");
		Console.ReadLine();

		var cancellationTokenSource = new CancellationTokenSource();
		var server = Task.Run(() => SubscribersAsync(cancellationTokenSource.Token));
		var client = Task.Run(() => PublisherAsync(cancellationTokenSource.Token));

		Console.ReadLine();

		cancellationTokenSource.Cancel();
		Task.WaitAll(server, client);
	}

	private void SubscribersAsync(CancellationToken cancelToken)
	{
		// or use "inproc://{name}" for in-process (e.g. inproc://job-service)
		var addr = "tcp://localhost:12345";

		var subscriberTasks = new List<Task>();
		for (var i = 0; i < topics.Count; i++)
		{
			var topicIndex = i;
			subscriberTasks.Add(Task.Run(() => RunTopicSubscriberAsync(topics[topicIndex], addr, topicIndex, cancelToken), cancelToken));
		}

		Task.WaitAll(subscriberTasks.ToArray());
	}

	private void PublisherAsync(CancellationToken cancelToken)
	{
		// or use "inproc://{name}" for in-process (e.g. inproc://job-service)
		var addr = "tcp://localhost:12345";

		using IMessagePublisher publisher = new MessagePublisher();
		publisher.Options.SendHighWatermark = 1000;

		Console.WriteLine("Publisher socket binding...");
		publisher.Bind(addr);

		// now that we've bound a socket, give subscriber a bit of time to initialize
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
			Thread.Sleep(50);

		} while (!cancelToken.IsCancellationRequested);

		publisher.Close(); // also consider Unbind(addr)
		Console.WriteLine($"==> Publisher done!");
	}

	private static void RunTopicSubscriberAsync(string topic, string addr, int id, CancellationToken cancelToken)
	{
		Console.WriteLine($"Subscriber #{id} socket connecting...");

		using IMessageSubscriber subscriber = new MessageSubscriber();
		subscriber.Options.ReceiveHighWatermark = 1000;
		subscriber.Connect(addr);
		subscriber.TopicSubscribe(topic);

		const int maxSecondsDelayBeforeCancelCheck = 2;
		var timeout = TimeSpan.FromSeconds(maxSecondsDelayBeforeCancelCheck);
		do
		{
			if (subscriber.TryReceiveMessage<TestMessage>(timeout, out var messageTopicReceived, out var entityReceived))
			{
				Console.WriteLine($"<== Subscriber #{id} message for topic \"{messageTopicReceived}\". Message: {JsonSerializer.Serialize(entityReceived)}");
			}

		} while (!cancelToken.IsCancellationRequested);

		subscriber.Close(); // also consider Disconnect(addr)
		Console.WriteLine($"<== Subscriber #{id} done!");
	}

    internal class TestMessage
    {
        public int Age { get; set; }

        public string Name { get; set; }

        public DateTime Now { get; set; }

        public TestMessage()
        {
            var names = new string[] { "Joe", "Sally", "Mary", "Steve", "Iris", "Bob" };
            var random = new Random();
            this.Age = random.Next(1, 100);
            this.Name = names[random.Next(names.Length)];
            this.Now = DateTime.Now;
        }
    }
}

```