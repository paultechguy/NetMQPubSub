namespace NetMQPubSubExample;

using System;

public interface IMessageSubscriber : IDisposable
{
	void Connect(string address);

	void Disconnect(string address);

	bool TryReceiveTopicMessage(TimeSpan timeout, out string? topic, out bool moreFrames);

	string? ReceiveMessage();

	T ReceiveMessage<T>() where T : class, new();

	void TopicSubscribe(string topic);

	MessageSubscriberOptions Options { get; }
}
